//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2021 by OM International
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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.IO;
using GNU.Gettext;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.OpenXml4Net.OPC;

namespace Ict.Common.IO
{
    /// provides methods for converting CSV file to and from XML;
    /// this helps with adding values, rearranging columns etc;
    /// expects a header line with column names
    public class TCsv2Xml
    {
        /// needed for writing CSV file header;
        /// also collects the node to avoid another recursion
        private static void GetAllAttributesAndNodes(XmlNode ANode, ref List <string>AAllAttributes, ref List <XmlNode>AAllNodes)
        {
            // don't use attributes from the root node
            if ((ANode.ParentNode != null) && (ANode.ParentNode.ParentNode != null))
            {
                AAllNodes.Add(ANode);

                if ((ANode.Attributes.GetNamedItem("name") != null) && !AAllAttributes.Contains("name"))
                {
                    AAllAttributes.Add("name");
                }

                if ((ANode.FirstChild != null) && (ANode.FirstChild.Name == ANode.Name) && !AAllAttributes.Contains("childOf"))
                {
                    AAllAttributes.Add("childOf");
                }

                foreach (XmlAttribute attr in ANode.Attributes)
                {
                    if (!AAllAttributes.Contains(attr.Name))
                    {
                        AAllAttributes.Add(attr.Name);
                    }
                }
            }

            foreach (XmlNode childNode in ANode.ChildNodes)
            {
                GetAllAttributesAndNodes(childNode, ref AAllAttributes, ref AAllNodes);
            }
        }

        /// <summary>
        /// format the XML into CSV so that it can be opened as a spreadsheet;
        /// this only works for quite simple files;
        /// hierarchical structures are flattened (using childOf column)
        /// </summary>
        public static bool Xml2Csv(XmlDocument ADoc, string AOutCSVFile)
        {
            StreamWriter sw = new StreamWriter(AOutCSVFile);

            sw.Write(Xml2CsvString(ADoc));

            sw.Close();

            return true;
        }

        /// <summary>
        /// format the XML into CSV so that it can be opened as a spreadsheet;
        /// this only works for quite simple files;
        /// hierarchical structures are flattened (using childOf column)
        /// </summary>
        public static string Xml2CsvString(XmlDocument ADoc)
        {
            // first write the header of the csv file
            List <string>AllAttributes = new List <string>();
            List <XmlNode>AllNodes = new List <XmlNode>();
            GetAllAttributesAndNodes(ADoc.DocumentElement, ref AllAttributes, ref AllNodes);

            string separator = TAppSettingsManager.GetValue("CSVSeparator",
                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);

            string headerLine = "";

            foreach (string attrName in AllAttributes)
            {
                if (headerLine.Length > 0)
                {
                    headerLine += separator;
                }

                headerLine += "\"" + attrName + "\"";
            }

            string result = headerLine + Environment.NewLine;

            foreach (XmlNode node in AllNodes)
            {
                string line = "";

                foreach (string attrName in AllAttributes)
                {
                    if (attrName == "childOf")
                    {
                        line = StringHelper.AddCSV(line, TXMLParser.GetAttribute(node.ParentNode, "name"), separator);
                    }
                    else
                    {
                        line = StringHelper.AddCSV(line, TXMLParser.GetAttribute(node, attrName), separator);
                    }
                }

                result += line + Environment.NewLine;
            }

            return result;
        }

