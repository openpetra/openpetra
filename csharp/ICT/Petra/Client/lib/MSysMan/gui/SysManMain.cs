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
    }
}