/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2010 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Xml;
using System.Windows.Forms;
using Mono.Unix;
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
        /// <param name="AParentFormHandle"></param>
        public static void ExportAllData(IntPtr AParentFormHandle)
        {
            MessageBox.Show(Catalog.GetString("This may take a while. Please just wait!"));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(TRemote.MSysMan.ImportExport.WebConnectors.ExportAllTables());
            TImportExportDialogs.ExportWithDialog(doc, Catalog.GetString("Save Database into File"));
        }

        /// <summary>
        /// this will delete the current database, and reset it with the data selected
        /// </summary>
        /// <param name="AParentFormHandle"></param>
        public static void ImportAllData(IntPtr AParentFormHandle)
        {
            DialogResult r = MessageBox.Show(
                Catalog.GetString("WARNING: this will reset the database! Do you really want to delete the current database?"),
                Catalog.GetString("WARNING: this will reset the database!"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (r == DialogResult.Yes)
            {
                XmlDocument doc = TImportExportDialogs.ImportWithDialog(Catalog.GetString("Please select the file to import from"));

                if (doc != null)
                {
                    if (TRemote.MSysMan.ImportExport.WebConnectors.ResetDatabase(TXMLParser.XmlToString(doc)))
                    {
                        // TODO: reset all caches? for comboboxes etc
                        MessageBox.Show(Catalog.GetString("Import of database was successful. Please restart your OpenPetra client"));
                    }
                    else
                    {
                        MessageBox.Show(Catalog.GetString("Failed import of database. Please check the Server.log file on the server"));
                    }
                }
            }
        }

        /// <summary>
        /// change the password of a user
        /// </summary>
        public static void SetUserPassword(IntPtr AParentFormHandle)
        {
            PetraInputBox input = new PetraInputBox(
                Catalog.GetString("Change the password of a user"),
                Catalog.GetString("Please enter the user name:"),
                "", false);

            if (input.ShowDialog() == DialogResult.OK)
            {
                string username = input.GetAnswer();
                input = new PetraInputBox(
                    Catalog.GetString("Change the password of a user"),
                    Catalog.GetString("Please enter the new password:"),
                    "", true);

                if (input.ShowDialog() == DialogResult.OK)
                {
                    string password = input.GetAnswer();

                    if (TRemote.MSysMan.Maintenance.WebConnectors.SetUserPassword(username, password))
                    {
                        MessageBox.Show(String.Format(Catalog.GetString("Password was successfully set for user {0}"), username));
                    }
                    else
                    {
                        MessageBox.Show(String.Format(Catalog.GetString("There was a problem setting the password for user {0}"), username));
                    }
                }
            }
        }
    }
}