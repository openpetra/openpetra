//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Tools.CodeGeneration;


namespace GenerateSharedCode
{
/// <summary>
/// this class generates the instantiators for remoting
/// it uses the file ICT/Petra/Definitions/NamespaceHierarchy.xml
/// it also uses the compiled dll file Petra/Shared/_bin/Server_Client/Debug/Ict.Petra.Shared.Interfaces.dll
/// to pick up the end points (basically from Petra\Shared\lib\Interfaces\*.EndPoints.cs
/// </summary>
class CreateInstantiators : AutoGenerationWriter
{
    private ProcessTemplate WriteLoaderClass(ProcessTemplate ATemplate, String module)
    {
        ProcessTemplate loaderClassSnippet = ATemplate.GetSnippet("LOADERCLASS");

        loaderClassSnippet.SetCodelet("MODULE", module);
        return loaderClassSnippet;
    }

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

                        string ParameterTypes = ";";

                        foreach (ParameterDeclarationExpression p in m.Parameters)
                        {
                            string ParameterName = p.TypeReference.Type.Replace("&", "");

                            if (ParameterName.Contains("."))
                            {
                                ParameterName = ParameterName.Substring(ParameterName.LastIndexOf(".") + 1);
                            }

                            ParameterName = ParameterName.Replace("Boolean", "bool");
                            ParameterName = ParameterName.Replace("Int32", "int");
                            ParameterName = ParameterName.Replace("Int64", "long");

                            ParameterTypes += ParameterName + ";";
                        }

