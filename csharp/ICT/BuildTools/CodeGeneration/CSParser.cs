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
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Parser;
using Microsoft.CSharp;
using Ict.Common.IO;

namespace Ict.Tools.CodeGeneration
{
    /// <summary>
    /// A wrapper for NRefactory from SharpDevelop
    /// </summary>
    public class CSParser
    {
        private CompilationUnit cu;

        /// <summary>
        /// constructor, parse the given file
        /// </summary>
        /// <param name="filename"></param>
        public CSParser(string filename)
        {
            cu = ParseFile(filename);
        }

        private static void PrintErrors(string AFileName, Errors AErrors)
        {
            Console.WriteLine("File: " + AFileName + "\n");
            Console.WriteLine(AErrors.ErrorOutput);
        }

        /// <summary>
        /// parse a c# file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public CompilationUnit ParseFile(string fileName)
        {
//        Console.WriteLine("\nParsing " + fileName);

            IParser parser = ParserFactory.CreateParser(fileName);

            parser.Parse();

            CompilationUnit cu = parser.CompilationUnit;

            if (parser.Errors.Count > 0)
            {
                Console.WriteLine();
                PrintErrors(fileName, parser.Errors);
                return null;
            }

            return cu;
        }

        /// <summary>
        /// get all classes in that namespace
        /// </summary>
        /// <param name="ACSFiles"></param>
        /// <param name="ANamespace"></param>
        /// <returns>list of classes</returns>
        public static List <TypeDeclaration>GetClassesInNamespace(List <CSParser>ACSFiles, string ANamespace)
        {
            List <TypeDeclaration>result = new List <TypeDeclaration>();

            foreach (CSParser file in ACSFiles)
            {
                foreach (NamespaceDeclaration nnode in GetNamespaces(file.cu))
                {
                    if (nnode.Name == ANamespace)
                    {
                        foreach (TypeDeclaration cnode in GetClasses(nnode))
                        {
                            result.Add(cnode);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// get all namespaces in the current file
        /// </summary>
        /// <returns></returns>
        public List <NamespaceDeclaration>GetNamespaces()
        {
            return CSParser.GetNamespaces(this.cu);
        }

        /// <summary>
        /// get all the namespaces from the file
        /// </summary>
        /// <returns></returns>
        public static List <NamespaceDeclaration>GetNamespaces(CompilationUnit cu)
        {
            List <NamespaceDeclaration>result = new List <NamespaceDeclaration>();

            foreach (object child in cu.Children)
            {
                if (child is INode)
                {
                    INode node = (INode)child;
                    Type type = node.GetType();

                    if (type.Name == "NamespaceDeclaration")
                    {
                        result.Add((NamespaceDeclaration)node);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// get all classes defined in this file
        /// </summary>
        /// <returns></returns>
        public static List <TypeDeclaration>GetClasses(CompilationUnit cu)
        {
            List <NamespaceDeclaration>Namespaces = GetNamespaces(cu);

            List <TypeDeclaration>result = new List <TypeDeclaration>();

            foreach (NamespaceDeclaration nnode in Namespaces)
            {
                result.AddRange(GetClasses(nnode));
            }

            return result;
        }

        /// <summary>
        /// overload that just uses the compilationunit of the current file
        /// </summary>
        /// <returns></returns>
        public List <TypeDeclaration>GetClasses()
        {
            return GetClasses(cu);
        }

        /// <summary>
        /// get all classes defined in this namespace
        /// </summary>
        /// <returns></returns>
        public static List <TypeDeclaration>GetClasses(NamespaceDeclaration nd)
        {
            List <TypeDeclaration>result = new List <TypeDeclaration>();

            foreach (object child in nd.Children)
            {
                if (child is INode)
                {
                    INode node = (INode)child;
                    Type type = node.GetType();

                    if (type.Name == "TypeDeclaration")
                    {
                        TypeDeclaration td = (TypeDeclaration)node;

                        if (td.Type == ClassType.Class)
                        {
                            TypeDeclaration t = (TypeDeclaration)node;
                            t.UserData = nd.Name;
                            result.Add(t);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// get a specific interface
        /// </summary>
        /// <returns></returns>
        public TypeDeclaration GetType(ref string ANamespace, string ATypeName, ClassType AClassType)
        {
            List <NamespaceDeclaration>namespaces = GetNamespaces(cu);

            foreach (NamespaceDeclaration nnode in namespaces)
            {
                if ((ANamespace.Length == 0) || (nnode.Name.StartsWith(ANamespace)))
                {
                    foreach (object child in nnode.Children)
                    {
                        if (child is INode)
                        {
                            INode node = (INode)child;
                            Type type = node.GetType();

                            if (type.Name == "TypeDeclaration")
                            {
                                TypeDeclaration td = (TypeDeclaration)node;

                                if (td.Type == AClassType)
                                {
                                    if (td.Name == ATypeName)
                                    {
                                        ANamespace = nnode.Name;
                                        return td;
                                    }

                                    // if the AInterfaceName contains the namespace
                                    if (ATypeName == nnode.Name + "." + td.Name)
                                    {
                                        ANamespace = nnode.Name;
                                        return td;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// get a specific interface
        /// </summary>
        /// <returns></returns>
        public TypeDeclaration GetInterface(string ANamespace, string AInterfaceName)
        {
            return GetType(ref ANamespace, AInterfaceName, ClassType.Interface);
        }

        /// <summary>
        /// get a specific class
        /// </summary>
        /// <returns></returns>
        public TypeDeclaration GetClass(string AClassName)
        {
            string Namespace = "";

            return GetType(ref Namespace, AClassName, ClassType.Class);
        }

        /// <summary>
        /// get the full name of a class with namespace separated by a dot
        /// </summary>
        /// <param name="AClassNode">the node of the class</param>
        /// <returns></returns>
        public string GetFullClassNameWithNamespace(TypeDeclaration AClassNode)
        {
            foreach (NamespaceDeclaration nnode in GetNamespaces(cu))
            {
                foreach (TypeDeclaration cnode in GetClasses(nnode))
                {
                    if (cnode == AClassNode)
                    {
                        return nnode.Name + "." + AClassNode.Name;
                    }
                }
            }

            throw new Exception("cannot find class " + AClassNode.Name);
        }

        /**
         * @returns a string collection of the field names of the class
         */
        public static StringCollection GetFields(TypeDeclaration cnode)
        {
            StringCollection list = new StringCollection();

            foreach (object child in cnode.Children)
            {
                if (child is INode)
                {
                    INode node = (INode)child;
                    Type type = node.GetType();

                    if (type.Name == "FieldDeclaration")
                    {
                        FieldDeclaration fd = (FieldDeclaration)node;

                        foreach (VariableDeclaration vd in fd.Fields)
                        {
                            list.Add(vd.Name);
                        }
                    }
                }
            }

            return list;
        }

        /**
         * @returns a list of the properties of the class
         */
        public static List <PropertyDeclaration>GetProperties(TypeDeclaration cnode)
        {
            List <PropertyDeclaration>result = new List <PropertyDeclaration>();

            foreach (object child in cnode.Children)
            {
                if (child is INode)
                {
                    INode node = (INode)child;
                    Type type = node.GetType();

                    if (type.Name == "PropertyDeclaration")
                    {
                        result.Add((PropertyDeclaration)node);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// get all methods of the class
        /// </summary>
        /// <returns></returns>
        public static List <MethodDeclaration>GetMethods(TypeDeclaration AClass)
        {
            List <MethodDeclaration>result = new List <MethodDeclaration>();

            foreach (object child in AClass.Children)
            {
                if (child is INode)
                {
                    INode node = (INode)child;
                    Type type = node.GetType();

                    if (type.Name == "MethodDeclaration")
                    {
                        result.Add((MethodDeclaration)node);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// get the first interface that this class implements
        /// </summary>
        /// <returns></returns>
        public static string GetImplementedInterface(TypeDeclaration AClass)
        {
            foreach (TypeReference t in AClass.BaseTypes)
            {
                if (t.Type.StartsWith("I"))
                {
                    return t.Type;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// get all constructors of the class
        /// </summary>
        /// <returns></returns>
        public static List <ConstructorDeclaration>GetConstructors(TypeDeclaration AClass)
        {
            List <ConstructorDeclaration>result = new List <ConstructorDeclaration>();

            foreach (object child in AClass.Children)
            {
                if (child is INode)
                {
                    INode node = (INode)child;
                    Type type = node.GetType();

                    if (type.Name == "ConstructorDeclaration")
                    {
                        result.Add((ConstructorDeclaration)node);
                    }
                }
            }

            return result;
        }

        /**
         * return the method by name
         */
        public static MethodDeclaration GetMethod(TypeDeclaration cnode, string AMethodName)
        {
            List <MethodDeclaration>methods = GetMethods(cnode);

            foreach (MethodDeclaration md in methods)
            {
                if (md.Name == AMethodName)
                {
                    return md;
                }
            }

            throw new Exception(
                "CSParser.GetMethod: cannot find method " +
                AMethodName +
                " in class " + cnode.Name);

            //return null;
        }

        /// <summary>
        /// find a class in a list of csharp files
        /// </summary>
        /// <param name="ACSFiles"></param>
        /// <param name="AClassName"></param>
        /// <returns>null if class cannot be found</returns>
        public static TypeDeclaration FindClass(List <CSParser>ACSFiles, string AClassName)
        {
            foreach (CSParser file in ACSFiles)
            {
                TypeDeclaration t = file.GetClass(AClassName);

                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        /// find an interface in a list of csharp files
        /// </summary>
        /// <param name="ACSFiles"></param>
        /// <param name="ANamespace"></param>
        /// <param name="AInterfaceName"></param>
        /// <returns>null if class cannot be found</returns>
        public static TypeDeclaration FindInterface(List <CSParser>ACSFiles, string ANamespace, string AInterfaceName)
        {
            foreach (CSParser file in ACSFiles)
            {
                TypeDeclaration t = file.GetInterface(ANamespace, AInterfaceName);

                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        /// contains the path for the csharp/ICT directory, eg. u:\openpetraorg\csharp\ICT
        /// </summary>
        public static string ICTPath;

        private static Hashtable _nsmap = null;
        private static Regex exceptionRegex = new Regex("^\\s*([a-zA-Z.]+)\\s*=\\s*([a-zA-Z.]+)");

        /// <summary>
        /// Reads in the name space maps for all other assemblies
        /// </summary>
        private static void ReadNamespaceMaps()
        {
            if (null == _nsmap)       // singelton
            {
                _nsmap = new Hashtable();

                string filename = ICTPath + "/../../delivery/projects/namespace.map";

                // Read in the file
                StreamReader sr = new StreamReader(filename);

                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    Match match = exceptionRegex.Match(line);

                    if (match.Success && (match.Groups.Count > 2))
                    {
                        string key = match.Groups[1].ToString();
                        string val = match.Groups[2].ToString();

                        _nsmap.Add(key, val);
                    }
                }

                sr.Close();
            }
        }

        /// <summary>
        /// Reads the nsmap generated from GenerateNamespaceMap and get the directory that includes this namespace
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static string GetSourceDirectory(string ns)
        {
            ReadNamespaceMaps();

            if (_nsmap[ns] == null)
            {
                return null;
            }

            return Path.GetFullPath(ICTPath + "/../" + ((string)_nsmap[ns]).Replace("Ict.", "ICT.").Replace(".", "/"));
        }

        private static Hashtable _CSFilesPerDir = new Hashtable();
        private static SortedList <string, CSParser>FParsedFiles = new SortedList <string, CSParser>();

        /// <summary>
        /// Returns CSParser instances for the cs files in the given directory.
        /// If you get the list more then one time, then you will get a cached copy
        /// </summary>
        /// <param name="dir">string with the directory to parse</param>
        /// <param name="option">search the subdirectories or not</param>
        /// <returns>List of CSParser instances</returns>
        public static List <CSParser>GetCSFilesForDirectory(string dir, SearchOption option)
        {
            string dirfull = Path.GetFullPath(dir);

            if (!_CSFilesPerDir.Contains(dirfull))
            {
                List <CSParser>CSFiles = new List <CSParser>();

                foreach (string filename in Directory.GetFiles(dir, "*.cs", option))
                {
                    if (!filename.EndsWith("-generated.cs") || filename.EndsWith("Cacheable-generated.cs")
                        || filename.EndsWith("ReferenceCount-generated.cs"))
                    {
                        if (!FParsedFiles.ContainsKey(filename))
                        {
                            FParsedFiles.Add(filename, new CSParser(filename));
                        }

                        CSFiles.Add(FParsedFiles[filename]);
                    }
                }

                _CSFilesPerDir.Add(dirfull, CSFiles);
            }

            return (List <CSParser> )_CSFilesPerDir[dirfull];
        }

        /// <summary>
        /// get the web connector classes that fit the server namespace
        /// </summary>
        /// <param name="AServerNamespace"></param>
        /// <returns></returns>
        public static List <TypeDeclaration>GetWebConnectorClasses(string AServerNamespace)
        {
            // Look up in nsmap for directory
            string directoryOfNamespace = GetSourceDirectory(AServerNamespace);

            if (null == directoryOfNamespace)
            {
                return null;
            }

            List <CSParser>CSFiles = new List <CSParser>();

            // Console.WriteLine("Namespace '" + AServerNamespace + "' found in '" + directoryOfNamespace + "'\n");

            foreach (CSParser tempCSFile in GetCSFilesForDirectory(directoryOfNamespace, SearchOption.TopDirectoryOnly))
            {
                // Copy the list, because namespace could be in more then one directory
                CSFiles.Add(tempCSFile);
            }

            return CSParser.GetClassesInNamespace(CSFiles, AServerNamespace);
        }

        /// <summary>
        /// check if there is a webconnector method for that table and function, eg CreateNew + AApDocument
        /// </summary>
        /// <param name="AWebConnectorClasses"></param>
        /// <param name="AWebFunction"></param>
        /// <param name="ATablename"></param>
        /// <param name="AClassOfFoundMethod"></param>
        /// <returns></returns>
        public static MethodDeclaration GetWebConnectorMethod(List <TypeDeclaration>AWebConnectorClasses,
            string AWebFunction,
            string ATablename,
            out TypeDeclaration AClassOfFoundMethod)
        {
            foreach (TypeDeclaration connectorClass in AWebConnectorClasses)
            {
                foreach (MethodDeclaration m in CSParser.GetMethods(connectorClass))
                {
                    string MethodName = m.Name;

                    if (MethodName == AWebFunction + ATablename)
                    {
                        AClassOfFoundMethod = connectorClass;
                        return m;
                    }
                }
            }

            AClassOfFoundMethod = null;
            return null;
        }
    }
}