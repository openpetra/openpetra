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
using System.Reflection;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using NamespaceHierarchy;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.CodeGeneration;
using GenerateSharedCode;

namespace GenerateGlue
{
    /// <summary>
    /// generate the code for the client
    /// </summary>
    public class GenerateClientGlue
    {
        static private string FTemplateDir = string.Empty;
        static private bool FCompileForStandalone = false;
        static SortedList <string, string>FUsingNamespaces = null;
        static bool FModuleHasUIConnector = false;
        static List <string>FUIConnectorsAdded = null;

        private static string GetExpectedReturnType(int ResultCounter, string returntype)
        {
            string expectedreturntype = string.Empty;

            if (!returntype.Contains("<"))
            {
                returntype = returntype == "string" || returntype == "String" ? "System.String" : returntype;
                returntype = returntype == "bool" || returntype == "Boolean" ? "System.Boolean" : returntype;

                if (returntype.Contains("UINT") || returntype.Contains("unsigned"))
                {
                    returntype = returntype.Contains("UInt32") || returntype == "unsigned int" ? "System.UInt32" : returntype;
                    returntype = returntype.Contains("UInt16") || returntype == "unsigned short" ? "System.UInt16" : returntype;
                    returntype = returntype.Contains("UInt64") || returntype == "unsigned long" ? "System.UInt64" : returntype;
                }
                else
                {
                    returntype = returntype.Contains("Int32") || returntype == "int" ? "System.Int32" : returntype;
                    returntype = returntype.Contains("Int16") || returntype == "short" ? "System.Int16" : returntype;
                    returntype = returntype.Contains("Int64") || returntype == "long" ? "System.Int64" : returntype;
                }

                returntype = returntype.Contains("Decimal") || returntype == "decimal" ? "System.Decimal" : returntype;
            }

            if (ResultCounter > 0)
            {
                expectedreturntype = "list";
            }
            else if ((returntype == "System.Int64") || (returntype == "System.Int32") || (returntype == "System.Int16")
                     || (returntype == "System.UInt64") || (returntype == "System.UInt32") || (returntype == "System.UInt16")
                     || (returntype == "System.Decimal")
                     || (returntype == "System.String") || (returntype == "System.Boolean") || (returntype == "void"))
            {
                expectedreturntype = returntype;
            }
            else
            {
                expectedreturntype = "binary";
            }

            return expectedreturntype;
        }

