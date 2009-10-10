/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
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
using System.IO;
using System.Xml;
using Mono.Unix;
using Ict.Common;
using System.Windows.Forms;

namespace Ict.Common.IO
{
    /// <summary>
    /// support data liberation;
    /// see also http://sourceforge.net/apps/mediawiki/openpetraorg/index.php?title=Data_liberation
    ///
    /// </summary>
    public class TImportExportDialogs
    {
        /// <summary>
        /// export data to a range of different file formats;
        /// ask the user for filename and file format
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ADialogTitle"></param>
        /// <returns></returns>
        public static bool ExportWithDialog(XmlDocument doc, string ADialogTitle)
        {
            // TODO: TExcel excel = new TExcel();
            // TODO: openoffice
            // See also http://sourceforge.net/apps/mediawiki/openpetraorg/index.php?title=Data_liberation
            SaveFileDialog DialogSave = new SaveFileDialog();

            DialogSave.DefaultExt = "yml";
            DialogSave.Filter = Catalog.GetString("Text file (*.yml)|*.yml|XML file (*.xml)|*.xml|Spreadsheet file (*.csv)|*.csv");
            DialogSave.AddExtension = true;
            DialogSave.RestoreDirectory = true;
            DialogSave.Title = ADialogTitle;

            if (DialogSave.ShowDialog() == DialogResult.OK)
            {
                if (DialogSave.FileName.ToLower().EndsWith("xml"))
                {
                    doc.Save(DialogSave.FileName);
                    return true;
                }
                else if (DialogSave.FileName.ToLower().EndsWith("csv"))
                {
                    return TCsv2Xml.Xml2Csv(doc, DialogSave.FileName);
                }
                else if (DialogSave.FileName.ToLower().EndsWith("yml"))
                {
                    return TYml2Xml.Xml2Yml(doc, DialogSave.FileName);
                }
            }

            return false;
        }

        /// <summary>
        /// convert from all sorts of formats into xml document;
        /// shows a dialog to the user to select the file to import
        /// </summary>
        /// <param name="ADialogTitle"></param>
        /// <returns></returns>
        public static XmlDocument ImportWithDialog(string ADialogTitle)
        {
            // TODO support import from Excel and OpenOffice files
            // See also http://sourceforge.net/apps/mediawiki/openpetraorg/index.php?title=Data_liberation
            OpenFileDialog DialogOpen = new OpenFileDialog();

            DialogOpen.Filter = Catalog.GetString(
                "Text file (*.yml)|*.yml|XML file (*.xml)|*.xml|Spreadsheet file (*.csv)|All supported file formats (*.yml, *.xml, *.csv)|*.csv;*.yml;*.xml|");
            DialogOpen.FilterIndex = 3;
            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = ADialogTitle;

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                if (DialogOpen.FileName.ToLower().EndsWith("csv"))
                {
                }
                else if (DialogOpen.FileName.ToLower().EndsWith("xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(DialogOpen.FileName);
                    return doc;
                }
                else if (DialogOpen.FileName.ToLower().EndsWith("yml"))
                {
                    TYml2Xml yml = new TYml2Xml(DialogOpen.FileName);
                    return yml.ParseYML2XML();
                }
            }

            return null;
        }
    }
}