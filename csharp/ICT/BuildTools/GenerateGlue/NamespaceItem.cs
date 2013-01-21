//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Ict.Common;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;
using ICSharpCode.NRefactory.Ast;

namespace NamespaceHierarchy
{
/// <summary>
/// to be parsed from the cs files
/// </summary>
public class TNamespace
{
    string name;

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
    /// name of the namespace
    /// </summary>
    public string Name
    {
        get
        {
            return name;
        }
    }

    /// <summary>
    /// the children of this namespace
    /// </summary>
    public SortedList <string, TNamespace>Children = new SortedList <string, TNamespace>();

    private static TNamespace FindOrCreateNamespace(TNamespace ARootNamespace, string ANamespaceName)
    {
        if (ANamespaceName.Contains("."))
        {
            TNamespace parent = FindOrCreateNamespace(ARootNamespace, ANamespaceName.Substring(0, ANamespaceName.LastIndexOf(".")));

            string childName = ANamespaceName.Substring(ANamespaceName.LastIndexOf(".") + 1);

            if (parent.Children.ContainsKey(childName))
            {
                return parent.Children[childName];
            }
            else
            {
                TNamespace Result = new TNamespace(childName);
                parent.Children.Add(childName, Result);
                return Result;
            }
        }
        else if (!ARootNamespace.Children.ContainsKey(ANamespaceName))
        {
            TNamespace Result = new TNamespace(ANamespaceName);
            ARootNamespace.Children.Add(ANamespaceName, Result);
            return Result;
        }
        else
        {
            return ARootNamespace.Children[ANamespaceName];
        }
    }

    /// <summary>
    /// parse the namespaces from an XmlDocument
    /// </summary>
    public static TNamespace ParseFromDirectory(string AServerLibPath)
    {
        TNamespace NamespaceRoot = new TNamespace();

        List <CSParser>CSFiles = CSParser.GetCSFilesForDirectory(AServerLibPath, SearchOption.AllDirectories);

        foreach (CSParser file in CSFiles)
        {
            foreach (NamespaceDeclaration namespaceDecl in file.GetNamespaces())
            {
                if (namespaceDecl.Name.EndsWith("Connectors"))
                {
                    string name = namespaceDecl.Name.Substring("Ict.Petra.Server.".Length + 1);
                    FindOrCreateNamespace(NamespaceRoot, name);
                }
            }
        }

        return NamespaceRoot;
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
}