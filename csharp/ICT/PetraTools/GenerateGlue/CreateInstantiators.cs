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
/// this class generates the instantiators for remoting
/// it uses the file ICT/Petra/Definitions/NamespaceHierarchy.xml
/// </summary>
class CreateInstantiators : AutoGenerationWriter
{
    private string FTemplateDir = string.Empty;

    private ProcessTemplate WriteLoaderClass(ProcessTemplate ATemplate, String module)
    {
        ProcessTemplate loaderClassSnippet = ATemplate.GetSnippet("LOADERCLASS");

        loaderClassSnippet.SetCodelet("MODULE", module);
        return loaderClassSnippet;
    }

    private ProcessTemplate WriteRemotableClass(ProcessTemplate ATemplate,
        String FullNamespace,
        String Classname,
        String Namespace,
        Boolean HighestLevel,
        List <TNamespace>children,
        SortedList <string, TypeDeclaration>connectors)
    {
        if (children.Count == 0)
        {
            return new ProcessTemplate();
        }

        ProcessTemplate remotableClassSnippet = ATemplate.GetSnippet("REMOTABLECLASS");

        remotableClassSnippet.SetCodelet("NAMESPACE", Namespace);

        String LocalClassname = Classname;

        if (!HighestLevel)
        {
            LocalClassname = "T" + Classname + "Namespace";
        }

        remotableClassSnippet.SetCodelet("LOCALCLASSNAME", LocalClassname);

        foreach (TNamespace sn in children)
        {
            ProcessTemplate subNamespaceSnippet = ATemplate.GetSnippet("SUBNAMESPACEPROPERTY");

            string NamespaceName = Namespace + sn.Name;

            if (HighestLevel)
            {
                NamespaceName = sn.Name;
            }

            subNamespaceSnippet.SetCodelet("OBJECTNAME", sn.Name);
            subNamespaceSnippet.SetCodelet("NAMESPACENAME", NamespaceName);
            subNamespaceSnippet.SetCodelet("NAMESPACE", Namespace);

            remotableClassSnippet.InsertSnippet("SUBNAMESPACEPROPERTIES", subNamespaceSnippet);

            remotableClassSnippet.InsertSnippet("CLIENTOBJECTFOREACHPROPERTY",
                TCreateClientRemotingClass.AddClientRemotingClass(
                    FTemplateDir,
                    "T" + NamespaceName + "NamespaceRemote",
                    "I" + NamespaceName + "Namespace",
                    TCollectConnectorInterfaces.FindTypesInNamespace(connectors, FullNamespace + "." + sn.Name)
                    ));
        }

        return remotableClassSnippet;
    }

    private void CreateAutoHierarchy(TNamespace tn, String AOutputPath, String AXmlFileName)
    {
        String OutputFile = AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                            Path.DirectorySeparatorChar + "Instantiator.AutoHierarchy-generated.cs";

        if (Directory.Exists(AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                Path.DirectorySeparatorChar + "connect"))
        {
            OutputFile = AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                         Path.DirectorySeparatorChar + "connect" +
                         Path.DirectorySeparatorChar + "Instantiator.AutoHierarchy-generated.cs";
        }

        Console.WriteLine("working on " + OutputFile);

        SortedList <string, TypeDeclaration>connectors = TCollectConnectorInterfaces.GetConnectors(tn.Name);

        ProcessTemplate Template = new ProcessTemplate(FTemplateDir + Path.DirectorySeparatorChar +
            "ClientServerGlue" + Path.DirectorySeparatorChar +
            "Instantiator.cs");

        OpenFile(OutputFile);
        WriteLine("// Auto generated with nant generateGlue");
        WriteLine("// based on " + Path.GetFullPath(AXmlFileName).Substring(Path.GetFullPath(AXmlFileName).IndexOf("csharp")));
        WriteLine("//");

        // load default header with license and copyright
        WriteLine(ProcessTemplate.LoadEmptyFileComment(FTemplateDir));

        ProcessTemplate headerSnippet = Template.GetSnippet("HEADER");

        headerSnippet.SetCodelet("TOPLEVELMODULE", tn.Name);

        WriteLine(headerSnippet.FinishWriting(true));

        WriteLine("using Ict.Petra.Shared.Interfaces.M" + tn.Name + ';');

        string InterfacePath = Path.GetFullPath(AOutputPath).Replace(Path.DirectorySeparatorChar, '/');
        InterfacePath = InterfacePath.Substring(0, InterfacePath.IndexOf("csharp/ICT/Petra")) + "csharp/ICT/Petra/Shared/lib/Interfaces";
        WriteLine(CreateInterfaces.AddNamespacesFromYmlFile(InterfacePath, tn.Name));

        WriteLine();

        ProcessTemplate topLevelNamespaceSnippet = Template.GetSnippet("TOPLEVELNAMESPACE");
        topLevelNamespaceSnippet.SetCodelet("TOPLEVELMODULE", tn.Name);
        topLevelNamespaceSnippet.InsertSnippet("LOADERCLASS", WriteLoaderClass(Template, tn.Name));
        topLevelNamespaceSnippet.InsertSnippet("MAINREMOTABLECLASS",
            WriteRemotableClass(
                Template,
                "Ict.Petra.Shared.M" + tn.Name,
                "TM" + tn.Name,
                "M" + tn.Name,
                true,
                tn.Children,
                connectors));

        topLevelNamespaceSnippet.SetCodelet("SUBNAMESPACEREMOTABLECLASSES", "");

        foreach (TNamespace sn in tn.Children)
        {
            topLevelNamespaceSnippet.InsertSnippet("SUBNAMESPACEREMOTABLECLASSES",
                WriteRemotableClass(
                    Template,
                    "Ict.Petra.Shared.M" + tn.Name + "." + sn.Name,
                    sn.Name,
                    sn.Name,
                    false,
                    sn.Children,
                    connectors));
        }

        WriteLine(topLevelNamespaceSnippet.FinishWriting(true));

        Close();
    }

    public void CreateFiles(List <TNamespace>ANamespaces, String AOutputPath, String AXmlFileName, String ATemplateDir)
    {
        FTemplateDir = ATemplateDir;

        foreach (TNamespace tn in ANamespaces)
        {
            CreateAutoHierarchy(tn, AOutputPath, AXmlFileName);
        }
    }
}
}