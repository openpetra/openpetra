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
/// this class generates the objects that can be serialized to the client
/// </summary>
class TCreateClientRemotingClass
{
    static ProcessTemplate ClientRemotingClassTemplate = null;

    /// <summary>
    /// generate the objects that can be serialized to the client
    /// </summary>
    public static ProcessTemplate AddClientRemotingClass(
        string ATemplateDir,
        string AClientObjectClass,
        string AInterfaceName,
        List <TypeDeclaration>ATypeImplemented,
        string AFullNamespace = "",
        SortedList <string, TNamespace>AChildrenNamespaces = null)
    {
        if (ClientRemotingClassTemplate == null)
        {
            ClientRemotingClassTemplate = new ProcessTemplate(ATemplateDir + Path.DirectorySeparatorChar +
                "ClientServerGlue" + Path.DirectorySeparatorChar +
                "ClientRemotingClass.cs");
        }

        ProcessTemplate snippet = ClientRemotingClassTemplate.GetSnippet("CLASS");

        snippet.SetCodelet("CLASSNAME", AClientObjectClass);
        snippet.SetCodelet("INTERFACE", AInterfaceName);

        // try to implement the properties and methods defined in the interface
        snippet.SetCodelet("METHODSANDPROPERTIES", string.Empty);

        // problem mit Partner.Extracts.UIConnectors; MCommon.UIConnectors tut

        if (AChildrenNamespaces != null)
        {
            // accessors for subnamespaces
            InsertSubnamespaces(snippet, AFullNamespace, AChildrenNamespaces);
        }
        else
        {
            foreach (TypeDeclaration t in ATypeImplemented)
            {
                if (t.UserData.ToString().EndsWith("UIConnectors"))
                {
                    if (AInterfaceName.EndsWith("Namespace"))
                    {
                        InsertConstructors(snippet, t);
                    }
                    else
                    {
                        // never gets here???
                        InsertMethodsAndProperties(snippet, t);
                    }
                }

                if (t.UserData.ToString().EndsWith("WebConnectors"))
                {
                    InsertMethodsAndProperties(snippet, t);
                }
            }
        }

        return snippet;
    }

    private static void InsertConstructors(ProcessTemplate template, TypeDeclaration t)
    {
        // foreach constructor create a method
        List <ConstructorDeclaration>constructors = CSParser.GetConstructors(t);

        if (constructors.Count == 0)
        {
            // will cause compile error if the constructor is missing, because it is not implementing the interface completely
            throw new Exception("missing a connector constructor in " + t.Name + "; details: " + t.ToString());
        }

        // find constructor and copy the parameters
        foreach (ConstructorDeclaration m in constructors)
        {
            ProcessTemplate methodSnippet = ClientRemotingClassTemplate.GetSnippet("METHOD");

            methodSnippet.SetCodelet("METHODNAME", t.Name.Substring(1, t.Name.Length - 1 - "UIConnector".Length));
            methodSnippet.SetCodelet("RETURNTYPE", CSParser.GetImplementedInterface(t));

            string ParameterDefinition = string.Empty;
            string ActualParameters = string.Empty;

            AutoGenerationTools.FormatParameters(m.Parameters, out ActualParameters, out ParameterDefinition);

            methodSnippet.SetCodelet("PARAMETERDEFINITION", ParameterDefinition);
            methodSnippet.SetCodelet("ACTUALPARAMETERS", ActualParameters);
            methodSnippet.SetCodelet("RETURN", "return ");

            template.InsertSnippet("METHODSANDPROPERTIES", methodSnippet);
        }
    }

    private static void InsertMethodsAndProperties(ProcessTemplate template, TypeDeclaration t)
    {
        // foreach public method create a method
        foreach (MethodDeclaration m in CSParser.GetMethods(t))
        {
            if (TCollectConnectorInterfaces.IgnoreMethod(m.Attributes, m.Modifier))
            {
                continue;
            }

            ProcessTemplate methodSnippet = ClientRemotingClassTemplate.GetSnippet("METHOD");

            string returntype = AutoGenerationTools.TypeToString(m.TypeReference, string.Empty);

            methodSnippet.SetCodelet("METHODNAME", m.Name);
            methodSnippet.SetCodelet("RETURNTYPE", returntype);

            string ParameterDefinition = string.Empty;
            string ActualParameters = string.Empty;

            AutoGenerationTools.FormatParameters(m.Parameters, out ActualParameters, out ParameterDefinition);

            methodSnippet.SetCodelet("PARAMETERDEFINITION", ParameterDefinition);
            methodSnippet.SetCodelet("ACTUALPARAMETERS", ActualParameters);

            if (returntype != "void")
            {
                methodSnippet.SetCodelet("RETURN", "return ");
            }
            else
            {
                methodSnippet.SetCodelet("RETURN", string.Empty);
            }

            template.InsertSnippet("METHODSANDPROPERTIES", methodSnippet);
        }

        // foreach public method create a method
        foreach (PropertyDeclaration p in CSParser.GetProperties(t))
        {
            if (TCollectConnectorInterfaces.IgnoreMethod(p.Attributes, p.Modifier))
            {
                continue;
            }

            ProcessTemplate propertySnippet = ClientRemotingClassTemplate.GetSnippet("PROPERTY");

            propertySnippet.SetCodelet("NAME", p.Name);
            propertySnippet.SetCodelet("TYPE", AutoGenerationTools.TypeToString(p.TypeReference, string.Empty));

            if (p.HasGetRegion)
            {
                propertySnippet.SetCodelet("GETTER", "yes");
            }

            if (p.HasSetRegion)
            {
                propertySnippet.SetCodelet("SETTER", "yes");
            }

            template.InsertSnippet("METHODSANDPROPERTIES", propertySnippet);
        }
    }

    private static void InsertSubnamespaces(ProcessTemplate template,
        string AFullNamespace,
        SortedList <string, TNamespace>ASubNamespaces)
    {
        foreach (TNamespace t in ASubNamespaces.Values)
        {
            ProcessTemplate propertySnippet = ClientRemotingClassTemplate.GetSnippet("PROPERTY");

            propertySnippet.SetCodelet("NAME", t.Name);

            string NamespaceInModule = AFullNamespace.Substring(
                AFullNamespace.IndexOf('.', "Ict.Petra.Shared.M".Length) + 1).Replace(".", string.Empty);

            propertySnippet.SetCodelet("TYPE", "I" + NamespaceInModule + t.Name + "Namespace");

            propertySnippet.SetCodelet("GETTER", "yes");

            template.InsertSnippet("METHODSANDPROPERTIES", propertySnippet);
        }
    }
}
}