//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.Data;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.IO;
using GNU.Gettext;
using ICSharpCode.SharpZipLib.Zip;

namespace Ict.Common.IO
{
    /// provides methods for reading Open Document files
    /// see http://en.wikipedia.org/wiki/OpenDocument
    public class TOpenDocumentParser
    {
        /// <summary>
        /// Load an OpenDocument .ods file into a datatable
        /// </summary>
        public static DataTable ParseODSStream2DataTable(MemoryStream AStream,
            bool AHasHeader = false,
            int AWorksheetID = 0,
            List <string>AColumnsToImport = null)
        {
            XmlDocument doc = GetContent(AStream);

            XmlNode OfficeBody = TXMLParser.GetChild(doc.DocumentElement, "office:body");
            XmlNode OfficeDocument = TXMLParser.GetChild(OfficeBody, "office:spreadsheet");

            int countWorksheets = 0;
            XmlNode worksheet = null;

            foreach (XmlNode worksheetLoop in OfficeDocument.ChildNodes)
            {
                if (worksheetLoop.Name != "table:table")
                {
                    continue;
                }

                if (countWorksheets == AWorksheetID)
                {
                    worksheet = worksheetLoop;
                }

                countWorksheets++;
            }

            DataTable result = new DataTable();

            if (worksheet == null)
            {
                return result;
            }

            List <string>ColumnNames = new List <string>();

            bool firstRow = true;

            foreach (XmlNode rowNode in worksheet.ChildNodes)
            {
                if (rowNode.Name != "table:table-row")
                {
                    continue;
                }

                // create columns
                if (firstRow)
                {
                    int columnCounter = 0;

                    foreach (XmlNode cellNode in rowNode.ChildNodes)
                    {
                        if (cellNode.Name != "table:table-cell")
                        {
                            continue;
                        }

                        if (TXMLParser.HasAttribute(cellNode, "table:number-columns-repeated"))
                        {
                            // just ignore duplicate columns in the header line, the values must be unique anyway
                            continue;
                        }

                        string ColumnName = (AHasHeader ? cellNode.FirstChild.InnerText : string.Format("Column {0}", columnCounter));
                        ColumnNames.Add(ColumnName);
                        columnCounter++;

                        if ((AColumnsToImport != null) && !AColumnsToImport.Contains(ColumnName))
                        {
                            continue;
                        }

                        result.Columns.Add(ColumnName);
                    }
                }

                // import row
                if (!firstRow || !AHasHeader)
                {
                    DataRow NewRow = result.NewRow();

                    int columnCounter = 0;

                    foreach (XmlNode cellNode in rowNode.ChildNodes)
                    {
                        if (cellNode.Name != "table:table-cell")
                        {
                            continue;
                        }

                        Int32 NumberColumnsRepeated = 1;

                        // handle columns with same value
                        if (TXMLParser.HasAttribute(cellNode, "table:number-columns-repeated"))
                        {
                            NumberColumnsRepeated = Convert.ToInt32(TXMLParser.GetAttribute(cellNode, "table:number-columns-repeated"));

                            if (!TXMLParser.HasAttribute(cellNode, "office:value-type"))
                            {
                                // skip empty columns
                                columnCounter += NumberColumnsRepeated;
                                continue;
                            }
                        }

                        while (NumberColumnsRepeated > 0)
                        {
                            string CellType = TXMLParser.GetAttribute(cellNode, "office:value-type");

                            if ((AColumnsToImport != null) && !AColumnsToImport.Contains(ColumnNames[columnCounter]))
                            {
                                // skip this column
                            }
                            else if (CellType == "float")
                            {
                                TVariant variant = new TVariant(TXMLParser.GetAttribute(cellNode, "office:value"));
                                NewRow[ColumnNames[columnCounter]] = variant.ToObject();
                            }
                            else if (CellType == "date")
                            {
                                NewRow[ColumnNames[columnCounter]] =
                                    new TVariant(TXMLParser.GetAttribute(cellNode, "office:date-value")).ToDate();
                            }
                            else if (CellType == "boolean")
                            {
                                NewRow[ColumnNames[columnCounter]] =
                                    (TXMLParser.GetAttribute(cellNode, "office:boolean-value") == "true");
                            }
                            else if (CellType == "string")
                            {
                                NewRow[ColumnNames[columnCounter]] = cellNode.FirstChild.InnerText;
                            }

                            columnCounter++;
                            NumberColumnsRepeated--;
                        }
                    }

                    result.Rows.Add(NewRow);
                }

                firstRow = false;
            }

            return result;
        }

        private static XmlDocument GetContent(Stream fileStream)
        {
            XmlDocument result = new XmlDocument();

            // Always ensure we are reading from the beginning...
            fileStream.Seek(0, SeekOrigin.Begin);

            using (ZipInputStream zipInputStream = new ZipInputStream(fileStream))
            {
                ZipEntry element = null;

                while ((element = zipInputStream.GetNextEntry()) != null)
                {
                    if (element.IsFile && (element.Name.ToLower() == "content.xml"))
                    {
                        break;
                    }
                }

                if (element.Name.ToLower() != "content.xml")
                {
                    throw new Exception("Missing content.xml");
                }

                var bytesResult = new byte[] { };
                var bytes = new byte[5000];
                var i = 0;

                while ((i = zipInputStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var arrayLength = bytesResult.Length;
                    Array.Resize <byte>(ref bytesResult, arrayLength + i);
                    Array.Copy(bytes, 0, bytesResult, arrayLength, i);
                }

                result.LoadXml(Encoding.UTF8.GetString(bytesResult));
            }
            return result;
        }
    }
}