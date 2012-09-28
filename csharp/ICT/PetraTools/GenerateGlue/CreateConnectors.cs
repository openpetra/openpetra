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


namespace GenerateSharedCode
{
/// <summary>
/// this class generates the connectors for remoting
/// it uses the file ICT/Petra/Definitions/NamespaceHierarchy.xml
/// </summary>
class TCreateConnectors
{
    private string FTemplateDir = string.Empty;

    private ProcessTemplate CreateModuleAccessPermissionCheck(ProcessTemplate ATemplate, string AConnectorClassWithNamespace, MethodDeclaration m)
    {
        if (m.Attributes != null)
        {
            foreach (AttributeSection attrSection in m.Attributes)
            {
                foreach (ICSharpCode.NRefactory.Ast.Attribute attr in attrSection.Attributes)
                {
                    if (attr.Name == "RequireModulePermission")
                    {
                        ProcessTemplate snippet = ATemplate.GetSnippet("CHECKUSERMODULEPERMISSIONS");
                        snippet.SetCodelet("METHODNAME", m.Name);
                        snippet.SetCodelet("CONNECTORWITHNAMESPACE", AConnectorClassWithNamespace);
                        snippet.SetCodelet("LEDGERNUMBER", "");

                        string ParameterTypes = ";";

                        foreach (ParameterDeclarationExpression p in m.Parameters)
                        {
                            if (p.ParameterName == "ALedgerNumber")
                            {
                                snippet.SetCodelet("LEDGERNUMBER", ", ALedgerNumber");
                            }

                            string ParameterType = p.TypeReference.Type.Replace("&", "").Replace("System.", String.Empty);

                            if (ParameterType == "List")
                            {
                                ParameterType = ParameterType.Replace("List", "List[" + p.TypeReference.GenericTypes[0].ToString() + "]");
                                ParameterType = ParameterType.Replace("System.", String.Empty);
                            }

                            if (ParameterType == "Dictionary")
                            {
                                ParameterType = ParameterType.Replace("Dictionary", "Dictionary[" +
                                    p.TypeReference.GenericTypes[0].ToString() + "," +
                                    p.TypeReference.GenericTypes[1].ToString() + "]");
                                ParameterType = ParameterType.Replace("System.", String.Empty);
                            }

                            if (ParameterType.Contains("."))
                            {
                                ParameterType = ParameterType.Substring(ParameterType.LastIndexOf(".") + 1);
                            }

                            if (p.TypeReference.Type == "System.Nullable")
                            {
                                ParameterType = ParameterType.Replace("Nullable", "Nullable[" + p.TypeReference.GenericTypes[0].ToString() + "]");
                            }

                            if (p.TypeReference.IsArrayType)
                            {
                                ParameterType += ".ARRAY";
                            }

                            ParameterType = ParameterType.Replace("Boolean", "bool");
                            ParameterType = ParameterType.Replace("Int32", "int");
                            ParameterType = ParameterType.Replace("Int64", "long");

                            ParameterTypes += ParameterType + ";";
                        }

                        ParameterTypes = ParameterTypes.ToUpper();
                        snippet.SetCodelet("PARAMETERTYPES", ParameterTypes);
                        return snippet;
                    }
                }
            }
        }

        TLogging.Log("Warning !!! Missing module access permissions for " + AConnectorClassWithNamespace + "::" + m.Name);

        return new ProcessTemplate();
    }

    List <string>UsingConnectorNamespaces = null;