        private static bool CheckServerAdminToken(MethodDeclaration m)
        {
            if (m.Attributes != null)
            {
                foreach (AttributeSection attrSection in m.Attributes)
                {
                    foreach (ICSharpCode.NRefactory.Ast.Attribute attr in attrSection.Attributes)
                    {
                        if (attr.Name == "CheckServerAdminToken")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// insert a method call
        /// </summary>
        private static void InsertMethodCall(ProcessTemplate snippet, TypeDeclaration connectorClass, MethodDeclaration m,
            ref List <string>AMethodNames)
        {
            string ParameterDefinition = string.Empty;
            string ActualParameters = string.Empty;

            AutoGenerationTools.FormatParameters(m.Parameters, out ActualParameters, out ParameterDefinition);

            // for standalone: add actual parameters directly
            snippet.AddToCodelet("ACTUALPARAMETERS", ActualParameters);

            string returntype = AutoGenerationTools.TypeToString(m.TypeReference, "");

            if (!returntype.Contains("<"))
            {
                returntype = returntype == "string" || returntype == "String" ? "System.String" : returntype;
                returntype = returntype == "bool" || returntype == "Boolean" ? "System.Boolean" : returntype;

                if (returntype.Contains("UINT") || returntype.Contains("unsigned"))
                {
                    returntype = returntype.Contains("UInt32") || returntype == "unsigned int" ? "System.UInt32" : returntype;
                    returntype = returntype.Contains("UInt16") || returntype == "unsigned short" ? "System.UInt16" : returntype;
                    returntype = returntype.Contains("UInt64") || returntype == "unsigned long" ? "System.UInt64" : returntype;
                }
                else
                {
                    returntype = returntype.Contains("Int32") || returntype == "int" ? "System.Int32" : returntype;
                    returntype = returntype.Contains("Int16") || returntype == "short" ? "System.Int16" : returntype;
                    returntype = returntype.Contains("Int64") || returntype == "long" ? "System.Int64" : returntype;
                }

                returntype = returntype.Contains("Decimal") || returntype == "decimal" ? "System.Decimal" : returntype;
            }

            snippet.SetCodelet("RETURN", returntype != "void" ? "return " : string.Empty);

            // avoid duplicate names for webservice methods
            string methodname = m.Name;
            int methodcounter = 1;

            while (AMethodNames.Contains(methodname))
            {
                methodcounter++;
                methodname = m.Name + methodcounter.ToString();
            }

            AMethodNames.Add(methodname);

            snippet.SetCodelet("UNIQUEMETHODNAME", methodname);
            snippet.SetCodelet("METHODNAME", m.Name);
            snippet.SetCodelet("PARAMETERDEFINITION", ParameterDefinition);
            snippet.SetCodelet("RETURNTYPE", returntype);
            snippet.SetCodelet("WEBCONNECTORCLASS", connectorClass.Name);
            snippet.SetCodelet("ASSIGNRESULTANDRETURN", string.Empty);
            snippet.SetCodelet("ADDACTUALPARAMETERS", string.Empty);

            int ResultCounter = 0;

            if (CheckServerAdminToken(m))
            {
                snippet.AddToCodelet("ADDACTUALPARAMETERS",
                    "ActualParameters.Add(\"AServerAdminSecurityToken\", THttpConnector.ServerAdminSecurityToken" +
                    ");" + Environment.NewLine);
            }

            foreach (ParameterDeclarationExpression p in m.Parameters)
            {
                if (((ParameterModifiers.Ref & p.ParamModifier) > 0) || ((ParameterModifiers.Out & p.ParamModifier) > 0))
                {
                    // need to assign the result to the ref and the out parameter
                    snippet.AddToCodelet("ASSIGNRESULTANDRETURN",
                        p.ParameterName + " = (" + p.TypeReference.ToString() + ") Result[" + ResultCounter.ToString() + "];" +
                        Environment.NewLine);
                    ResultCounter++;
                }

                if ((ParameterModifiers.Out & p.ParamModifier) == 0)
                {
                    snippet.AddToCodelet("ADDACTUALPARAMETERS",
                        "ActualParameters.Add(\"" + p.ParameterName + "\", " +
                        p.ParameterName + ");" + Environment.NewLine);
                }
            }

            string expectedreturntype = GetExpectedReturnType(ResultCounter, returntype);

            snippet.SetCodelet("EXPECTEDRETURNTYPE", expectedreturntype);
            snippet.SetCodelet("RESULT", string.Empty);

            if ((returntype != "void") || (ResultCounter > 0))
            {
                snippet.SetCodelet("RESULT", "List<object> Result = ");
            }

            if (returntype != "void")
            {
                snippet.AddToCodelet("ASSIGNRESULTANDRETURN",
                    "return (" + returntype + ") Result[" + ResultCounter.ToString() + "];" + Environment.NewLine);
            }
        }

        private static void ImplementWebConnector(
            SortedList <string, TypeDeclaration>connectors,
            ProcessTemplate ATemplate, string AFullNamespace)
        {
            string ConnectorNamespace = AFullNamespace.
                                        Replace("Instantiator.", string.Empty);

            List <TypeDeclaration>ConnectorClasses = TCollectConnectorInterfaces.FindTypesInNamespace(connectors, ConnectorNamespace);

            ConnectorNamespace = ConnectorNamespace.
                                 Replace(".Shared.", ".Server.");

            foreach (TypeDeclaration connectorClass in ConnectorClasses)
            {
                List <string>MethodNames = new List <string>();

                foreach (MethodDeclaration m in CSParser.GetMethods(connectorClass))
                {
                    if (TCollectConnectorInterfaces.IgnoreMethod(m.Attributes, m.Modifier))
                    {
                        continue;
                    }

                    ProcessTemplate snippet;

                    if (FCompileForStandalone)
                    {
                        if (!FUsingNamespaces.ContainsKey(ConnectorNamespace))
                        {
                            FUsingNamespaces.Add(ConnectorNamespace, ConnectorNamespace);
                        }

                        snippet = ATemplate.GetSnippet("WEBCONNECTORMETHODSTANDALONE");
                    }
                    else
                    {
                        snippet = ATemplate.GetSnippet("WEBCONNECTORMETHODREMOTE");
                    }

                    InsertMethodCall(snippet, connectorClass, m, ref MethodNames);

                    ATemplate.InsertSnippet("CONNECTORMETHODS", snippet);
                }
            }
        }

        private static void InsertMethodsAndProperties(ProcessTemplate template, TypeDeclaration t)
        {
            List <string>MethodNames = new List <string>();

            // foreach public method create a method
            foreach (MethodDeclaration m in CSParser.GetMethods(t))
            {
                if (TCollectConnectorInterfaces.IgnoreMethod(m.Attributes, m.Modifier))
                {
                    continue;
                }

                ProcessTemplate methodSnippet = template.GetSnippet("UICONNECTORMETHOD");

                InsertMethodCall(methodSnippet, t, m, ref MethodNames);

                template.InsertSnippet("METHODSANDPROPERTIES", methodSnippet);
            }

            // foreach public property create a method
            foreach (PropertyDeclaration p in CSParser.GetProperties(t))
            {
                if (TCollectConnectorInterfaces.IgnoreMethod(p.Attributes, p.Modifier))
                {
                    continue;
                }

                ProcessTemplate propertySnippet = template.GetSnippet("UICONNECTORPROPERTY");

                string type = AutoGenerationTools.TypeToString(p.TypeReference, string.Empty);
                propertySnippet.SetCodelet("NAME", p.Name);
                propertySnippet.SetCodelet("TYPE", type);

                string expectedreturntype = GetExpectedReturnType(0, AutoGenerationTools.TypeToString(p.TypeReference, string.Empty));

                propertySnippet.SetCodelet("EXPECTEDRETURNTYPE", expectedreturntype);

                if (p.HasGetRegion)
                {
                    if (type.StartsWith("I"))
                    {
                        propertySnippet.SetCodelet(
                            "GETTER",
                            "return new T" + type.Substring(
                                1) +
                            "(\"M{#TOPLEVELMODULE}\", THttpConnector.ReadUIConnectorProperty(FObjectID, \"M{#TOPLEVELMODULE}\", \"{#UICONNECTORCLASSNAME}\", \"{#NAME}\", \"System.String\").ToString());");
                    }
                    else
                    {
                        propertySnippet.SetCodelet(
                            "GETTER",
                            "return ({#TYPE}) THttpConnector.ReadUIConnectorProperty(FObjectID, \"M{#TOPLEVELMODULE}\", \"{#UICONNECTORCLASSNAME}\", \"{#NAME}\", \"{#EXPECTEDRETURNTYPE}\");");
                    }
                }

                if (p.HasSetRegion)
                {
                    propertySnippet.SetCodelet("SETTER", "yes");
                }

                template.InsertSnippet("METHODSANDPROPERTIES", propertySnippet);
            }
        }

        private static ProcessTemplate GenerateUIConnector(ProcessTemplate ATemplate, TypeDeclaration connectorClass, string interfacename)
        {
            ProcessTemplate snippet = ATemplate.GetSnippet("UICONNECTORCLASS");

            snippet.SetCodelet("UICONNECTORINTERFACE", interfacename);
            snippet.SetCodelet("UICONNECTORCLASSNAME", connectorClass.Name);
            snippet.SetCodelet("CONSTRUCTORS", string.Empty);

            int constructorCounter = 0;

            foreach (ConstructorDeclaration m in CSParser.GetConstructors(connectorClass))
            {
                if (TCollectConnectorInterfaces.IgnoreMethod(m.Attributes, m.Modifier))
                {
                    continue;
                }

                constructorCounter++;

                ProcessTemplate snippetConstructor = ATemplate.GetSnippet("UICONNECTORCONSTRUCTOR");

                string ParameterDefinition = string.Empty;
                string ActualParameters = string.Empty;

                AutoGenerationTools.FormatParameters(m.Parameters, out ActualParameters, out ParameterDefinition);

                snippetConstructor.SetCodelet("PARAMETERDEFINITION", ParameterDefinition);
                snippetConstructor.SetCodelet("UICONNECTORCLASSNAME", connectorClass.Name);
                snippetConstructor.SetCodelet("ACTUALPARAMETERS", ActualParameters);
                snippetConstructor.SetCodelet("ADDACTUALPARAMETERS", string.Empty);

                foreach (ParameterDeclarationExpression p in m.Parameters)
                {
                    if (((ParameterModifiers.Ref & p.ParamModifier) > 0) || ((ParameterModifiers.Out & p.ParamModifier) > 0))
                    {
                        throw new Exception("we do not support ref or out parameters in UIConnector constructor calls! " + connectorClass.Name);
                    }

                    snippetConstructor.AddToCodelet("ADDACTUALPARAMETERS",
                        "ActualParameters.Add(\"" + p.ParameterName + "\", " +
                        p.ParameterName + ");" + Environment.NewLine);
                }

                string methodname = m.Name;

                if (constructorCounter > 1)
                {
                    methodname += constructorCounter.ToString();
                }

                snippetConstructor.SetCodelet("METHODNAME", methodname);

                snippet.InsertSnippet("CONSTRUCTORS", snippetConstructor);
            }

            InsertMethodsAndProperties(snippet, connectorClass);

            return snippet;
        }

        private static void ImplementUIConnector(
            SortedList <string, TypeDeclaration>connectors,
            ProcessTemplate ATemplate, string AFullNamespace)
        {
            string ConnectorNamespace = AFullNamespace.
                                        Replace("Instantiator.", string.Empty);

            List <TypeDeclaration>ConnectorClasses = TCollectConnectorInterfaces.FindTypesInNamespace(connectors, ConnectorNamespace);

            ConnectorNamespace = ConnectorNamespace.
                                 Replace(".Shared.", ".Server.");

            foreach (TypeDeclaration connectorClass in ConnectorClasses)
            {
                foreach (ConstructorDeclaration m in CSParser.GetConstructors(connectorClass))
                {
                    if (TCollectConnectorInterfaces.IgnoreMethod(m.Attributes, m.Modifier))
                    {
                        continue;
                    }

                    ProcessTemplate snippet;

                    FModuleHasUIConnector = true;

                    if (FCompileForStandalone)
                    {
                        if (!FUsingNamespaces.ContainsKey(ConnectorNamespace))
                        {
                            FUsingNamespaces.Add(ConnectorNamespace, ConnectorNamespace);
                        }

                        snippet = ATemplate.GetSnippet("UICONNECTORMETHODSTANDALONE");
                    }
                    else
                    {
                        snippet = ATemplate.GetSnippet("UICONNECTORMETHODREMOTE");
                    }

                    string ParameterDefinition = string.Empty;
                    string ActualParameters = string.Empty;

                    AutoGenerationTools.FormatParameters(m.Parameters, out ActualParameters, out ParameterDefinition);

                    string methodname = m.Name.Substring(1);

                    if (methodname.EndsWith("UIConnector"))
                    {
                        methodname = methodname.Substring(0, methodname.LastIndexOf("UIConnector"));
                    }

                    string interfacename = CSParser.GetImplementedInterface(connectorClass);
                    snippet.SetCodelet("METHODNAME", methodname);
                    snippet.SetCodelet("PARAMETERDEFINITION", ParameterDefinition);
                    snippet.SetCodelet("ACTUALPARAMETERS", ActualParameters);
                    snippet.SetCodelet("UICONNECTORINTERFACE", interfacename);
                    snippet.SetCodelet("UICONNECTORCLASSNAME", connectorClass.Name);

                    snippet.SetCodelet("UICONNECTORCLASS", string.Empty);

                    if (!FCompileForStandalone)
                    {
                        if (!FUIConnectorsAdded.Contains(connectorClass.Name))
                        {
                            FUIConnectorsAdded.Add(connectorClass.Name);

                            snippet.InsertSnippet("UICONNECTORCLASS",
                                GenerateUIConnector(ATemplate, connectorClass, interfacename));
                        }
                    }

                    ATemplate.InsertSnippet("CONNECTORMETHODS", snippet);
                }
            }
        }

        static private void InsertSubNamespaces(ProcessTemplate ATemplate,
            SortedList <string, TypeDeclaration>connectors,
            string AParentNamespaceName,
            string AFullNamespace,
            TNamespace AParentNamespace)
        {
            ATemplate.SetCodelet("SUBNAMESPACECLASSES", string.Empty);
            ATemplate.SetCodelet("SUBNAMESPACEPROPERTIES", string.Empty);
            ATemplate.SetCodelet("CONNECTORMETHODS", string.Empty);

            foreach (TNamespace sn in AParentNamespace.Children.Values)
            {
                ProcessTemplate templateProperty = ATemplate.GetSnippet("SUBNAMESPACEPROPERTY");
                templateProperty.SetCodelet("NAMESPACENAME", AParentNamespaceName + sn.Name);
                templateProperty.SetCodelet("SUBNAMESPACENAME", sn.Name);
                templateProperty.SetCodelet("OBJECTNAME", sn.Name);

                ProcessTemplate templateClass = ATemplate.GetSnippet("NAMESPACECLASS");
                templateClass.SetCodelet("NAMESPACENAME", AParentNamespaceName + sn.Name);
                InsertSubNamespaces(templateClass, connectors,
                    AParentNamespaceName + sn.Name,
                    AFullNamespace + "." + sn.Name,
                    sn);

                ATemplate.InsertSnippet("SUBNAMESPACECLASSES", templateClass);

                ATemplate.InsertSnippet("SUBNAMESPACEPROPERTIES", templateProperty);
            }

            if (AParentNamespaceName.EndsWith("WebConnectors"))
            {
                ImplementWebConnector(connectors, ATemplate, AFullNamespace);
            }
            else
            {
                ImplementUIConnector(connectors, ATemplate, AFullNamespace);
            }
        }

        static private void CreateClientGlue(TNamespace tn, SortedList <string, TypeDeclaration>connectors, string AOutputPath)
        {
            String OutputFile = AOutputPath + Path.DirectorySeparatorChar + "ClientGlue.M" + tn.Name +
                                "-generated.cs";

            Console.WriteLine("working on " + OutputFile);

            ProcessTemplate Template = new ProcessTemplate(FTemplateDir + Path.DirectorySeparatorChar +
                "ClientServerGlue" + Path.DirectorySeparatorChar +
                "ClientGlue.cs");

            FUsingNamespaces = new SortedList <string, string>();
            FModuleHasUIConnector = false;
            FUIConnectorsAdded = new List <string>();

            // load default header with license and copyright
            Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(FTemplateDir));

            Template.SetCodelet("TOPLEVELMODULE", tn.Name);

            string InterfacePath = Path.GetFullPath(AOutputPath).Replace(Path.DirectorySeparatorChar, '/');
            InterfacePath = InterfacePath.Substring(0, InterfacePath.IndexOf("csharp/ICT/Petra")) + "csharp/ICT/Petra/Shared/lib/Interfaces";
            Template.AddToCodelet("USINGNAMESPACES", CreateInterfaces.AddNamespacesFromYmlFile(InterfacePath, tn.Name));

            if (AOutputPath.Contains("ICT/Petra/Plugins/"))
            {
                // add namespaces that are required by the plugin
                InterfacePath = Path.GetFullPath(AOutputPath + "/../").Replace(Path.DirectorySeparatorChar, '/');
                Template.AddToCodelet("USINGNAMESPACES", CreateInterfaces.AddNamespacesFromYmlFile(InterfacePath, "Plugin"));
            }

            string SharedPathName = "Ict.Petra.Shared.M" + tn.Name;

            if (SharedPathName.Contains("ServerAdmin"))
            {
                SharedPathName = "Ict.Petra.Server.App.Core." + tn.Name;
            }
            else if (OutputFile.Contains("ICT/Petra/Plugins"))
            {
                SharedPathName = "Ict.Petra.Plugins." + tn.Name;
            }

            InsertSubNamespaces(Template, connectors, tn.Name, SharedPathName, tn);

            if (FModuleHasUIConnector)
            {
                Template.AddToCodelet("USINGNAMESPACES", "using Ict.Petra.Shared.Interfaces.M" + tn.Name + ";" + Environment.NewLine);
            }

            foreach (string usingNamespace in FUsingNamespaces.Keys)
            {
                Template.AddToCodelet("USINGNAMESPACES", "using " + usingNamespace + ";" + Environment.NewLine);
            }

            if (OutputFile.Contains("ClientGlue.MServerAdmin"))
            {
                Template.SetCodelet("REMOTEOBJECTSNAMESPACE", "Ict.Petra.ServerAdmin.App.Core.RemoteObjects");
            }
            else if (OutputFile.Contains("ICT/Petra/Plugins"))
            {
                string pluginWithNamespace = TAppSettingsManager.GetValue("plugin");
                Template.SetCodelet("REMOTEOBJECTSNAMESPACE", pluginWithNamespace + ".RemoteObjects");
            }
            else
            {
                Template.SetCodelet("REMOTEOBJECTSNAMESPACE", "Ict.Petra.Client.App.Core.RemoteObjects");
            }

            Template.FinishWriting(OutputFile, ".cs", true);
        }

        /// <summary>
        /// generate the connector code for the client
        /// </summary>
        static public void GenerateConnectorCode(String AOutputPath, String ATemplateDir)
        {
            String OutputFile = AOutputPath + Path.DirectorySeparatorChar + "ClientGlue.Connector-generated.cs";

            Console.WriteLine("working on " + OutputFile);

            ProcessTemplate Template = new ProcessTemplate(FTemplateDir + Path.DirectorySeparatorChar +
                "ClientServerGlue" + Path.DirectorySeparatorChar +
                "ClientGlue.Connector.cs");

            // load default header with license and copyright
            Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(FTemplateDir));
            Template.SetCodelet("USINGNAMESPACES", string.Empty);

            if (FCompileForStandalone)
            {
                Template.AddToCodelet("USINGNAMESPACES", "using Ict.Common.DB;" + Environment.NewLine);
                Template.AddToCodelet("USINGNAMESPACES", "using Ict.Common.Remoting.Server;" + Environment.NewLine);
                Template.AddToCodelet("USINGNAMESPACES", "using Ict.Petra.Server.App.Core;" + Environment.NewLine);
                Template.AddToCodelet("USINGNAMESPACES", "using Ict.Petra.Server.App.Delegates;" + Environment.NewLine);
                Template.AddToCodelet("USINGNAMESPACES", "using Ict.Petra.Shared;" + Environment.NewLine);
                Template.AddToCodelet("USINGNAMESPACES", "using System.Security.Principal;" + Environment.NewLine);
                Template.InsertSnippet("CONNECTOR", Template.GetSnippet("CONNECTORSTANDALONE"));
                Template.InsertSnippet("STANDALONECLIENTMANAGER", Template.GetSnippet("STANDALONECLIENTMANAGER"));
            }
            else
            {
                Template.InsertSnippet("CONNECTOR", Template.GetSnippet("CONNECTORCLIENTSERVER"));
                Template.SetCodelet("STANDALONECLIENTMANAGER", string.Empty);
                Template.SetCodelet("HTTPREMOTING", "true");
            }

            Template.FinishWriting(OutputFile, ".cs", true);
        }

        /// <summary>
        /// write the client code
        /// </summary>
        static public void GenerateCode(TNamespace ANamespaces, String AOutputPath, String ATemplateDir)
        {
            FTemplateDir = ATemplateDir;
            FCompileForStandalone = TAppSettingsManager.GetValue("compileForStandalone", "false", false) == "true";

            foreach (TNamespace tn in ANamespaces.Children.Values)
            {
                string module = TAppSettingsManager.GetValue("module", "all");

                if ((module == "all") || (tn.Name == module))
                {
                    SortedList <string, TypeDeclaration>connectors = TCollectConnectorInterfaces.GetConnectors(AOutputPath, tn.Name);
                    CreateClientGlue(tn, connectors, AOutputPath);
                }
            }
        }
    }
}
