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

namespace Ict.Tools.CodeGeneration
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
    ///        sequence: [element1, element2, element3] are translated to <element name="element1"/><element name="element2/> below the parent node
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
    ///                               add an attribute to elements that have been in base, base="yes"
    ///                               this way the order of elements can be maintained easily,
    ///                               but it is still known which elements and attributes have been added since the tag
    ///              reactivate the check again, in ProcessXAML.cs: if (TXMLParser.GetAttribute(rootNode, "ClassType") != "abstract")
    ///        todo: save the last file after modification have been done to the xml structure (eg. loaded from csharp file)
    /// </summary>
    public class TYml2Xml
    {
        string[] lines = null;
        Int32 currentLine = -1;
        string filename = "";

        public TYml2Xml(string AFilename)
        {
            filename = AFilename;

            // file should be in unicode encoding
            TextReader reader = new StreamReader(filename, true);
            lines = reader.ReadToEnd().Replace("\r", "").Split(new char[] { '\n' });
            reader.Close();
            currentLine = -1;
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

        protected void ThrowException(string AMessage, Int32 lineNr)
        {
            throw new Exception("Error in Yml2Xml: " + AMessage + "; " +
                "(file: " + filename + " line " + (lineNr + 1).ToString() + ")");
        }

        // returns -1 for comments or invalid line numbers,
        // otherwise the number of spaces
        // throws exception if there is a tab in the indentation
        protected Int32 GetAbsoluteIndentation(Int32 lineNr)
        {
            if ((lineNr < 0) || (lineNr >= lines.Length))
            {
                return -1;
            }

            string line = lines[lineNr];

            if (line.Trim().StartsWith("#"))
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

        // does not return comment lines
        // @returns null if no line available
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

        private bool SplitNode(string line, out string nodeName, out string nodeContent)
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

        private string StripQuotes(string s)
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

        // this function does not know and care about base and hierarchical structures; use Tag for that
        // only for one thing it needs to consider base: check if the leaf to be added is already in base;
        // that would mean that the leaf needs to be moved back, and only the attributes go into base?
        // this would also solve the problem of leafs becoming nodes
        // todo: need to think about sequences
        private void ParseNode(XmlDocument myDoc, XmlNode parent, int ADepth)
        {
            string line = GetNextLine();

            if (line != null)
            {
                string nodeName, nodeContent;

                if (SplitNode(line, out nodeName, out nodeContent))
                {
                    // check if the node already exists
                    // at this stage existing elements are just modified and added to
                    // to get different versions, use the method Tag()
                    // TYml2Xml.LoadChild will either
                    // reuse an element, move an existing leaf from base to the main node, or create a new element
                    XmlNode newElement = TYml2Xml.LoadChild(parent, nodeName, ADepth);

                    if (nodeContent.Length == 0)
                    {
                        // this is just a parent node, without attributes etc.
                        if (GetIndentationNext(currentLine) > 0)
                        {
                            // There are children
                            Int32 childrenAbsoluteIndentation = GetAbsoluteIndentationNext(currentLine);

                            do
                            {
                                ParseNode(myDoc, newElement, ADepth);
                            } while (GetAbsoluteIndentationNext(currentLine) == childrenAbsoluteIndentation);
                        }
                    }
                    else
                    {
                        if (GetIndentationNext(currentLine) > 0)
                        {
                            throw new Exception("Problem in file " + System.IO.Path.GetFullPath(
                                    filename) + ", line " +
                                (currentLine + 1).ToString() + "; we don't support attributes on the one line and then sub elements additionaly");
                        }

                        // there is some content directly in the line
                        // can be scalar, sequence, or mapping
                        if (nodeContent.StartsWith("{"))
                        {
                            // mapping
                            // first get the list without brackets
                            string list = nodeContent.Substring(1, nodeContent.Length - 2).Trim();

                            // now use getNextCSV which is able to deal with quoted strings
                            while (list.Length > 0)
                            {
                                string mapping = StringHelper.GetNextCSV(ref list, ",");
                                string mappingName = StringHelper.GetNextCSV(ref mapping, "=").Trim();
                                string mappingValue = StripQuotes(mapping.Trim());
                                TYml2Xml.SetAttribute(newElement, mappingName, mappingValue);
                            }
                        }
                        else if (nodeContent.StartsWith("["))
                        {
                            // sequence
                            // first get the list without brackets
                            string list = nodeContent.Substring(1, nodeContent.Length - 2).Trim();

                            // now use getNextCSV which is able to deal with quoted strings
                            while (list.Length > 0)
                            {
                                // beware of duplicates
                                // tilde character ~ in front of name removes the element from the list
                                // don't worry about base class
                                string value = StripQuotes(StringHelper.GetNextCSV(ref list, ",").Trim());
                                bool negated = false;

                                if (value[0] == '~')
                                {
                                    negated = true;
                                    value = value.Substring(1);
                                }

                                List <XmlNode>children = TYml2Xml.GetChildren(newElement, false);
                                XmlNode childFound = null;

                                foreach (XmlNode childNode in children)
                                {
                                    if (childNode.Attributes["name"].Value == value)
                                    {
                                        childFound = childNode;
                                        break;
                                    }
                                }

                                // does the name already exist in the list?
                                if (childFound != null)
                                {
                                    if (negated)
                                    {
                                        newElement.RemoveChild(childFound);
                                    }
                                }
                                else
                                {
                                    XmlElement sequenceElement = myDoc.CreateElement("", "Element", "");
                                    newElement.AppendChild(sequenceElement);
                                    XmlAttribute attr = myDoc.CreateAttribute("name");
                                    attr.Value = value;
                                    sequenceElement.Attributes.Append(attr);
                                }
                            }
                        }
                        else
                        {
                            // scalar
                            // this should not be an element, but an attribute of the parent
                            parent.RemoveChild(newElement);
                            TYml2Xml.SetAttribute(parent, nodeName, nodeContent);
                        }
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

        // fill sorted list which contains a reference to each node by name
        public static SortedList ReferenceNodes(XmlDocument myDoc)
        {
            SortedList xmlNodes = new SortedList();

            // get the XmlDeclaration section first
            XmlNode xmlnode = myDoc.FirstChild;

            // get the first element
            AddToSortedList(ref xmlNodes, xmlnode.NextSibling);
            return xmlNodes;
        }

        static public XmlDocument CreateXmlDocument()
        {
            XmlDocument myDoc = new XmlDocument();

            // add the XML declaration section
            XmlNode xmlnode = myDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");

            myDoc.AppendChild(xmlnode);

            // add the root element
            XmlElement root = myDoc.CreateElement("", "RootNodeInternal", "");
            myDoc.AppendChild(root);
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

            // recursive parsing of the yml document
            ParseNode(myDoc, root, 0);
            return myDoc;
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
            // root should be RootNode
            XmlNode root = AMergeDoc.FirstChild.NextSibling;

            // recursive parsing of the yml document
            ParseNode(AMergeDoc, root, ADepth);
            return AMergeDoc;
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

            return list;
        }

        public static XmlNode Parent(XmlNode node)
        {
            XmlNode ParentNode = node.ParentNode;

            if ((ParentNode != null) && (node.ParentNode.Name == "base"))
            {
                ParentNode = node.ParentNode.ParentNode;
            }

            return ParentNode;
        }

        // SetAttribute will never consider the base and the xml hierarchy; use Tag to move things to base
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
        /// check for the attribute; if the current node does not have it, check the base node
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// <param name="name"></param>
        /// <returns></returns>
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
            return TXMLParser.GetAttribute(xmlNode, name);
        }

        // convert elements of a sequence into a string collection
        // checks for duplicates, and removes names with a tilde character ~ in front
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
                        result.Add(grandchild.Attributes[0].Value);
                        grandchild = grandchild.NextSibling;
                    }

                    child = child.NextSibling;
                }

                while ((child != null) && (child.Name == "Element"))
                {
                    string value = child.Attributes[0].Value;

                    // check for duplicates, and negations
                    bool negate = false;

                    if (value[0] == '~')
                    {
                        negate = true;
                        value = value.Substring(1);
                    }

                    if (result.Contains(value))
                    {
                        if (negate)
                        {
                            result.Remove(value);
                        }
                    }
                    else
                    {
                        result.Add(child.Attributes[0].Value);
                    }

                    child = child.NextSibling;
                }

                return result;
            }
            else
            {
                System.Console.WriteLine("Yml2Xml.GetElements: could not find elements");
            }

            return new StringCollection();
        }

        // TYml2Xml.LoadChild will either
        // reuse an element, move an existing leaf from base to the main node, or create a new element
        protected static XmlNode LoadChild(XmlNode parent, string nodeName, int ADepth)
        {
            XmlNode newElement = TXMLParser.GetChild(parent, nodeName);

            if (nodeName.Contains("Separator"))
            {
                // special case: separators have the same name
                newElement = null;
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
                            baseElement.Attributes.Append(newElement.Attributes[0]);
                        }
                    }
                }
            }

            if (newElement == null)
            {
                newElement = parent.OwnerDocument.CreateElement("", nodeName, "");
                XmlAttribute attr = parent.OwnerDocument.CreateAttribute("depth");
                attr.Value = ADepth.ToString();
                newElement.Attributes.Append(attr);
                parent.AppendChild(newElement);
            }

            return newElement;
        }

        // check if parent already has a base element
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
            if (ANode.ParentNode.Name == "base")
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
                    // attribute is automatically moved, so no removal is necessary
                    baseNode.Attributes.Append(ANode.Attributes[0]);
                }
            }
        }

        #endregion
    }

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
                    }
                    else if (realParentNode1 == realParentNode2)
                    {
                        // are the nodes siblings? then keep the order

                        foreach (XmlNode child in node1.ParentNode.ChildNodes)
                        {
                            if (child == node1)
                            {
                                returnValue = -1;
                                break;
                            }
                            else if (child == node2)
                            {
                                returnValue = +1;
                                break;
                            }
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

/*
 *                      if (node1.Name == "tpgReportSpecific" || node2.Name == "tpgReportSpecific"
 || node1.Name == "tpgOutputDestinations" || node2.Name == "tpgOutputDestinations"
 || node1.Name == "tpgSorting" || node2.Name == "tpgSorting"
 *                        )
 *          Console.WriteLine("sorting " + node1.Name + " " + (returnValue == 0? "==": (returnValue == -1? "<":">")) +
 *                                        " " + node2.Name +
 *                                        " depth: " + depth1.ToString() + " " + depth2.ToString());
 */
            }

            return returnValue;
        }
    }
    public class CtrlItemOrderComparer : IComparer <TControlDef>
    {
        /// <summary>
        /// compare two nodes; considering base nodes and depth of the node, and the order attribute
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns>+1 if node1 is greater than node2, -1 if node1 is less than node2, and 0 if they are the same or identical</returns>
        public int Compare(TControlDef node1, TControlDef node2)
        {
            return YamlItemOrderComparer.CompareNodes(node1.xmlNode, node2.xmlNode);
        }
    }
}