    void ImplementWebConnector(
        SortedList <string, TypeDeclaration>connectors,
        ProcessTemplate ATemplate, string AFullNamespace)
    {
        string ConnectorNamespace = AFullNamespace.
                                    Replace("Instantiator.", string.Empty);

        List <TypeDeclaration>ConnectorClasses = TCollectConnectorInterfaces.FindTypesInNamespace(connectors, ConnectorNamespace);

        ConnectorNamespace = ConnectorNamespace.
                             Replace("Ict.Petra.Shared.", "Ict.Petra.Server.");

        ATemplate.SetCodelet("CLIENTOBJECTFOREACHUICONNECTOR", string.Empty);

        foreach (TypeDeclaration connectorClass in ConnectorClasses)
        {
            foreach (MethodDeclaration m in CSParser.GetMethods(connectorClass))
            {
                if (TCollectConnectorInterfaces.IgnoreMethod(m.Attributes, m.Modifier))
                {
                    continue;
                }

                ProcessTemplate snippet = ATemplate.GetSnippet("WEBCONNECTORMETHOD");

                string ParameterDefinition = string.Empty;
                string ActualParameters = string.Empty;

                AutoGenerationTools.FormatParameters(m.Parameters, out ActualParameters, out ParameterDefinition);

                snippet.InsertSnippet("CHECKUSERMODULEPERMISSIONS",
                    CreateModuleAccessPermissionCheck(
                        ATemplate,
                        connectorClass.Name,
                        m));

                string returntype = AutoGenerationTools.TypeToString(m.TypeReference, "");

                snippet.SetCodelet("RETURN", returntype != "void" ? "return " : string.Empty);

                snippet.SetCodelet("METHODNAME", m.Name);
                snippet.SetCodelet("ACTUALPARAMETERS", ActualParameters);
                snippet.SetCodelet("PARAMETERDEFINITION", ParameterDefinition);
                snippet.SetCodelet("RETURNTYPE", returntype);
                snippet.SetCodelet("WEBCONNECTORCLASS", connectorClass.Name);

                if (!UsingConnectorNamespaces.Contains(ConnectorNamespace))
                {
                    UsingConnectorNamespaces.Add(ConnectorNamespace);
                }

                ATemplate.InsertSnippet("REMOTEDMETHODS", snippet);
            }
        }
    }

    void ImplementUIConnector(
        SortedList <string, TypeDeclaration>connectors,
        ProcessTemplate ATemplate, string AFullNamespace)
    {
        string ConnectorNamespace = AFullNamespace.
                                    Replace("Instantiator.", string.Empty);

        List <TypeDeclaration>ConnectorClasses = TCollectConnectorInterfaces.FindTypesInNamespace(connectors, ConnectorNamespace);

        ConnectorNamespace = ConnectorNamespace.
                             Replace("Ict.Petra.Shared.", "Ict.Petra.Server.");

        ATemplate.SetCodelet("CLIENTOBJECTFOREACHUICONNECTOR", string.Empty);

        foreach (TypeDeclaration connectorClass in ConnectorClasses)
        {
            foreach (ConstructorDeclaration m in CSParser.GetConstructors(connectorClass))
            {
                if (TCollectConnectorInterfaces.IgnoreMethod(m.Attributes, m.Modifier))
                {
                    continue;
                }

                ProcessTemplate snippet = ATemplate.GetSnippet("UICONNECTORMETHOD");

                string ParameterDefinition = string.Empty;
                string ActualParameters = string.Empty;

                AutoGenerationTools.FormatParameters(m.Parameters, out ActualParameters, out ParameterDefinition);

                string methodname = m.Name.Substring(1);

                if (methodname.EndsWith("UIConnector"))
                {
                    methodname = methodname.Substring(0, methodname.LastIndexOf("UIConnector"));
                }

                snippet.SetCodelet("METHODNAME", methodname);
                snippet.SetCodelet("PARAMETERDEFINITION", ParameterDefinition);
                snippet.SetCodelet("ACTUALPARAMETERS", ActualParameters);
                snippet.SetCodelet("UICONNECTORINTERFACE", CSParser.GetImplementedInterface(connectorClass));
                snippet.SetCodelet("UICONNECTORCLIENTREMOTINGCLASS", connectorClass.Name + "Remote");
                snippet.SetCodelet("UICONNECTORCLASS", connectorClass.Name);

                if (!UsingConnectorNamespaces.Contains(ConnectorNamespace))
                {
                    UsingConnectorNamespaces.Add(ConnectorNamespace);
                }

                ATemplate.InsertSnippet("REMOTEDMETHODS", snippet);
            }

            List <TypeDeclaration>tempList = new List <TypeDeclaration>();
            tempList.Add(connectorClass);

            ATemplate.InsertSnippet("CLIENTOBJECTFOREACHUICONNECTOR",
                TCreateClientRemotingClass.AddClientRemotingClass(
                    FTemplateDir,
                    connectorClass.Name + "Remote",
                    CSParser.GetImplementedInterface(connectorClass),
                    tempList
                    ));
        }
    }

