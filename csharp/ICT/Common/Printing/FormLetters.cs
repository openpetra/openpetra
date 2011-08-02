//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Globalization;
using System.Drawing.Printing;
using Ict.Common.IO;

namespace Ict.Common.Printing
{
    /// <summary>
    /// helper functions for form letters, which can be used for printing to paper or preparing emails etc
    /// </summary>
    public class TFormLettersTools
    {
        /// <summary>
        /// for form letter files, we need to check if there is a template specific for the country or form.
        /// Otherwise the next best fitting template is used
        /// </summary>
        /// <param name="APath"></param>
        /// <param name="AFileID"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AFormsID">several classifications be separated by a dot, eg. adult.serve. If there is no template for adult.serve, use the template for adult</param>
        /// <param name="AExtension"></param>
        /// <returns></returns>
        public static string GetRoleSpecificFile(string APath,
            string AFileID,
            string ACountryCode,
            string AFormsID,
            string AExtension)
        {
            if (!AFileID.EndsWith("."))
            {
                AFileID += ".";
            }

            if (!AExtension.StartsWith("."))
            {
                AExtension = "." + AExtension;
            }

            if (ACountryCode == null)
            {
                ACountryCode = string.Empty;
            }

            if (ACountryCode.Length > 0)
            {
                ACountryCode += ".";
            }

            APath = Path.GetFullPath(APath);
            string FileName = Path.Combine(APath, AFileID);

            if (File.Exists(FileName + ACountryCode + AFormsID + AExtension))
            {
                return FileName + ACountryCode + AFormsID + AExtension;
            }

            string OrigFormsID = AFormsID;

            while (AFormsID != null && AFormsID.Contains("."))
            {
                AFormsID = AFormsID.Substring(0, AFormsID.LastIndexOf('.'));

                if (File.Exists((FileName + ACountryCode + AFormsID + AExtension).Replace("..", ".")))
                {
                    return (FileName + ACountryCode + AFormsID + AExtension).Replace("..", ".");
                }
            }

            if (File.Exists((FileName + ACountryCode + AExtension).Replace("..", ".")))
            {
                return (FileName + ACountryCode + AExtension).Replace("..", ".");
            }

            // we cannot find the file, so we just return a filename for the error message
            return FileName + ACountryCode + OrigFormsID + AExtension;
        }

        /// <summary>
        /// attach a new page to the overall document that will be printed later
        /// </summary>
        /// <param name="AResultDocument"></param>
        /// <param name="ANewPage"></param>
        /// <returns>false if the new page was empty</returns>
        public static bool AttachNextPage(ref string AResultDocument, string ANewPage)
        {
            if (ANewPage.Length > 0)
            {
                if (AResultDocument.Length > 0)
                {
                    // AResultDocument += "<div style=\"page-break-before: always;\"/>";
                    string body = ANewPage.Substring(ANewPage.IndexOf("<body"));
                    body = body.Substring(0, body.IndexOf("</html"));
                    AResultDocument += body;
                }
                else
                {
                    // without closing html
                    AResultDocument += ANewPage.Substring(0, ANewPage.IndexOf("</html"));
                }

                return true;
            }

            return false;
        }

        private static int CountString(string haystack, string needle)
        {
            int counter = 0;
            string localHaystack = haystack;

            while (localHaystack.IndexOf(needle) != -1)
            {
                localHaystack = localHaystack.Substring(localHaystack.IndexOf(needle) + 1);
                counter++;
            }

            return counter;
        }

        /// <summary>
        /// return the contents of the given div tag
        /// </summary>
        /// <param name="ADocument"></param>
        /// <param name="ADivName"></param>
        /// <returns></returns>
        public static string GetContentsOfDiv(string ADocument, string ADivName)
        {
            if (ADocument.Length == 0)
            {
                return string.Empty;
            }

            if (ADocument.IndexOf("<div name=\"" + ADivName + "\"") == -1)
            {
                return string.Empty;
            }

            string Result = string.Empty;
            string Document = ADocument.Substring(ADocument.IndexOf("<div name=\"" + ADivName + "\""));

            while (Result.Length == 0 || CountString(Result, "<div") != CountString(Result, "</div>"))
            {
                int IndexEndDiv = Document.IndexOf("</div>") + 6;

                if (IndexEndDiv == -1 + 6)
                {
                    throw new Exception("missing </div>");
                }

                Result += Document.Substring(0, IndexEndDiv);
                Document = Document.Substring(IndexEndDiv);
            }

            // contents of div:
            Result = Result.Substring(Result.IndexOf(">") + 1);
            Result = Result.Substring(0, Result.LastIndexOf("<"));

            return Result;
        }

