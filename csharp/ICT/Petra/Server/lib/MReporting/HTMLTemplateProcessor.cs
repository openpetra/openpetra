//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2022 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Data;

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared.MReporting;

using HtmlAgilityPack;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Ict.Petra.Server.MReporting
{
    /// parse and replace HTML templates for reports
    public class HTMLTemplateProcessor
    {
        private string FHTMLTemplate;
        private HtmlDocument FHTMLDocument;
        private Dictionary<string, string> FSQLQueries = new Dictionary<string, string>();
        // do not print warning too many times for the same variable
        private SortedList<string, Int32> VariablesNotFound = new SortedList<string, int>();
        private TParameterList FParameters;

        /// <summary>
        /// constructor
        /// </summary>
        public HTMLTemplateProcessor(string AHTMLTemplate, TParameterList AParameters)
        {
            FHTMLTemplate = AHTMLTemplate;
            FParameters = AParameters;

            SeparateSQLQueries();

            FHTMLDocument = new HtmlDocument();
            FHTMLDocument.LoadHtml(FHTMLTemplate);

            var head = FHTMLDocument.DocumentNode.SelectSingleNode("//div[@id='head']");
            string strHead = head.InnerHtml;
            strHead = InsertParameters("{{", "}}", strHead, ReplaceOptions.NoQuotes);
            strHead = InsertParameters("{", "}", strHead, ReplaceOptions.NoQuotes);

            strHead = EvaluateVisible(strHead);
            head.InnerHtml = strHead;
        }

        /// <summary>
        /// Adds the parameters from row.
        /// </summary>
        public void AddParametersFromRow(DataRow row)
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                FParameters.Add(c.ColumnName, new TVariant(row[c]));
            }
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        public void SetParameter(string name, TVariant value)
        {
            FParameters.Add(name, value);
        }

        /// separate the sql queries from the HTML template
        private void SeparateSQLQueries()
        {
            int pos = FHTMLTemplate.IndexOf("<!-- BeginSQL ");
            while (pos != -1)
            {
                int posAfterName = FHTMLTemplate.IndexOf("-->", pos);
                string name = FHTMLTemplate.Substring(pos + "<!-- BeginSQL ".Length, posAfterName - (pos + "<!-- BeginSQL ".Length)).Trim();
                int posAfterSQL = FHTMLTemplate.IndexOf("<!-- EndSQL", pos);
                string sql = FHTMLTemplate.Substring(posAfterName + "-->".Length, posAfterSQL - (posAfterName + "-->".Length)).
                                          Trim().
                                          Replace("&gt;", ">").
                                          Replace("&lt;", "<");

                FSQLQueries.Add(name, sql);

                // remove sql from template
                FHTMLTemplate = FHTMLTemplate.Substring(0, pos) +
                    FHTMLTemplate.Substring(FHTMLTemplate.IndexOf("-->", posAfterSQL)+"-->".Length);
                pos = FHTMLTemplate.IndexOf("<!-- BeginSQL");
            }
        }

        /// <summary>
        /// Gets the SQL query.
        /// </summary>
        public string GetSQLQuery(string queryname)
        {
            string sql = FSQLQueries[queryname];

            sql = ProcessIfDefs(sql);

            // TODO: use prepared statements to pass parameters
            sql = InsertParameters("{{", "}}", sql, ReplaceOptions.SqlParameters);
            sql = InsertParameters("{LISTCMP ", "}", sql, ReplaceOptions.SqlParameters);
            sql = InsertParameters("{LIST ", "}", sql, ReplaceOptions.SqlParameters);
            sql = InsertParameters("{", "}", sql, ReplaceOptions.SqlParameters);

            return sql;
        }

        /// <summary>
        ///  the processed HTML
        /// </summary>
        public HtmlDocument GetHTML()
        {
            return FHTMLDocument;
        }

        /// <summary>
        /// Selects the nodes.
        /// </summary>
        public static HtmlNodeCollection SelectNodes(HtmlNode node, string xpath)
        {
            // important: use a dot at the beginning of xpath if you want to limit the search to node.
            var result = node.SelectNodes(xpath);

            if (result == null)
            {
                // return empty collection
                result = new HtmlNodeCollection(node);
            }

            return result;
        }

        /// <summary>
        /// Selects the first matching node.
        /// </summary>
        public static HtmlNode SelectSingleNode(HtmlNode node, string xpath)
        {
            var result = node.SelectSingleNode(xpath);

            if (result == null)
            {
                throw new Exception("SelectSingleNode: cannot find " + xpath);
            }

            return result;
        }

        /// how to replace values
        public enum ReplaceOptions {
            /// no quotes
            NoQuotes,
            /// use quotes for strings
            QuotesForStrings,
            /// use sql parameters
            SqlParameters };

        /// <summary>
        /// Inserts the parameters into a template
        /// </summary>
        public string InsertParameters(string searchOpen, string searchClose, string template, ReplaceOptions options)
        {
            int bracket = template.IndexOf(searchOpen);
            int prevBracket = bracket;

            while (bracket != -1)
            {
                int firstRealChar = bracket + searchOpen.Length;
                int paramEndIdx = template.IndexOf(searchClose, firstRealChar);

                if (paramEndIdx <= 0)
                {
                    // missing closing bracket; can happen with e.g. #testdate; should be #testdate#
                    if (template.Length > bracket + 20)
                    {
                        throw new Exception("Cannot find closing bracket " + searchClose + " for " + template.Substring(bracket, 20));
                    }
                    else
                    {
                        throw new Exception("Cannot find closing bracket " + searchClose + " for " + template.Substring(bracket));
                    }
                }

                String parameter = template.Substring(firstRealChar, paramEndIdx - firstRealChar);
                string placeHolder = searchOpen + parameter + searchClose;
                string otherValue = string.Empty;

                if (parameter.Contains(" "))
                {
                    string[] split = parameter.Split(new char[] {' '});
                    parameter = split[0];
                    // for LISTCMP
                    otherValue = split[1];
                }
                bool ParameterExists = false;
                TVariant newvalue;
                if (FParameters != null)
                {
                    newvalue = FParameters.Get(parameter, -1, -1, eParameterFit.eBestFitEvenLowerLevel);

                    ParameterExists = (newvalue.TypeVariant != eVariantTypes.eEmpty);
                }
                else
                {
                    newvalue = new TVariant();
                }

                if (!ParameterExists)
                {
                    // if date is given, use the parameter itself
                    if ((parameter[0] >= '0') && (parameter[0] <= '9'))
                    {
                        newvalue = new TVariant(parameter);
                    }
                    else
                    {
                        int CountWarning = 1;

                        // do not print warning too many times for the same variable
                        if (!VariablesNotFound.ContainsKey(parameter))
                        {
                            VariablesNotFound.Add(parameter, 1);
                        }
                        else
                        {
                            VariablesNotFound[parameter] = VariablesNotFound[parameter] + 1;
                            CountWarning = VariablesNotFound[parameter];
                        }

                        if (CountWarning < 5)
                        {
                            // this can be alright, for empty values; for example method of giving can be empty; for report GiftTransactions
                            TLogging.Log(
                                "Variable " + parameter + " empty or not found");
                        }
                        else if (CountWarning % 20 == 0)
                        {
                            TLogging.Log("20 times: Variable " + parameter + " empty or not found.");
                        }
                    }
                }

                try
                {
                    if (newvalue.TypeVariant == eVariantTypes.eDateTime)
                    {
                        // remove the time from the timestamp, only use the date at 0:00
                        DateTime date = newvalue.ToDate();
                        newvalue = new TVariant(new DateTime(date.Year, date.Month, date.Day));
                    }

                    string strValue = newvalue.ToString();
                    string Quotes = (options == ReplaceOptions.NoQuotes ? "" : "'");
                    if (searchOpen == "{LIST ")
                    {
                        string[] elements = newvalue.ToString().Split(new char[] { ',' });
                        strValue = "(";
                        foreach (string element in elements)
                        {
                            if (strValue.Length > 1)
                            {
                                strValue += ",";
                            }
                            strValue += Quotes + element + Quotes;
                        }
                        strValue += ")";
                    }
                    else if (searchOpen == "{LISTCMP ")
                    {
                        string[] elements = newvalue.ToString().Split(new char[] { ',' });
                        strValue = "(";
                        foreach (string element in elements)
                        {
                            if (strValue.Length > 1)
                            {
                                strValue += " AND ";
                            }
                            strValue += Quotes + element + Quotes + " = " + otherValue;
                        }
                        strValue += ")";
                    }
                    else if (newvalue.TypeVariant == eVariantTypes.eDateTime)
                    {
                        strValue = Quotes + newvalue.ToDate().ToString("yyyy-MM-dd") + Quotes;
                    }
                    else if ((searchOpen != "{{") && 
                             !(parameter.Length > 2 && parameter.Substring(parameter.Length - 2) == "_i") &&
                             (newvalue.TypeVariant == eVariantTypes.eString))
                    {
                        if (newvalue.ToString() != "*NOTUSED*")
                        {
                            strValue = Quotes + newvalue.ToString().Replace('*', '%') + Quotes;
                        }
                        else
                        {
                            strValue = Quotes + newvalue.ToString() + Quotes;
                        }
                    }
                    else if (
                        (newvalue.TypeVariant == eVariantTypes.eCurrency) ||
                        ((newvalue.TypeVariant == eVariantTypes.eDecimal) && (Quotes.Length == 0)))
                    {
                        strValue = newvalue.ToDecimal().ToString("0.00");
                    }
                    template = template.Replace(placeHolder, strValue);
                }
                catch (Exception e)
                {
                    throw new Exception(
                        "While trying to format parameter " + parameter + ", there was a problem with formatting." + Environment.NewLine + e.Message);
                }

                prevBracket = bracket;
                bracket = template.IndexOf(searchOpen);
                if ((prevBracket >= bracket) && (bracket >= 0)) 
                {
                    TLogging.Log("endless loop in InsertParameters at pos " + bracket.ToString());
                    TLogging.Log("after replacing " + placeHolder);
                    TLogging.Log(template);
                    throw new Exception("Problem in InsertParameters, endless loop");
                }
            } // while

            return template;
        }

        string ProcessIfDefs(string s)
        {
            int posPlaceholder = s.IndexOf("#if ");

            // to avoid issues with ifdefs at the end
            s += "\n";

            while (posPlaceholder > -1)
            {
                string condition = s.Substring(posPlaceholder + "#if ".Length, s.IndexOf("\n", posPlaceholder) - posPlaceholder - "#if ".Length);
                condition = InsertParameters("{{", "}}", condition, ReplaceOptions.QuotesForStrings);
                condition = InsertParameters("{", "}", condition, ReplaceOptions.QuotesForStrings);

                // TODO: support nested ifdefs???
                int posPlaceholderAfter = s.IndexOf("#endif", posPlaceholder);

                if (posPlaceholderAfter == -1)
                {
                    throw new Exception("The template has a bug: " +
                        "We are missing and #endif");
                }

                if ((condition == "") || (condition == "''") || (condition == "0") || (condition == "'*NOTUSED*'") || (condition == "false"))
                {
                    // drop the content of the ifdef section
                    s = s.Substring(0, posPlaceholder) + s.Substring(s.IndexOf("\n", posPlaceholderAfter) + 1);
                }
                else
                {
                    s = s.Substring(0, posPlaceholder) +
                         s.Substring(s.IndexOf("\n", posPlaceholder) + 1, posPlaceholderAfter - s.IndexOf("\n", posPlaceholder) - 1) +
                         s.Substring(s.IndexOf("\n", posPlaceholderAfter) + 1);
                }

                posPlaceholder = s.IndexOf("#if ");
            }

            return s;
        }

        string EvaluateVisible(string template)
        {
            int visiblePos = template.IndexOf("visible=");

            while (visiblePos != -1)
            {
                int firstRealChar = visiblePos + "visible='".Length;
                int paramEndIdx = template.IndexOf('"', firstRealChar);
                string hidden = "style='visibility:hidden'";
                // TODO: evaluate the condition, eg. with jint
                template = template.Replace(template.Substring(visiblePos, paramEndIdx - visiblePos + 1), hidden);
                visiblePos = template.IndexOf("visible=");
            }
            return template;
        }

        /// print simple reports, from one table using an HTML template file
        public static HtmlDocument Table2Html(DataTable ATable, string ATemplateFilename, string AReportLanguage)
        {
            String PathCustomReports = TAppSettingsManager.GetValue("Reporting.PathCustomReports");

            if (File.Exists(PathCustomReports + Path.DirectorySeparatorChar + ATemplateFilename))
            {
                ATemplateFilename = PathCustomReports + Path.DirectorySeparatorChar + ATemplateFilename;
            }
            else
            {
                String PathStandardReports = TAppSettingsManager.GetValue("Reporting.PathStandardReports");
                ATemplateFilename = PathStandardReports + Path.DirectorySeparatorChar + ATemplateFilename;
            }

            if (!File.Exists(ATemplateFilename))
            {
                throw new Exception("Table2Html: Cannot find file " + ATemplateFilename);
            }
    
            HtmlDocument HTMLDocument = new HtmlDocument();
            string TemplateText = String.Empty;

            using (StreamReader sr = new StreamReader(ATemplateFilename))
            {
                TemplateText = sr.ReadToEnd();
            }

            // TODO: #592: Use i18next on C# server side for translating labels on reports
            // TemplateText = I18N.Translate(TemplateText, AReportLanguage);
            
            HTMLDocument.LoadHtml(TemplateText);

            var RowTemplate = HTMLDocument.DocumentNode.SelectSingleNode("//div[@id='child_template']");

            if (RowTemplate == null)
            {
                throw new Exception("Table2Html: cannot find node with id child_template");
            }

            var ParentNode = RowTemplate.ParentNode;

            int countRow = 0;
            foreach (DataRow row in ATable.Rows)
            {
                var NewHTMLRow = RowTemplate.Clone();
                string RowId = "row" + countRow.ToString();
                NewHTMLRow.SetAttributeValue("id", RowId);

                string InnerHtml = NewHTMLRow.InnerHtml;
                foreach (DataColumn c in ATable.Columns)
                {
                    InnerHtml = InnerHtml.Replace("{" + c.ColumnName + "}", row[c.ColumnName].ToString());
                }

                // fix empty email column
                InnerHtml = InnerHtml.Replace(">,</div>", "></div>");

                NewHTMLRow.InnerHtml = InnerHtml;

                ParentNode.AppendChild(NewHTMLRow);

                countRow++;
            }

            RowTemplate.Remove();

            return HTMLDocument;
        }

        /// <summary>
        /// Create an Excel file from the HTML
        /// </summary>
        public static XSSFWorkbook HTMLToCalc(HtmlDocument html)
        {
            XSSFWorkbook xssWorkbook = new XSSFWorkbook();
            IRow wsrow = null;
            ICell wscell = null;

            ISheet worksheet = xssWorkbook.CreateSheet("Data Export");

            ICellStyle wsstyle_bold = xssWorkbook.CreateCellStyle();
            IFont wsfont = wsstyle_bold.GetFont(xssWorkbook);
            wsfont.IsBold = true;
            wsstyle_bold.SetFont(wsfont);

            ICellStyle wsstyle_dateformat = xssWorkbook.CreateCellStyle();
            ICreationHelper createHelper = xssWorkbook.GetCreationHelper();
            wsstyle_dateformat.DataFormat = createHelper.CreateDataFormat().GetFormat("dd/mm/yyyy");

            // write the column headings
            var elements = HTMLTemplateProcessor.SelectNodes(html.DocumentNode, "//div[@id='column_headings']/div");
            int colCounter = 0;
            int rowCounter = 0;
            wsrow = worksheet.CreateRow(rowCounter);
            foreach (var element in elements)
            {
                wscell = wsrow.CreateCell(colCounter);
                wscell.SetCellValue(element.InnerText);
                wscell.CellStyle = wsstyle_bold;
                colCounter++;
            }
            rowCounter+=1;

            var rows = HTMLTemplateProcessor.SelectNodes(html.DocumentNode, "//div[@id='content']//div[contains(@class, 'row')]");
            foreach (var row in rows)
            {
                wsrow = worksheet.CreateRow(rowCounter);
                colCounter = 0;
                elements = HTMLTemplateProcessor.SelectNodes(row, ".//div[contains(@class, 'col-')]");
                foreach (var element in elements)
                {
                    wscell = wsrow.CreateCell(colCounter);
                    string value = element.InnerText;

                    if (value == "&nbsp;")
                    {
                        value = String.Empty;
                    }

                    if (element.HasClass("currency"))
                    {
                        TVariant v = new TVariant(value);
                        wscell.SetCellValue((double)v.ToDecimal());
                    }
                    else if (element.HasClass("date"))
                    {
                        TVariant v = new TVariant(value);
                        wscell.SetCellValue(v.ToDate());
                        wscell.CellStyle = wsstyle_dateformat;
                    }
                    else
                    {
                        wscell.SetCellValue(value);
                    }

                    if (element.InnerHtml.Contains("<strong>"))
                    {
                        wscell.CellStyle = wsstyle_bold;
                    }

                    colCounter++;
                }

                rowCounter++;
            }

            for (int colIndex = 1; colIndex < colCounter; colIndex++)
            {
                worksheet.AutoSizeColumn(colIndex);
            }

            return xssWorkbook;
        }

    }
}
