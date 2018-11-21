//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2018 by OM International
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
using Ict.Petra.Shared.MReporting;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using System.IO;
using OfficeOpenXml;
using HtmlAgilityPack;

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
                string sql = FHTMLTemplate.Substring(posAfterName + "-->".Length, posAfterSQL - (posAfterName + "-->".Length)).Trim();
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

        public enum ReplaceOptions { NoQuotes, QuotesForStrings, SqlParameters };

        public string InsertParameters(string searchOpen, string searchClose, string template, ReplaceOptions options)
        {
            int bracket = template.IndexOf(searchOpen);

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
                        strValue = String.Empty;
                        foreach (string element in elements)
                        {
                            if (strValue.Length > 0)
                            {
                                strValue += ",";
                            }
                            strValue += Quotes + element + Quotes;
                        }
                    }
                    else if (newvalue.TypeVariant == eVariantTypes.eDateTime)
                    {
                        strValue = Quotes + newvalue.ToDate().ToString("yyyy-MM-dd") + Quotes;
                    }
                    else if ((searchOpen != "{{") && 
                             !(parameter.Length > 2 && parameter.Substring(parameter.Length - 2) == "_i") &&
                             (newvalue.TypeVariant == eVariantTypes.eString))
                    {
                        strValue = Quotes + newvalue.ToString() + Quotes;
                    }
                    else if (
                        (newvalue.TypeVariant == eVariantTypes.eCurrency) ||
                        ((newvalue.TypeVariant == eVariantTypes.eDecimal) && (Quotes.Length == 0)))
                    {
                        strValue = newvalue.ToDecimal().ToString("0.00");
                    }
                    template = template.Replace(searchOpen + parameter + searchClose, strValue);
                }
                catch (Exception e)
                {
                    throw new Exception(
                        "While trying to format parameter " + parameter + ", there was a problem with formatting." + Environment.NewLine + e.Message);
                }

                bracket = template.IndexOf(searchOpen);
            } // while

            return template;
        }

        string ProcessIfDefs(string s)
        {
            int posPlaceholder = s.IndexOf("#ifdef ");

            // to avoid issues with ifdefs at the end
            s += "\n";

            while (posPlaceholder > -1)
            {
                string condition = s.Substring(posPlaceholder + "#ifdef ".Length, s.IndexOf("\n", posPlaceholder) - posPlaceholder - "#ifdef ".Length);
                condition = InsertParameters("{{", "}}", condition, ReplaceOptions.QuotesForStrings);
                condition = InsertParameters("{", "}", condition, ReplaceOptions.QuotesForStrings);

                // TODO: support nested ifdefs???
                int posPlaceholderAfter = s.IndexOf("#endif", posPlaceholder);

                if (posPlaceholderAfter == -1)
                {
                    throw new Exception("The template has a bug: " +
                        "We are missing and #endif");
                }

                if ((condition == "") || (condition == "''") || (condition == "0") || (condition == "'*NOTUSED*'"))
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

                posPlaceholder = s.IndexOf("#ifdef ");
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

        /// <summary>
        /// Create a Calc file from the HTML
        /// </summary>
        public static ExcelPackage HTMLToCalc(HtmlDocument html)
        {
            ExcelPackage pck = new ExcelPackage();

            ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Data Export");

            // write the column headings
            var elements = HTMLTemplateProcessor.SelectNodes(html.DocumentNode, "//div[@id='column_headings']/div");
            int colCounter = 1;
            int rowCounter = 3;
            foreach (var element in elements)
            {
                worksheet.Cells[rowCounter, colCounter].Value = element.InnerText;
                worksheet.Cells[rowCounter, colCounter].Style.Font.Bold = true;
                colCounter++;
            }
            rowCounter+=2;

            var rows = HTMLTemplateProcessor.SelectNodes(html.DocumentNode, "//div[@id='content']//div[contains(@class, 'row')]");
            foreach (var row in rows)
            {
                colCounter = 1;
                elements = HTMLTemplateProcessor.SelectNodes(row, ".//div[contains(@class, 'col-')]");
                foreach (var element in elements)
                {
                    string value = element.InnerText;

                    if (value == "&nbsp;")
                    {
                        value = String.Empty;
                    }

                    if (element.HasClass("currency"))
                    {
                        TVariant v = new TVariant(value);
                        worksheet.Cells[rowCounter, colCounter].Value = v.ToDecimal();
                    }
                    else if (element.HasClass("date"))
                    {
                        TVariant v = new TVariant(value);
                        worksheet.Cells[rowCounter, colCounter].Value = v.ToDate();
                        worksheet.Cells[rowCounter, colCounter].Style.Numberformat.Format = "dd/mm/yyyy";
                    }
                    else
                    {
                        worksheet.Cells[rowCounter, colCounter].Value = value;
                    }

                    if (element.InnerHtml.Contains("<strong>"))
                    {
                        worksheet.Cells[rowCounter, colCounter].Style.Font.Bold = true;
                    }

                    colCounter++;
                }

                rowCounter++;
            }

            worksheet.Cells.AutoFitColumns();

            return pck;
        }
    }
}