        /// <summary>
        /// store the data into Excel format, Open Office XML, .xlsx
        /// </summary>
        private static void Xml2ExcelWorksheet(XmlDocument ADoc, IWorkbook AWorkbook, ISheet AWorksheet, bool AWithHashInCaption = true)
        {
            Int32 rowCounter = 1;
            Int16 colCounter = 1;
            IRow wsrow = null;
            ICell wscell = null;

            ICellStyle wsstyle_dateformat = AWorkbook.CreateCellStyle();
            ICreationHelper createHelper = AWorkbook.GetCreationHelper();
            wsstyle_dateformat.DataFormat = createHelper.CreateDataFormat().GetFormat("dd/mm/yyyy");

            // first write the header of the csv file
            List <string>AllAttributes = new List <string>();
            List <XmlNode>AllNodes = new List <XmlNode>();
            GetAllAttributesAndNodes(ADoc.DocumentElement, ref AllAttributes, ref AllNodes);

            wsrow = AWorksheet.CreateRow(rowCounter);
            foreach (string attrName in AllAttributes)
            {
                wscell = wsrow.CreateCell(colCounter);
                wscell.SetCellValue((AWithHashInCaption?"#":"") + attrName);
                colCounter++;
            }

            rowCounter++;
            colCounter = 1;

            foreach (XmlNode node in AllNodes)
            {
                wsrow = AWorksheet.CreateRow(rowCounter);

                foreach (string attrName in AllAttributes)
                {
                    wscell = wsrow.CreateCell(colCounter);

                    if (attrName == "childOf")
                    {
                        wscell.SetCellValue(TXMLParser.GetAttribute(node.ParentNode, "name"));
                    }
                    else
                    {
                        string value = TXMLParser.GetAttribute(node, attrName);

                        if (value.StartsWith(eVariantTypes.eDateTime.ToString() + ":"))
                        {
                            wscell.SetCellValue(TVariant.DecodeFromString(value).ToDate());
                            wscell.CellStyle = wsstyle_dateformat;
                        }
                        else if (value.StartsWith(eVariantTypes.eInteger.ToString() + ":"))
                        {
                            wscell.SetCellValue(TVariant.DecodeFromString(value).ToInt64());
                        }
                        else if (value.StartsWith(eVariantTypes.eDecimal.ToString() + ":"))
                        {
                            wscell.SetCellValue((double)TVariant.DecodeFromString(value).ToDecimal());
                        }
                        else
                        {
                            wscell.SetCellValue(value);
                        }
                    }

                    colCounter++;
                }

                rowCounter++;
                colCounter = 1;
            }
        }

