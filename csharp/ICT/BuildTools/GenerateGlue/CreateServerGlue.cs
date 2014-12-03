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
/// generate the code for the server
/// </summary>
public class GenerateServerGlue
{
    static private string FTemplateDir = string.Empty;
    static SortedList <string, string>FUsingNamespaces = null;
    static bool FContainsAsynchronousExecutionProgress = false;

    static private ProcessTemplate CreateModuleAccessPermissionCheck(ProcessTemplate ATemplate,
        string AConnectorClassWithNamespace,
        MethodDeclaration m)
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

//                            if (ParameterType == "SortedList")
//                            {
//Console.WriteLine(p.ParameterName + "'s ParameterType = SortedList");
//                                ParameterType = ParameterType.Replace("List", "List[" +
//                                    p.TypeReference.GenericTypes[0].ToString() + "," +
//                                    p.TypeReference.GenericTypes[1].ToString() + "]");
//                                ParameterType = ParameterType.Replace("System.", String.Empty);
//                            }

                            ParameterType = ParameterType.Replace("Boolean", "bool");
                            ParameterType = ParameterType.Replace("Int32", "int");
                            ParameterType = ParameterType.Replace("Int64", "long");

                            ParameterTypes += ParameterType + ";";
                        }

                        ParameterTypes = ParameterTypes.ToUpper();
                        snippet.SetCodelet("PARAMETERTYPES", ParameterTypes);
                        return snippet;
                    }
                    else if (attr.Name == "CheckServerAdminToken")
                    {
                        ProcessTemplate snippet = ATemplate.GetSnippet("CHECKSERVERADMINPERMISSION");
                        string paramdefinition = "string AServerAdminSecurityToken";

                        if (ATemplate.FCodelets["PARAMETERDEFINITION"].Length != 0)
                        {
                            paramdefinition += ", ";
                        }

                        ATemplate.AddToCodeletPrepend("PARAMETERDEFINITION", paramdefinition);
                        return snippet;
                    }
                }
            }
        }

        TLogging.Log("Warning !!! Missing module access permissions for " + AConnectorClassWithNamespace + "::" + m.Name);

        return new ProcessTemplate();
    }

    private static void PrepareParametersForMethod(
        ProcessTemplate snippet,
        TypeReference AReturnType,
        List <ParameterDeclarationExpression>AParameters,
        string AMethodName,
        ref List <string>AMethodNames)
    {
        string ParameterDefinition = string.Empty;
        string ActualParameters = string.Empty;

        snippet.SetCodelet("LOCALVARIABLES", string.Empty);
        string returnCodeFatClient = string.Empty;
        string returnCodeJSClient = string.Empty;
        int returnCounter = 0;

        foreach (ParameterDeclarationExpression p in AParameters)
        {
            string parametertype = p.TypeReference.ToString();
            bool ArrayParameter = false;

            // check if the parametertype is not a generic type, eg. dictionary or list
            if (!parametertype.Contains("<"))
            {
                if (parametertype.EndsWith("[]"))
                {
                    ArrayParameter = true;
//Console.WriteLine("ArrayParameter found: " + parametertype);
                }

                parametertype = parametertype == "string" || parametertype == "String" ? "System.String" : parametertype;
                parametertype = parametertype == "bool" || parametertype == "Boolean" ? "System.Boolean" : parametertype;
                if (parametertype.Contains("UINT") || parametertype.Contains("unsigned"))
                {
                    parametertype = parametertype.Contains("UInt32") || parametertype == "unsigned int" ? "System.UInt32" : parametertype;
                    parametertype = parametertype.Contains("UInt16") || parametertype == "unsigned short" ? "System.UInt16" : parametertype;
                    parametertype = parametertype.Contains("UInt64") || parametertype == "unsigned long" ? "System.UInt64" : parametertype;
                }
                else
                {
                    parametertype = parametertype.Contains("Int32") || parametertype == "int" ? "System.Int32" : parametertype;
                    parametertype = parametertype.Contains("Int16") || parametertype == "short" ? "System.Int16" : parametertype;
                    parametertype = parametertype.Contains("Int64") || parametertype == "long" ? "System.Int64" : parametertype;
                }
                parametertype = parametertype.Contains("Decimal") || parametertype == "decimal" ? "System.Decimal" : parametertype;

                if (ArrayParameter
                    && !(parametertype.EndsWith("[]")))
                {
                    // need to restore Array type!
                    parametertype += "[]";

//Console.WriteLine("ArrayParameter found - new parametertype = " + parametertype);
                }
            }

            bool EnumParameter = parametertype.EndsWith("Enum");
            bool BinaryParameter =
                !((parametertype.StartsWith("System.Int64")) || (parametertype.StartsWith("System.Int32"))
                  || (parametertype.StartsWith("System.Int16"))
                  || (parametertype.StartsWith("System.String")) || (parametertype.StartsWith("System.Boolean"))
                  || EnumParameter);

            if (ActualParameters.Length > 0)
            {
                ActualParameters += ", ";
            }

            // ignore out parameters in the web service method definition
            if ((ParameterModifiers.Out & p.ParamModifier) == 0)
            {
                if (ParameterDefinition.Length > 0)
                {
                    ParameterDefinition += ", ";
                }

                if ((!BinaryParameter) && (!ArrayParameter) && (!EnumParameter))
                {
                    ParameterDefinition += parametertype + " " + p.ParameterName;
                }
                else
                {
                    ParameterDefinition += "string " + p.ParameterName;
                }
            }

            // for string parameters, check if they have been encoded binary due to special characters in the string;
            // this obviously does not apply to out parameters
            if ((parametertype == "System.String") && ((ParameterModifiers.Out & p.ParamModifier) == 0))
            {
                snippet.AddToCodelet(
                    "LOCALVARIABLES",
                    p.ParameterName + " = (string) THttpBinarySerializer.DeserializeObject(" + p.ParameterName + ",\"System.String\");" +
                    Environment.NewLine);
            }

            // EnumParameters are also binary encoded
            // this obviously does not apply to out parameters
            if (EnumParameter && ((ParameterModifiers.Out & p.ParamModifier) == 0))
            {
                snippet.AddToCodelet(
                    "LOCALVARIABLES",
                    p.ParameterName + " = THttpBinarySerializer.DeserializeObject(" + p.ParameterName + ",\"System.String\").ToString();" +
                    Environment.NewLine);
            }

            if ((ParameterModifiers.Out & p.ParamModifier) != 0)
            {
                snippet.AddToCodelet("LOCALVARIABLES", parametertype + " " + p.ParameterName + ";" + Environment.NewLine);
                ActualParameters += "out " + p.ParameterName;
            }
            else if ((ParameterModifiers.Ref & p.ParamModifier) != 0)
            {
                if (BinaryParameter)
                {
                    snippet.AddToCodelet("LOCALVARIABLES", parametertype + " Local" + p.ParameterName + " = " +
                        " (" + parametertype + ")THttpBinarySerializer.DeserializeObject(" + p.ParameterName + ",\"binary\");" +
                        Environment.NewLine);
                    ActualParameters += "ref Local" + p.ParameterName;
                }
                else if (EnumParameter)
                {
                    snippet.AddToCodelet("LOCALVARIABLES", parametertype + " Local" + p.ParameterName + " = " +
                        " (" + parametertype + ") Enum.Parse(typeof(" + parametertype + "), " + p.ParameterName + ");" +
                        Environment.NewLine);
                    ActualParameters += "ref Local" + p.ParameterName;
                }
                else
                {
                    ActualParameters += "ref " + p.ParameterName;
                }
            }
            else
            {
                if (BinaryParameter
                    || ArrayParameter)
                {
                    ActualParameters += "(" + parametertype + ")THttpBinarySerializer.DeserializeObject(" + p.ParameterName + ",\"binary\")";
                }
                else if (EnumParameter)
                {
                    ActualParameters += " (" + parametertype + ") Enum.Parse(typeof(" + parametertype + "), " + p.ParameterName + ")";
                }
                else
                {
                    ActualParameters += p.ParameterName;
                }
            }

            if (((ParameterModifiers.Ref & p.ParamModifier) > 0) || ((ParameterModifiers.Out & p.ParamModifier) > 0))
            {
                returnCodeFatClient +=
                     (returnCodeFatClient.Length > 0 ? "+\",\"+" : string.Empty) +
                     "THttpBinarySerializer.SerializeObjectWithType(" +
                     (((ParameterModifiers.Ref & p.ParamModifier) > 0 && BinaryParameter) ? "Local" : string.Empty) +
                     p.ParameterName + ")";

                if (returnCounter == 1)
                {
                    returnCodeJSClient = "\"{ \\\"0\\\": \" + " + returnCodeJSClient;
                }

                if (returnCounter > 0)
                {
                    returnCodeJSClient += " + \", \\\"" + returnCounter.ToString() + "\\\": \" + ";
                }

                returnCodeJSClient +=
                    "THttpBinarySerializer.SerializeObjectWithType(" +
                    (((ParameterModifiers.Ref & p.ParamModifier) > 0 && BinaryParameter) ? "Local" : string.Empty) +
                    p.ParameterName + ")";

                returnCounter++;
            }
        }

        if (AReturnType != null)
        {
            string returntype = AutoGenerationTools.TypeToString(AReturnType, "");

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

            if (returnCounter > 0)
            {
                if (returntype != "void")
                {
                    returnCodeFatClient += (returnCodeFatClient.Length > 0 ? "+\",\"+" : string.Empty) + "THttpBinarySerializer.SerializeObjectWithType(Result)";

                    if (returnCounter == 1)
                    {
                        returnCodeJSClient = "\"{ \\\"0\\\": \" + " + returnCodeJSClient;
                    }

                    returnCodeJSClient += "+\", \\\"" + returnCounter.ToString() + "\\\": \"+" + "THttpBinarySerializer.SerializeObjectWithType(Result)";

                    returnCounter++;
                }

                returntype = "string";
            }
            else if (returntype == "System.String")
            {
                returntype = "string";
                returnCodeFatClient = "THttpBinarySerializer.SerializeObjectWithType(Result)";
                returnCodeJSClient = "Result";
            }
            else if (!((returntype == "System.Int64") || (returntype == "System.Int32") || (returntype == "System.Int16")
                       || (returntype == "System.UInt64") || (returntype == "System.UInt32") || (returntype == "System.UInt16")
                       || (returntype == "System.Decimal")
                       || (returntype == "System.Boolean")) && (returntype != "void"))
            {
                returntype = "string";
                returnCodeFatClient = returnCodeJSClient = "THttpBinarySerializer.SerializeObject(Result)";
            }

            string localreturn = AutoGenerationTools.TypeToString(AReturnType, "");

            if (localreturn == "void")
            {
                localreturn = string.Empty;
            }
            else if (returnCodeFatClient.Length > 0 || returnCodeJSClient.Length > 0)
            {
                localreturn += " Result = ";
            }
            else
            {
                localreturn = "return ";
            }

            if (returnCounter > 1)
            {
                returnCodeJSClient += "+ \"}\"";
            }

            snippet.SetCodelet("RETURN", string.Empty);

            if (returnCodeFatClient.Length > 0 || returnCodeJSClient.Length > 0)
            {
                snippet.SetCodelet("RETURN", returntype != "void" ? "return isJSClient()?" + returnCodeJSClient + ":" + returnCodeFatClient + ";" : string.Empty);
            }

            snippet.SetCodelet("RETURNTYPE", returntype);
            snippet.SetCodelet("LOCALRETURN", localreturn);
        }

//Console.WriteLine("Final ParameterDefinition = " + ParameterDefinition);
        snippet.SetCodelet("PARAMETERDEFINITION", ParameterDefinition);
        snippet.SetCodelet("ACTUALPARAMETERS", ActualParameters);

        // avoid duplicate names for webservice methods
        string methodname = AMethodName;
        int methodcounter = 1;

        while (AMethodNames.Contains(methodname))
        {
            methodcounter++;
            methodname = AMethodName + methodcounter.ToString();
        }

        AMethodNames.Add(methodname);

        snippet.SetCodelet("UNIQUEMETHODNAME", methodname);
    }

    /// <summary>
    /// insert a method call
    /// </summary>
    private static void InsertWebConnectorMethodCall(ProcessTemplate snippet,
        TypeDeclaration connectorClass,
        MethodDeclaration m,
        ref List <string>AMethodNames)
    {
        PrepareParametersForMethod(snippet, m.TypeReference, m.Parameters, m.Name, ref AMethodNames);

        snippet.SetCodelet("METHODNAME", m.Name);
        snippet.SetCodelet("WEBCONNECTORCLASS", connectorClass.Name);
        snippet.InsertSnippet("CHECKUSERMODULEPERMISSIONS",
            CreateModuleAccessPermissionCheck(
                snippet,
                connectorClass.Name,
                m));
    }

    static private void WriteWebConnector(string connectorname, TypeDeclaration connectorClass, ProcessTemplate Template)
    {
        List <string>MethodNames = new List <string>();

        foreach (MethodDeclaration m in CSParser.GetMethods(connectorClass))
        {
            if (TCollectConnectorInterfaces.IgnoreMethod(m.Attributes, m.Modifier))
            {
                continue;
            }

            string connectorNamespaceName = ((NamespaceDeclaration)connectorClass.Parent).Name;

            if (!FUsingNamespaces.ContainsKey(connectorNamespaceName))
            {
                FUsingNamespaces.Add(connectorNamespaceName, connectorNamespaceName);
            }

            ProcessTemplate snippet = Template.GetSnippet("WEBCONNECTOR");

            InsertWebConnectorMethodCall(snippet, connectorClass, m, ref MethodNames);

            Template.InsertSnippet("WEBCONNECTORS", snippet);
        }
    }

    static private void WriteUIConnector(string connectorname, TypeDeclaration connectorClass, ProcessTemplate Template)
    {
        List <string>MethodNames = new List <string>();

        foreach (ConstructorDeclaration m in CSParser.GetConstructors(connectorClass))
        {
            if (TCollectConnectorInterfaces.IgnoreMethod(m.Attributes, m.Modifier))
            {
                continue;
            }

            string connectorNamespaceName = ((NamespaceDeclaration)connectorClass.Parent).Name;

            if (!FUsingNamespaces.ContainsKey(connectorNamespaceName))
            {
                FUsingNamespaces.Add(connectorNamespaceName, connectorNamespaceName);
            }

            ProcessTemplate snippet = Template.GetSnippet("UICONNECTORCONSTRUCTOR");

            snippet.SetCodelet("UICONNECTORCLASS", connectorClass.Name);
            snippet.SetCodelet("CHECKUSERMODULEPERMISSIONS", "// TODO CHECKUSERMODULEPERMISSIONS");

            PrepareParametersForMethod(snippet, null, m.Parameters, m.Name, ref MethodNames);

            Template.InsertSnippet("UICONNECTORS", snippet);
        }

        // foreach public method create a method
        foreach (MethodDeclaration m in CSParser.GetMethods(connectorClass))
        {
            if (TCollectConnectorInterfaces.IgnoreMethod(m.Attributes, m.Modifier))
            {
                continue;
            }

            ProcessTemplate snippet = Template.GetSnippet("UICONNECTORMETHOD");

            snippet.SetCodelet("METHODNAME", m.Name);
            snippet.SetCodelet("UICONNECTORCLASS", connectorClass.Name);
            snippet.SetCodelet("CHECKUSERMODULEPERMISSIONS", "// TODO CHECKUSERMODULEPERMISSIONS");

            PrepareParametersForMethod(snippet, m.TypeReference, m.Parameters, m.Name, ref MethodNames);

            if (snippet.FCodelets["PARAMETERDEFINITION"].Length > 0)
            {
                snippet.AddToCodeletPrepend("PARAMETERDEFINITION", ", ");
            }

            Template.InsertSnippet("UICONNECTORS", snippet);
        }

        // foreach public property create a method
        foreach (PropertyDeclaration p in CSParser.GetProperties(connectorClass))
        {
            if (TCollectConnectorInterfaces.IgnoreMethod(p.Attributes, p.Modifier))
            {
                continue;
            }

            string propertytype = p.TypeReference.ToString();

            // check if the parametertype is not a generic type, eg. dictionary or list
            if (!propertytype.Contains("<"))
            {
                propertytype = propertytype == "string" || propertytype == "String" ? "System.String" : propertytype;
                propertytype = propertytype == "bool" || propertytype == "Boolean" ? "System.Boolean" : propertytype;
                if (propertytype.Contains("UINT") || propertytype.Contains("unsigned"))
                {
                    propertytype = propertytype.Contains("UInt32") || propertytype == "unsigned int" ? "System.UInt32" : propertytype;
                    propertytype = propertytype.Contains("UInt16") || propertytype == "unsigned short" ? "System.UInt16" : propertytype;
                    propertytype = propertytype.Contains("UInt64") || propertytype == "unsigned long" ? "System.UInt64" : propertytype;
                }
                else
                {
                    propertytype = propertytype.Contains("Int32") || propertytype == "int" ? "System.Int32" : propertytype;
                    propertytype = propertytype.Contains("Int16") || propertytype == "short" ? "System.Int16" : propertytype;
                    propertytype = propertytype.Contains("Int64") || propertytype == "long" ? "System.Int64" : propertytype;
                }
                propertytype = propertytype.Contains("Decimal") || propertytype == "decimal" ? "System.Decimal" : propertytype;
            }

            bool BinaryReturn = !((propertytype == "System.Int64") || (propertytype == "System.Int32") || (propertytype == "System.Int16")
                                  || (propertytype == "System.String") || (propertytype == "System.Boolean"));

            string EncodedType = propertytype;
            string EncodeReturnType = string.Empty;
            string ActualValue = "AValue";

            if (BinaryReturn)
            {
                EncodedType = "System.String";
                EncodeReturnType = "THttpBinarySerializer.SerializeObject";
                ActualValue = "(" + propertytype + ")THttpBinarySerializer.DeserializeObject(AValue)";
            }

            ProcessTemplate propertySnippet = Template.GetSnippet("UICONNECTORPROPERTY");
            propertySnippet.SetCodelet("UICONNECTORCLASS", connectorClass.Name);
            propertySnippet.SetCodelet("ENCODEDTYPE", EncodedType);
            propertySnippet.SetCodelet("PROPERTYNAME", p.Name);

            if (p.HasGetRegion)
            {
                if (p.TypeReference.ToString().StartsWith("I"))
                {
                    if (p.TypeReference.ToString() == "IAsynchronousExecutionProgress")
                    {
                        FContainsAsynchronousExecutionProgress = true;
                    }

                    // return the ObjectID of the Sub-UIConnector
                    propertySnippet.InsertSnippet("GETTER", Template.GetSnippet("GETSUBUICONNECTOR"));
                    propertySnippet.SetCodelet("ENCODEDTYPE", "System.String");
                }
                else
                {
                    propertySnippet.SetCodelet("GETTER",
                        "return {#ENCODERETURNTYPE}((({#UICONNECTORCLASS})FUIConnectors[ObjectID]).{#PROPERTYNAME});");
                    propertySnippet.SetCodelet("ENCODERETURNTYPE", EncodeReturnType);
                }
            }

            if (p.HasSetRegion)
            {
                propertySnippet.SetCodelet("SETTER", "true");
                propertySnippet.SetCodelet("ACTUALPARAMETERS", ActualValue);
            }

            Template.InsertSnippet("UICONNECTORS", propertySnippet);
        }
    }

    static private void CreateServerGlue(TNamespace tn, SortedList <string, TypeDeclaration>connectors, string AOutputPath)
    {
        String OutputFile = AOutputPath + Path.DirectorySeparatorChar + "ServerGlue.M" + tn.Name +
                            "-generated.cs";

        Console.WriteLine("working on " + OutputFile);

        ProcessTemplate Template = new ProcessTemplate(FTemplateDir + Path.DirectorySeparatorChar +
            "ClientServerGlue" + Path.DirectorySeparatorChar +
            "ServerGlue.cs");

        // load default header with license and copyright
        Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(FTemplateDir));

        Template.SetCodelet("TOPLEVELMODULE", tn.Name);
        Template.SetCodelet("WEBCONNECTORS", string.Empty);
        Template.SetCodelet("UICONNECTORS", string.Empty);

        string InterfacePath = Path.GetFullPath(AOutputPath).Replace(Path.DirectorySeparatorChar, '/');
        InterfacePath = InterfacePath.Substring(0, InterfacePath.IndexOf("csharp/ICT/Petra")) + "csharp/ICT/Petra/Shared/lib/Interfaces";
        Template.AddToCodelet("USINGNAMESPACES", CreateInterfaces.AddNamespacesFromYmlFile(InterfacePath, tn.Name));

        FUsingNamespaces = new SortedList <string, string>();
        FContainsAsynchronousExecutionProgress = false;

        foreach (string connector in connectors.Keys)
        {
            if (connector.Contains(":"))
            {
                WriteUIConnector(connector, connectors[connector], Template);
            }
            else
            {
                WriteWebConnector(connector, connectors[connector], Template);
            }
        }

        if (FContainsAsynchronousExecutionProgress)
        {
            Template.InsertSnippet("UICONNECTORS", Template.GetSnippet("ASYNCEXECPROCESSCONNECTOR"));

            if (!FUsingNamespaces.ContainsKey("Ict.Petra.Server.MCommon"))
            {
                FUsingNamespaces.Add("Ict.Petra.Server.MCommon", "Ict.Petra.Server.MCommon");
            }
        }

        foreach (string usingNamespace in FUsingNamespaces.Keys)
        {
            Template.AddToCodelet("USINGNAMESPACES", "using " + usingNamespace + ";" + Environment.NewLine);
        }

        Template.FinishWriting(OutputFile, ".cs", true);
    }

    /// <summary>
    /// write the server code
    /// </summary>
    static public void GenerateCode(TNamespace ANamespaces, String AOutputPath, String ATemplateDir)
    {
        if (TAppSettingsManager.GetValue("compileForStandalone", "false", false) == "yes")
        {
            // this code is not needed for standalone
            return;
        }

        FTemplateDir = ATemplateDir;

        foreach (TNamespace tn in ANamespaces.Children.Values)
        {
            string module = TAppSettingsManager.GetValue("module", "all");

            if ((module == "all") || (tn.Name == module))
            {
                SortedList <string, TypeDeclaration>connectors = TCollectConnectorInterfaces.GetConnectors(tn.Name);
                CreateServerGlue(tn, connectors, AOutputPath);
            }
        }
    }
}
}
