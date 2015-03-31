//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2015 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.IO
{
    /// <summary>
    /// TYml2Xml is able to parse a YML file and store it in an XmlDocument
    ///
    /// Simple YAML to XML converter
    /// could have also used http://yaml-net-parser.sourceforge.net/default.html#intro
    /// But I prefer to write YAML files but to work with XML in the program.
    /// The yaml net parser would just be another interface to understand how to step through the document.
    ///
    /// See also the spec http://yaml.org/spec/1.2/
    ///
    /// We only support:
    ///   indentation: http://yaml.org/spec/1.2/#id2577368
    ///        only use spaces, no tabs for indentation
    ///        Each node must be indented further than its parent node.
    ///        All sibling nodes must use the exact same indentation level.
    ///
    ///        comment lines start with #, can be indented
    ///        separators are comma and colon, lists are defined by curly and square brackets {} and []
    ///        values can be quoted to escape the colon or comma
    ///
    ///        name and colon (name:) is converted into an XML element as a parent for the following indented lines
    ///          indention only happens after node and colon without content
    ///        scalar: name and colon and a literal is converted into an attribute of the current XML element
    ///        sequence: [element1, element2, element3] are translated to &lt;element name=&quot;element1&quot;/&gt;&lt;element name=&quot;element2&quot;/&gt; below the parent node
    ///        mapping: name and value assignments {size=10, help=Test with spaces} are converted into attributes of the parent node
    ///
    ///        not supported:
    ///           node names cannot be quoted to escape colons
    ///           {params: size=10, help=Test with spaces}: leave out the params
    ///           [=name, =id, address] sorting columns are not identified this way
    ///
    ///        This class supports the loading of several derived files,
    ///        and overwriting the data of the base files;
    ///        todo: make a tag so that everything is pushed into base, before the last file is loaded
    ///              change the code: move only attributes into base tag
    ///                               add an attribute to elements that have been in base, base=&quot;yes&quot;
    ///                               this way the order of elements can be maintained easily,
    ///                               but it is still known which elements and attributes have been added since the tag
    ///              reactivate the check again, in ProcessXAML.cs: if (TXMLParser.GetAttribute(rootNode, &quot;ClassType&quot;) != &quot;abstract&quot;)
    ///        todo: save the last file after modification have been done to the xml structure (eg. loaded from csharp file)
    /// </summary>
    public class TYml2Xml
    {
        /// <summary>
        /// contains the lines of the yml document
        /// </summary>
        protected string[] lines = null;
        /// <summary>
        /// the current line that we are parsing
        /// </summary>
        protected Int32 currentLine = -1;
        /// <summary>
        /// the filename of the file that we are parsing
        /// </summary>
        protected string filename = "";

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AFilename"></param>
        public TYml2Xml(string AFilename)
        {
            filename = AFilename;

            // file should be in unicode encoding
            // StreamReader DetectEncodingFromByteOrderMarks does not work for ANSI?
            TextReader reader = new StreamReader(filename, TTextFile.GetFileEncoding(filename), false);
            lines = reader.ReadToEnd().Replace("\r", "").Split(new char[] { '\n' });
            reader.Close();
            currentLine = -1;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TYml2Xml(string[] ALines)
        {
            filename = String.Empty;
            lines = ALines;
            currentLine = -1;
        }

        /// <summary>
        /// return either the name from the attribute, or the element name
        /// </summary>
        public static string GetElementName(XmlNode ANode)
        {
            if (TXMLParser.HasAttribute(ANode, "name"))
            {
                return TXMLParser.GetAttribute(ANode, "name");
            }

            return ANode.Name;
        }

        /// <summary>
        /// check if the name can be used as an element name for XML
        /// </summary>
        public static bool CheckName(string AElementName)
        {
            if ((AElementName.Length > 0) && Char.IsDigit(AElementName[0]))
            {
                return false;
            }

            try
            {
                new XmlDocument().CreateElement(AElementName);
                return true;
            }
            catch (XmlException)
            {
                return false;
            }
        }

        private static Int32 DEFAULTINDENT = 4;
        private static void WriteXmlNode2Yml(StringBuilder AYmlDocument,
            Int32 ACurrentIndent,
            XmlNode ANode,
            SortedList <string, string>AInheritedAttributes)
        {
            string Indent = "".PadLeft(ACurrentIndent);

            if (ACurrentIndent == 0)
            {
                if (ROOTNODEINTERNAL.StartsWith(ANode.Name))
                {
                    AYmlDocument.Append(ANode.Name + ":");
                }
                else
                {
                    AYmlDocument.Append(ROOTNODEINTERNAL + ":");
                }
            }
            else
            {
                if ((ANode.Name == XMLELEMENT) && TXMLParser.HasAttribute(ANode, "name"))
                {
                    AYmlDocument.Append(Indent + TXMLParser.GetAttribute(ANode, "name") + ":");
                }
                else
                {
                    AYmlDocument.Append(Indent + ANode.Name + ":");
                }
            }

            // only write attributes if they are different from the parent node;
            // always write explicitly defined empty attributes
            List <XmlAttribute>attributesToWrite = new List <XmlAttribute>();

            foreach (XmlAttribute attr in ANode.Attributes)
            {
                if ((ANode.ParentNode == null) || (attr.Value == String.Empty)
                    || !AInheritedAttributes.ContainsKey(attr.Name)
                    || (AInheritedAttributes[attr.Name] != attr.Value))
                {
                    if (!((ANode.Name == XMLELEMENT) && (attr.Name == "name")) && (attr.Name != "depth"))
                    {
                        attributesToWrite.Add(attr);
                    }
                }
            }

            if (ACurrentIndent == 0)
            {
                AYmlDocument.Append(Environment.NewLine);

                // for the root node, write all attributes into a separate line
                foreach (XmlAttribute attrToWrite in attributesToWrite)
                {
                    attrToWrite.Value = attrToWrite.Value.Replace("\\", "\\\\");
                    attrToWrite.Value = attrToWrite.Value.Replace("\r", "").Replace("\n", "\\\\n");

                    string value = attrToWrite.Value;

                    if (value.Contains(",") || value.Contains(":") || value.Contains("=")
                        || value.Contains("\"") || value.Contains("#"))
                    {
                        value = "\"" + value.Replace("\"", "\\\"") + "\"";
                    }

                    AYmlDocument.Append("".PadLeft(ACurrentIndent + DEFAULTINDENT) + attrToWrite.Name + ":" + value + Environment.NewLine);
                }
            }
            else
            {
                StringBuilder attributesYml = new StringBuilder(400);
                bool firstAttribute = true;

                foreach (XmlAttribute attrToWrite in attributesToWrite)
                {
                    if (!firstAttribute)
                    {
                        attributesYml.Append(", ");
                    }

                    firstAttribute = false;

                    attributesYml.Append(attrToWrite.Name + "=");

                    attrToWrite.Value = attrToWrite.Value.Replace("\\", "\\\\");
                    attrToWrite.Value = attrToWrite.Value.Replace("\r", "").Replace("\n", "\\\\n");

                    if (attrToWrite.Value.Contains(",") || attrToWrite.Value.Contains(":") || attrToWrite.Value.Contains("=")
                        || attrToWrite.Value.Contains("\"") || attrToWrite.Value.Contains("#"))
                    {
                        attributesYml.Append("\"" + attrToWrite.Value.Replace("\"", "\\\"") + "\"");
                    }
                    else
                    {
                        attributesYml.Append(attrToWrite.Value);
                    }
                }

                if (!firstAttribute)
                {
                    AYmlDocument.Append("{" + attributesYml.ToString() + "}" + Environment.NewLine);
                }
                else
                {
                    AYmlDocument.Append(Environment.NewLine);
                }
            }

            foreach (XmlNode childNode in ANode.ChildNodes)
            {
                if (childNode.HasChildNodes && (childNode.FirstChild.Name == XMLLIST))
                {
                    // write a list of values, eg: mylist: [test1, test2, test3]
                    AYmlDocument.Append("".PadLeft(ACurrentIndent + DEFAULTINDENT) + childNode.Name + ": [");

                    foreach (XmlNode elementNode in childNode.ChildNodes)
                    {
                        if (elementNode != childNode.FirstChild)
                        {
                            AYmlDocument.Append(", ");
                        }

                        AYmlDocument.Append(TXMLParser.GetAttribute(elementNode, "name"));
                    }

                    AYmlDocument.Append("]" + Environment.NewLine);
                    continue;
                }

                // make a deep copy of the sorted list, so that we don't modify the original list
                SortedList <string, string>NewAttributesList = new SortedList <string, string>();

                foreach (string key in AInheritedAttributes.Keys)
                {
                    NewAttributesList.Add(key, AInheritedAttributes[key]);
                }

                // add only the attributes written in this loop
                foreach (XmlAttribute attrToWrite in attributesToWrite)
                {
                    if (NewAttributesList.ContainsKey(attrToWrite.Name))
                    {
                        NewAttributesList[attrToWrite.Name] = attrToWrite.Value;
                    }
                    else
                    {
                        NewAttributesList.Add(attrToWrite.Name, attrToWrite.Value);
                    }
                }

                WriteXmlNode2Yml(AYmlDocument, ACurrentIndent + DEFAULTINDENT, childNode, NewAttributesList);
            }
        }

        /// <summary>
        /// format the XML into zipped YML and return as Base64 string
        /// </summary>
        public static string Xml2YmlGz(XmlDocument ADoc)
        {
            StringBuilder sb = new StringBuilder(1024 * 1024 * 5);

            WriteXmlNode2Yml(sb, 0, ADoc.DocumentElement, new SortedList <string, string>());

            return PackTools.ZipString(sb.ToString());
        }

        /// <summary>
        /// format the XML into YML to increase readability and save to file
        /// </summary>
        public static bool Xml2Yml(XmlDocument ADoc, string AOutYMLFile)
        {
            StringBuilder sb = new StringBuilder(1024 * 1024 * 5);

            if ((ADoc.DocumentElement.Name == ROOTNODEINTERNAL) && ROOTNODEINTERNAL.StartsWith(ADoc.DocumentElement.FirstChild.Name))
            {
                WriteXmlNode2Yml(sb, 0, ADoc.DocumentElement.FirstChild, new SortedList <string, string>());
            }
            else
            {
                WriteXmlNode2Yml(sb, 0, ADoc.DocumentElement, new SortedList <string, string>());
            }

            StreamWriter sw = new StreamWriter(AOutYMLFile, false, System.Text.Encoding.UTF8);

            sw.Write(sb.ToString());

            sw.Close();

            return true;
        }

        /// <summary>
        /// This method can be used to check if this is the correct file type,
        /// and also find out the baseyaml or baseclass
        /// </summary>
        /// <returns>false if the yaml file cannot be read or interpreted</returns>
        public static bool ReadHeader(string AFilename, out string baseYamlOrClass)
        {
            // file should be in unicode encoding
            TextReader reader = new StreamReader(AFilename, true);

            baseYamlOrClass = "";
            string line = reader.ReadLine();

            while ((line != null) && line.Trim().StartsWith("#"))
            {
                line = reader.ReadLine();
            }

            // first line should be RootNode
            line = reader.ReadLine();

            // second line should be the BaseYaml, if it is there, or BaseClass
            reader.Close();

            if (line != null)
            {
                string nodeName, nodeContent;

                if (SplitNodeStatic(line, out nodeName, out nodeContent))
                {
                    if (nodeName == "BaseYaml")
                    {
                        baseYamlOrClass = nodeContent;
                    }
                    else if (nodeName == "BaseClass")
                    {
                        baseYamlOrClass = nodeContent;
                    }
                }
            }

            return baseYamlOrClass.Length > 0;
        }

        /// throw an exception and tell the current position while reading the yaml file
        protected void ThrowException(string AMessage, Int32 lineNr)
        {
            throw new Exception("Error in Yml2Xml: " + AMessage + "; " +
                "(file: " + filename + " line " + (lineNr + 1).ToString() + ")");
        }

        /// returns -1 for comments or invalid line numbers,
        /// otherwise the number of spaces
        /// throws exception if there is a tab in the indentation
        protected Int32 GetAbsoluteIndentation(Int32 lineNr)
        {
            if ((lineNr < 0) || (lineNr >= lines.Length))
            {
                return -1;
            }

            string line = lines[lineNr];

            if (line.Trim().StartsWith("#") || (line.Trim().Length == 0))
            {
                return -1;
            }

            Int32 counterSpaces = 0;

            while (counterSpaces < line.Length)
            {
                if ((byte)line[counterSpaces] == 9)
                {
                    ThrowException("There is a Tab character in the indentation", lineNr);
                }

                if (line[counterSpaces] == ' ')
                {
                    counterSpaces++;
                }
                else
                {
                    break;
                }
            }

            return counterSpaces;
        }

        /// @returns the absolute indentation of the next valid line (skipping comments)
        protected Int32 GetAbsoluteIndentationNext(Int32 lineNr)
        {
            Int32 nextIndentation = -1;

            while (lineNr < lines.Length - 1 && nextIndentation == -1)
            {
                lineNr++;
                nextIndentation = GetAbsoluteIndentation(lineNr);
            }

            return nextIndentation;
        }

        /// @returns +1 if next line is more indented than the current line (skipping comments),
        /// -1 if next line is less indented, or current line is last line
        /// 0 if current line has same identation than the next line
        protected Int32 GetIndentationNext(Int32 lineNr)
        {
            Int32 currentIndentation = GetAbsoluteIndentation(lineNr);

            if (currentIndentation == -1)
            {
                ThrowException("Invalid request for relative indentation", lineNr);
            }

            Int32 nextIndentation = -1;

            while (lineNr < lines.Length - 1 && nextIndentation == -1)
            {
                lineNr++;
                nextIndentation = GetAbsoluteIndentation(lineNr);
            }

            if ((nextIndentation == -1) || (nextIndentation < currentIndentation))
            {
                return -1;
            }
            else if (nextIndentation > currentIndentation)
            {
                return +1;
            }

            return 0;
        }

        /// <summary>
        /// does not return comment lines
        /// </summary>
        /// <returns>null if no line available</returns>
        protected string GetNextLine()
        {
            string line = null;

            currentLine++;

            while (currentLine > -1 && currentLine < lines.Length && line == null)
            {
                line = lines[currentLine];

                // skip over the comment lines
                if (GetAbsoluteIndentation(currentLine) == -1)
                {
                    line = null;
                    currentLine++;
                }
            }

            return line;
        }

        /// <summary>
        /// split a line into the node name and the node content
        /// </summary>
        protected bool SplitNode(string line, out string nodeName, out string nodeContent)
        {
            Int32 posFirstColon = -1;

            if (line != null)
            {
                posFirstColon = line.IndexOf(':');
            }

            if (posFirstColon == -1)
            {
                nodeName = "";
                nodeContent = "";
                Console.WriteLine(line);
                ThrowException("cannot find a colon in line", currentLine);
                return false;
            }

            nodeName = line.Substring(0, posFirstColon).Trim();

            if (posFirstColon == line.Length - 1)
            {
                // colon is the last character in the line
                nodeContent = "";
            }
            else
            {
                nodeContent = line.Substring(posFirstColon + 1).Trim();
            }

            return true;
        }

        /// <summary>
        /// same as SplitNode, but without an Exception, because that requires attributes from the class
        /// </summary>
        /// <param name="line"></param>
        /// <param name="nodeName"></param>
        /// <param name="nodeContent"></param>
        /// <returns></returns>
        private static bool SplitNodeStatic(string line, out string nodeName, out string nodeContent)
        {
            Int32 posFirstColon = -1;

            if (line != null)
            {
                posFirstColon = line.IndexOf(':');
            }

            if (posFirstColon == -1)
            {
                nodeName = "";
                nodeContent = "";
                Console.WriteLine(line);

                // does not work in static mode: ThrowException("cannot find a colon in line", currentLine);
                return false;
            }

            nodeName = line.Substring(0, posFirstColon).Trim();

            if (posFirstColon == line.Length - 1)
            {
                // colon is the last character in the line
                nodeContent = "";
            }
            else
            {
                nodeContent = line.Substring(posFirstColon + 1).Trim();
            }

            return true;
        }

        /// <summary>
        /// strip the quotes from a string
        /// </summary>
        protected string StripQuotes(string s)
        {
            if ((s != null) && (s.Length > 1))
            {
                if (s.StartsWith("\"") && s.EndsWith("\""))
                {
                    s = s.Substring(1, s.Length - 2);
                }
            }

            return s;
        }

        private static bool PrintedOriginalError = false;

        /// <summary>
        /// this function just parses a YML file into an empty XmlDocument (created with CreateXmlDocument).
        /// No merging of documents is done here.
        /// </summary>
        private void ParseNode(XmlNode parent, int ADepth)
        {
            string line = GetNextLine();

            if (line != null)
            {
                string nodeName, nodeContent;

                // strip end of line comment, starting with #, that is not inside a quotation
                if (line.LastIndexOf('#') > line.LastIndexOf('"'))
                {
                    line = line.Substring(0, line.LastIndexOf('#'));
                }

                try
                {
                    if (SplitNode(line, out nodeName, out nodeContent))
                    {
                        XmlNode newElement = null;

                        if (!CheckName(nodeName))
                        {
                            // work around invalid XML element name
                            newElement = TYml2Xml.LoadChild(parent, TYml2Xml.XMLELEMENT, ADepth);
//                            newElement = parent.OwnerDocument.CreateElement("", TYml2Xml.XMLELEMENT, "");
                            ((XmlElement)newElement).SetAttribute("name", nodeName);
                        }
                        else
                        {
                            newElement = TYml2Xml.LoadChild(parent, nodeName, ADepth);
//                            newElement = parent.OwnerDocument.CreateElement("", nodeName, "");
                        }

                        ((XmlElement)newElement).SetAttribute("depth", ADepth.ToString());

                        if (nodeContent.Length > 0)
                        {
                            // there is some content directly in the line
                            // can be scalar, sequence, or mapping
                            if (nodeContent.StartsWith("{"))
                            {
                                if (!nodeContent.EndsWith("}"))
                                {
                                    throw new Exception("missing closing curly bracket");
                                }

                                // mapping
                                // first get the list without brackets
                                string list = nodeContent.Substring(1, nodeContent.Length - 2).Trim();

                                // now use getNextCSV which is able to deal with quoted strings
                                while (list.Length > 0)
                                {
                                    string mapping = StringHelper.GetNextCSV(ref list, ",");
                                    string mappingName = StringHelper.GetNextCSV(ref mapping, new string[] { "=", ":" }).Trim();
                                    string mappingValue = StripQuotes(mapping.Trim()).Replace("\\n", Environment.NewLine);
                                    try
                                    {
                                        TYml2Xml.SetAttribute(newElement, mappingName, mappingValue);
                                    }
                                    catch (XmlException e)
                                    {
                                        ThrowException(e.Message, currentLine);
                                    }
                                }
                            }
                            else if (nodeContent.StartsWith("["))
                            {
                                if (!nodeContent.EndsWith("]"))
                                {
                                    throw new Exception("missing closing square bracket");
                                }

                                TYml2Xml.SetAttribute(newElement, "IsYMLSequence", "true");

                                // sequence
                                // first get the list without brackets
                                string list = nodeContent.Substring(1, nodeContent.Length - 2).Trim();

                                // now use getNextCSV which is able to deal with quoted strings
                                while (list.Length > 0)
                                {
                                    string value = StripQuotes(StringHelper.GetNextCSV(ref list, ",").Trim());

                                    bool negated = false;

                                    if (value[0] == '~')
                                    {
                                        negated = true;
                                        value = value.Substring(1);
                                    }

                                    XmlElement sequenceElement = parent.OwnerDocument.CreateElement("", TYml2Xml.XMLLIST, "");
                                    newElement.AppendChild(sequenceElement);
                                    ((XmlElement)sequenceElement).SetAttribute("name", value);

                                    if (negated)
                                    {
                                        ((XmlElement)sequenceElement).SetAttribute("negated", "true");
                                    }
                                }
                            }
                            else
                            {
                                // scalar
                                // this should not be an element, but an attribute of the parent
                                parent.RemoveChild(newElement);
                                nodeContent = nodeContent.Trim().Replace("\\n", "\n");

                                if (nodeContent.StartsWith("\"") && nodeContent.EndsWith("\""))
                                {
                                    nodeContent = nodeContent.Substring(1, nodeContent.Length - 2);
                                }

                                TYml2Xml.SetAttribute(parent, nodeName, nodeContent);
                            }
                        }

                        // this is a parent node, so read the child nodes as well
                        if (GetIndentationNext(currentLine) > 0)
                        {
                            // There are children
                            Int32 childrenAbsoluteIndentation = GetAbsoluteIndentationNext(currentLine);

                            do
                            {
                                ParseNode(newElement, ADepth);
                            } while (GetAbsoluteIndentationNext(currentLine) == childrenAbsoluteIndentation);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!PrintedOriginalError)
                    {
                        TLogging.Log("Problem in line " + currentLine.ToString() + " " + line);

                        if (TLogging.DebugLevel > 0)
                        {
                            TLogging.Log(e.StackTrace);
                        }

                        PrintedOriginalError = true;
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// this function merges two XML documents.
        /// this function does not know and care about base and hierarchical structures; use Tag for that.
        /// only for one thing it needs to consider base: check if the leaf to be added is already in base
        /// </summary>
        private static void MergeNode(XmlNode parent, XmlNode ANewNode, int ADepth)
        {
            // check if the node already exists
            // at this stage existing elements are just modified and added to
            // to get different versions, use the method Tag()
            // TYml2Xml.LoadChild will either
            // reuse an element, move an existing leaf from base to the main node, or create a new element
            XmlNode newElement = TYml2Xml.LoadChild(parent, ANewNode.Name, ADepth);

            // Sequences
            if (TXMLParser.GetAttribute(ANewNode, "IsYMLSequence") == "true")
            {
                // if we find one value that is already part of the base list, then we want to overwrite the whole list
                StringCollection baseElements = TYml2Xml.GetElements(newElement);

                foreach (XmlNode element in ANewNode.ChildNodes)
                {
                    string elementName = element.Name;

                    if (element.Attributes["name"] != null)
                    {
                        elementName = element.Attributes["name"].Value;
                    }

                    if (baseElements.Contains(elementName)
                        && (element.Attributes["negated"] == null))
                    {
                        baseElements.Clear();
                        newElement.RemoveAll();
                        break;
                    }
                }

                // add elements
                foreach (XmlNode element in ANewNode.ChildNodes)
                {
                    string elementName = element.Name;

                    if (element.Attributes["name"] != null)
                    {
                        elementName = element.Attributes["name"].Value;
                    }

                    // remove negated element from the list
                    if (element.Attributes["negated"] != null)
                    {
                        if (baseElements.Contains(elementName))
                        {
                            // newElement.RemoveChild(TXMLParser.FindNodeRecursive(newElement, "Element", elementName));
                            List <XmlNode>children = TYml2Xml.GetChildren(newElement, false);

                            foreach (XmlNode childNode in children)
                            {
                                if (childNode.Attributes["name"].Value == elementName)
                                {
                                    newElement.RemoveChild(childNode);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        XmlElement sequenceElement = parent.OwnerDocument.CreateElement("", TYml2Xml.XMLLIST, "");
                        newElement.AppendChild(sequenceElement);
                        ((XmlElement)sequenceElement).SetAttribute("name", elementName);
                    }
                }
            }
            else
            {
                // Attributes
                foreach (XmlAttribute attr in ANewNode.Attributes)
                {
                    TYml2Xml.SetAttribute(newElement, attr.Name, attr.Value);
                }

                // Children
                if (ANewNode.HasChildNodes)
                {
                    foreach (XmlNode child in ANewNode.ChildNodes)
                    {
                        MergeNode(newElement, child, ADepth);
                    }
                }
            }
        }

        private static void AddToSortedList(ref SortedList xmlNodes, XmlNode currentNode)
        {
            // don't add duplicate names; e.g. element, controls, etc
            // we will add the first one, but not the second
            if (xmlNodes.IndexOfKey(currentNode.Name) != -1)
            {
                return;
            }

            xmlNodes.Add(currentNode.Name, currentNode);
            currentNode = currentNode.FirstChild;

            while (currentNode != null)
            {
                AddToSortedList(ref xmlNodes, currentNode);
                currentNode = currentNode.NextSibling;
            }
        }

        /// fill sorted list which contains a reference to each node by name
        public static SortedList ReferenceNodes(XmlDocument myDoc)
        {
            SortedList xmlNodes = new SortedList();

            // get the XmlDeclaration section first
            XmlNode xmlnode = myDoc.FirstChild;

            // get the first element
            AddToSortedList(ref xmlNodes, xmlnode.NextSibling);
            return xmlNodes;
        }

        /// the name used for elements in a list
        public static string XMLLIST = "XmlList";

        /// the name used for elements in generated xml code
        public static string XMLELEMENT = "XmlElement";

        /// the name used for root node in generated xml code
        public static string ROOTNODEINTERNAL = "RootNodeInternal";

        /// create an empty xml document, which will be filled with the data from the yaml file
        static public XmlDocument CreateXmlDocument()
        {
            XmlDocument myDoc = new XmlDocument();

            // add the XML declaration section
            XmlDeclaration xmlDeclaration = myDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            myDoc.InsertBefore(xmlDeclaration, myDoc.DocumentElement);

            // it is necessary to have this root node without attributes, to comply with the xml documents generated by TYML2XML
            XmlElement rootNode = myDoc.CreateElement(ROOTNODEINTERNAL);
            myDoc.AppendChild(rootNode);

            return myDoc;
        }

        /// <summary>
        /// loads an yml document into one xml document
        /// </summary>
        /// <returns></returns>
        public XmlDocument ParseYML2XML()
        {
            XmlDocument myDoc = CreateXmlDocument();
            XmlNode root = myDoc.FirstChild.NextSibling;

            PrintedOriginalError = false;

            try
            {
                // recursive parsing of the yml document
                ParseNode(root, 0);
            }
            catch (Exception e)
            {
                TLogging.Log("Error while parsing yml, line " + currentLine.ToString() + ". " + e.Message);

                if (TLogging.DebugLevel > 0)
                {
                    TLogging.Log(e.StackTrace);
                }

                throw;
            }

            return myDoc;
        }

        /// <summary>
        /// loads an yml document into one xml document; then returns only the part meant
        /// to be passed directly to a TaskList constructor as the TaskList root.
        /// </summary>
        /// <returns></returns>
        public XmlNode ParseYML2TaskListRoot()
        {
            XmlDocument xmldoc = ParseYML2XML();

            return (XmlNode)xmldoc.FirstChild.NextSibling.FirstChild;
        }

        /// <summary>
        /// merges several yml documents into one xml document, and maintains their inheritance hierarchy
        /// for nodes that have the same name
        /// </summary>
        /// <param name="AMergeDoc"></param>
        /// <param name="ADepth"></param>
        /// <returns></returns>
        public XmlDocument ParseYML2XML(XmlDocument AMergeDoc, int ADepth)
        {
            XmlDocument newDoc = ParseYML2XML();

            Merge(ref AMergeDoc, newDoc, ADepth);

            return AMergeDoc;
        }

        /// <summary>
        /// merge the specificDoc into the mergeDoc.
        /// merges 2 xml documents, maintains their inheritance hierarchy
        /// for nodes that have the same name.
        /// allows caching of base yaml files.
        /// </summary>
        static public bool Merge(ref XmlDocument AMergeDoc, XmlDocument ANewDoc, int ADepth)
        {
            // root should be RootNode
            XmlNode root = AMergeDoc.FirstChild.NextSibling;

            // recursive parsing of the yml document
            MergeNode(root, ANewDoc.FirstChild.NextSibling, ADepth);

            return true;
        }

        #region ParseXml with derived/base in mind

        /// <summary>
        /// get all children nodes of a given node;
        /// depending on parameter, include the inherited nodes or not
        /// the nodes will be sorted by the order flag (AlwaysFirst, AlwaysLast)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="AConsiderBase">include the inherited nodes</param>
        /// <returns></returns>
        public static List <XmlNode>GetChildren(XmlNode node, bool AConsiderBase)
        {
            List <XmlNode>list = new List <XmlNode>();

            if (node == null)
            {
                return list;
            }

            // first add all nodes
            bool inBase = false;
            XmlNode firstChild = node.FirstChild;

            if ((firstChild != null) && (firstChild.Name == "base"))
            {
                if (AConsiderBase)
                {
                    inBase = true;
                    firstChild = firstChild.FirstChild;

                    if (firstChild == null)
                    {
                        // base does not have children, just used for attributes
                        firstChild = node.FirstChild.NextSibling;
                        inBase = false;
                    }
                }
                else
                {
                    firstChild = firstChild.NextSibling;
                }
            }

            if (firstChild == null)
            {
                return list;
            }

            list.Add(firstChild);

            XmlNode nextSibling = firstChild;

            while (nextSibling != null)
            {
                nextSibling = nextSibling.NextSibling;

                if ((nextSibling == null) && (inBase == true))
                {
                    nextSibling = firstChild.ParentNode.NextSibling;
                    inBase = false;
                }

                if (nextSibling != null)
                {
                    list.Add(nextSibling);
                }
            }

            // now sort the list by the order attribute
            list.Sort(new YamlItemOrderComparer());

            if (TLogging.DebugLevel >= 1)
            {
                TLogging.LogAtLevel(1, "Result of sorting the Yaml controls:");

                foreach (XmlNode x in list)
                {
                   TLogging.LogAtLevel(1, "  " + x.Name);
                }
            }

            return list;
        }

        /// get the parent node
        public static XmlNode Parent(XmlNode node)
        {
            XmlNode ParentNode = node.ParentNode;

            if ((ParentNode != null) && (node.ParentNode.Name == "base"))
            {
                ParentNode = node.ParentNode.ParentNode;
            }

            return ParentNode;
        }

        /// SetAttribute will never consider the base and the xml hierarchy; use Tag to move things to base
        public static void SetAttribute(XmlNode xmlNode, string name, string value)
        {
            if (TXMLParser.HasAttribute(xmlNode, name))
            {
                xmlNode.Attributes[name].Value = value;
            }
            else
            {
                XmlAttribute newAttribute = xmlNode.OwnerDocument.CreateAttribute(name);
                newAttribute.Value = value;
                xmlNode.Attributes.Append(newAttribute);
            }
        }

        /// <summary>
        /// remove attribute
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="name"></param>
        public static void ClearAttribute(XmlNode xmlNode, string name)
        {
            XmlNode baseElement = xmlNode;

            while (baseElement != null)
            {
                if (TXMLParser.HasAttribute(baseElement, name))
                {
                    baseElement.Attributes.Remove(baseElement.Attributes[name]);
                }

                baseElement = TXMLParser.GetChild(baseElement, "base");
            }
        }

        /// <summary>
        /// check for the attribute; if the current node does not have it, check the base node
        /// </summary>
        public static bool HasAttribute(XmlNode xmlNode, string name)
        {
            XmlNode baseElement = xmlNode;

            while (baseElement != null)
            {
                if (TXMLParser.HasAttribute(baseElement, name))
                {
                    return true;
                }

                baseElement = TXMLParser.GetChild(baseElement, "base");
            }

            return false;
        }

        /// <summary>
        /// get the attribute; if the current node does not have it, check the base node
        /// </summary>
        public static string GetAttribute(XmlNode xmlNode, string name)
        {
            XmlNode baseElement = xmlNode;

            while (baseElement != null)
            {
                if (TXMLParser.HasAttribute(baseElement, name))
                {
                    return TXMLParser.GetAttribute(baseElement, name);
                }

                baseElement = TXMLParser.GetChild(baseElement, "base");
            }

            // return default empty string
            return String.Empty;
        }

        /// <summary>
        /// if the current node does not have the attribute, try the parent nodes
        /// </summary>
        public static string GetAttributeRecursive(XmlNode xmlNode, string name)
        {
            if (xmlNode == null)
            {
                return "";
            }

            if (HasAttribute(xmlNode, name))
            {
                return GetAttribute(xmlNode, name);
            }

            return GetAttributeRecursive(xmlNode.ParentNode, name);
        }

        /// <summary>
        /// if the current node does not have the attribute, try the parent nodes
        /// </summary>
        public static bool HasAttributeRecursive(XmlNode xmlNode, string name)
        {
            if (xmlNode == null)
            {
                return false;
            }

            if (HasAttribute(xmlNode, name))
            {
                return true;
            }

            return HasAttributeRecursive(xmlNode.ParentNode, name);
        }

        /// <summary>
        /// get all attributes, even from base node
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static SortedList <string, string>GetAttributes(XmlNode xmlNode)
        {
            XmlNode baseElement = xmlNode;

            SortedList <string, string>ReturnValue = new SortedList <string, string>();

            while (baseElement != null)
            {
                IEnumerator enumerator = baseElement.Attributes.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)enumerator.Current;

                    if (ReturnValue.ContainsKey(attr.Name))
                    {
                        ReturnValue[attr.Name] = attr.Value;
                    }
                    else
                    {
                        ReturnValue.Add(attr.Name, attr.Value);
                    }
                }

                baseElement = TXMLParser.GetChild(baseElement, "base");
            }

            return ReturnValue;
        }

        /// <summary>
        /// get the child node with the given name;
        /// considers base nodes as well
        /// </summary>
        /// <param name="node"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static XmlNode GetChild(XmlNode node, string childName)
        {
            XmlNode child = TXMLParser.GetChild(node, childName);

            if (child == null)
            {
                XmlNode baseNode = TXMLParser.GetChild(node, "base");

                if (baseNode != null)
                {
                    return TXMLParser.GetChild(baseNode, childName);
                }
            }

            return child;
        }

        /// <summary>
        /// overload for GetElements, with the main node and the name of the child node that has the elements
        /// </summary>
        /// <param name="node"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static StringCollection GetElements(XmlNode node, string childName)
        {
            XmlNode child = GetChild(node, childName);

            if (child == null)
            {
                return new StringCollection();
            }

            return GetElements(child);
        }

        /// convert elements of a sequence into a string collection
        /// checks for duplicates, and removes names with a tilde character ~ in front
        public static StringCollection GetElements(XmlNode node)
        {
            if ((node != null) && node.HasChildNodes)
            {
                StringCollection result = new StringCollection();
                XmlNode child = node.FirstChild;

                if (child.Name == "base")
                {
                    // first parse the base
                    XmlNode grandchild = child.FirstChild;

                    while (grandchild != null)
                    {
                        result.Add(grandchild.Attributes["name"].Value);
                        grandchild = grandchild.NextSibling;
                    }

                    child = child.NextSibling;
                }

                while ((child != null) && (child.Name == TYml2Xml.XMLLIST))
                {
                    string value = child.Attributes["name"].Value;

                    // check for duplicates, and negations
                    bool negate = false;

                    if (child.Attributes["negate"] != null)
                    {
                        negate = true;
                    }

                    if (result.Contains(value))
                    {
                        if (negate)
                        {
                            result.Remove(value);
                        }
                    }
                    else if (!negate)
                    {
                        result.Add(child.Attributes["name"].Value);
                    }

                    child = child.NextSibling;
                }

                return result;
            }

            return new StringCollection();
        }

        /// TYml2Xml.LoadChild will either
        /// reuse an element, move an existing leaf from base to the main node, or create a new element
        protected static XmlNode LoadChild(XmlNode parent, string nodeName, int ADepth)
        {
            XmlNode newElement = TXMLParser.GetChild(parent, nodeName);

            if (nodeName.Contains("Separator") || nodeName.Contains(TYml2Xml.XMLELEMENT))
            {
                // special case: separators have the same name
                newElement = null;
            }
            else if (newElement != null)
            {
                // make sure there is no control with the same name in the file that is currently parsed
                if (TXMLParser.HasAttribute(newElement, "depth") && (TXMLParser.GetAttribute(newElement, "depth") == ADepth.ToString()))
                {
                    throw new Exception("Problem: the element " + nodeName + " should only appear once in the file");
                }
            }
            else if (newElement == null)
            {
                XmlNode baseElement = TXMLParser.GetChild(parent, "base");

                if (baseElement != null)
                {
                    newElement = TXMLParser.GetChild(baseElement, nodeName);

                    if (newElement != null)
                    {
                        // move it to the main node, and move the attributes to its own new base node
                        parent.AppendChild(newElement);
                        baseElement = parent.OwnerDocument.CreateElement("base");
                        newElement.PrependChild(baseElement);

                        while (newElement.Attributes.Count > 0)
                        {
                            XmlAttribute AttribToMove = newElement.Attributes[0];
                            newElement.Attributes.Remove(AttribToMove);
                            baseElement.Attributes.Append(AttribToMove);
                        }
                    }
                }
            }

            if ((newElement == null) && (nodeName == TYml2Xml.ROOTNODEINTERNAL))
            {
                newElement = parent.OwnerDocument.DocumentElement;
            }
            else if (newElement == null)
            {
                newElement = parent.OwnerDocument.CreateElement("", nodeName, "");
                XmlAttribute attr = parent.OwnerDocument.CreateAttribute("depth");
                attr.Value = ADepth.ToString();
                newElement.Attributes.Append(attr);
                parent.AppendChild(newElement);
            }

            return newElement;
        }

        /// check if parent already has a base element
        protected static XmlNode GetBaseNode(XmlNode ANode)
        {
            XmlNode baseNode = ANode.FirstChild;

            if (baseNode.Name != "base")
            {
                baseNode = ANode.OwnerDocument.CreateElement("base");
                ANode.PrependChild(baseNode);
            }

            return baseNode;
        }

        /// <summary>
        /// Tag will move all leafs to a new child element called base
        /// a leaf is an XmlNode that has no children, only attributes
        /// all attributes are moved to base
        /// can only be called once
        /// todo: what about sequences
        /// </summary>
        /// <param name="ANode"></param>
        public static void Tag(XmlNode ANode)
        {
            if ((ANode.ParentNode != null) && (ANode.ParentNode.Name == "base"))
            {
                throw new Exception("TXml2Yml.Tag can only be called once");
            }

            XmlNode baseNode = TYml2Xml.GetBaseNode(ANode.ParentNode);

            if (ANode.HasChildNodes)
            {
                XmlNode child = ANode.FirstChild;

                if (child.Name == "base")
                {
                    child = child.NextSibling;
                }

                while (child != null)
                {
                    Tag(child);

                    // child was either kept, or moved to base
                    if (child.ParentNode.Name == "base")
                    {
                        // it was moved to base, so find again the first child of the parent
                        child = ANode.FirstChild;

                        if (child.Name == "base")
                        {
                            child = child.NextSibling;
                        }
                    }
                    else
                    {
                        // it was kept, so use the next sibling
                        child = child.NextSibling;
                    }
                }
            }
            else
            {
                // move this node to base element of parent
                baseNode.AppendChild(ANode);
            }

            // attributes are all moved to a child node called base, if the node has not already moved to base
            if (ANode.ParentNode.Name != "base")
            {
                baseNode = TYml2Xml.GetBaseNode(ANode);

                while (ANode.Attributes.Count > 0)
                {
                    // Mono requires the attribute to be removed first before adding it
                    XmlAttribute origAttribute = ANode.Attributes[0];
                    ANode.Attributes.Remove(origAttribute);

                    if (baseNode.Attributes[origAttribute.Name] != null)
                    {
                        baseNode.Attributes[origAttribute.Name].Value = origAttribute.Value;
                    }
                    else
                    {
                        baseNode.Attributes.Append(origAttribute);
                    }
                }
            }
        }

        #endregion
    }

    /// for sorting the controls, depending on Order attribute, and depth
    public class YamlItemOrderComparer : IComparer <XmlNode>
    {
        private static int CheckOrderAttribute(string order1, string order2)
        {
            if (order1 == "AlwaysFirst")
            {
                return -1;
            }
            else if (order2 == "AlwaysFirst")
            {
                return +1;
            }
            else if (order1 == "AlwaysLast")
            {
                return +1;
            }
            else if (order2 == "AlwaysLast")
            {
                return -1;
            }

            return 0;
        }

        /// required method of IComparer interface
        public int Compare(XmlNode node1, XmlNode node2)
        {
            return CompareNodes(node1, node2);
        }

        /// <summary>
        /// compare two nodes; considering base nodes and depth of the node, and the order attribute
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns>+1 if node1 is greater than node2, -1 if node1 is less than node2, and 0 if they are the same or identical</returns>
        public static int CompareNodes(XmlNode node1, XmlNode node2)
        {
            int returnValue = 0;

            if ((node1 != null) && (node2 != null) && (node1 != node2))
            {
                string order1 = TYml2Xml.GetAttribute(node1, "Order");
                string order2 = TYml2Xml.GetAttribute(node2, "Order");
                int depth1 = (TYml2Xml.HasAttribute(node1, "depth") ? Convert.ToInt32(TYml2Xml.GetAttribute(node1, "depth")) : -1);
                int depth2 = (TYml2Xml.HasAttribute(node2, "depth") ? Convert.ToInt32(TYml2Xml.GetAttribute(node2, "depth")) : -1);

                if (order1 == order2)
                {
                    returnValue = 0;
                    XmlNode realParentNode1 = node1.ParentNode;

                    while (realParentNode1 != null && realParentNode1.Name == "base")
                    {
                        realParentNode1 = realParentNode1.ParentNode;
                    }

                    XmlNode realParentNode2 = node2.ParentNode;

                    while (realParentNode2 != null && realParentNode2.Name == "base")
                    {
                        realParentNode2 = realParentNode2.ParentNode;
                    }

                    // order of both objects is the same, but depending on the depth it has a different priority
                    // deeper depth (negative numbers) has more priority
                    if (depth1 < depth2)
                    {
                        if (order1 == "AlwaysFirst")
                        {
                            returnValue = -1;
                        }
                        else if (order1 == "AlwaysLast")
                        {
                            returnValue = 1;
                        }
                        else
                        {
                            // no specific order; if first
                            returnValue = -1;
                        }
                    }
                    else if (depth1 > depth2)
                    {
                        if (order2 == "AlwaysFirst")
                        {
                            returnValue = 1;
                        }
                        else if (order2 == "AlwaysLast")
                        {
                            returnValue = -1;
                        }
                        else
                        {
                            returnValue = 1;
                        }
                    }
                    else if (realParentNode1 == realParentNode2)
                    {
                        // are the nodes siblings? then keep the order
                        XmlNode child = realParentNode1.FirstChild;
                        int pos1 = -1, pos2 = -1;
                        int currentPos = 0;

                        while (child != null)
                        {
                            if (child == node1)
                            {
                                pos1 = currentPos;
                            }
                            else if (child == node2)
                            {
                                pos2 = currentPos;
                            }

                            child = child.NextSibling;
                            currentPos++;
                        }

                        if (pos1 == -1 || pos2 == -1)
                        {
                            // they are not from the same parent, just children of Controls, but from different yaml files. order does not matter
                            returnValue = 0;
                        }
                        else if (pos1 < pos2)
                        {
                            returnValue = -1;
                        }
                        else
                        {
                            returnValue = 1;
                        }

                    }
                    else
                    {
                        Console.WriteLine("problem sorting " + node1.Name + " " + node2.Name);
                    }
                }
                else
                {
                    returnValue = CheckOrderAttribute(order1, order2);
                }

            }

            TLogging.LogAtLevel(1, "Sorting Yaml Controls: " + node1.Name + (returnValue == 0? " = ": (returnValue > 0 ? " > " : " < ")) + node2.Name);

            return returnValue;
        }
    }
}