        /// <summary>
        /// store the data into Excel format, Open Office XML, .xlsx
        /// </summary>
        public static bool Xml2ExcelStream(XmlDocument ADoc, Stream AStream, bool AWithHashInCaption = true)
        {
            try
            {
                XSSFWorkbook workbook = new XSSFWorkbook();
                ISheet worksheet = workbook.CreateSheet("Data Export");

                Xml2ExcelWorksheet(ADoc, workbook, worksheet, AWithHashInCaption);

                workbook.Write(AStream);

                return true;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// store the data into Excel format, Open Office XML, .xlsx
        /// this overload stores several worksheets
        /// </summary>
        public static bool Xml2ExcelStream(SortedList <string, XmlDocument>ADocs, MemoryStream AStream, bool AWithHashInCaption = true)
        {
            try
            {
                XSSFWorkbook workbook = new XSSFWorkbook();

                foreach (string WorksheetTitle in ADocs.Keys)
                {
                    ISheet worksheet = workbook.CreateSheet(WorksheetTitle);

                    Xml2ExcelWorksheet(ADocs[WorksheetTitle], workbook, worksheet, AWithHashInCaption);
                }

                workbook.Write(AStream);
                return true;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// store the data into Excel format, Open Office XML, .xlsx
        /// </summary>
        private static void DataTable2ExcelWorksheet(DataTable table, IWorkbook AWorkbook, ISheet AWorksheet, bool AWithHashInCaption = true)
        {
            IRow wsrow = null;
            ICell wscell = null;
            Int32 rowCounter = 1;
            Int16 colCounter = 1;

            ICellStyle wsstyle_dateformat = AWorkbook.CreateCellStyle();
            ICreationHelper createHelper = AWorkbook.GetCreationHelper();
            wsstyle_dateformat.DataFormat = createHelper.CreateDataFormat().GetFormat("dd/mm/yyyy");

            // first write the header of the csv file
            wsrow = AWorksheet.CreateRow(rowCounter);
            foreach (DataColumn col in table.Columns)
            {
                wscell = wsrow.CreateCell(colCounter);
                wscell.SetCellValue((AWithHashInCaption?"#":"") + col.ColumnName);

                colCounter++;
            }

            rowCounter++;
            colCounter = 1;

            foreach (DataRow row in table.Rows)
            {
                wsrow = AWorksheet.CreateRow(rowCounter);

                foreach (DataColumn col in table.Columns)
                {
                    wscell = wsrow.CreateCell(colCounter);

                    if (row.IsNull(col) || (row[col] == null))
                    {
                        wscell.SetCellValue("");
                        colCounter++;
                        continue;
                    }

                    object value = row[col];

                    if (value is DateTime)
                    {
                        wscell.SetCellValue((DateTime)value);
                        wscell.CellStyle = wsstyle_dateformat;
                    }
                    else if (value is Int32 || value is Int64 || value is Int16)
                    {
                        wscell.SetCellValue(Convert.ToInt64(value));
                    }
                    else
                    {
                        wscell.SetCellValue(value.ToString());
                    }

                    colCounter++;
                }

                rowCounter++;
                colCounter = 1;
            }
        }

        /// <summary>
        /// save a generic DataTable to an Excel file
        /// </summary>
        public static bool DataTable2ExcelStream(DataTable table, MemoryStream AStream, bool AWithHashInCaption = true)
        {
            try
            {
                XSSFWorkbook workbook = new XSSFWorkbook();

                ISheet worksheet = workbook.CreateSheet(table.TableName);

                DataTable2ExcelWorksheet(table, workbook, worksheet, AWithHashInCaption);

                workbook.Write(AStream);

                return true;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// save a generic CSV string to an Excel file
        /// </summary>
        public static bool CSV2ExcelStream(string ACSVData, MemoryStream AStream, string ASeparator = ",", string ATableName = "data")
        {
            try
            {
                XSSFWorkbook workbook = new XSSFWorkbook();

                ISheet worksheet = workbook.CreateSheet(ATableName);

                IRow wsrow = null;
                ICell wscell = null;
                Int32 rowCounter = 1;
                Int16 colCounter = 1;

                // we don't have headers for the columns

                List<String> Lines = ACSVData.Split(Environment.NewLine).ToList();
                Int32 LineCounter = 0;

                while (LineCounter < Lines.Count)
                {
                    wsrow = worksheet.CreateRow(rowCounter);
                    string line = Lines[LineCounter];

                    while (line.Trim().Length > 0)
                    {
                        wscell = wsrow.CreateCell(colCounter);
                        string value = StringHelper.GetNextCSV(ref line, Lines, ref LineCounter, ASeparator);

                        TVariant v = new TVariant(value);
                        if (v.TypeVariant == eVariantTypes.eDecimal)
                        {
                            wscell.SetCellValue((double)v.ToDecimal());
                        }
                        else if (v.TypeVariant == eVariantTypes.eInteger)
                        {
                            wscell.SetCellValue(v.ToInt32());
                        }
                        else if (v.TypeVariant == eVariantTypes.eDateTime)
                        {
                            wscell.SetCellValue(v.ToDate());
                        }
                        else
                        {
                            wscell.SetCellValue(value);
                        }
                        colCounter++;
                    }

                    LineCounter++;
                    rowCounter++;
                    colCounter = 1;
                }

                workbook.Write(AStream);

                return true;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Load an xlsx Excel file into a datatable
        /// </summary>
        public static DataTable ParseExcelWorkbook2DataTable(Stream AStream,
            bool AHasHeader = false,
            int AWorksheetID = 0,
            List <string>AColumnsToImport = null)
        {
            XSSFWorkbook workbook = new XSSFWorkbook(AStream);
            ISheet worksheet = workbook[AWorksheetID];

            DataTable result = new DataTable();

            if (worksheet == null)
            {
                return result;
            }

            List <string>ColumnNames = new List <string>();

            IRow firstRow = worksheet.GetRow(0);
            int cellCount = firstRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = firstRow.GetCell(j);
                string ColumnName = (AHasHeader ? cell.ToString() : string.Format("Column {0}", j+1));
                ColumnNames.Add(ColumnName);

                if ((AColumnsToImport != null) && !AColumnsToImport.Contains(ColumnName))
                {
                    continue;
                }

                result.Columns.Add(ColumnName);
            }

            int firstDataRow = AHasHeader ? 1 : 0;
            for (int countRow = worksheet.FirstRowNum + firstDataRow; countRow <= worksheet.LastRowNum; countRow++)
            {
                IRow wsrow = worksheet.GetRow(countRow);
                if (wsrow == null) continue;
                if (wsrow.Cells.All(d => d.CellType == CellType.Blank)) continue;

                DataRow NewRow = result.NewRow();

                for (int j = wsrow.FirstCellNum; j < cellCount; j++)
                {
                    if ((AColumnsToImport != null) && !AColumnsToImport.Contains(ColumnNames[j - wsrow.FirstCellNum]))
                    {
                        continue;
                    }

                    if (wsrow.GetCell(j) != null)
                    {
                        NewRow[ColumnNames[j - wsrow.FirstCellNum]] = wsrow.GetCell(j);
                    }
                }

                result.Rows.Add(NewRow);
            }

            return result;
        }

        /// <summary>
        /// convert a CSV file to an XmlDocument.
        /// the first line is expected to contain the column names/captions, in quotes.
        /// from the header line, the separator can be determined, if the parameter ASeparator is empty
        /// </summary>
        /// <param name="ACSVFilename">The filename</param>
        /// <param name="AFallbackEncoding">The file encoding will be automatically determined, but if a fallback is specified that does not match
        /// the encoding that was determined, it will be used.  Usually this paramter can be null.</param>
        /// <param name="ASeparator">A column separator</param>
        public static XmlDocument ParseCSVFile2Xml(string ACSVFilename, string ASeparator = null, Encoding AFallbackEncoding = null)
        {
            string fileContent;
            Encoding fileEncoding;
            bool hasBOM, isAmbiguous;

            byte[] rawBytes;

            if (TTextFile.AutoDetectTextEncodingAndOpenFile(ACSVFilename, out fileContent, out fileEncoding,
                    out hasBOM, out isAmbiguous, out rawBytes))
            {
                if ((AFallbackEncoding != null) && !fileEncoding.Equals(AFallbackEncoding))
                {
                    fileContent = AFallbackEncoding.GetString(rawBytes);
                }

                return ParseCSVContent2Xml(fileContent, ASeparator);
            }

            // Either we could not open the file or it is empty
            List<string> AllColumns = null;
            return ParseCSV2Xml(new List <string>(), ASeparator, out AllColumns);
        }

        /// <summary>
        /// convert the content of a CSV file to an XmlDocument.
        /// the first line is expected to contain the column names/captions, in quotes.
        /// from the header line, the separator can be determined, if the parameter ASeparator is empty
        /// </summary>
        public static XmlDocument ParseCSVContent2Xml(string AFileContent, string ASeparator = null)
        {
            List <string>Lines = new List <string>();

            using (StringReader reader = new StringReader(AFileContent))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    Lines.Add(line);
                    line = reader.ReadLine();
                }

                reader.Close();
            }

            List<string> AllColumns = null;
            return ParseCSV2Xml(Lines, ASeparator, out AllColumns);
        }

        /// <summary>
        /// convert a CSV file to an XmlDocument.
        /// the first line is expected to contain the column names/captions, in quotes.
        /// from the header line, the separator can be determined, if the parameter ASeparator is empty
        /// </summary>
        public static XmlDocument ParseCSV2Xml(TextReader AReader, string ASeparator = null)
        {
            List <string>CSVRows = new List <string>();

            while (true)
            {
                string line = AReader.ReadLine();

                if (line == null)
                {
                    break;
                }

                CSVRows.Add(line);
            }

            AReader.Close();

            List<string> AllColumns = null;
            return ParseCSV2Xml(CSVRows, ASeparator, out AllColumns);
        }

        /// <summary>
        /// convert a CSV file to an XmlDocument.
        /// the first line is expected to contain the column names/captions, in quotes.
        /// from the header line, the separator can be determined, if the parameter ASeparator is empty
        /// </summary>
        public static XmlDocument ParseCSV2Xml(List <string>ALines, string ASeparator, out List <string> AAllAttributes)
        {
            XmlDocument myDoc = TYml2Xml.CreateXmlDocument();

            int LineCounter = 1;
            string headerLine = ALines[0];
            string separator = ASeparator;

            if (string.IsNullOrEmpty(ASeparator))
            {
                if (!headerLine.StartsWith("\""))
                {
                    throw new Exception(Catalog.GetString("Cannot open CSV file, because it is missing the header line.") +
                        Environment.NewLine +
                        Catalog.GetString("There must be a row with the column captions, at least the first caption must be in quotes."));
                }
                else
                {
                    // read separator from header line. at least the first column needs to be quoted
                    separator = headerLine[StringHelper.FindMatchingQuote(headerLine, 0) + 2].ToString();
                }
            }

            AAllAttributes = new List <string>();

            while (headerLine.Length > 0)
            {
                string attrName = StringHelper.GetNextCSV(ref headerLine, separator);

                if (attrName.Length == 0)
                {
                    TLogging.Log("Csv2Xml: found empty column header, will not consider any following columns");
                    break;
                }

                if (attrName.Length > 1)
                {
                    attrName = attrName[0] + StringHelper.UpperCamelCase(attrName, ' ', false, false).Substring(1);
                }

                // some characters are not allowed in the name of an XmlAttribute
                attrName = attrName.Replace("%", "percent");
                attrName = attrName.Replace("-", "hyphen");
                attrName = attrName.Replace("/", "slash");
                attrName = attrName.Replace(" ", "space");

                try
                {
                    myDoc.CreateAttribute(attrName);
                }
                catch (Exception)
                {
                    char[] arr = attrName.ToCharArray();

                    // filter only letters and digits
                    arr = Array.FindAll <char>(arr, (c => (char.IsLetterOrDigit(c))));
                    attrName = new string(arr);
                }

                AAllAttributes.Add(attrName);
            }

            LineCounter = 1;

            while (LineCounter < ALines.Count)
            {
                string line = ALines[LineCounter];

                if (line.Trim().Length > 0)
                {
                    SortedList <string, string>AttributePairs = new SortedList <string, string>();

                    foreach (string attrName in AAllAttributes)
                    {
                        // support csv values that contain line breaks
                        AttributePairs.Add(attrName, StringHelper.GetNextCSV(ref line, ALines, ref LineCounter, separator));
                    }

                    string rowName = "Element";

                    if (AttributePairs.ContainsKey("name"))
                    {
                        rowName = AttributePairs["name"];
                    }

                    XmlNode newNode = myDoc.CreateElement("", rowName, "");

                    if (AttributePairs.ContainsKey("childOf"))
                    {
                        XmlNode parentNode = TXMLParser.FindNodeRecursive(myDoc.DocumentElement, AttributePairs["childOf"]);

                        if (parentNode == null)
                        {
                            parentNode = myDoc.DocumentElement;
                        }

                        parentNode.AppendChild(newNode);
                    }
                    else
                    {
                        myDoc.DocumentElement.AppendChild(newNode);
                    }

                    foreach (string attrName in AAllAttributes)
                    {
                        if ((attrName != "name") && (attrName != "childOf"))
                        {
                            XmlAttribute attr = myDoc.CreateAttribute(attrName);
                            attr.Value = AttributePairs[attrName];
                            newNode.Attributes.Append(attr);
                        }
                    }
                }

                LineCounter++;
            }

            return myDoc;
        }

        /// <summary>
        /// convert a CSV file to a DataTable.
        /// the first line is expected to contain the column names/captions, in quotes.
        /// from the header line, the separator can be determined, if the parameter ASeparator is empty
        /// </summary>
        public static DataTable ParseCSV2DataTable(List <string>ALines, string ASeparator)
        {
            string headerLine = ALines[0];
            string separator = ASeparator;
            DataTable result = new DataTable();

            if (string.IsNullOrEmpty(ASeparator))
            {
                if (!headerLine.StartsWith("\""))
                {
                    throw new Exception(Catalog.GetString("Cannot open CSV file, because it is missing the header line.") +
                        Environment.NewLine +
                        Catalog.GetString("There must be a row with the column captions, at least the first caption must be in quotes."));
                }
                else
                {
                    // read separator from header line. at least the first column needs to be quoted
                    separator = headerLine[StringHelper.FindMatchingQuote(headerLine, 0) + 2].ToString();
                }
            }

            while (headerLine.Length > 0)
            {
                string attrName = StringHelper.GetNextCSV(ref headerLine, separator);

                if (attrName.Length == 0)
                {
                    TLogging.Log("Csv2DataTable: found empty column header, will not consider any following columns");
                    break;
                }

                if (attrName.Length > 1)
                {
                    attrName = attrName[0] + StringHelper.UpperCamelCase(attrName, ' ', false, false).Substring(1);
                }

                result.Columns.Add(attrName);
            }

            int LineCounter = 1;

            while (LineCounter < ALines.Count)
            {
                DataRow NewRow = result.NewRow();
                string line = ALines[LineCounter];

                if (line.Trim().Length > 0)
                {
                    foreach (DataColumn c in result.Columns)
                    {
                        // support csv values that contain line breaks
                        NewRow[c.ColumnName] = StringHelper.GetNextCSV(ref line, ALines, ref LineCounter, separator);
                    }

                    result.Rows.Add(NewRow);
                }

                LineCounter++;
            }

            return result;
        }
    }
}