    private ProcessTemplate WriteConnectorClass(ProcessTemplate ATemplate,
        String FullNamespace,
        String Classname,
        String Namespace,
        SortedList <string, TNamespace>children,
        SortedList <string, TypeDeclaration>connectors)
    {
        if (children.Count > 0)
        {
            foreach (TNamespace sn in children.Values)
            {
                WriteConnectorClass(
                    ATemplate,
                    FullNamespace + "." + sn.Name,
                    sn.Name,
                    sn.Name,
                    sn.Children,
                    connectors);
            }

            return ATemplate;
        }

        ProcessTemplate connectorClassSnippet = ATemplate.GetSnippet("CONNECTORCLASS");

        string NamespaceInModule = FullNamespace.Substring(
            FullNamespace.IndexOf('.', "Ict.Petra.Shared.M".Length) + 1).Replace(".", string.Empty);

        connectorClassSnippet.SetCodelet("NAMESPACE", NamespaceInModule);

        connectorClassSnippet.SetCodelet("REMOTEDMETHODS", string.Empty);

        if (Namespace.EndsWith("WebConnectors"))
        {
            ImplementWebConnector(connectors, connectorClassSnippet, FullNamespace);
        }
        else
        {
            ImplementUIConnector(connectors, connectorClassSnippet, FullNamespace);
        }

        ATemplate.InsertSnippet("CONNECTORCLASSES", connectorClassSnippet);
        return ATemplate;
    }

    private void CreateConnectors(TNamespace tn, String AOutputPath, String ATemplateDir)
    {
        String OutputFile = AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                            Path.DirectorySeparatorChar + "Instantiator.Connectors-generated.cs";

        if (Directory.Exists(AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                Path.DirectorySeparatorChar + "connect"))
        {
            OutputFile = AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                         Path.DirectorySeparatorChar + "connect" +
                         Path.DirectorySeparatorChar + "Instantiator.Connectors-generated.cs";
        }

        Console.WriteLine("working on " + OutputFile);

        SortedList <string, TypeDeclaration>connectors = TCollectConnectorInterfaces.GetConnectors(tn.Name);

        ProcessTemplate Template = new ProcessTemplate(ATemplateDir + Path.DirectorySeparatorChar +
            "ClientServerGlue" + Path.DirectorySeparatorChar +
            "Connector.cs");

        // load default header with license and copyright
        Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(ATemplateDir));

        Template.SetCodelet("TOPLEVELMODULE", tn.Name);

        Template.AddToCodelet("USINGNAMESPACES", "using Ict.Petra.Shared.Interfaces.M" + tn.Name + ";" + Environment.NewLine);

        UsingConnectorNamespaces = new List <string>();

        string InterfacePath = Path.GetFullPath(AOutputPath).Replace(Path.DirectorySeparatorChar, '/');
        InterfacePath = InterfacePath.Substring(0, InterfacePath.IndexOf("csharp/ICT/Petra")) + "csharp/ICT/Petra/Shared/lib/Interfaces";
        Template.AddToCodelet("USINGNAMESPACES", (CreateInterfaces.AddNamespacesFromYmlFile(InterfacePath, tn.Name)));

        Template.SetCodelet("CONNECTORCLASSES", string.Empty);

        foreach (TNamespace sn in tn.Children.Values)
        {
            WriteConnectorClass(
                Template,
                "Ict.Petra.Shared.M" + tn.Name + "." + sn.Name,
                sn.Name,
                sn.Name,
                sn.Children,
                connectors);
        }

        foreach (string n in UsingConnectorNamespaces)
        {
            Template.AddToCodelet("USINGNAMESPACES", "using " + n + ";" + Environment.NewLine);
        }

        Template.FinishWriting(OutputFile, ".cs", true);
    }

    public void CreateFiles(TNamespace ANamespaces, String AOutputPath, String ATemplateDir)
    {
        FTemplateDir = ATemplateDir;

        foreach (TNamespace tn in ANamespaces.Children.Values)
        {
            string module = TAppSettingsManager.GetValue("module", "all");

            if ((module == "all") || (tn.Name == module))
            {
                CreateConnectors(tn, AOutputPath, ATemplateDir);
            }
        }
    }
}
}