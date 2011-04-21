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
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Text;
using DDW.Collections;
using DDW;
using Microsoft.CSharp;
using Ict.Common.IO;

namespace Ict.Tools.CodeGeneration
{
    /// <summary>
    /// A wrapper around CMicroParser.dll from http://www.codeplex.com/csparser
    /// </summary>
    public class CSParser
    {
        private CompilationUnitNode cu;
        public CSParser(string filename)
        {
            cu = ParseFile(filename);
        }

        private static void PrintErrors(IEnumerable <Parser.Error>errors)
        {
            foreach (Parser.Error error in errors)
            {
                if ((error.Token.ID == TokenID.Eof) && (error.Line == -1))
                {
                    Console.WriteLine(error.Message + "\nFile: " + error.FileName + "\n");
                }
                else
                {
                    Console.WriteLine(error.Message + " in token " + error.Token.ID +
                        "\nline: " + error.Line + ", column: " + error.Column +
                        "\nin file: " + error.FileName + "\n");
                }
            }
        }

        public static string ToSource(ISourceCode code)
        {
            StringBuilder sb = new StringBuilder();

            code.ToSource(sb);
            return sb.ToString();
        }

        public static string ToSource(ExpressionList code)
        {
            StringBuilder sb = new StringBuilder();

            code.ToSource(sb);
            return sb.ToString();
        }

        public CompilationUnitNode ParseFile(string fileName)
        {
            List <Parser.Error>errors = new List <Parser.Error>();

//        Console.WriteLine("\nParsing " + fileName);
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, true);
            Lexer l = new Lexer(sr);
            TokenCollection toks = l.Lex();

            Parser p = null;
            CompilationUnitNode cu = null;

            p = new Parser(fileName);
            cu = p.Parse(toks, l.StringLiterals);

            if (p.Errors.Count != 0)
            {
                Console.WriteLine();
                PrintErrors(p.Errors);
                errors.AddRange(p.Errors);
                return null;
            }

            return cu;
        }

        public ClassNode GetFirstClass()
        {
            foreach (NamespaceNode nnode in cu.Namespaces)
            {
                foreach (ClassNode cnode in nnode.Classes)
                {
                    return cnode;
                }
            }

            return null;
        }

