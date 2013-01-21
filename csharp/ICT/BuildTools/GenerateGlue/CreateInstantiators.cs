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
class CreateInstantiators
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
        SortedList <string, TNamespace>children,
        SortedList <string, TypeDeclaration>connectors)
    {
        if ((children.Count == 0) && !HighestLevel)
        {
            return new ProcessTemplate();
        }

        ProcessTemplate remotableClassSnippet = ATemplate.GetSnippet("REMOTABLECLASS");

        remotableClassSnippet.SetCodelet("SUBNAMESPACEREMOTABLECLASSES", string.Empty);

        remotableClassSnippet.SetCodelet("NAMESPACE", Namespace);

        remotableClassSnippet.SetCodelet("CLIENTOBJECTFOREACHPROPERTY", string.Empty);
        remotableClassSnippet.SetCodelet("SUBNAMESPACEPROPERTIES", string.Empty);

        foreach (TNamespace sn in children.Values)
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

            if (sn.Children.Count > 0)
            {
                // properties for each sub namespace
                foreach (TNamespace subnamespace in sn.Children.Values)
                {
                    ATemplate.InsertSnippet("SUBNAMESPACEREMOTABLECLASSES",
                        WriteRemotableClass(ATemplate,
                            FullNamespace + "." + sn.Name + "." + subnamespace.Name,
                            sn.Name + subnamespace.Name,
                            NamespaceName + subnamespace.Name,
                            false,
                            subnamespace.Children,
                            connectors));
                }

                remotableClassSnippet.InsertSnippet("CLIENTOBJECTFOREACHPROPERTY",
                    TCreateClientRemotingClass.AddClientRemotingClass(
                        FTemplateDir,
                        "T" + NamespaceName + "NamespaceRemote",
                        "I" + NamespaceName + "Namespace",
                        new List <TypeDeclaration>(),
                        FullNamespace + "." + sn.Name,
                        sn.Children
                        ));
            }
            else
            {
                remotableClassSnippet.InsertSnippet("CLIENTOBJECTFOREACHPROPERTY",
                    TCreateClientRemotingClass.AddClientRemotingClass(
                        FTemplateDir,
                        "T" + NamespaceName + "NamespaceRemote",
                        "I" + NamespaceName + "Namespace",
                        TCollectConnectorInterfaces.FindTypesInNamespace(connectors, FullNamespace + "." + sn.Name)
                        ));
            }
        }

        return remotableClassSnippet;
    }

    private void CreateAutoHierarchy(TNamespace tn, String AOutputPath)
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

        // load default header with license and copyright
        Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(FTemplateDir));

        Template.SetCodelet("TOPLEVELMODULE", tn.Name);

        Template.AddToCodelet("USINGNAMESPACES", "using Ict.Petra.Shared.Interfaces.M" + tn.Name + ";" + Environment.NewLine);

        string InterfacePath = Path.GetFullPath(AOutputPath).Replace(Path.DirectorySeparatorChar, '/');
        InterfacePath = InterfacePath.Substring(0, InterfacePath.IndexOf("csharp/ICT/Petra")) + "csharp/ICT/Petra/Shared/lib/Interfaces";
        Template.AddToCodelet("USINGNAMESPACES", CreateInterfaces.AddNamespacesFromYmlFile(InterfacePath, tn.Name));

        ProcessTemplate topLevelNamespaceSnippet = Template.GetSnippet("TOPLEVELNAMESPACE");
        topLevelNamespaceSnippet.SetCodelet("SUBNAMESPACEREMOTABLECLASSES", "");
        topLevelNamespaceSnippet.SetCodelet("TOPLEVELMODULE", tn.Name);
        topLevelNamespaceSnippet.InsertSnippet("LOADERCLASS", WriteLoaderClass(Template, tn.Name));
        topLevelNamespaceSnippet.InsertSnippet("MAINREMOTABLECLASS",
            WriteRemotableClass(
                topLevelNamespaceSnippet,
                "Ict.Petra.Shared.M" + tn.Name,
                "TM" + tn.Name,
                "M" + tn.Name,
                true,
                tn.Children,
                connectors));

        foreach (TNamespace sn in tn.Children.Values)
        {
            topLevelNamespaceSnippet.InsertSnippet("SUBNAMESPACEREMOTABLECLASSES",
                WriteRemotableClass(
                    topLevelNamespaceSnippet,
                    "Ict.Petra.Shared.M" + tn.Name + "." + sn.Name,
                    sn.Name,
                    sn.Name,
                    false,
                    sn.Children,
                    connectors));
        }

        Template.InsertSnippet("CONTENT", topLevelNamespaceSnippet);

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
                CreateAutoHierarchy(tn, AOutputPath);
            }
        }
    }
}
}