//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Xml;
using GNU.Gettext;
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
        /// Select a path and filename for file export
        /// </summary>
        /// <param name="ADialogTitle"></param>
        /// <returns>Local path, or empty sctring if no path selected.</returns>
        public static String GetExportFilename(string ADialogTitle)
        {
            SaveFileDialog DialogSave = new SaveFileDialog();

            DialogSave.DefaultExt = "yml";
            DialogSave.Filter = Catalog.GetString(
                "Text file (*.yml)|*.yml|XML file (*.xml)|*.xml|Petra export (*.ext)|*.ext|Spreadsheet file (*.csv)|*.csv");
            DialogSave.AddExtension = true;
            DialogSave.RestoreDirectory = true;
            DialogSave.Title = ADialogTitle;

            if (DialogSave.ShowDialog() == DialogResult.OK)
            {
                return DialogSave.FileName.ToLower();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Put this (ext formatted) string onto a file
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static bool ExportTofile(string doc, string FileName)
        {
            if (FileName.EndsWith("ext"))
            {
                StreamWriter outfile = new StreamWriter(FileName);
                outfile.Write(doc);
                outfile.Close();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Put this (XML formatted) data on a local file
        /// </summary>
        /// <param name="doc">XML data to be exported</param>
        /// <param name="FileName">Filename from GetExportFilename, above</param>
        /// <returns>true if successful</returns>
        public static bool ExportTofile(XmlDocument doc, string FileName)
        {
            if (FileName.EndsWith("xml"))
            {
                doc.Save(FileName);
                return true;
            }
            else if (FileName.EndsWith("csv"))
            {
                return TCsv2Xml.Xml2Csv(doc, FileName);
            }
            else if (FileName.EndsWith("yml"))
            {
                return TYml2Xml.Xml2Yml(doc, FileName);
            }

            return false;
        }

        /// <summary>
        /// Export data to a range of different file formats;
        /// ask the user for filename and file format
        ///
        /// NOTE this has been replaced by the two-part scheme above:
        ///   first get the filename, then the caller loads the data
        ///   from the server, then it calls ExportToFile.
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
            DialogSave.Filter = Catalog.GetString(
                "Text file (*.yml)|*.yml|XML file (*.xml)|*.xml|Petra export (*.ext)|*.ext|Spreadsheet file (*.csv)|*.csv");
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
                else if (DialogSave.FileName.ToLower().EndsWith("ext"))
                {
                    return false;
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
        /// export zipped yml string to file.
        /// ask the user for filename
        /// </summary>
        /// <param name="AZippedYML"></param>
        /// <param name="ADialogTitle"></param>
        /// <returns></returns>
        public static bool ExportWithDialogYMLGz(string AZippedYML, string ADialogTitle)
        {
            SaveFileDialog DialogSave = new SaveFileDialog();

            DialogSave.DefaultExt = "yml.gz";
            DialogSave.Filter = Catalog.GetString("Zipped Text file (*.yml.gz)|*.yml.gz");
            DialogSave.AddExtension = true;
            DialogSave.RestoreDirectory = true;
            DialogSave.Title = ADialogTitle;

            if (DialogSave.ShowDialog() == DialogResult.OK)
            {
                string filename = DialogSave.FileName;

                // it seems there was a bug that only .gz was added if user did not type the extension
                if (!filename.EndsWith(".yml.gz"))
                {
                    filename = Path.GetFileNameWithoutExtension(filename) + ".yml.gz";
                }

                FileStream fs = new FileStream(filename, FileMode.Create);
                byte[] buffer = Convert.FromBase64String(AZippedYML);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
                return true;
            }

            return false;
        }

        /// <summary>
        /// import a zipped yml file.
        /// if the file is a plain yml file, the content will be zipped.
        /// shows a dialog to the user to select the file to import
        /// </summary>
        /// <returns></returns>
        public static string ImportWithDialogYMLGz(string ADialogTitle)
        {
            OpenFileDialog DialogOpen = new OpenFileDialog();

            DialogOpen.Filter = Catalog.GetString(
                "Zipped Text file (*.yml.gz)|*.yml.gz|Text file (*.yml)|*.yml");
            DialogOpen.FilterIndex = 0;
            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = ADialogTitle;

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                if (DialogOpen.FileName.ToLower().EndsWith("gz"))
                {
                    FileStream fs = new FileStream(DialogOpen.FileName, FileMode.Open);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    return Convert.ToBase64String(buffer);
                }
                else if (DialogOpen.FileName.ToLower().EndsWith("yml"))
                {
                    TextReader reader = new StreamReader(DialogOpen.FileName, true);
                    string ymlData = reader.ReadToEnd();
                    reader.Close();
                    return PackTools.ZipString(ymlData);
                }
            }

            return null;
        }

        /// <summary>
        /// convert from all sorts of formats into xml document;
        /// shows a dialog to the user to select the file to import
        /// </summary>
        /// <returns></returns>
        public static XmlDocument ImportWithDialog(string ADialogTitle)
        {
            string temp;

            return ImportWithDialog(ADialogTitle, out temp);
        }

        /// <summary>
        /// convert from all sorts of formats into xml document;
        /// shows a dialog to the user to select the file to import
        /// </summary>
        /// <returns></returns>
        public static XmlDocument ImportWithDialog(string ADialogTitle, out string AFilename)
        {
            // TODO support import from Excel and OpenOffice files
            // See also http://sourceforge.net/apps/mediawiki/openpetraorg/index.php?title=Data_liberation
            OpenFileDialog DialogOpen = new OpenFileDialog();

            AFilename = "";

            DialogOpen.Filter = Catalog.GetString(
                "Text file (*.yml)|*.yml|XML file (*.xml)|*.xml|Spreadsheet file (*.csv)|All supported file formats (*.yml, *.xml, *.csv)|*.csv;*.yml;*.xml|");
            DialogOpen.FilterIndex = 4;
            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = ADialogTitle;

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                AFilename = DialogOpen.FileName;

                if (DialogOpen.FileName.ToLower().EndsWith("csv"))
                {
                    // select separator, make sure there is a header line with the column captions/names
                    TDlgSelectCSVSeparator dlgSeparator = new TDlgSelectCSVSeparator(true);
                    Boolean fileCanOpen = dlgSeparator.OpenCsvFile(DialogOpen.FileName);

                    if (!fileCanOpen)
                    {
                        throw new Exception(String.Format(Catalog.GetString("File {0} Cannot be opened."), DialogOpen.FileName));
                    }

                    if (dlgSeparator.ShowDialog() == DialogResult.OK)
                    {
                        XmlDocument doc = TCsv2Xml.ParseCSV2Xml(DialogOpen.FileName, dlgSeparator.SelectedSeparator);
                        return doc;
                    }
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