                        ParameterTypes = ParameterTypes.ToUpper();
                        snippet.SetCodelet("PARAMETERTYPES", ParameterTypes);
                        return snippet;
                    }
                }
            }
        }

        TLogging.Log("Warning: Missing module access permissions for " + AConnectorClassWithNamespace + "::" + m.Name);

        return new ProcessTemplate();
    }

    private ProcessTemplate CreateInstanceOfConnector(ProcessTemplate ATemplate, MethodDeclaration m, string ATypeConnector)
    {
        bool outHasBeenFound = false;
        bool firstParameter;

        foreach (ParameterDeclarationExpression p in m.Parameters)
        {
            if ((p.ParamModifier & (ParameterModifiers.Out | ParameterModifiers.Ref)) != 0)
            {
                outHasBeenFound = true;
            }
        }

        if (!outHasBeenFound)
        {
            // simple: no need to call GetData

            String createObject = "return new T" + m.Name + ATypeConnector + "(";
            firstParameter = true;

            foreach (ParameterDeclarationExpression p in m.Parameters)
            {
                if (!firstParameter)
                {
                    createObject += ", ";
                }

                firstParameter = false;
                createObject += p.ParameterName;
            }

            createObject += ");";

            ProcessTemplate snippet = new ProcessTemplate();
            snippet.FTemplateCode = createObject;
            return snippet;
        }
        else
        {
            ProcessTemplate snippet = ATemplate.GetSnippet("CALLPROCEDUREWITHGETDATA");

            // the first parameters are for the constructor
            // then the out parameter is for the dataset,
            // and all the following parameters are for GetData

            snippet.SetCodelet("CONNECTORTYPE", m.Name + ATypeConnector);

            String createObject = "T" + m.Name + ATypeConnector + " ReturnValue = new T" + m.Name +
                                  ATypeConnector + "(";
            StringCollection parameters = new StringCollection();

            foreach (ParameterDeclarationExpression p in m.Parameters)
            {
                if ((p.ParamModifier & (ParameterModifiers.Out | ParameterModifiers.Ref)) != 0)
                {
                    break;
                }

                parameters.Add(p.ParameterName);
            }

            snippet.SetCodelet("CALLPROCEDUREINTERNAL", WriteLineMethodCallToString(createObject, parameters));

            // find the out parameter, and use the following parameters as parameters for GetData
            String getData = "";
            outHasBeenFound = false;
            firstParameter = true;

            foreach (ParameterDeclarationExpression p in m.Parameters)
            {
                if (outHasBeenFound)
                {
                    if (!firstParameter)
                    {
                        getData += ", ";
                    }

                    firstParameter = false;
                    getData += p.ParameterName;
                }

                if ((p.ParamModifier & (ParameterModifiers.Out | ParameterModifiers.Ref)) != 0)
                {
                    getData += p.ParameterName + " = ReturnValue.GetData(";
                    outHasBeenFound = true;
                }
            }

            getData += ");";
            snippet.SetCodelet("GETDATA", getData);
            return snippet;
        }
    }

    private ProcessTemplate ImplementInterface(ProcessTemplate ATemplate, String AFullNamespace, String AInterfaceName)
    {
        // e.g. FullNamespace: Ict.Petra.Server.MPartner.Instantiator.Partner.UIConnectors
        // e.g. InterfaceNamespace: Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors
        string InterfaceNamespace = AFullNamespace.Replace("Server.", "Shared.Interfaces.").Replace("Instantiator.", "");

        // try to implement the methods defined in the interface
        TypeDeclaration t = CSParser.FindInterface(CSFiles, InterfaceNamespace, AInterfaceName);

        if (t == null)
        {
            return new ProcessTemplate();
        }

        ProcessTemplate interfacesSnippet = ATemplate.GetSnippet("INTERFACEMETHODS");
        interfacesSnippet.SetCodelet("METHOD", "");

        foreach (MethodDeclaration m in CSParser.GetMethods(t))
        {
            string MethodName = m.Name;

            if (MethodName.Equals("InitializeLifetimeService")
                || MethodName.Equals("GetLifetimeService")
                || MethodName.Equals("CreateObjRef")
                || MethodName.Equals("GetType")
                || MethodName.Equals("ToString")
                || MethodName.Equals("Equals")
                || MethodName.Equals("GetHashCode"))
            {
                continue;
            }

            String returnType = m.TypeReference.Type;

            if (returnType == "System.Void")
            {
                returnType = "void";
            }

            int align = (returnType + " " + MethodName).Length + 1 + ("public ").Length;
            String formattedMethod = "public " + returnType + " " + MethodName + "(";

            bool firstParameter = true;

            foreach (ParameterDeclarationExpression p in m.Parameters)
            {
                AddParameter(ref formattedMethod, ref firstParameter, align, p.ParameterName, p.ParamModifier, p.TypeReference.Type);
            }

            formattedMethod += ")";

            ProcessTemplate ProcedureSnippet = ATemplate.GetSnippet("GENERATEDMETHODFROMINTERFACE");
            ProcedureSnippet.SetCodelet("PROCEDUREHEADER", formattedMethod);

            // TODO: there is a manual exception for TReportingUIConnectorsNamespace: better do a generic way
            if (AInterfaceName.EndsWith("UIConnectorsNamespace")
                && (AInterfaceName != "IReportingUIConnectorsNamespace"))
            {
                ProcedureSnippet.InsertSnippet("CALLPROCEDURE", CreateInstanceOfConnector(ATemplate, m, "UIConnector"));
            }
            else if (AInterfaceName.EndsWith("LogicConnectorsNamespace"))
            {
                ProcedureSnippet.InsertSnippet("CALLPROCEDURE", CreateInstanceOfConnector(ATemplate, m, "LogicConnector"));
            }
            else if (AInterfaceName.EndsWith("WebConnectorsNamespace"))
            {
                // don't create instance of connector, since we use static functions
                // should never get here, already called CreateStaticCallConnector
            }

            // what about them?
            //     || AInterfaceName.EndsWith("LogicConnectorsNamespace")
            //     || AInterfaceName.EndsWith("ServerLookupsNamespace")
            //    || AInterfaceName.EndsWith("CacheableNamespace")

            // might be implemented with manual code
            if (!ProcedureSnippet.FCodelets.Contains("CALLPROCEDURE"))
            {
                ProcedureSnippet.SetCodelet("CALLPROCEDURE", "");
            }

            interfacesSnippet.InsertSnippet("METHOD", ProcedureSnippet);
        }

        return interfacesSnippet;
    }

    ProcessTemplate ImplementStaticCallConnector(ProcessTemplate ATemplate, string AFullNamespace)
    {
        // e.g. FullNamespace: Ict.Petra.Server.MFinance.Instantiator.AccountsPayable.WebConnectors
        // e.g. ConnectorNamespace: Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors
        string ConnectorNamespace = AFullNamespace.Replace("Instantiator.", "");

        List <CSParser>CSFiles = null;
        string module = AFullNamespace.Split('.')[3];

        if (Directory.Exists(CSParser.ICTPath + "/Petra/Server/lib/" + module))
        {
            // any class in the module can contain a webconnector
            CSFiles = CSParser.GetCSFilesForDirectory(CSParser.ICTPath + "/Petra/Server/lib/" + module,
                SearchOption.AllDirectories);
        }
        else
        {
            CSFiles = new List <CSParser>();
        }

        ProcessTemplate interfacesSnippet = ATemplate.GetSnippet("INTERFACEMETHODS");
        interfacesSnippet.SetCodelet("METHOD", "");

        List <TypeDeclaration>ConnectorClasses = CSParser.GetClassesInNamespace(CSFiles, ConnectorNamespace);

        foreach (TypeDeclaration connectorClass in ConnectorClasses)
        {
            foreach (MethodDeclaration m in CSParser.GetMethods(connectorClass))
            {
                string MethodName = m.Name;

                String returnType = m.TypeReference.Type;

                if (returnType == "System.Void")
                {
                    returnType = "void";
                }

                if ((m.Modifier & Modifiers.Public) == 0)
                {
                    continue;
                }

                int align = (returnType + " " + MethodName).Length + 1 + ("public ").Length;
                String formattedMethod = "public " + returnType + " " + MethodName + "(";
                bool firstParameter = true;
                string actualParameters = "";

                foreach (ParameterDeclarationExpression p in m.Parameters)
                {
                    if (!firstParameter)
                    {
                        actualParameters += ", ";
                    }

                    if ((p.ParamModifier & ParameterModifiers.Out) > 0)
                    {
                        actualParameters += "out ";
                    }

                    if ((p.ParamModifier & ParameterModifiers.Ref) > 0)
                    {
                        actualParameters += "ref ";
                    }

                    actualParameters += p.ParameterName;
                    AddParameter(ref formattedMethod, ref firstParameter, align, p.ParameterName, p.ParamModifier, p.TypeReference.Type);
                }

                formattedMethod += ")";

                ProcessTemplate ProcedureSnippet = ATemplate.GetSnippet("GENERATEDMETHODFROMCONNECTOR");
                ProcedureSnippet.SetCodelet("PROCEDUREHEADER", formattedMethod);

                // eg: return Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors.TTransactionEditWebConnector.GetDocument(ALedgerNumber, AAPNumber);
                ProcedureSnippet.InsertSnippet("CHECKUSERMODULEPERMISSIONS",
                    CreateModuleAccessPermissionCheck(
                        ATemplate,
                        ConnectorNamespace + "." + connectorClass.Name,
                        m));
                ProcedureSnippet.SetCodelet("CALLPROCEDURE", "return " + ConnectorNamespace + "." +
                    connectorClass.Name + "." +
                    MethodName + "(" + actualParameters + ");");

                interfacesSnippet.InsertSnippet("METHOD", ProcedureSnippet);
            }
        }

        return interfacesSnippet;
    }

    private ProcessTemplate WriteRemotableClass(ProcessTemplate ATemplate,
        String FullNamespace,
        String Classname,
        String Namespace,
        Boolean HighestLevel,
        List <TNamespace>children)
    {
        ProcessTemplate remotableClassSnippet = ATemplate.GetSnippet("REMOTABLECLASS");

        if (HighestLevel)
        {
            remotableClassSnippet.SetCodelet("HIGHESTLEVEL", "true");
        }

        remotableClassSnippet.SetCodelet("NAMESPACE", Namespace);

        String LocalClassname = Classname;

        if (!HighestLevel)
        {
            LocalClassname = "T" + Classname + "Namespace";
        }

        remotableClassSnippet.SetCodelet("LOCALCLASSNAME", LocalClassname);

        remotableClassSnippet.SetCodelet("SUBNAMESPACEDEFINITIONS", "");

        foreach (TNamespace sn in children)
        {
            if (HighestLevel)
            {
                remotableClassSnippet.AddToCodelet("SUBNAMESPACEDEFINITIONS",
                    "private T" + sn.Name + "Namespace F" + sn.Name + "SubNamespace;" + Environment.NewLine);
            }
            else
            {
                remotableClassSnippet.AddToCodelet("SUBNAMESPACEDEFINITIONS",
                    "private T" + Namespace + sn.Name + "Namespace F" + Namespace + sn.Name + "SubNamespace;" + Environment.NewLine);
            }
        }

        if (children.Count == 0)
        {
            if (Namespace.EndsWith("WebConnectors"))
            {
                remotableClassSnippet.InsertSnippet("SUBNAMESPACESREMOTABLECLASS", ImplementStaticCallConnector(ATemplate, FullNamespace));
            }
            else
            {
                remotableClassSnippet.InsertSnippet("SUBNAMESPACESREMOTABLECLASS",
                    ImplementInterface(ATemplate, FullNamespace, "I" + Namespace + "Namespace"));
            }
        }
        else
        {
            remotableClassSnippet.SetCodelet("SUBMODULENAMESPACES", "true");
        }

        foreach (TNamespace sn in children)
        {
            ProcessTemplate subNamespaceSnippet = ATemplate.GetSnippet("SUBNAMESPACE");

            if (HighestLevel)
            {
                subNamespaceSnippet.SetCodelet("NAMESPACENAME", sn.Name);
                subNamespaceSnippet.SetCodelet("OBJECTNAME", sn.Name);
            }
            else
            {
                subNamespaceSnippet.SetCodelet("NAMESPACENAME", Namespace + sn.Name);
                subNamespaceSnippet.SetCodelet("OBJECTNAME", sn.Name);
            }

            subNamespaceSnippet.SetCodelet("NAMESPACE", Namespace);

            remotableClassSnippet.InsertSnippet("SUBNAMESPACESREMOTABLECLASS", subNamespaceSnippet);
        }

        return remotableClassSnippet;
    }

    private ProcessTemplate WriteNamespace(ProcessTemplate ATemplate, String NamespaceName, String ClassName, TNamespace sn)
    {
        ProcessTemplate namespaceTemplate = ATemplate.GetSnippet("NAMESPACE");

        namespaceTemplate.SetCodelet("NAMESPACENAME", NamespaceName);

        namespaceTemplate.InsertSnippet("REMOTABLECLASS", WriteRemotableClass(ATemplate, NamespaceName,
                ClassName,
                ClassName,
                false,
                sn.Children));

        namespaceTemplate.SetCodelet("SUBNAMESPACES1", "");

        foreach (TNamespace sn2 in sn.Children)
        {
            namespaceTemplate.InsertSnippet("SUBNAMESPACES1", WriteNamespace(ATemplate, NamespaceName + "." + sn2.Name, ClassName + sn2.Name, sn2));
        }

        return namespaceTemplate;
    }

    private void CreateAutoHierarchy(TNamespace tn, String AOutputPath, String AXmlFileName, String ATemplateDir)
    {
        String OutputFile = AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                            Path.DirectorySeparatorChar + "Instantiator.AutoHierarchy.cs";

        if (Directory.Exists(AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                Path.DirectorySeparatorChar + "connect"))
        {
            OutputFile = AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                         Path.DirectorySeparatorChar + "connect" +
                         Path.DirectorySeparatorChar + "Instantiator.AutoHierarchy.cs";
        }

        Console.WriteLine("working on " + OutputFile);

        ProcessTemplate Template = new ProcessTemplate(ATemplateDir + Path.DirectorySeparatorChar +
            "ClientServerGlue" + Path.DirectorySeparatorChar +
            "Instantiator.cs");

        OpenFile(OutputFile);
        WriteLine("// Auto generated with nant generateGlue");
        WriteLine("// based on " + Path.GetFullPath(AXmlFileName).Substring(Path.GetFullPath(AXmlFileName).IndexOf("csharp")));
        WriteLine("//");

        // load default header with license and copyright
        WriteLine(ProcessTemplate.LoadEmptyFileComment(ATemplateDir));

        ProcessTemplate headerSnippet = Template.GetSnippet("HEADER");

        WriteLine(headerSnippet.FinishWriting(true));

        WriteLine("using Ict.Petra.Shared.Interfaces.M" + tn.Name + ';');

        foreach (TNamespace sn in tn.Children)
        {
            WriteLine("using Ict.Petra.Shared.Interfaces.M" + tn.Name + "." + sn.Name + ';');
        }

        foreach (TNamespace sn in tn.Children)
        {
            CommonNamespace.WriteUsingNamespace(this, "Ict.Petra.Shared.Interfaces.M" + tn.Name + "." + sn.Name, sn.Name, sn, sn.Children);
        }

        foreach (TNamespace sn in tn.Children)
        {
            WriteLine("using Ict.Petra.Server.M" + tn.Name + ".Instantiator." + sn.Name + ';');
        }

        foreach (TNamespace sn in tn.Children)
        {
            CommonNamespace.WriteUsingNamespace(this, "Ict.Petra.Server.M" + tn.Name + ".Instantiator." + sn.Name, sn.Name, sn, sn.Children);
        }

        foreach (TNamespace sn in tn.Children)
        {
            WriteLine("using Ict.Petra.Server.M" + tn.Name + "." + sn.Name + ';');
        }

        foreach (TNamespace sn in tn.Children)
        {
            CommonNamespace.WriteUsingNamespace(this, "Ict.Petra.Server.M" + tn.Name + "." + sn.Name, sn.Name, sn, sn.Children);
        }

        WriteLine();

        ProcessTemplate topLevelNamespaceSnippet = Template.GetSnippet("TOPLEVELNAMESPACE");
        topLevelNamespaceSnippet.SetCodelet("TOPLEVELMODULE", tn.Name);
        topLevelNamespaceSnippet.InsertSnippet("LOADERCLASS", WriteLoaderClass(Template, tn.Name));
        topLevelNamespaceSnippet.InsertSnippet("REMOTABLECLASS",
            WriteRemotableClass(
                Template,
                "Ict.Petra.Server.M" + tn.Name + ".Instantiator",
                "TM" + tn.Name,
                "M" + tn.Name,
                true,
                tn.Children));

        topLevelNamespaceSnippet.SetCodelet("SUBNAMESPACES", "");

        foreach (TNamespace sn in tn.Children)
        {
            topLevelNamespaceSnippet.InsertSnippet("SUBNAMESPACES",
                WriteNamespace(Template, "Ict.Petra.Server.M" + tn.Name + ".Instantiator." + sn.Name, sn.Name, sn));
        }

        // need to use WriteLine to keep all the manual code!
        WriteLine(topLevelNamespaceSnippet.FinishWriting(true));

        Close();
    }

    private List <CSParser>CSFiles = null;
    public void CreateFiles(List <TNamespace>ANamespaces, String AOutputPath, String AXmlFileName, String ATemplateDir)
    {
        // get the appropriate cs file
        CSFiles = CSParser.GetCSFilesForDirectory(CSParser.ICTPath + "/Petra/Shared/lib/Interfaces",
            SearchOption.TopDirectoryOnly);

        foreach (TNamespace tn in ANamespaces)
        {
            // for testing:
//        if (tn.Name != "Reporting") continue;
            CreateAutoHierarchy(tn, AOutputPath, AXmlFileName, ATemplateDir);
        }
    }
}
}