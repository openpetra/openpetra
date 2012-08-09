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

    private ProcessTemplate WriteRemotableClass(ProcessTemplate ATemplate,
        String FullNamespace,
        String Classname,
        String Namespace,
        Boolean HighestLevel,
        List <TNamespace>children)
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
                            Path.DirectorySeparatorChar + "Instantiator.AutoHierarchy-generated.cs";

        if (Directory.Exists(AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                Path.DirectorySeparatorChar + "connect"))
        {
            OutputFile = AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                         Path.DirectorySeparatorChar + "connect" +
                         Path.DirectorySeparatorChar + "Instantiator.AutoHierarchy-generated.cs";
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

        headerSnippet.SetCodelet("TOPLEVELMODULE", tn.Name);

        WriteLine(headerSnippet.FinishWriting(true));

        WriteLine("using Ict.Petra.Shared.Interfaces.M" + tn.Name + ';');

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

    public void CreateFiles(List <TNamespace>ANamespaces, String AOutputPath, String AXmlFileName, String ATemplateDir)
    {
        foreach (TNamespace tn in ANamespaces)
        {
            CreateAutoHierarchy(tn, AOutputPath, AXmlFileName, ATemplateDir);
        }
    }
}
}