//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Ict.Tools.DevelopersAssistant
{
    /// <summary>
    /// Static helper class that has methods for dealing with Xml formatted settings files
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Gets a property from a configuration file
        /// </summary>
        /// <param name="xmlDoc">The Xml document object</param>
        /// <param name="PropertyName">The name of the property to be set</param>
        /// <param name="DefaultValue">The default value for the property if it is not found</param>
        /// <param name="ParentNode">The parent node of the property</param>
        /// <param name="XmlNodeName">The element name containing the key/value pair of attributes</param>
        /// <param name="NameAttribute">The Xml attribute name for the 'key'</param>
        /// <param name="ValueAttribute">The Xml attribute name for the 'value'</param>
        /// <returns>The value of the property, or the default value</returns>
        public static string GetPropertyValue(XmlDocument xmlDoc,
            string PropertyName,
            string DefaultValue,
            XmlNode ParentNode,
            string XmlNodeName,
            string NameAttribute,
            string ValueAttribute)
        {
            string ret = DefaultValue;

            string xPath = String.Format("{0}[@{1}='{2}']", XmlNodeName, NameAttribute, PropertyName);
            XmlNode n = ParentNode.SelectSingleNode(xPath);

            if (n != null)
            {
                XmlAttribute att = n.Attributes[ValueAttribute];

                if (att != null)
                {
                    ret = att.Value;
                }
            }

            return ret;
        }

        /// <summary>
        /// Sets a property value in a configuration file, creating the element if it does not already exist
        /// </summary>
        /// <param name="xmlDoc">The Xml document object</param>
        /// <param name="PropertyName">The name of the property to be set</param>
        /// <param name="NewValue">The new value for the property</param>
        /// <param name="ParentNode">The parent node of the property</param>
        /// <param name="XmlNodeName">The element name containing the key/value pair of attributes</param>
        /// <param name="NameAttribute">The Xml attribute name for the 'key'</param>
        /// <param name="ValueAttribute">The Xml attribute name for the 'value'</param>
        /// <returns>True if successful</returns>
        public static bool SetPropertyValue(XmlDocument xmlDoc,
            string PropertyName,
            string NewValue,
            XmlNode ParentNode,
            string XmlNodeName,
            string NameAttribute,
            string ValueAttribute)
        {
            string xPath = String.Format("{0}[@{1}='{2}']", XmlNodeName, NameAttribute, PropertyName);
            XmlNode n = ParentNode.SelectSingleNode(xPath);
            XmlAttribute att = null;

            if (n == null)
            {
                n = xmlDoc.CreateElement(XmlNodeName);
                att = xmlDoc.CreateAttribute(NameAttribute);
                att.Value = PropertyName;
                n.Attributes.Append(att);
                ParentNode.AppendChild(xmlDoc.CreateWhitespace("    "));
                ParentNode.AppendChild(n);
                ParentNode.AppendChild(xmlDoc.CreateWhitespace("\r\n"));
            }

            att = n.Attributes[ValueAttribute];

            if (att == null)
            {
                att = xmlDoc.CreateAttribute(ValueAttribute);
                n.Attributes.Append(att);
            }

            att.Value = NewValue;
            return true;
        }

        /// <summary>
        /// Removes a property from a configuration file
        /// </summary>
        /// <param name="xmlDoc">The Xml document object</param>
        /// <param name="PropertyName">The name of the property to be set</param>
        /// <param name="ParentNode">The parent node of the property</param>
        /// <param name="XmlNodeName">The element name containing the key/value pair of attributes</param>
        /// <param name="NameAttribute">The Xml attribute name for the 'key'</param>
        /// <returns>True, if successful</returns>
        public static bool RemoveProperty(XmlDocument xmlDoc, string PropertyName, XmlNode ParentNode, string XmlNodeName, string NameAttribute)
        {
            string xPath = String.Format("{0}[@{1}='{2}']", XmlNodeName, NameAttribute, PropertyName);
            XmlNode n = ParentNode.SelectSingleNode(xPath);

            if (n == null)
            {
                return true;                 // The property is not there anyway
            }

            ParentNode.RemoveChild(n);
            return true;
        }
    }
}