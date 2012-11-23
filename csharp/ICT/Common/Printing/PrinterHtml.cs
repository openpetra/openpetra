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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
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

        /// <summary>path for embedded images</summary>
        protected string FPath;

        /// local version used for preprinting HTML to discover total number of pages
        protected bool FHasMorePages = false;

        private string FPageHeader = string.Empty;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AHtmlDocument"></param>
        /// <param name="APath">Used for embedding images</param>
        /// <param name="APrinter"></param>
        public TPrinterHtml(string AHtmlDocument, string APath, TPrinter APrinter)
        {
            FPrinter = APrinter;
            FPath = APath;

            if (AHtmlDocument != "")
            {
                AHtmlDocument = AHtmlDocument.Replace("<pagebreak/>", "</body><body>");
                AHtmlDocument = AHtmlDocument.Replace("<pagebreak>", "</body><body>");
                AHtmlDocument = RemoveElement(AHtmlDocument, "div", "class", "PageHeader", out FPageHeader);

                FHtmlDoc = ParseHtml(AHtmlDocument);
            }
            else
            {
                FHtmlDoc = new XmlDocument();
            }
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
            htmlDocument = RemoveElement(htmlDocument, "div", "class", "PageHeader", out FPageHeader);
            FHtmlDoc = ParseHtml(htmlDocument);
        }

        /// try to parse HTML document
        public static XmlDocument ParseHtml(string AHtmlDocument)
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

                if (!AHtmlDocument.StartsWith("<?xml version="))
                {
                    AHtmlDocument = "<?xml version='1.0' encoding='UTF-8'?>" + Environment.NewLine + AHtmlDocument;
                }

                AHtmlDocument = AHtmlDocument.Replace("<br>", "<br/>");
                AHtmlDocument = AHtmlDocument.Replace("&", "&amp;");

                result.LoadXml(AHtmlDocument);
            }
            catch (Exception e)
            {
                TLogging.Log("error parsing HTML text: " + Environment.NewLine + e.Message + Environment.NewLine + AHtmlDocument);
                throw new Exception("Error while parsing HTML file " +
                    Environment.NewLine + "(see log file for details: " + TLogging.GetLogFileName() +
                    ")" + Environment.NewLine + e.Message);
            }

            return result;
        }

        /// <summary>
        /// get the page size defined in the css styles of body
        /// </summary>
        /// <param name="APaperKind"></param>
        /// <param name="AMargins"></param>
        /// <param name="AWidthInPoint"></param>
        /// <param name="AHeightInPoint"></param>
        /// <returns></returns>
        public override bool GetPageSize(out PaperKind APaperKind, out Margins AMargins, out float AWidthInPoint, out float AHeightInPoint)
        {
            bool Result = false;
            XmlNode BodyNode = TXMLParser.FindNodeRecursive(FHtmlDoc.DocumentElement, "body");

            APaperKind = PaperKind.A4;
            AMargins = new Margins(20, 20, 20, 39);
            AWidthInPoint = -1;
            AHeightInPoint = -1;

            if (TXMLParser.HasAttribute(BodyNode, "style"))
            {
                Dictionary <string, string>Styles = TFormLettersTools.GetStyles(BodyNode);

                foreach (string StyleName in Styles.Keys)
                {
                    if (StyleName == "margin")
                    {
                        Result = true;

                        // TODO support more than just 0px
                        if (Styles[StyleName] == "0px")
                        {
                            AMargins = new Margins(0, 0, 0, 0);
                        }
                    }
                    else if (StyleName == "size")
                    {
                        // http://www.w3.org/TR/css3-page/#page-size
                        // currently support: either a name of PaperKind enum, or width and height in inches or cm
                        // eg size: 8.5in 11in;
                        // eg size: A4;
                        Result = true;

                        PaperKind FoundPaperKind = PaperKind.Custom;

                        foreach (PaperKind MyPaperKind in Enum.GetValues(typeof(PaperKind)))
                        {
                            if (MyPaperKind.ToString() == Styles[StyleName])
                            {
                                FoundPaperKind = MyPaperKind;
                                break;
                            }
                        }

                        if (FoundPaperKind != PaperKind.Custom)
                        {
                            APaperKind = FoundPaperKind;
                        }
                        else
                        {
                            string[] dimensions = Styles[StyleName].Trim().Split(' ');

                            if (dimensions.Length != 2)
                            {
                                TLogging.Log("body styles must contain 2 numbers for page size: " + Styles[StyleName]);
                                TLogging.Log(Styles[StyleName]);
                                return false;
                            }

                            dimensions[0] = dimensions[0].Trim().ToLower();
                            dimensions[1] = dimensions[1].Trim().ToLower();

                            CultureInfo OrigCulture = Catalog.SetCulture(CultureInfo.InvariantCulture);

                            AWidthInPoint = ToPoint(dimensions[0], eResolution.eHorizontal);
                            AHeightInPoint = ToPoint(dimensions[1], eResolution.eVertical);

                            Catalog.SetCulture(OrigCulture);
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public override void StartPrintDocument()
        {
            FHasMorePages = true;
        }

        /// <summary>
        /// get the xmlnode for the given page
        /// </summary>
        /// <param name="ADocumentNr">starting with 1</param>
        /// <returns></returns>
        XmlNode GetDocumentNode(Int32 ADocumentNr)
        {
            if (FHtmlDoc == null)
            {
                return null;
            }

            XmlNode result = FHtmlDoc.FirstChild; // should be <html>

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

            Int32 counter = 1;

            while ((result != null) && (result.Name == "body"))
            {
                if (counter == ADocumentNr)
                {
                    return result;
                }

                result = result.NextSibling;
                counter++;
            }

            throw new Exception("cannot find the body tag");
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public override void PrintPageHeader()
        {
            if (FPrinter is TGfxPrinter)
            {
                TGfxPrinter printer = (TGfxPrinter)FPrinter;

                // todo: paper selection? source tray?
                // Paper Size Width is inch/100
                //printer.Inch2Twips(printer.Document.PrinterSettings.PaperSizes[0].Width*100);
                //float edge = printer.Cm2Twips(1.5f);
                float pageRightMargin = TGfxPrinter.Cm2Inch(1.3f) - printer.RightMargin;
                float pageLeftMargin = TGfxPrinter.Cm2Inch(2.5f) - printer.LeftMargin;

                if (pageRightMargin < 0)
                {
                    pageRightMargin = printer.RightMargin;
                }

                if (pageLeftMargin < 0)
                {
                    pageLeftMargin = printer.LeftMargin;
                }

                XmlNode BodyNode = TXMLParser.FindNodeRecursive(FHtmlDoc.DocumentElement, "body");

                if (TXMLParser.HasAttribute(BodyNode, "style"))
                {
                    Dictionary <string, string>Styles = TFormLettersTools.GetStyles(BodyNode);

                    foreach (string StyleName in Styles.Keys)
                    {
                        if (StyleName == "margin-left")
                        {
                            // TODO: PixelToInch? support not just 0px left margin
                            if (Styles[StyleName] == "0px")
                            {
                                pageLeftMargin = printer.LeftMargin;
                            }
                        }
                        else if (StyleName == "margin-right")
                        {
                            // TODO: PixelToInch? support not just 0px right margin
                            if (Styles[StyleName] == "0px")
                            {
                                pageRightMargin = printer.RightMargin;
                            }
                        }
                        else if (StyleName == "margin")
                        {
                            // TODO: PixelToInch? support not just 0px
                            if (Styles[StyleName] == "0px")
                            {
                                pageLeftMargin = printer.LeftMargin;
                                pageRightMargin = printer.RightMargin;
                            }
                        }
                        else if (StyleName == "background-image")
                        {
                            // only supporting url(...) at the moment
                            if (Styles[StyleName].StartsWith("url(") && Styles[StyleName].EndsWith(")"))
                            {
                                if (FPath.Length == 0)
                                {
                                    FPath = Environment.CurrentDirectory;
                                }

                                string filename = System.IO.Path.Combine(FPath, Styles[StyleName].Substring(4, Styles[StyleName].Length - 5));
                                float oldXPos = FPrinter.CurrentXPos;
                                float oldYPos = FPrinter.CurrentYPos;
                                FPrinter.DrawBitmap(filename, oldXPos, oldYPos);
                                FPrinter.CurrentXPos = oldXPos;
                                FPrinter.CurrentYPos = oldYPos;
                            }
                        }
                    }
                }

                // TODO set the margins and the font sizes in the HTML file???
                printer.FPageXPos = pageLeftMargin;

                if (printer.CurrentPageNr == 1)
                {
                    printer.FPageWidthAvailable = printer.Width - ((pageRightMargin) + (pageLeftMargin));

                    if (printer.FPageWidthAvailable < 0)
                    {
                        // this happens for using HTML renderer with eMarginType.eDefaultMargins instead of eMarginType.ePrintableArea
                        // printer.FPageWidthAvailable = 3;
                    }

                    printer.FDefaultFont = new System.Drawing.Font("Arial", 12);
                    printer.FDefaultBoldFont = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                    printer.CurrentRelativeFontSize = 0;
                }

                if (FPageHeader.Length > 0)
                {
                    string TempPageHeader = FPageHeader;
                    TempPageHeader = TempPageHeader.Replace("#PAGENR", printer.CurrentPageNr.ToString());
                    TempPageHeader = TempPageHeader.Replace("#PAGETOTAL", printer.NumberOfPages.ToString());

                    XmlDocument HeaderDoc = ParseHtml(TempPageHeader);
                    XmlNode CurrentNode = HeaderDoc.DocumentElement;

                    // TODO: stack: push and pop environment variables?
                    XmlNode BackupContinueNextPageNode = FContinueNextPageNode;
                    List <TTableRowGfx>BackupRowsLeftOver = FRowsLeftOver;
                    TTableRowGfx BackupHeaderRow = FTableHeaderRow;
                    FTableHeaderRow = null;
                    FRowsLeftOver = null;
                    FContinueNextPageNode = null;
                    printer.PushCurrentState();
                    printer.CurrentRelativeFontSize = 0;
                    RenderContent(printer.FPageXPos, printer.FPageWidthAvailable, ref CurrentNode);
                    printer.PopCurrentStateApartFromYPosition();
                    FContinueNextPageNode = BackupContinueNextPageNode;
                    FRowsLeftOver = BackupRowsLeftOver;
                    FTableHeaderRow = BackupHeaderRow;
                }
            }
        }

        // this should be null, while printing a page; it is set to a node, when the node does not fit on the page anymore
        private XmlNode FContinueNextPageNode = null;

        /// <summary>
        /// print one page of the HTML, one body tag per page???
        /// TODO: or until div with page break?
        /// </summary>
        /// <returns>void</returns>
        public override void PrintPageBody()
        {
            XmlNode CurrentNode = null;

            if (FContinueNextPageNode != null)
            {
                CurrentNode = FContinueNextPageNode;
                FContinueNextPageNode = null;
            }

            if (CurrentNode == null)
            {
                FCurrentNodeNextPage = GetDocumentNode(FPrinter.CurrentDocumentNr);

                if (FCurrentNodeNextPage != null)
                {
                    CurrentNode = FCurrentNodeNextPage.FirstChild;
                    FPrinter.CurrentDocumentNr++;
                }
            }

            if (FPrinter is TGfxPrinter)
            {
                TGfxPrinter printer = (TGfxPrinter)FPrinter;
                RenderContent(printer.FPageXPos, printer.FPageWidthAvailable, ref CurrentNode);

                FHasMorePages = false;

                if ((CurrentNode == null) && (FContinueNextPageNode == null))
                {
                    // there can be several body blocks, each representing a page
                    FCurrentNodeNextPage = FCurrentNodeNextPage.NextSibling;
                    FHasMorePages = FCurrentNodeNextPage != null;
                }
                else
                {
                    if ((CurrentNode != null) && (FContinueNextPageNode == null))
                    {
                        FContinueNextPageNode = CurrentNode;
                    }

                    FHasMorePages = true;
                }

                FPrinter.SetHasMorePages(FHasMorePages);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public override void PrintPageFooter()
        {
        }

        /// <summary>
        /// special codes need to be converted from HTML to printable text
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
            s = s.Replace("&amp;", "&");
            s = s.Replace("&nbsp;", "  ");
            s = s.Replace("&gt;", ">");
            s = s.Replace("&lt;", "<");

            // other special characters? e.g. &uuml; etc
            // solution: use UTF-8 in the text editor when editing the template

            return s;
        }

        /// <summary>
        /// the px value from an HTML document is converted to an inch value for printing
        /// </summary>
        /// <param name="APixel"></param>
        /// <returns></returns>
        public static float Pixel2Inch(Int32 APixel)
        {
            // without any margin, a table with width of approx 800 px does fit on a page
            // pagewidth: 20 cm = 7.87 inches
            return ((float)Convert.ToDouble(APixel) * 7.87f) / 800.0f;
        }

        private float GetFloat(string AStyleValue)
        {
            CultureInfo OrigCulture = Catalog.SetCulture(CultureInfo.InvariantCulture);

            AStyleValue = AStyleValue.Trim().ToLower();
            float DoubleValue = 0.0f;

            if (AStyleValue.EndsWith("cm") || AStyleValue.EndsWith("in") || AStyleValue.EndsWith("px"))
            {
                DoubleValue = (float)Convert.ToDouble(AStyleValue.Substring(0, AStyleValue.Length - 2).Trim());
            }
            else
            {
                DoubleValue = (float)Convert.ToDouble(AStyleValue);
            }

            Catalog.SetCulture(OrigCulture);

            return DoubleValue;
        }

        /// <summary>
        /// convert any given position measurement to points
        /// </summary>
        /// <param name="AStyleValue"></param>
        /// <param name="AResolution"></param>
        /// <returns></returns>
        private float ToPoint(string AStyleValue, eResolution AResolution)
        {
            float FloatValue = GetFloat(AStyleValue);

            if (AStyleValue.EndsWith("cm"))
            {
                return FloatValue / 2.54f * 72.0f;
            }
            else if (AStyleValue.EndsWith("in"))
            {
                return FloatValue * 72.0f;
            }
            else if (AStyleValue.EndsWith("px"))
            {
            }

            // assume default unit to be pixel
            if (AResolution == eResolution.eHorizontal)
            {
                return FPrinter.PixelHorizontal(FloatValue);
            }

            return FPrinter.PixelVertical(FloatValue);
        }

        /// <summary>
        /// convert any given position measurement to inches
        /// </summary>
        /// <param name="AStyleValue"></param>
        /// <param name="AResolution"></param>
        /// <returns></returns>
        private float ToInch(string AStyleValue, eResolution AResolution)
        {
            float FloatValue = GetFloat(AStyleValue);

            if (AStyleValue.EndsWith("cm"))
            {
                return FloatValue / 2.54f;
            }
            else if (AStyleValue.EndsWith("in"))
            {
                return FloatValue;
            }
            else if (AStyleValue.EndsWith("px"))
            {
            }

            // assume default unit to be pixel
            if (AResolution == eResolution.eHorizontal)
            {
                return FPrinter.PixelHorizontal(FloatValue);
            }

            return FPrinter.PixelVertical(FloatValue);
        }

        /// <summary>
        /// convert any given position measurement to pixel
        /// </summary>
        /// <param name="AStyleValue"></param>
        /// <returns></returns>
        private Int32 ToPixel(string AStyleValue)
        {
            float FloatValue = GetFloat(AStyleValue);

            if (AStyleValue.EndsWith("px"))
            {
                return (Int32)Math.Round(FloatValue);
            }
            else if (AStyleValue.EndsWith("cm"))
            {
                return (Int32)Math.Round(((float)TGfxPrinter.DEFAULTPRINTERRESOLUTION * 2.54f) / FloatValue);
            }
            else if (AStyleValue.EndsWith("in"))
            {
                return (Int32)Math.Round(TGfxPrinter.DEFAULTPRINTERRESOLUTION / FloatValue);
            }

            return (Int32)Math.Round(FloatValue);
        }

        /// <summary>
        /// set the current position if there is a CSS definition for the current node;
        /// eg. position:absolute, top:35px, left:240px,
        /// </summary>
        /// <param name="curNode"></param>
        /// <param name="AWidthAvailable"></param>
        private bool SetPositionFromStyle(XmlNode curNode, ref float AWidthAvailable)
        {
            bool absolutePosition = false;
            bool PositionWasSet = false;

            if (TXMLParser.HasAttribute(curNode, "style"))
            {
                Dictionary <string, string>Styles = TFormLettersTools.GetStyles(curNode);

                foreach (string StyleName in Styles.Keys)
                {
                    if ((StyleName.ToLower() == "position") && (Styles[StyleName].Trim().ToLower() == "absolute"))
                    {
                        absolutePosition = true;
                    }
                    else if ((StyleName.ToLower() == "position") && (Styles[StyleName].Trim().ToLower() == "relative"))
                    {
                        absolutePosition = false;
                    }
                    else if (StyleName.ToLower() == "top")
                    {
                        if (absolutePosition)
                        {
                            FPrinter.CurrentYPos = ToInch(Styles[StyleName], eResolution.eVertical);
                        }
                        else
                        {
                            FPrinter.CurrentYPos = FPrinter.AnchorYPos + ToInch(Styles[StyleName], eResolution.eVertical);
                        }

                        PositionWasSet = true;
                    }
                    else if (StyleName.ToLower() == "left")
                    {
                        if (absolutePosition)
                        {
                            FPrinter.CurrentXPos = ToInch(Styles[StyleName], eResolution.eHorizontal);
                        }
                        else
                        {
                            FPrinter.CurrentXPos = FPrinter.AnchorXPos + ToInch(Styles[StyleName], eResolution.eHorizontal);
                        }

                        PositionWasSet = true;
                    }
                    else if (StyleName.ToLower() == "width")
                    {
                        AWidthAvailable = ToInch(Styles[StyleName], eResolution.eHorizontal);
                    }
                    else if (StyleName.ToLower() == "transform")
                    {
                        // see also http://www.w3schools.com/cssref/css3_pr_transform.asp
                        string transformValue = Styles[StyleName].Trim().ToLower();

                        if (transformValue.StartsWith("rotate(") && transformValue.EndsWith("deg)"))
                        {
                            CultureInfo OrigCulture = Catalog.SetCulture(CultureInfo.InvariantCulture);
                            Double DegreeValue =
                                Convert.ToDouble(transformValue.Substring("rotate(".Length, transformValue.Length - "rotate(deg)".Length));
                            Catalog.SetCulture(OrigCulture);

                            FPrinter.RotateAtTransform(
                                DegreeValue,
                                FPrinter.CurrentXPos,
                                FPrinter.CurrentYPos);
                        }
                        else
                        {
                            TLogging.Log("TPrinterHtml: unsupported transform style. we only support rotation at the moment");
                        }
                    }
                }
            }

            if (absolutePosition)
            {
                FPrinter.AnchorXPos = FPrinter.CurrentXPos;
                FPrinter.AnchorYPos = FPrinter.CurrentYPos;
            }

            return PositionWasSet;
        }

        /// <summary>
        /// interpret HTML code
        /// </summary>
        /// <param name="AXPos">the X position to start the content</param>
        /// <param name="AWidthAvailable">AWidthAvailable</param>
        /// <param name="curNode"></param>
        /// <returns>the height of the content</returns>
        public override float RenderContent(float AXPos,
            float AWidthAvailable,
            ref XmlNode curNode)
        {
            float oldYPos = FPrinter.CurrentYPos;

            XmlNode origNode = curNode;
            float OrigWidthAvailable = AWidthAvailable;

            while (curNode != null && FPrinter.ValidYPos() && FContinueNextPageNode == null)
            {
                AWidthAvailable = OrigWidthAvailable;

                FPrinter.SaveState();

                bool HasPositionInfo = SetPositionFromStyle(curNode, ref AWidthAvailable);

                if (HasPositionInfo)
                {
                    AXPos = FPrinter.CurrentXPos;
                }

                if (curNode.Name == "table")
                {
                    PrintTable(AXPos, AWidthAvailable, ref curNode);
                }
                else if (curNode.Name == "pdf")
                {
                    // insert PDF. this currently only works for printing to PDF
                    string src = TXMLParser.GetAttribute(curNode, "src");
                    src = System.IO.Path.Combine(FPath, src);

                    FPrinter.InsertDocument(src);
                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "img")
                {
                    // insert image
                    string src = TXMLParser.GetAttribute(curNode, "src");
                    src = System.IO.Path.Combine(FPath, src);

                    // todo: embed image into text flow
                    if (FPrinter.CurrentXPos > AXPos)
                    {
                        FPrinter.LineFeed();
                        FPrinter.CurrentXPos = AXPos;
                    }

                    if (TXMLParser.HasAttribute(curNode, "width") && TXMLParser.HasAttribute(curNode, "height"))
                    {
                        float WidthPercentage = 0.0f;
                        float HeightPercentage = 0.0f;
                        float Width = 0.0f;
                        float Height = 0.0f;

                        string WidthString = TXMLParser.GetAttribute(curNode, "width");
                        string HeightString = TXMLParser.GetAttribute(curNode, "height");

                        if (WidthString.EndsWith("%"))
                        {
                            WidthPercentage = (float)Convert.ToDouble(WidthString.Substring(0, WidthString.Length - 1)) / 100.0f;
                        }
                        else
                        {
                            Width = ToPixel(WidthString);
                        }

                        if (HeightString.EndsWith("%"))
                        {
                            HeightPercentage = (float)Convert.ToDouble(HeightString.Substring(0, HeightString.Length - 1)) / 100.0f;
                        }
                        else
                        {
                            Height = ToPixel(HeightString);
                        }

                        FPrinter.DrawBitmap(src, FPrinter.CurrentXPos, FPrinter.CurrentYPos, Width, Height, WidthPercentage, HeightPercentage);
                    }
                    else
                    {
                        FPrinter.DrawBitmap(src, FPrinter.CurrentXPos, FPrinter.CurrentYPos);
                    }

                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "font")
                {
                    // TODO change font name and/or size
                    float previousFontSize = FPrinter.CurrentRelativeFontSize;
                    eFont previousFont = FPrinter.CurrentFont;

                    if (TXMLParser.HasAttribute(curNode, "size"))
                    {
                        CultureInfo OrigCulture = Catalog.SetCulture(CultureInfo.InvariantCulture);

                        FPrinter.CurrentRelativeFontSize += (float)TXMLParser.GetDecimalAttribute(curNode, "size");

                        Catalog.SetCulture(OrigCulture);
                    }

                    if (TXMLParser.HasAttribute(curNode, "face"))
                    {
                        foreach (eFont MyFont in Enum.GetValues(typeof(eFont)))
                        {
                            if (MyFont.ToString() == TXMLParser.GetAttribute(curNode, "face"))
                            {
                                FPrinter.CurrentFont = MyFont;
                                break;
                            }
                        }
                    }

                    // recursively call RenderContent
                    XmlNode child = curNode.FirstChild;
                    RenderContent(AXPos, AWidthAvailable, ref child);

                    if (FContinueNextPageNode != null)
                    {
                        break;
                    }

                    if (!FPrinter.ValidYPos())
                    {
                        curNode = child;
                        break;
                    }

                    // reset font
                    FPrinter.CurrentRelativeFontSize = previousFontSize;
                    FPrinter.CurrentFont = previousFont;

                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "b")
                {
                    // bold
                    eFont previousFont = FPrinter.CurrentFont;
                    FPrinter.CurrentFont = eFont.eDefaultBoldFont;
                    XmlNode child = curNode.FirstChild;
                    RenderContent(AXPos, AWidthAvailable, ref child);

                    if (FContinueNextPageNode != null)
                    {
                        break;
                    }

                    if (!FPrinter.ValidYPos())
                    {
                        curNode = child;
                        break;
                    }

                    FPrinter.CurrentFont = previousFont;
                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "i")
                {
                    // todo italic; similar to bold
                    XmlNode child = curNode.FirstChild;
                    RenderContent(AXPos, AWidthAvailable, ref child);

                    if (FContinueNextPageNode != null)
                    {
                        break;
                    }

                    if (!FPrinter.ValidYPos())
                    {
                        curNode = child;
                        break;
                    }

                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "br")
                {
                    // line break
                    FPrinter.LineFeed();
                    FPrinter.CurrentXPos = AXPos;
                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "ul")
                {
                    if (!HasPositionInfo)
                    {
                        FPrinter.LineFeed();
                    }

                    FPrinter.CurrentXPos = AXPos;

                    // list with bullet points
                    foreach (XmlNode bulletPoint in curNode.ChildNodes)
                    {
                        if (bulletPoint.Name == "li")
                        {
                            FPrinter.PrintStringWrap("* ", FPrinter.CurrentFont, AXPos, AWidthAvailable, FPrinter.CurrentAlignment);

                            foreach (XmlNode bulletChild in bulletPoint.ChildNodes)
                            {
                                XmlNode loopTemp = bulletChild;
                                RenderContent(FPrinter.CurrentXPos, AWidthAvailable - (FPrinter.CurrentXPos - AXPos), ref loopTemp);
                            }

                            FPrinter.LineFeed();
                            FPrinter.CurrentXPos = AXPos;
                        }
                    }

                    curNode = curNode.NextSibling;
                }
                else if (curNode.Name == "div")
                {
                    XmlNode child = curNode.FirstChild;
                    RenderContent(AXPos, AWidthAvailable, ref child);

                    if (FContinueNextPageNode != null)
                    {
                        break;
                    }

                    if (!FPrinter.ValidYPos())
                    {
                        curNode = child;
                        break;
                    }

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

                    if (!HasPositionInfo)
                    {
                        FPrinter.LineFeed();
                    }

                    FPrinter.CurrentXPos = AXPos;
                    RenderContent(AXPos, AWidthAvailable, ref child);

                    if (FContinueNextPageNode != null)
                    {
                        break;
                    }

                    if (!FPrinter.ValidYPos())
                    {
                        curNode = child;
                        break;
                    }

                    FPrinter.LineFeed();
                    FPrinter.CurrentXPos = AXPos;
                    curNode = curNode.NextSibling;
                    FPrinter.CurrentAlignment = origAlignment;
                }
                else if ((curNode.Name.Length > 1) && (curNode.Name[0] == 'h') && char.IsDigit(curNode.Name[1]))
                {
                    // heading
                    eFont previousFont = FPrinter.CurrentFont;
                    FPrinter.CurrentFont = eFont.eHeadingFont;
                    float previousFontSize = FPrinter.CurrentRelativeFontSize;

                    if (curNode.Name[1] == '1')
                    {
                        FPrinter.CurrentRelativeFontSize += 2;
                    }
                    else
                    {
                        FPrinter.CurrentRelativeFontSize += 1;
                    }

                    XmlNode child = curNode.FirstChild;
                    RenderContent(AXPos, AWidthAvailable, ref child);

                    if (FContinueNextPageNode != null)
                    {
                        break;
                    }

                    if (!FPrinter.ValidYPos())
                    {
                        curNode = child;
                        break;
                    }

                    FPrinter.CurrentFont = previousFont;
                    FPrinter.CurrentRelativeFontSize = previousFontSize;
                    curNode = curNode.NextSibling;
                    FPrinter.LineFeed();
                }
                else if (curNode.Name == "#comment")
                {
                    // just skip comments
                    curNode = curNode.NextSibling;
                }
                // unrecognised HTML element, text
                else if (curNode.InnerText.Length > 0)
                {
                    string toPrint = HtmlToText(curNode.InnerText);

                    // continues text in the same row
                    FPrinter.PrintStringWrap(toPrint, FPrinter.CurrentFont, AXPos, AWidthAvailable, FPrinter.CurrentAlignment);
                    curNode = curNode.NextSibling;
                }
                // unrecognised HTML element which is empty, eg. hr
                else
                {
                    curNode = curNode.NextSibling;
                }

                if (HasPositionInfo)
                {
                    // reset to top of paper, so that there is no unintended page break
                    FPrinter.CurrentYPos = 0;
                }

                FPrinter.RestoreState();

                // todo: h1, etc headings???
                // todo: code, fixed width font (for currency amounts?) ???
                // todo: don't print to paper if class="preprinted"; but is printed for PDF
                // todo: checked and unchecked checkbox
                // todo: page break
                // todo: header div style with tray information; config file with local tray names???
            }

            if ((origNode == curNode) && (curNode != null) && FPrinter.ValidYPos() && (FRowsLeftOver == null))
            {
                throw new Exception("page too small, at " + curNode.Name);
            }

            return FPrinter.CurrentYPos - oldYPos;
        }

        private List <TTableRowGfx>FRowsLeftOver = new List <TTableRowGfx>();
        private TTableRowGfx FTableHeaderRow = null;

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

            int border = 0;
            string outsideborders = "none";
            string insidelines = "none";
            int height = -1;

            // http://www.htmlcodetutorial.com/tables/index_famsupp_189.html
            // and http://www.htmlcodetutorial.com/tables/index_famsupp_147.html
            if (TXMLParser.HasAttribute(tableNode, "border"))
            {
                border = Convert.ToInt32(TXMLParser.GetAttribute(tableNode, "border"));

                if (border != 0)
                {
                    outsideborders = "box";
                    insidelines = "all";
                }
            }

            if (TXMLParser.HasAttribute(tableNode, "rules"))
            {
                // rules can be all, rows, cols
                insidelines = TXMLParser.GetAttribute(tableNode, "rules").ToLower();
                outsideborders = "box";
            }

            if (TXMLParser.HasAttribute(tableNode, "frame"))
            {
                // frame can be box, or hsides, or vsides, or none
                outsideborders = TXMLParser.GetAttribute(tableNode, "frame").ToLower();
            }

            if (TXMLParser.HasAttribute(tableNode, "width"))
            {
                string width = TXMLParser.GetAttribute(tableNode, "width");

                if (width.EndsWith("%"))
                {
                    AWidthAvailable *= (float)Convert.ToDouble(width.Substring(0, width.Length - 1)) / 100.0f;
                }
                else
                {
                    AWidthAvailable = Pixel2Inch(Convert.ToInt32(width));
                }
            }

            if (TXMLParser.HasAttribute(tableNode, "height"))
            {
                // TODO: convert from html pixel into printing height, using Pixel2Inch
                // height = TXMLParser.GetIntAttribute(tableNode, "height");
            }

            List <TTableRowGfx>preparedRows = new List <TTableRowGfx>();

            if ((FRowsLeftOver != null) && (FRowsLeftOver.Count > 0))
            {
                preparedRows = FRowsLeftOver;
                FRowsLeftOver = new List <TTableRowGfx>();
            }
            else
            {
                curNode = curNode.FirstChild;     // should be tbody, or tr, or colgroup

                // widths of columns defined in colgroup
                List <Int32>colWidth = new List <Int32>();

                while (curNode != null && curNode.Name != "colgroup"
                       && curNode.Name != "tbody" && curNode.Name != "tr")
                {
                    curNode = curNode.NextSibling;
                }

                if ((curNode != null) && (curNode.Name == "colgroup"))
                {
                    XmlNode colNode = curNode.FirstChild;
                    Int32 TableWidth = 0;

                    while (colNode != null)
                    {
                        if (TXMLParser.HasAttribute(colNode, "width"))
                        {
                            Int32 width = ToPixel(TXMLParser.GetAttribute(colNode, "width"));
                            colWidth.Add(width);
                            TableWidth += width;
                        }

                        colNode = colNode.NextSibling;
                    }

                    if (Pixel2Inch(TableWidth) < AWidthAvailable)
                    {
                        AWidthAvailable = Pixel2Inch(TableWidth);
                    }

                    // calculate percentages
                    for (Int32 counter = 0; counter < colWidth.Count; counter++)
                    {
                        colWidth[counter] = Convert.ToInt32((colWidth[counter] * 100.0f / TableWidth));
                    }

                    curNode = curNode.NextSibling;
                }

                while (curNode != null && curNode.Name != "tbody" && curNode.Name != "tr")
                {
                    curNode = curNode.NextSibling;
                }

                if ((curNode != null) && (curNode.Name == "tbody"))
                {
                    curNode = curNode.FirstChild;
                }

                bool firstRow = true;

                while (curNode != null && curNode.Name == "tr")
                {
                    TTableRowGfx preparedRow = new TTableRowGfx();
                    preparedRow.cells = new List <TTableCellGfx>();
                    XmlNode row = curNode;
                    XmlNode cell = curNode.FirstChild;

                    bool lastRow = (curNode.NextSibling == null) || (curNode.NextSibling.Name != "tr");

                    bool firstColumn = true;

                    while (cell != null && (cell.Name == "td" || cell.Name == "th"))
                    {
                        TTableCellGfx preparedCell = new TTableCellGfx();
                        preparedCell.borderWidth = border;
                        preparedCell.borderBitField = 0;

                        bool lastColumn = (cell.NextSibling == null) || (cell.NextSibling.Name != "td" && cell.NextSibling.Name != "th");

                        if (border > 0)
                        {
                            if (firstRow && ((outsideborders == "box") || (outsideborders == "hsides")))
                            {
                                preparedCell.borderBitField |= TTableCellGfx.TOP;
                            }
                            else if (!firstRow && ((insidelines == "all") || (insidelines == "rows")))
                            {
                                preparedCell.borderBitField |= TTableCellGfx.TOP;
                            }

                            if (lastRow && ((outsideborders == "box") || (outsideborders == "hsides")))
                            {
                                preparedCell.borderBitField |= TTableCellGfx.BOTTOM;
                            }
                            else if (!lastRow && ((insidelines == "all") || (insidelines == "rows")))
                            {
                                preparedCell.borderBitField |= TTableCellGfx.BOTTOM;
                            }

                            if (firstColumn && ((outsideborders == "box") || (outsideborders == "vsides")))
                            {
                                preparedCell.borderBitField |= TTableCellGfx.LEFT;
                            }
                            else if (!firstColumn && ((insidelines == "all") || (insidelines == "cols")))
                            {
                                preparedCell.borderBitField |= TTableCellGfx.LEFT;
                            }

                            if (lastColumn && ((outsideborders == "box") || (outsideborders == "vsides")))
                            {
                                preparedCell.borderBitField |= TTableCellGfx.RIGHT;
                            }
                            else if (!lastColumn && ((insidelines == "all") || (insidelines == "cols")))
                            {
                                preparedCell.borderBitField |= TTableCellGfx.RIGHT;
                            }
                        }

                        preparedCell.content = cell.FirstChild;

                        if (TXMLParser.HasAttribute(cell, "colspan"))
                        {
                            preparedCell.colSpan = Convert.ToInt16(TXMLParser.GetAttribute(cell, "colspan"));
                        }

                        preparedCell.bold = (cell.Name == "th");

                        if (TXMLParser.GetAttribute(cell, "nowrap") == "nowrap")
                        {
                            preparedCell.nowrap = true;
                        }

                        if (cell.Name == "th")
                        {
                            // remember title row for next pages
                            FTableHeaderRow = preparedRow;
                        }

                        if (TXMLParser.GetAttribute(cell, "align") == "right")
                        {
                            preparedCell.align = eAlignment.eRight;
                        }
                        else if ((TXMLParser.GetAttribute(cell, "align") == "center") || (cell.Name == "th"))
                        {
                            preparedCell.align = eAlignment.eCenter;
                        }

                        preparedRow.cells.Add(preparedCell);

                        // add a few dummy cells for column spanning
                        for (int colspanCounter = 1; colspanCounter < preparedCell.colSpan; colspanCounter++)
                        {
                            preparedRow.cells.Add(new TTableCellGfx());
                        }

                        cell = cell.NextSibling;
                        firstColumn = false;
                    }

                    // make sure the percentages are right;
                    // todo: what if only some columns have a percentage?
                    Int32 counter = 0;

                    foreach (TTableCellGfx preparedCell in preparedRow.cells)
                    {
                        if (preparedCell.contentWidth == -1)
                        {
                            if (colWidth.Count > counter)
                            {
                                preparedCell.contentWidth =
                                    (AWidthAvailable * colWidth[counter]) / 100.0f;
                            }
                            else
                            {
                                preparedCell.contentWidth =
                                    (AWidthAvailable / preparedRow.cells.Count);
                            }
                        }

                        counter++;
                    }

                    // implement colspan
                    Int16 CounterColumnSpan = 0;
                    TTableCellGfx spanningCell = null;

                    foreach (TTableCellGfx preparedCell in preparedRow.cells)
                    {
                        if (CounterColumnSpan > 0)
                        {
                            spanningCell.contentWidth += preparedCell.contentWidth;
                            preparedCell.contentWidth = 0;
                        }
                        else
                        {
                            CounterColumnSpan = preparedCell.colSpan;
                            spanningCell = preparedCell;
                        }

                        CounterColumnSpan--;
                    }

                    preparedRows.Add(preparedRow);
                    curNode = row.NextSibling;
                    firstRow = false;
                }
            }

            // first simulate the printing, and see how many rows will fit on the page
            Int32 RowsFittingOnPage;

            FPrinter.StartSimulatePrinting();
            FPrinter.PrintTable(AXPos, AWidthAvailable, preparedRows, out RowsFittingOnPage);
            FPrinter.FinishSimulatePrinting();

            FContinueNextPageNode = null;

            if (RowsFittingOnPage != preparedRows.Count)
            {
                // the rows did not all fit on the page
                FRowsLeftOver = new List <TTableRowGfx>();

                if (FTableHeaderRow != null)
                {
                    FRowsLeftOver.Add(FTableHeaderRow);
                }

                while (preparedRows.Count > RowsFittingOnPage)
                {
                    FRowsLeftOver.Add(preparedRows[RowsFittingOnPage]);
                    preparedRows.RemoveAt(RowsFittingOnPage);
                }

                curNode = tableNode;
            }
            else
            {
                // table was completely printed
                curNode = tableNode.NextSibling;
            }

            FPrinter.PrintTable(AXPos, AWidthAvailable, preparedRows, out RowsFittingOnPage);

            if (curNode == tableNode)
            {
                FContinueNextPageNode = tableNode;
            }

            if (height < FPrinter.CurrentYPos - oldYPos)
            {
                return FPrinter.CurrentYPos - oldYPos;
            }

            FPrinter.CurrentYPos = oldYPos + height;
            return height;
        }

        /// <summary>
        /// find the &lt;tr&gt; tag that contains the ASearchFor, and return the full tr tag and contents
        /// </summary>
        /// <param name="ATemplate"></param>
        /// <param name="ASearchFor"></param>
        /// <param name="ATemplateRow">template for one row</param>
        /// <returns>modified template, replace tr tag with #ROWTEMPLATE</returns>
        public static string GetTableRow(string ATemplate, string ASearchFor, out string ATemplateRow)
        {
            // TODO: use XML tree to avoid problems with whitespaces etc?
            Int32 posSearchText = ATemplate.IndexOf(ASearchFor);

            if (posSearchText == -1)
            {
                ATemplateRow = "";
                return ATemplate;
            }

            Int32 posTableRowStart = ATemplate.Substring(0, posSearchText).LastIndexOf("<tr");
            Int32 posTableRowEnd = ATemplate.IndexOf("</tr>", posSearchText) + 5;
            ATemplateRow = ATemplate.Substring(posTableRowStart, posTableRowEnd - posTableRowStart);
            return ATemplate.Replace(ATemplateRow, "#ROWTEMPLATE");
        }

        private static void FindAllChildren(XmlNode ANode, ref List <XmlNode>AResult, string AElement, string AAttributeName, string AName)
        {
            if ((ANode.Name == AElement) && (TXMLParser.GetAttribute(ANode, AAttributeName) == AName))
            {
                AResult.Add(ANode);
            }

            foreach (XmlNode node in ANode.ChildNodes)
            {
                FindAllChildren(node, ref AResult, AElement, AAttributeName, AName);
            }
        }

        /// <summary>
        /// returns the title of the html document
        /// </summary>
        /// <param name="AHtmlMessage"></param>
        /// <returns></returns>
        public static string GetTitle(string AHtmlMessage)
        {
            XmlDocument htmlDoc = Ict.Common.Printing.TPrinterHtml.ParseHtml(AHtmlMessage);

            List <XmlNode>children = new List <XmlNode>();
            FindAllChildren(htmlDoc.DocumentElement, ref children, "title", "", "");

            if (children.Count < 1)
            {
                return "NO VALID TITLE IN TEMPLATE";
            }

            return children[0].InnerText;
        }

        /// <summary>
        /// remove a div with the given name
        /// </summary>
        /// <param name="AHtmlMessage"></param>
        /// <param name="ADivName"></param>
        /// <returns></returns>
        public static string RemoveDivWithName(string AHtmlMessage, string ADivName)
        {
            string dummy;

            return RemoveElement(AHtmlMessage, "div", "name", ADivName, out dummy);
        }

        /// remove all divs of the given class
        public static string RemoveDivWithClass(string AHtmlMessage, string ADivClass)
        {
            string dummy;

            return RemoveElement(AHtmlMessage, "div", "class", ADivClass, out dummy);
        }

        /// remove all elments with given name or class
        public static string RemoveElement(string AHtmlMessage, string AElementName, string AAttributeName, string ADivID, out string AElementCode)
        {
            // use xml tree to avoid whitespace differences etc?
            XmlDocument htmlDoc = Ict.Common.Printing.TPrinterHtml.ParseHtml(AHtmlMessage);

            List <XmlNode>children = new List <XmlNode>();
            FindAllChildren(htmlDoc.DocumentElement, ref children, AElementName, AAttributeName, ADivID);

            AElementCode = "";

            foreach (XmlNode child in children)
            {
                AElementCode += child.OuterXml.Replace("&amp;", "&");
                child.ParentNode.RemoveChild(child);
            }

            return htmlDoc.OuterXml.Replace("&amp;", "&");
        }
    }
}