        /// <summary>
        /// get all classes in that namespace
        /// </summary>
        /// <param name="ACSFiles"></param>
        /// <param name="ANamespace"></param>
        /// <returns>list of classes</returns>
        public static List <ClassNode>GetClassesInNamespace(List <CSParser>ACSFiles, string ANamespace)
        {
            List <ClassNode>result = new List <ClassNode>();

            foreach (CSParser file in ACSFiles)
            {
                foreach (NamespaceNode nnode in file.cu.Namespaces)
                {
                    if (CSParser.GetName(nnode.Name) == ANamespace)
                    {
                        foreach (ClassNode cnode in nnode.Classes)
                        {
                            result.Add(cnode);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// get all classes defined in this file
        /// </summary>
        /// <returns></returns>
        public List <ClassNode>GetClasses()
        {
            List <ClassNode>result = new List <ClassNode>();

            foreach (NamespaceNode nnode in cu.Namespaces)
            {
                foreach (ClassNode cnode in nnode.Classes)
                {
                    result.Add(cnode);
                }
            }

            return result;
        }

        /// <summary>
        /// return the name of the class
        /// </summary>
        /// <param name="AClass"></param>
        /// <returns></returns>
        public static string GetName(IdentifierExpression AName)
        {
            StringBuilder sb = new StringBuilder();

            AName.ToSource(sb);
            return sb.ToString();
        }

/// <summary>
        /// return the name of the namespace
        /// </summary>
        /// <param name="AClass"></param>
        /// <returns></returns>
        public static string GetName(QualifiedIdentifierExpression AName)
        {
            StringBuilder sb = new StringBuilder();

            AName.ToSource(sb);
            return sb.ToString();
        }

        /// <summary>
        /// get a specific interface
        /// </summary>
        /// <returns></returns>
        public InterfaceNode GetInterface(string ANamespace, string AInterfaceName)
        {
            foreach (NamespaceNode nnode in cu.Namespaces)
            {
                if (GetName(nnode.Name) == ANamespace)
                {
                    foreach (InterfaceNode cnode in nnode.Interfaces)
                    {
                        if (GetName(cnode.Name) == AInterfaceName)
                        {
                            return cnode;
                        }

                        // if the AInterfaceName contains the namespace
                        StringBuilder sb = new StringBuilder();
                        nnode.Name.ToSource(sb);

                        if (AInterfaceName == sb.ToString() + "." + GetName(cnode.Name))
                        {
                            return cnode;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// get a specific class
        /// </summary>
        /// <returns></returns>
        public ClassNode GetClass(string AClassName)
        {
            foreach (NamespaceNode nnode in cu.Namespaces)
            {
                foreach (ClassNode cnode in nnode.Classes)
                {
                    if (GetName(cnode.Name) == AClassName)
                    {
                        return cnode;
                    }

                    // if the AClassName contains the namespace
                    StringBuilder sb = new StringBuilder();
                    nnode.Name.ToSource(sb);

                    if (AClassName == sb.ToString() + "." + GetName(cnode.Name))
                    {
                        return cnode;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// get the full name of a class with namespace separated by a dot
        /// </summary>
        /// <param name="AClassNode">the node of the class</param>
        /// <returns></returns>
        public string GetFullClassNameWithNamespace(ClassNode AClassNode)
        {
            foreach (NamespaceNode nnode in cu.Namespaces)
            {
                foreach (ClassNode cnode in nnode.Classes)
                {
                    if (cnode == AClassNode)
                    {
                        StringBuilder sb = new StringBuilder();
                        nnode.Name.ToSource(sb);
                        return sb.ToString() + "." + GetName(AClassNode.Name);
                    }
                }
            }

            throw new Exception("cannot find class " + AClassNode.Name);
        }

        /**
         * @returns a string collection of the field names of the class
         */
        public static StringCollection GetFields(ClassNode cnode)
        {
            StringCollection list = new StringCollection();

            foreach (FieldNode fnode in cnode.Fields)
            {
                StringBuilder sb = new StringBuilder();
                fnode.Names.ToSource(sb);
                string fieldname = sb.ToString();
                list.Add(fieldname);
            }

            return list;
        }

        /**
         * @returns a list of the properties of the class
         */
        public static List <PropertyNode>GetProperties(ClassNode cnode)
        {
            List <PropertyNode>result = new List <PropertyNode>();

            foreach (PropertyNode pnode in cnode.Properties)
            {
                result.Add(pnode);
            }

            return result;
        }

        /// <summary>
        /// get all methods of the class
        /// </summary>
        /// <returns></returns>
        public static List <MethodNode>GetMethods(ClassNode AClass)
        {
            List <MethodNode>result = new List <MethodNode>();

            foreach (MethodNode mnode in AClass.Methods)
            {
                result.Add(mnode);
            }

            return result;
        }

        /// <summary>
        /// get all methods of the interface
        /// </summary>
        /// <returns></returns>
        public static List <InterfaceMethodNode>GetMethods(InterfaceNode AInterface)
        {
            List <InterfaceMethodNode>result = new List <InterfaceMethodNode>();

            foreach (InterfaceMethodNode mnode in AInterface.Methods)
            {
                result.Add(mnode);
            }

            return result;
        }

        /// <summary>
        /// get all properties of the interface
        /// </summary>
        /// <returns></returns>
        public static List <InterfacePropertyNode>GetProperties(InterfaceNode AInterface)
        {
            List <InterfacePropertyNode>result = new List <InterfacePropertyNode>();

            foreach (InterfacePropertyNode mnode in AInterface.Properties)
            {
                result.Add(mnode);
            }

            return result;
        }

        /// <summary>
        /// the name is packed a little, here a normal string is returned
        /// </summary>
        /// <param name="mnode"></param>
        /// <returns></returns>
        public static string GetName(NodeCollection <QualifiedIdentifierExpression>ANames)
        {
            StringBuilder sb = new StringBuilder();

            ANames.ToSource(sb);
            return sb.ToString();
        }

        /// <summary>
        /// get the name of a type
        /// </summary>
        /// <param name="AType"></param>
        /// <returns></returns>
        public static string GetName(IType AType)
        {
            StringBuilder sb = new StringBuilder();

            AType.ToSource(sb);

            // replace space before < to conform to uncrustify rules
            return sb.ToString().Replace("<", " <");
        }

        /**
         * return the method by name
         */
        public static MethodNode GetMethod(ClassNode cnode, string AMethodName)
        {
            foreach (MethodNode mnode in cnode.Methods)
            {
                if (GetName(mnode.Names) == AMethodName)
                {
                    return mnode;
                }
            }

            throw new Exception(
                "CSParser.GetMethod: cannot find method " +
                AMethodName +
                " in class " + cnode.Name.Identifier);

            //return null;
        }

        private static SortedList <string, List <CSParser>>CSFilesPerProject = new SortedList <string, List <CSParser>>();

        /// <summary>
        /// parse the XML csproj file and return a list of parsed cs files
        /// </summary>
        /// <param name="AProjName"></param>
        /// <returns></returns>
        public static void GetCSFilesInProject(string AProjName, ref List <CSParser>ACSFiles)
        {
            throw new NotImplementedException();

            if (!File.Exists(AProjName))
            {
                return;
            }

            if (CSFilesPerProject.ContainsKey(AProjName))
            {
                ACSFiles = CSFilesPerProject[AProjName];
                return;
            }

            Console.WriteLine("parsing " + AProjName);
            TXMLParser parser = new TXMLParser(AProjName, false);
            XmlDocument doc = parser.GetDocument();

            if (doc.FirstChild.Name != "Project")
            {
                throw new Exception("Ict.Tools.CodeGeneration.CSParser.GetCSFilesInProject: problems parsing csproj xml file " + AProjName);
            }

            XmlNode child = doc.FirstChild.FirstChild;

            while (child != null)
            {
                if (child.Name == "ItemGroup")
                {
                    XmlNode compileNode = child.FirstChild;

                    while (compileNode != null && compileNode.Name == "Compile")
                    {
                        string filename = System.IO.Path.GetDirectoryName(AProjName) +
                                          System.IO.Path.DirectorySeparatorChar +
                                          TXMLParser.GetAttribute(compileNode, "Include");

                        if (File.Exists(filename))
                        {
                            try
                            {
                                CSParser csfile = new CSParser(
                                    filename);
                                ACSFiles.Add(csfile);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Exception while parsing " + filename);
                                Console.WriteLine(e.Message);
                                throw e;
                            }
                        }

                        compileNode = compileNode.NextSibling;
                    }
                }

                child = child.NextSibling;
            }

            CSFilesPerProject.Add(AProjName, ACSFiles);

            return;
        }

        /// <summary>
        /// find a class in a list of csharp files
        /// </summary>
        /// <param name="ACSFiles"></param>
        /// <param name="AClassName"></param>
        /// <returns>null if class cannot be found</returns>
        public static ClassNode FindClass(List <CSParser>ACSFiles, string AClassName)
        {
            foreach (CSParser file in ACSFiles)
            {
                ClassNode t = file.GetClass(AClassName);

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
        public static InterfaceNode FindInterface(List <CSParser>ACSFiles, string ANamespace, string AInterfaceName)
        {
            foreach (CSParser file in ACSFiles)
            {
                InterfaceNode t = file.GetInterface(ANamespace, AInterfaceName);

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
                // TODO: The directory should not be hardcoded!!
                string[] filePaths = Directory.GetFiles(ICTPath + "/../../delivery/nsMap", "*.namespace-map");

                foreach (string filename in filePaths)
                {
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

                            if (!_nsmap.Contains(key))
                            {
                                _nsmap.Add(key, new List <string>());
                            }

                            // Add value as directory name TODO: Convention of "Ict." only once in name!!
                            ((List <string> )_nsmap[key]).Add(ICTPath + val.Replace("Ict.", "/").Replace('.', '/'));
                        }
                    }

                    sr.Close();
                }
            }
        }

        /// <summary>
        /// Reads the nsmap generated from csdepend and searches for all directories
        /// including this namespace
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static List <string>GetSourceDirectory(string ns)
        {
            ReadNamespaceMaps();
            return (List <string> )_nsmap[ns];
        }

        private static Hashtable _CSFilesPerDir = new Hashtable();

        /// <summary>
        /// Returns CSParser instances for the cs files in the given directory.
        /// If you get the list more then one time, then you will get a cached copy
        /// </summary>
        /// <param name="dir">string with the directory to parse</param>
        /// <returns>List of CSParser instances</returns>
        public static List <CSParser>GetCSFilesForDirectory(string dir, SearchOption option)
        {
            string dirfull = Path.GetFullPath(dir);

            if (!_CSFilesPerDir.Contains(dirfull))
            {
                List <CSParser>CSFiles = new List <CSParser>();

                foreach (string filename in Directory.GetFiles(dir, "*.cs", option))
                {
                    CSFiles.Add(new CSParser(filename));
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
        public static List <ClassNode>GetWebConnectorClasses(string AServerNamespace)
        {
            // Look up in nsmap for directory
            List <string>dirList = GetSourceDirectory(AServerNamespace);

            if (null == dirList)
            {
                return null;
            }

            List <CSParser>CSFiles = new List <CSParser>();

            foreach (string dir in dirList)
            {
                Console.WriteLine("Namespace '" + AServerNamespace + "' found in '" + dir + "'\n");

                foreach (CSParser tempCSFile in GetCSFilesForDirectory(dir, SearchOption.TopDirectoryOnly))
                {
                    // Copy the list, because namespace could be in more then one directory
                    CSFiles.Add(tempCSFile);
                }
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
        public static MethodNode GetWebConnectorMethod(List <ClassNode>AWebConnectorClasses,
            string AWebFunction,
            string ATablename,
            out ClassNode AClassOfFoundMethod)
        {
            foreach (ClassNode connectorClass in AWebConnectorClasses)
            {
                foreach (MethodNode m in CSParser.GetMethods(connectorClass))
                {
                    string MethodName = CSParser.GetName(m.Names);

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

        /*
         * todo: function that returns the lines of a method, including the line numbers???
         */

        /*
         * int firstLine = -1;
         * int lastLine = -1;
         * foreach (StatementNode snode in mnode.StatementBlock.Statements)
         * {
         *  if (firstLine == -1)
         *  {
         *    firstLine = snode.RelatedToken.Line;
         *  }
         *  lastLine = snode.RelatedToken.Line;
         *  string line = ACSharpfile.ToSource(snode);
         * }
         */
    }
}