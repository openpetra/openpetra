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
        protected eAlignment FCurrentAlignment;

        /// <summary>todoComment</summary>
        protected eFont FCurrentFont;

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
            FCurrentFont = eFont.eDefaultFont;
            FCurrentAlignment = eAlignment.eLeft;
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
            FCurrentFont = eFont.eDefaultFont;
            FCurrentAlignment = eAlignment.eLeft;
            FCurrentNodeNextPage = InitHtmlParser(FHtmlDoc);
        }

        /// try to parse HTML document
        protected XmlDocument ParseHtml(string AHtmlDocument)
        {
            XmlDocument result;

            // todo should we first preparse and make sure it is proper XHTML?
            // eg. img needs to have closing tag etc
            // see http://www.majestic12.co.uk/projects/html_parser.php
            try
            {
                result = new XmlDocument();
                result.LoadXml(AHtmlDocument);
            }
            catch (Exception e)
            {
                TLogging.Log("error parsing HTML text: " + e.Message);
                result = null;
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
                result = null;
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
                RenderContent(printer.LeftMargin, printer.Width, ref FCurrentNodeNextPage);
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
            s = s.Replace("&nbsp;", " ");
            s = s.Replace("&gt;", "<");
            s = s.Replace("&lt;", ">");

            // todo: other special characters? e.g. &uuml; etc
            s = s.Replace("&amp;", "&");
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
                    FPrinter.DrawBitmap(src, FPrinter.CurrentXPos, FPrinter.CurrentYPos);

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
                    eFont previousFont = FCurrentFont;
                    FCurrentFont = eFont.eDefaultBoldFont;
                    XmlNode child = curNode.FirstChild;
                    RenderContent(AXPos, AWidthAvailable, ref child);
                    FCurrentFont = previousFont;
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
                // unrecognised HTML element, text
                else if (curNode.InnerText.Length > 0)
                {
                    string toPrint = HtmlToText(curNode.InnerText);

                    // continues text in the same row
                    FPrinter.PrintStringWrap(toPrint, FCurrentFont, AXPos, AWidthAvailable, FCurrentAlignment);
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

                while (cell != null && cell.Name == "td")
                {
                    TTableCellGfx preparedCell = new TTableCellGfx();
                    preparedCell.borderWidth = 1;
                    preparedCell.content = cell.FirstChild;

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