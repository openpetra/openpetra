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

        /// <summary>
        /// print HTML labels into an HTML template, create pages
        /// </summary>
        /// <param name="AFilename"></param>
        /// <param name="ALabels"></param>
        /// <returns></returns>
        public static string PrintLabels(string AFilename, List <string>ALabels)
        {
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

            XmlDocument TemplateDoc = new XmlDocument();
            TemplateDoc.LoadXml(Template);

            XmlNode bodyNode = TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "body");
            Dictionary <string, string>styles = GetStyles(bodyNode);

            string unit = "in";

            if (styles["margin-left"].EndsWith("cm"))
            {
                unit = "cm";
            }

            CultureInfo OrigCulture = Catalog.SetCulture(CultureInfo.InvariantCulture);

            // we need to use the margin-left and margin-top of the body for the position of the first label
            float marginLeft = (float)Convert.ToDouble(styles["margin-left"].Replace(unit, ""));
            float marginTop = (float)Convert.ToDouble(styles["margin-top"].Replace(unit, ""));

            // we need the width and height of body to know how many labels will fit on one page
            float pageWidth = (float)Convert.ToDouble(styles["width"].Replace(unit, ""));
            float pageHeight = (float)Convert.ToDouble(styles["height"].Replace(unit, ""));

            // we need padding-left and padding-bottom for the space between labels
            styles = GetStyles(TXMLParser.FindNodeRecursive(TemplateDoc.DocumentElement, "div", "label"));
            float paddingLeft = (float)Convert.ToDouble(styles["padding-left"].Replace(unit, ""));
            float paddingBottom = (float)Convert.ToDouble(styles["padding-bottom"].Replace(unit, ""));
            float labelWidth = (float)Convert.ToDouble(styles["width"].Replace(unit, ""));
            float labelHeight = (float)Convert.ToDouble(styles["height"].Replace(unit, ""));

            int maxLabelsHorizontal = (int)Math.Floor((pageWidth - marginLeft) / (labelWidth + paddingLeft));
            int maxLabelsVertical = (int)Math.Floor((pageHeight - marginTop) / (labelHeight + paddingBottom));

            int currentLabelX = 0;
            int currentLabelY = 0;

            string ResultDocument =
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">" +
                Environment.NewLine +
                "<html><body>" + Environment.NewLine;

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
                    ResultDocument += "</body><body>" + Environment.NewLine;
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

            ResultDocument += "</body></html>";

            Catalog.SetCulture(OrigCulture);

            return ResultDocument;
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
    }
}