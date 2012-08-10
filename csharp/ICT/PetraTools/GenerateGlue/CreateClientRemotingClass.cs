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
class TCreateClientRemotingClass : AutoGenerationWriter
{
    static ProcessTemplate ClientRemotingClassTemplate = null;

    /// <summary>
    /// generate the objects that can be serialized to the client
    /// </summary>
    public static ProcessTemplate AddClientRemotingClass(
        string ATemplateDir,
        string AClientObjectClass,
        string AInterfaceName,
        List <TypeDeclaration>ATypeImplemented)
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

        foreach (TypeDeclaration t in ATypeImplemented)
        {
            if (t.Name.EndsWith("UIConnector"))
            {
                InsertUIConnectors(snippet, t);
            }

            if (t.Name.EndsWith("WebConnector"))
            {
                InsertWebConnectors(snippet, t);
            }
        }

        return snippet;
    }

    private static void InsertUIConnectors(ProcessTemplate template, TypeDeclaration t)
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

            AutoGenerationWriter.FormatParameters(m.Parameters, out ActualParameters, out ParameterDefinition);

            methodSnippet.SetCodelet("PARAMETERDEFINITION", ParameterDefinition);
            methodSnippet.SetCodelet("ACTUALPARAMETERS", ActualParameters);
            methodSnippet.SetCodelet("RETURN", "return ");

            template.InsertSnippet("METHODSANDPROPERTIES", methodSnippet);
        }
    }

    private static void InsertWebConnectors(ProcessTemplate template, TypeDeclaration t)
    {
        // foreach public method create a method
        foreach (MethodDeclaration m in CSParser.GetMethods(t))
        {
            string MethodName = m.Name;

            bool AttributeNoRemoting = false;

            foreach (AttributeSection attrSection in m.Attributes)
            {
                foreach (ICSharpCode.NRefactory.Ast.Attribute attr in attrSection.Attributes)
                {
                    if (attr.Name == "NoRemoting")
                    {
                        AttributeNoRemoting = true;
                    }
                }
            }

            if (((m.Modifier & Modifiers.Public) == 0)
                || AttributeNoRemoting)
            {
                continue;
            }

            ProcessTemplate methodSnippet = ClientRemotingClassTemplate.GetSnippet("METHOD");

            methodSnippet.SetCodelet("METHODNAME", m.Name);
            methodSnippet.SetCodelet("RETURNTYPE", AutoGenerationWriter.TypeToString(m.TypeReference, string.Empty));

            string ParameterDefinition = string.Empty;
            string ActualParameters = string.Empty;

            AutoGenerationWriter.FormatParameters(m.Parameters, out ActualParameters, out ParameterDefinition);

            methodSnippet.SetCodelet("PARAMETERDEFINITION", ParameterDefinition);
            methodSnippet.SetCodelet("ACTUALPARAMETERS", ActualParameters);
            methodSnippet.SetCodelet("RETURN", "return ");

            template.InsertSnippet("METHODSANDPROPERTIES", methodSnippet);
        }
    }
}
}