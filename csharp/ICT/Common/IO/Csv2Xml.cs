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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.IO;
using Mono.Unix;

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

            // first write the header of the csv file
            List <string>AllAttributes = new List <string>();
            List <XmlNode>AllNodes = new List <XmlNode>();
            GetAllAttributesAndNodes(ADoc.DocumentElement, ref AllAttributes, ref AllNodes);

            string separator = TAppSettingsManager.GetValueStatic("CSVSeparator",
                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);

            string headerLine = "";

            foreach (string attrName in AllAttributes)
            {
                headerLine = StringHelper.AddCSV(headerLine, "#" + attrName, separator);
            }

            sw.WriteLine(headerLine);

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

                sw.WriteLine(line);
            }

            sw.Close();

            return true;
        }

        /// <summary>
        /// convert a CSV file to an XmlDocument
        /// </summary>
        /// <param name="ACSVFilename"></param>
        /// <returns></returns>
        public static XmlDocument ParseCSV2Xml(string ACSVFilename)
        {
            XmlDocument myDoc = TYml2Xml.CreateXmlDocument();

            StreamReader sr = new StreamReader(ACSVFilename);

            string headerLine = sr.ReadLine();

            if (!headerLine.StartsWith("#"))
            {
                throw new Exception(Catalog.GetString("Cannot open CSV file, because it is missing the header line"));
            }

            // read separator from header line
            string separator = headerLine[headerLine.IndexOf("#", 2) - 1].ToString();

            List <string>AllAttributes = new List <string>();

            while (headerLine.Length > 0)
            {
                string attrName = StringHelper.GetNextCSV(ref headerLine, separator);

                if (attrName.StartsWith("#"))
                {
                    AllAttributes.Add(attrName.Substring(1));
                }
                else
                {
                    throw new Exception(
                        String.Format(Catalog.GetString("Cannot parse CSV file header of file {0}, caption {1} is missing the #"),
                            ACSVFilename, attrName));
                }
            }

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                SortedList <string, string>AttributePairs = new SortedList <string, string>();

                foreach (string attrName in AllAttributes)
                {
                    AttributePairs.Add(attrName, StringHelper.GetNextCSV(ref line, separator));
                }

                XmlNode newNode = myDoc.CreateElement("", AttributePairs["name"], "");

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

                foreach (string attrName in AllAttributes)
                {
                    if ((attrName != "name") && (attrName != "childOf"))
                    {
                        XmlAttribute attr = myDoc.CreateAttribute(attrName);
                        attr.Value = AttributePairs[attrName];
                        newNode.Attributes.Append(attr);
                    }
                }
            }

            return myDoc;
        }
    }
}