        /// <summary>
        /// split styles into a sorted list with keys and values
        /// </summary>
        public static Dictionary <string, string>GetStyles(XmlNode ANode)
        {
            Dictionary <string, string>Result = new Dictionary <string, string>();

            string styles = TXMLParser.GetAttribute(ANode, "style");
            string[] namevaluepairs = styles.Split(new char[] { ',', ';' });

            try
            {
                foreach (string namevaluepair in namevaluepairs)
                {
                    string DetailName = namevaluepair.Substring(0, namevaluepair.IndexOf(':'));
                    string DetailValue = namevaluepair.Substring(namevaluepair.IndexOf(':') + 1);

                    Result.Add(DetailName.Trim(), DetailValue.Trim());
                }
            }
            catch (Exception ex)
            {
                TLogging.Log(styles);
                TLogging.Log(ex.ToString());
            }
            return Result;
        }

        private static float GetFloat(Dictionary <string, string>AStyles, string AName, string AUnit)
        {
            try
            {
                return (float)Convert.ToDouble(AStyles[AName].Replace(AUnit, ""));
            }
            catch (Exception)
            {
                return 0.0f;
            }
        }

        private class THTMLFormLetter
        {
            public float marginLeft;
            public float marginTop;
            public float marginBottom;
            public float pageWidth;
            public float pageHeight;
            public float footerLeft;
            public float footerHeight;
            public float headerLeft;
            public float headerHeight;
            public float reportFooterLeft;
            public float reportFooterHeight;
            public string unit;
            public int currentPage;
            public XmlDocument TemplateDoc;
            public string ResultDocument;
            public XmlNode FooterNode;
            public XmlNode HeaderNode;
            public XmlNode ReportFooterNode;
            private CultureInfo OrigCulture;

            public THTMLFormLetter(string AFilename)
            {
                OrigCulture = Catalog.SetCulture(CultureInfo.InvariantCulture);

                StreamReader sr = new StreamReader(AFilename);
                string Template = sr.ReadToEnd();

                sr.Close();

                if (Template.Trim().StartsWith("<!DOCTYPE"))
                {
                    Template = Template.Substring(Template.IndexOf(">") + 1);
                }

                if (!Template.StartsWith("<?xml version="))
                {
                    Template = "<?xml version='1.0' encoding='UTF-8'?>" + Environment.NewLine + Template;
                }

                TemplateDoc = new XmlDocument();
                TemplateDoc.LoadXml(Template);

                XmlNode bodyNode = TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "body");
                Dictionary <string, string>styles = GetStyles(bodyNode);

                unit = "in";

                if (!styles.ContainsKey("margin-left"))
                {
                    TLogging.Log("missing margin-left for getting the unit. assuming inch for now.");
                }
                else if (styles["margin-left"].EndsWith("cm"))
                {
                    unit = "cm";
                }

                // we need to use the margin-left and margin-top of the body for the position of the first label
                marginLeft = GetFloat(styles, "margin-left", unit);
                marginTop = GetFloat(styles, "margin-top", unit);
                marginBottom = GetFloat(styles, "margin-bottom", unit);

                // we need the width and height of body to know how many labels will fit on one page
                pageWidth = GetFloat(styles, "width", unit);
                pageHeight = GetFloat(styles, "height", unit);

                footerLeft = 0.0f;
                footerHeight = 0.0f;
                FooterNode = TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "footer");

                if (FooterNode != null)
                {
                    styles = GetStyles(FooterNode);
                    footerLeft = GetFloat(styles, "left", unit);;
                    footerHeight = GetFloat(styles, "height", unit);;
                }

