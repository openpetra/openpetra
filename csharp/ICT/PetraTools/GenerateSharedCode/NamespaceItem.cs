//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;

namespace NamespaceHierarchy
{
/// <summary>
/// to be parsed from xml file
/// </summary>
public class TNamespace
{
    string name;
    string description;

    /// <summary>
    /// constructor
    /// </summary>
    public TNamespace()
    {
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="name"></param>
    public TNamespace(string name)
    {
        this.name = name;
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="AName"></param>
    /// <param name="ADescription"></param>
    public TNamespace(string AName, string ADescription)
    {
        this.name = AName;
        this.description = ADescription;
    }

    /// <summary>
    /// name of the namespace
    /// </summary>
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    /// <summary>
    /// description
    /// </summary>
    public string Description
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }

    /// <summary>
    /// the children of this namespace
    /// </summary>
    public List <TNamespace>Children = new List <TNamespace>();

    /// <summary>
    /// parse the namespaces from an XmlDocument
    /// </summary>
    public static List <TNamespace>ReadFromFile(XmlDocument ADoc)
    {
        List <TNamespace>items = new List <TNamespace>();
        XmlNode NamespaceHierarchyNode = ADoc.DocumentElement.FirstChild;

        if (NamespaceHierarchyNode.Name != "NamespaceHierarchy")
        {
            throw new Exception("cannot find root node NamespaceHierarchy");
        }

        foreach (XmlNode child in NamespaceHierarchyNode.ChildNodes)
        {
            items.Add(TNamespace.Serialize(child));
        }

        return items;
    }

    private static TNamespace Serialize(XmlNode ANode)
    {
        TNamespace newNameSpace = new TNamespace();

        newNameSpace.Name = TYml2Xml.GetElementName(ANode);
        newNameSpace.Description = TXMLParser.GetAttribute(ANode, "description");

        foreach (XmlNode child in ANode.ChildNodes)
        {
            newNameSpace.Children.Add(Serialize(child));
        }

        return newNameSpace;
    }

    private static String FindModuleName = "";

    /// <summary>
    /// find a module in the top namespace
    /// </summary>
    /// <param name="AList"></param>
    /// <param name="AModule"></param>
    /// <returns></returns>
    public static Int32 FindModuleIndex(List <TNamespace>AList, String AModule)
    {
        FindModuleName = AModule;
        return AList.FindIndex(TNamespace.FindModule);
    }

    /// <summary>
    /// check if this is the current module
    /// </summary>
    /// <param name="AModule"></param>
    /// <returns></returns>
    public static bool FindModule(TNamespace AModule)
    {
        if (AModule.Name == FindModuleName)      //FindModuleName
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

/// <summary>
/// for generating the code
/// </summary>
public class CommonNamespace
{
    /// <summary>
    /// write the code for the namespace; use this namespace; recursive method
    /// </summary>
    /// <param name="tw"></param>
    /// <param name="ParentNamespace"></param>
    /// <param name="ParentInterfaceName"></param>
    /// <param name="sn"></param>
    /// <param name="children"></param>
    public static void WriteUsingNamespace(AutoGenerationWriter tw,
        String ParentNamespace,
        String ParentInterfaceName,
        TNamespace sn,
        List <TNamespace>children)
    {
        // don't automatically write the end points.
        if (children.Count == 0)
        {
            return;
        }

        foreach (TNamespace child in children)
        {
            //        if (child.Children.Count > 0)
            {
                tw.WriteLine("using " + ParentNamespace + "." + child.Name + ';');
                WriteUsingNamespace(tw, ParentNamespace + "." + child.Name, ParentInterfaceName + child.Name, child, child.Children);
            }
        }
    }
}
}