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
using System.Collections.Generic;
using System.Drawing;
using Ict.Common.IO;

namespace Ict.Common.Printing
{
    /// <summary>
    /// a class that renders HTML, ie. prints HTML to Screen or PDF
    /// </summary>
    public class TPrinterHtml : TPrinterLayout
    {
        /// <summary>todoComment</summary>
        protected TPrinter FPrinter;

        /// <summary>todoComment</summary>
        protected XmlDocument FHtmlDoc;

        /// <summary>todoComment</summary>
        protected XmlNode FCurrentNodeNextPage;

        /// <summary>todoComment</summary>
        protected string FPath;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AHtmlDocument"></param>
        /// <param name="APath">is required for embedded images</param>
        /// <param name="APrinter"></param>
        public TPrinterHtml(string AHtmlDocument, string APath, TPrinter APrinter)
        {
            FPrinter = APrinter;
            FPath = APath;
            FHtmlDoc = ParseHtml(AHtmlDocument);
            FCurrentNodeNextPage = InitHtmlParser(FHtmlDoc);
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AHtmlFile"></param>
        /// <param name="APrinter"></param>
        public TPrinterHtml(string AHtmlFile, TPrinter APrinter)
        {
            FPrinter = APrinter;
            FPath = System.IO.Path.GetDirectoryName(AHtmlFile);
            StreamReader sr = new StreamReader(AHtmlFile, System.Text.Encoding.Default);
            string htmlDocument = sr.ReadToEnd();
            sr.Close();
            FHtmlDoc = ParseHtml(htmlDocument);
            FCurrentNodeNextPage = InitHtmlParser(FHtmlDoc);
        }

        /// try to parse HTML document
        protected XmlDocument ParseHtml(string AHtmlDocument)
        {
            XmlDocument result;

            // TODO should we first preparse and make sure it is proper XHTML?
            // eg. img needs to have closing tag etc
            // see http://www.majestic12.co.uk/projects/html_parser.php
            try
            {
                result = new XmlDocument();

                if (AHtmlDocument.Trim().StartsWith("<!DOCTYPE"))
                {
                    AHtmlDocument = AHtmlDocument.Substring(AHtmlDocument.IndexOf(">") + 1);
                }

                AHtmlDocument = "<?xml version='1.0' encoding='UTF-16'?>" + Environment.NewLine + AHtmlDocument;
                AHtmlDocument = AHtmlDocument.Replace("<br>", "<br/>");
                AHtmlDocument = AHtmlDocument.Replace("&amp;", "&amp;amp;");
                AHtmlDocument = AHtmlDocument.Replace("&nbsp;", "&amp;nbsp;");
                AHtmlDocument = AHtmlDocument.Replace("&gt;", "&amp;gt;");
                AHtmlDocument = AHtmlDocument.Replace("&lt;", "&amp;lt;");
                result.LoadXml(AHtmlDocument);
            }
            catch (Exception e)
            {
                TLogging.Log("error parsing HTML text: " + Environment.NewLine + e.Message + Environment.NewLine + AHtmlDocument);
                throw new Exception("Error while parsing HTML file " +
                    Environment.NewLine + "(see log file for details: " + TLogging.GetLogFileName() +
                    ")" + Environment.NewLine + e.Message);
            }

            if (result == null)
            {
                return result;
            }

            return result;
        }

        private XmlNode InitHtmlParser(XmlDocument doc)
        {
            if (doc == null)
            {
                return null;
            }

            XmlNode result = doc.FirstChild; // should be <html>

            while (result != null && result.Name != "html")
            {
                result = result.NextSibling;
            }

            if (result != null)
            {
                result = result.FirstChild; // should be head, body etc
            }

            while (result != null && result.Name != "body")
            {
                result = result.NextSibling;
            }

            if ((result != null) && (result.Name == "body"))
            {
                result = result.FirstChild;
            }
            else
            {
                throw new Exception("cannot find the body tag");
            }

            return result;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public override void StartPrintDocument()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public override void PrintPageHeader()
        {
        }

        /// <summary>
        /// print one page of the HTML, until div with page break comes up
        /// </summary>
        /// <returns>void</returns>
        public override void PrintPageBody()
        {
            if (FCurrentNodeNextPage == null)
            {
                return;
            }

            if (FPrinter is TGfxPrinter)
            {
                TGfxPrinter printer = (TGfxPrinter)FPrinter;

                // todo: paper selection? source tray?
                // Paper Size Width is inch/100
                //printer.Inch2Twips(printer.Document.PrinterSettings.PaperSizes[0].Width*100);
                //float edge = printer.Cm2Twips(1.5f);
                printer.CurrentXPos = printer.LeftMargin;

                // TODO set the margins and the font sizes in the HTML file???
                printer.FDefaultFont = new System.Drawing.Font("Arial", 12);
                printer.FDefaultBoldFont = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                RenderContent(printer.LeftMargin + TGfxPrinter.Cm2Inch(1), printer.Width - TGfxPrinter.Cm2Inch(2), ref FCurrentNodeNextPage);
            }
            else
            {
                // todo: do something for e.g. PDF printing
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public override void PrintPageFooter()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sOrig"></param>
        /// <returns></returns>
        protected string HtmlToText(string sOrig)
        {
            string s = sOrig;

            // remove normal spaces, line breaks
            s = s.Trim();
            s = s.Replace("\t", " ");

            while (s.Contains("  "))
            {
                s = s.Replace("  ", " ");
            }

            s = s.Replace("\r", "");
            s = s.Replace("\n", "");

            // replace special codes
            s = s.Replace("&nbsp;", "  ");
            s = s.Replace("&gt;", "<");
            s = s.Replace("&lt;", ">");
            s = s.Replace("&amp;", "&");

            // other special characters? e.g. &uuml; etc
            // solution: use UTF-8 in the text editor when editing the template

            return s;
        }

        /// <summary>
        /// interpret HTML code
        /// </summary>
        /// <param name="AXPos">the X position to start the content</param>
        /// <param name="AWidthAvailable">AWidthAvailable</param>
        /// <param name="curNode"></param>
        /// <returns>s the height of the content</returns>
        public override float RenderContent(float AXPos,
            float AWidthAvailable,
            ref XmlNode curNode)
        {
            float oldYPos = FPrinter.CurrentYPos;

            while (curNode != null)
            {
                if (curNode.Name == "table")
                {
                    PrintTable(AXPos, AWidthAvailable, ref curNode);
                }
                else if (curNode.Name == "img")
                {
                    // insert image
                    string src = TXMLParser.GetAttribute(curNode, "src");
                    src = System.IO.Path.Combine(FPath, src);

                    // todo: embed image into text flow
                    FPrinter.LineFeed();

                    if (TXMLParser.HasAttribute(curNode, "width") && TXMLParser.HasAttribute(curNode, "height"))
                    {
                        float WidthPercentage = 1.0f;
                        float HeightPercentage = 1.0f;

                        string Width = TXMLParser.GetAttribute(curNode, "width");
                        string Height = TXMLParser.GetAttribute(curNode, "height");

                        if (Width.EndsWith("%"))
                        {
                            WidthPercentage = (float)Convert.ToDouble(Width.Substring(0, Width.Length - 1)) / 100.0f;
                        }

                        if (Height.EndsWith("%"))
                        {
                            HeightPercentage = (float)Convert.ToDouble(Height.Substring(0, Width.Length - 1)) / 100.0f;
                        }

                        FPrinter.DrawBitmap(src, FPrinter.CurrentXPos, FPrinter.CurrentYPos, WidthPercentage, HeightPercentage);
                    }
                    else
                    {
                        FPrinter.DrawBitmap(src, FPrinter.CurrentXPos, FPrinter.CurrentYPos);
                    }

                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "font")
                {
                    // todo change font name and/or size
                    // FCurrentFont
                    // recursively call RenderContent
                    // reset FCurrentFont to backed up font
                    XmlNode child = curNode.FirstChild;
                    RenderContent(AXPos, AWidthAvailable, ref child);
                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "b")
                {
                    // bold
                    eFont previousFont = FPrinter.CurrentFont;
                    FPrinter.CurrentFont = eFont.eDefaultBoldFont;
                    XmlNode child = curNode.FirstChild;
                    RenderContent(AXPos, AWidthAvailable, ref child);
                    FPrinter.CurrentFont = previousFont;
                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "i")
                {
                    // todo italic; similar to bold
                    XmlNode child = curNode.FirstChild;
                    RenderContent(AXPos, AWidthAvailable, ref child);
                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "br")
                {
                    // line break
                    FPrinter.LineFeed();
                    FPrinter.CurrentXPos = AXPos;
                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "p")
                {
                    eAlignment origAlignment = FPrinter.CurrentAlignment;

                    if (TXMLParser.GetAttribute(curNode, "align") == "right")
                    {
                        FPrinter.CurrentAlignment = eAlignment.eRight;
                    }
                    else if (TXMLParser.GetAttribute(curNode, "align") == "center")
                    {
                        FPrinter.CurrentAlignment = eAlignment.eCenter;
                    }
                    else if (TXMLParser.GetAttribute(curNode, "align") == "left")
                    {
                        FPrinter.CurrentAlignment = eAlignment.eLeft;
                    }

                    XmlNode child = curNode.FirstChild;
                    FPrinter.LineFeed();
                    FPrinter.CurrentXPos = AXPos;
                    RenderContent(AXPos, AWidthAvailable, ref child);
                    FPrinter.LineFeed();
                    FPrinter.CurrentXPos = AXPos;
                    curNode = curNode.NextSibling;
                    FPrinter.CurrentAlignment = origAlignment;
                }
                // unrecognised HTML element, text
                else if (curNode.InnerText.Length > 0)
                {
                    if (curNode.InnerText.Contains("nbsp"))
                    {
                        TLogging.Log(HtmlToText(curNode.InnerText) + "testnbsp");
                    }

                    string toPrint = HtmlToText(curNode.InnerText);

                    // continues text in the same row
                    FPrinter.PrintStringWrap(toPrint, FPrinter.CurrentFont, AXPos, AWidthAvailable, FPrinter.CurrentAlignment);
                    curNode = curNode.NextSibling;
                }

                // todo: h1, etc headings???
                // todo: code, fixed width font (for currency amounts?) ???
                // todo: don't print to paper if class="preprinted"; but is printed for PDF
                // todo: checked and unchecked checkbox
                // todo: page break
                // todo: header div style with tray information; config file with local tray names???
            }

            return FPrinter.CurrentYPos - oldYPos;
        }

        /// <summary>
        /// print an html table
        /// </summary>
        /// <returns>the height of the table</returns>
        protected float PrintTable(float AXPos,
            float AWidthAvailable,
            ref XmlNode curNode)
        {
            XmlNode tableNode = curNode;
            float oldYPos = FPrinter.CurrentYPos;

            FPrinter.LineFeed();
            FPrinter.CurrentXPos = AXPos;

            int border = 1;

            if (TXMLParser.HasAttribute(tableNode, "border"))
            {
                border = Convert.ToInt32(TXMLParser.GetAttribute(tableNode, "border"));
            }

            if (TXMLParser.HasAttribute(tableNode, "width"))
            {
                string width = TXMLParser.GetAttribute(tableNode, "width");

                if (width.EndsWith("%"))
                {
                    AWidthAvailable *= (float)Convert.ToDouble(width.Substring(0, width.Length - 1)) / 100.0f;
                }
            }

            // todo: read border value from table attributes
            // todo: read table width from table styles
            curNode = curNode.FirstChild; // should be tbody, or tr

            if (curNode.Name == "tbody")
            {
                curNode = curNode.FirstChild;
            }

            List <TTableRowGfx>preparedRows = new List <TTableRowGfx>();

            while (curNode != null && curNode.Name == "tr")
            {
                TTableRowGfx preparedRow = new TTableRowGfx();
                preparedRow.cells = new List <TTableCellGfx>();
                XmlNode row = curNode;
                XmlNode cell = curNode.FirstChild;

                while (cell != null && (cell.Name == "td" || cell.Name == "th"))
                {
                    TTableCellGfx preparedCell = new TTableCellGfx();
                    preparedCell.borderWidth = border;
                    preparedCell.content = cell.FirstChild;
                    preparedCell.bold = (cell.Name == "th");

                    if (TXMLParser.GetAttribute(cell, "align") == "right")
                    {
                        preparedCell.align = eAlignment.eRight;
                    }
                    else if ((TXMLParser.GetAttribute(cell, "align") == "center") || (cell.Name == "th"))
                    {
                        preparedCell.align = eAlignment.eCenter;
                    }

                    // todo set preparedCell.columnWidthInPercentage
                    preparedCell.columnWidthInPercentage = -1;
                    preparedRow.cells.Add(preparedCell);
                    cell = cell.NextSibling;
                }

                // make sure the percentages are right;
                // todo: what if only some columns have a percentage?
                foreach (TTableCellGfx preparedCell in preparedRow.cells)
                {
                    if (preparedCell.columnWidthInPercentage == -1)
                    {
                        preparedCell.columnWidthInPercentage = 100.0f / preparedRow.cells.Count;
                    }
                }

                preparedRows.Add(preparedRow);
                curNode = row.NextSibling;
            }

            FPrinter.PrintTable(AXPos, AWidthAvailable, preparedRows);

            curNode = tableNode.NextSibling;
            return FPrinter.CurrentYPos - oldYPos;
        }
    }
}