                reportFooterLeft = 0.0f;
                reportFooterHeight = 0.0f;
                ReportFooterNode = TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "reportFooter");

                if (ReportFooterNode != null)
                {
                    styles = GetStyles(ReportFooterNode);
                    reportFooterLeft = GetFloat(styles, "left", unit);;
                    reportFooterHeight = GetFloat(styles, "height", unit);;
                }

                headerLeft = 0.0f;
                headerHeight = 0.0f;
                HeaderNode = TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "header");

                if (HeaderNode != null)
                {
                    styles = GetStyles(HeaderNode);
                    headerLeft = GetFloat(styles, "left", unit);;
                    headerHeight = GetFloat(styles, "height", unit);;
                }
            }

            public virtual void StartDocument()
            {
                ResultDocument =
                    "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">" +
                    Environment.NewLine +
                    "<html><body>" + Environment.NewLine;

                AddHeader();

                currentPage = 1;
            }

            public void AddFooter()
            {
                if ((ResultDocument.Length > 0) && (FooterNode != null))
                {
                    string footer = FooterNode.OuterXml;
                    footer = footer.Replace("#CURRENTPAGE", currentPage.ToString());
                    ResultDocument += footer;
                }
            }

            public void AddHeader()
            {
                if (HeaderNode != null)
                {
                    string header = HeaderNode.OuterXml;
                    header = header.Replace("#CURRENTPAGE", currentPage.ToString());
                    ResultDocument += header;
                }
            }

            public virtual void AddPageBreak()
            {
                AddFooter();

                ResultDocument += "</body><body>" + Environment.NewLine;

                currentPage++;

                AddHeader();
            }

            public void FinishDocument()
            {
                ResultDocument = ResultDocument.Replace("#TOTALPAGES", currentPage.ToString());
                ResultDocument = ResultDocument.Replace("#TODAY", DateTime.Now.ToString("dd-MMM-yyyy"));
                ResultDocument += "</body></html>";

                Catalog.SetCulture(OrigCulture);
            }
        }

        private class THTMLFormLabels : THTMLFormLetter
        {
            public float paddingLeft;
            public float paddingBottom;
            public float labelWidth;
            public float labelHeight;
            public int maxLabelsHorizontal;
            public int maxLabelsVertical;
            public int currentLabelX;
            public int currentLabelY;


            public THTMLFormLabels(string AFilename) : base(AFilename)
            {
                // we need padding-left and padding-bottom for the space between labels
                Dictionary <string, string>styles = GetStyles(TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "label"));
                paddingLeft = GetFloat(styles, "padding-left", unit);
                paddingBottom = GetFloat(styles, "padding-bottom", unit);
                labelWidth = GetFloat(styles, "width", unit);
                labelHeight = GetFloat(styles, "height", unit);

                maxLabelsHorizontal = (int)Math.Floor((pageWidth - marginLeft) / (labelWidth + paddingLeft));
                maxLabelsVertical = (int)Math.Floor((pageHeight - footerHeight - marginTop - marginBottom) / (labelHeight + paddingBottom));

                currentLabelX = 0;
                currentLabelY = 0;
            }

            public void PrintDocument(List <string>ALabels)
            {
                foreach (string label in ALabels)
                {
                    if (currentLabelX == maxLabelsHorizontal)
                    {
                        currentLabelX = 0;
                        currentLabelY++;
                    }

                    if (currentLabelY == maxLabelsVertical)
                    {
                        currentLabelY = 0;

                        AddPageBreak();
                    }

                    ResultDocument += Environment.NewLine +
                                      String.Format("<div style='position:absolute, left:{0}{2}, top:{1}{2}'>",
                        marginLeft + currentLabelX * (labelWidth + paddingLeft),
                        marginTop + currentLabelY * (labelHeight + paddingBottom),
                        unit);
                    ResultDocument += label;
                    ResultDocument += "</div>" + Environment.NewLine;

                    currentLabelX++;
                }

                AddFooter();
            }
        }

        private class THTMLFormReport : THTMLFormLetter
        {
            public float currentY;
            public float detailHeight;
            public float groupTopHeight;
            public float groupBottomHeight;

            public THTMLFormReport(string AFilename) : base(AFilename)
            {
                Dictionary <string, string>styles = GetStyles(TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "detail"));
                detailHeight = GetFloat(styles, "height", unit);
                styles = GetStyles(TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "groupTop"));
                groupTopHeight = GetFloat(styles, "height", unit);
                styles = GetStyles(TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "groupBottom"));
                groupBottomHeight = GetFloat(styles, "height", unit);
            }

            public override void StartDocument()
            {
                base.StartDocument();
                currentY = marginTop + headerHeight;
            }

            public void ConditionalPageBreak(float AHeightToAdd)
            {
                if (currentY + AHeightToAdd >= pageHeight - footerHeight - headerHeight)
                {
                    currentY = 0;

                    AddPageBreak();
                }
            }

            public override void AddPageBreak()
            {
                base.AddPageBreak();

                currentY = marginTop + headerHeight;
            }

            public void PrintDocument(SortedList <string, List <string>>AData)
            {
                XmlNode GroupTopNode = TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "groupTop");
                XmlNode GroupBottomNode = TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "groupBottom");
                XmlNode DetailNode = TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "detail");

                int TotalOverall = 0;

                foreach (string group in AData.Keys)
                {
                    List <int>Total = new List <int>();

                    ConditionalPageBreak(groupTopHeight + detailHeight * 3);

                    string GroupTopText = GroupTopNode.InnerXml;
                    GroupTopText = GroupTopText.Replace("#GROUPNAME", group);

                    ResultDocument += Environment.NewLine +
                                      String.Format("<div style='position:absolute, left:{0}{2}, top:{1}{2}'>",
                        marginLeft,
                        currentY,
                        unit);
                    ResultDocument += GroupTopText;
                    ResultDocument += "</div>" + Environment.NewLine;
                    currentY += groupTopHeight;

                    foreach (string line in AData[group])
                    {
                        string DetailText = DetailNode.InnerXml;

                        string CSVLine = line;
                        int CountCSVValue = 1;

                        while (CSVLine.Length > 0)
                        {
                            string value = StringHelper.GetNextCSV(ref CSVLine);
                            DetailText = DetailText.Replace("#VALUE" + CountCSVValue.ToString(), value);

                            if (Total.Count < CountCSVValue)
                            {
                                Total.Add(0);
                            }

                            if (value.Length > 0)
                            {
                                Total[CountCSVValue - 1]++;
                            }

                            CountCSVValue++;
                        }

                        ConditionalPageBreak(detailHeight);

                        ResultDocument += Environment.NewLine +
                                          String.Format("<div style='position:absolute, left:{0}{2}, top:{1}{2}'>",
                            marginLeft,
                            currentY,
                            unit);
                        ResultDocument += DetailText;
                        ResultDocument += "</div>" + Environment.NewLine;
                        currentY += detailHeight;
                    }

                    ConditionalPageBreak(groupBottomHeight);
                    string GroupBottomText = GroupBottomNode.InnerXml;
                    GroupBottomText = GroupBottomText.Replace("#TOTALPERGROUP", AData[group].Count.ToString());
                    TotalOverall += AData[group].Count;

                    for (int TotalCount = 0; TotalCount < Total.Count; TotalCount++)
                    {
                        GroupBottomText = GroupBottomText.Replace("#TOTAL" + (TotalCount + 1).ToString(), Total[TotalCount].ToString());
                    }

                    ResultDocument += Environment.NewLine +
                                      String.Format("<div style='position:absolute, left:{0}{2}, top:{1}{2}'>",
                        marginLeft,
                        currentY,
                        unit);
                    ResultDocument += GroupBottomText;
                    ResultDocument += "</div>" + Environment.NewLine;
                    currentY += groupBottomHeight;
                }

                ConditionalPageBreak(reportFooterHeight);

                if ((ResultDocument.Length > 0) && (ReportFooterNode != null))
                {
                    string footer = ReportFooterNode.OuterXml;
                    footer = footer.Replace("#TOTALOVERALL", TotalOverall.ToString());

                    ResultDocument += Environment.NewLine +
                                      String.Format("<div style='position:absolute, left:{0}{2}, top:{1}{2}'>",
                        marginLeft,
                        currentY,
                        unit);
                    ResultDocument += footer;
                    ResultDocument += "</div>" + Environment.NewLine;
                    currentY += groupBottomHeight;
                }

                AddFooter();
            }
        }


        /// <summary>
        /// print HTML labels into an HTML template, create pages
        /// </summary>
        /// <param name="AFilename"></param>
        /// <param name="ALabels"></param>
        /// <param name="ATitle">title to print in the page footer</param>
        /// <returns></returns>
        public static string PrintLabels(string AFilename, List <string>ALabels, string ATitle)
        {
            THTMLFormLabels formletter = new THTMLFormLabels(AFilename);

            formletter.StartDocument();

            formletter.PrintDocument(ALabels);

            formletter.FinishDocument();

            formletter.ResultDocument = formletter.ResultDocument.Replace("#TITLE", ATitle);

            return formletter.ResultDocument;
        }

        /// <summary>
        /// print a report into an HTML template
        /// </summary>
        /// <param name="AFilename"></param>
        /// <param name="AData"></param>
        /// <param name="ATitle">title to print in the page footer</param>
        /// <returns></returns>
        public static string PrintReport(string AFilename, SortedList <string, List <string>>AData, string ATitle)
        {
            THTMLFormReport formletter = new THTMLFormReport(AFilename);

            formletter.StartDocument();

            formletter.PrintDocument(AData);

            formletter.FinishDocument();

            formletter.ResultDocument = formletter.ResultDocument.Replace("#TITLE", ATitle);

            return formletter.ResultDocument;
        }

        /// check for all image paths, if the images actually exist
        public static bool CheckImagesFileExist(string AHTMLText)
        {
            string CheckImages = AHTMLText;
            int IndexImg;
            bool MissingImage = false;

            while ((IndexImg = CheckImages.ToLower().IndexOf("img src=")) != -1)
            {
                CheckImages = CheckImages.Substring(IndexImg + 1);

                int PosQuote1 = CheckImages.IndexOf('"');
                int LengthPath = CheckImages.Substring(PosQuote1 + 1).IndexOf('"');

                string path = CheckImages.Substring(PosQuote1 + 1, LengthPath);

                if (!File.Exists(path))
                {
                    TLogging.Log("Cannot find path " + path);
                    MissingImage = true;
                }
            }

            return !MissingImage;
        }

        /// <summary>
        /// close the document after one or several pages have been inserted.
        /// </summary>
        /// <param name="AResultDocument"></param>
        /// <returns>false if the document is empty</returns>
        public static bool CloseDocument(ref string AResultDocument)
        {
            if (AResultDocument.Length > 0)
            {
                AResultDocument += "</html>";
                return true;
            }

            return false;
        }

        /// <summary>
        /// generate a PDF from an HTML Document, can contain several pages
        /// </summary>
        /// <returns>path of the temporary PDF file</returns>
        public static string GeneratePDFFromHTML(string AHTMLDoc, string APdfPath)
        {
            if (AHTMLDoc.Length == 0)
            {
                return string.Empty;
            }

            if (!Directory.Exists(APdfPath))
            {
                Directory.CreateDirectory(APdfPath);
            }

            Random rand = new Random();
            string filename = string.Empty;

            do
            {
                filename = APdfPath + Path.DirectorySeparatorChar +
                           rand.Next(1, 1000000).ToString() + ".txt";
            } while (File.Exists(filename));

            TLogging.Log(filename);

            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine(AHTMLDoc);
            sw.Close();

            try
            {
                PrintDocument doc = new PrintDocument();

                TPdfPrinter pdfPrinter = new TPdfPrinter(doc, TGfxPrinter.ePrinterBehaviour.eFormLetter);
                TPrinterHtml htmlPrinter = new TPrinterHtml(AHTMLDoc,
                    String.Empty,
                    pdfPrinter);

                pdfPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);

                pdfPrinter.SavePDF(filename.Replace(".txt", ".pdf"));
            }
            catch (Exception e)
            {
                TLogging.Log("Exception while printing badge: " + e.Message);
                TLogging.Log(e.StackTrace);
                throw e;
            }

            return filename.Replace(".txt", ".pdf");
        }
    }
}