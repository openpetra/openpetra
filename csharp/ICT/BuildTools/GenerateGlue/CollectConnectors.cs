//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using NamespaceHierarchy;
using Ict.Common;
using Ict.Tools.CodeGeneration;

namespace GenerateSharedCode
{
/// <summary>
/// parse the code and collect all connector classes that we want to have interfaces for
/// </summary>
public class TCollectConnectorInterfaces
{
    /// <summary>
    /// this will return a SortedList, the key is the interface name,
    /// and the value is the type definition of the class that implements that interface;
    /// connectors are identified namespace ending with Connectors
    /// </summary>
    private static SortedList <string, TypeDeclaration>GetConnectors(List <CSParser>ACSFiles)
    {
        SortedList <string, TypeDeclaration>Result = new SortedList <string, TypeDeclaration>();

        foreach (CSParser CSFile in ACSFiles)
        {
            foreach (TypeDeclaration t in CSFile.GetClasses())
            {
                if (t.UserData.ToString().EndsWith("Connectors"))
                {
                    string Interface = CSParser.GetImplementedInterface(t);

                    if (Interface.Length > 0)
                    {
                        string ServerNamespace = t.UserData.ToString();
                        string ServerNamespaceWithClassName = ServerNamespace + "." + t.Name;
                        string key = ServerNamespaceWithClassName + ":" + Interface;

                        if (Result.ContainsKey(ServerNamespaceWithClassName))
                        {
                            // there is already the other part of the partial class

                            TypeDeclaration partialType = Result[ServerNamespaceWithClassName];

                            Result.Remove(ServerNamespaceWithClassName);

                            foreach (INode child in partialType.Children)
                            {
                                t.AddChild(child);
                            }
                        }

                        Result.Add(key, t);

                        if (TLogging.DebugLevel > 1)
                        {
                            // TLogging.Log("adding new Connector " + key);
                        }
                    }
                    // either a webconnector, or a partial class
                    else
                    {
                        // web connectors don't derive from an interface, because the methods are static

                        string ServerNamespace = t.UserData.ToString();
                        string key = ServerNamespace + "." + t.Name;

                        if (Result.ContainsKey(key))
                        {
                            // partial class
                            foreach (INode child in t.Children)
                            {
                                Result[key].AddChild(child);
                            }
                        }
                        else if (t.Name.EndsWith("UIConnector"))
                        {
                            // this could be the partial class of a UIConnector
                            // try to find a key that starts with this type
                            bool foundType = false;

                            foreach (string k in Result.Keys)
                            {
                                if (k.StartsWith(key + ":"))
                                {
                                    foundType = true;

                                    foreach (INode child in t.Children)
                                    {
                                        Result[k].AddChild(child);
                                    }
                                }
                            }

                            if (!foundType)
                            {
                                Result.Add(key, t);
                            }
                        }
                        else
                        {
                            Result.Add(key, t);
                        }

                        if (TLogging.DebugLevel > 1)
                        {
                            // TLogging.Log("adding new Connector " + key);
                        }
                    }
                }
            }
        }

        return Result;
    }

    /// <summary>
    /// get all connectors that are using interfaces from a given namespace
    /// </summary>
    public static List <TypeDeclaration>FindTypesInNamespace(SortedList <string, TypeDeclaration>AConnectors, string ANamespace)
    {
        ANamespace = ANamespace.Replace("Ict.Petra.Shared.", "Ict.Petra.Server.");

        List <TypeDeclaration>Result = new List <TypeDeclaration>();

//        TLogging.Log("implementing " + ANamespace);
        foreach (string key in AConnectors.Keys)
        {
            if (key.StartsWith(ANamespace) && (key.LastIndexOf(".") == ANamespace.Length))
            {
//                TLogging.Log("   " + AConnectors[key]);
                Result.Add(AConnectors[key]);
            }
        }

        if (Result.Count == 0)
        {
//            TLogging.Log("not found for " + ANamespace);
        }

        return Result;
    }

    /// <summary>
    /// do we want this method to be available to the client?
    /// </summary>
    public static bool IgnoreMethod(List <AttributeSection>AAttributes, Modifiers AModifier)
    {
        if ((AModifier & Modifiers.Public) == 0)
        {
            return true;
        }

        foreach (AttributeSection attrSection in AAttributes)
        {
            foreach (ICSharpCode.NRefactory.Ast.Attribute attr in attrSection.Attributes)
            {
                if (attr.Name == "NoRemoting")
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static SortedList <string,
                               SortedList <string, TypeDeclaration>>ConnectorsByModule = new SortedList <string, SortedList <string, TypeDeclaration>>();

    /// <summary>
    /// main function to collect the connectors from the code.
    /// </summary>
    public static SortedList <string, TypeDeclaration>GetConnectors(string AOutputPath, string AModuleName)
    {
        if (!ConnectorsByModule.ContainsKey(AModuleName))
        {
            // get all csharp files that might hold implementations of remotable classes
            List <CSParser>CSFiles = null;

            if (AOutputPath.Contains("ICT/Petra/Plugins"))
            {
                // search for connectors in the directory of the plugin
                CSFiles = CSParser.GetCSFilesForDirectory(Path.GetFullPath(AOutputPath + "/../Server"),
                    SearchOption.AllDirectories);
            }
            else if (Directory.Exists(CSParser.ICTPath + "/Petra/Server/lib/M" + AModuleName))
            {
                // any class in the module can contain a connector
                CSFiles = CSParser.GetCSFilesForDirectory(CSParser.ICTPath + "/Petra/Server/lib/M" + AModuleName,
                    SearchOption.AllDirectories);
            }
            else if (AModuleName == "ServerAdmin")
            {
                CSFiles = CSParser.GetCSFilesForDirectory(CSParser.ICTPath + "/Petra/Server/app/Core",
                    SearchOption.AllDirectories);
            }
            else
            {
                CSFiles = new List <CSParser>();
            }

            ConnectorsByModule.Add(AModuleName, GetConnectors(CSFiles));
        }

        return ConnectorsByModule[AModuleName];
    }
}
}