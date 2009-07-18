/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Reflection;
using DDW.Collections;
using DDW;
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
    private void WriteLoaderClass(String module)
    {
        WriteLine("/// <summary>");
        WriteLine("/// LOADER CLASS. Creates and dynamically exposes an instance of the remoteable");
        WriteLine("/// class to make it callable remotely from the Client.");
        WriteLine("/// </summary>");
        WriteLine("public class TM" + module + "NamespaceLoader : TConfigurableMBRObject");
        WriteLine("{");
        Indent();
        WriteLine("/// <summary>URL at which the remoted object can be reached</summary>");
        WriteLine("private String FRemotingURL;");
        WriteLine("/// <summary>holds reference to the TM" + module + " object</summary>");
        WriteLine("private ObjRef FRemotedObject;");
        WriteLine();
        WriteLine("/// <summary>Constructor</summary>");
        StartBlock("public TM" + module + "NamespaceLoader()");
        WriteLine("#if DEBUGMODE");
        StartBlock("if (TSrvSetting.DL >= 9)");
        WriteLine("Console.WriteLine(this.GetType().FullName + \" created in application domain: \" + Thread.GetDomain().FriendlyName);");
        EndBlock();
        WriteLine("#endif");
        EndBlock();

        WriteLine();
        WriteLine("/// <summary>");
        WriteLine("/// Creates and dynamically exposes an instance of the remoteable TM" + module + "");
        WriteLine("/// class to make it callable remotely from the Client.");
        WriteLine("///");
        WriteLine("/// @comment This function gets called from TRemoteLoader.LoadPetraModuleAssembly.");
        WriteLine("/// This call is done late-bound through .NET Reflection!");
        WriteLine("///");
        WriteLine("/// WARNING: If the name of this function or its parameters should change, this");
        WriteLine("/// needs to be reflected in the call to this function in");
        WriteLine("/// TRemoteLoader.LoadPetraModuleAssembly!!!");
        WriteLine("///");
        WriteLine("/// </summary>");
        WriteLine("/// <returns>The URL at which the remoted object can be reached.</returns>");

        StartBlock("public String GetRemotingURL()");
        WriteLine("TM" + module + " RemotedObject;");
        WriteLine("DateTime RemotingTime;");
        WriteLine("String RemoteAtURI;");
        WriteLine("String RandomString;");
        WriteLine("System.Security.Cryptography.RNGCryptoServiceProvider rnd;");
        WriteLine("Byte rndbytespos;");
        WriteLine("Byte[] rndbytes = new Byte[5];");
        WriteLine();
        WriteLine("#if DEBUGMODE");
        StartBlock("if (TSrvSetting.DL >= 9)");
        WriteLine("Console.WriteLine(\"TM" + module + "NamespaceLoader.GetRemotingURL in AppDomain: \" + Thread.GetDomain().FriendlyName);");
        EndBlock();
        WriteLine("#endif");
        WriteLine();
        WriteLine("RandomString = \"\";");
        WriteLine("rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();");
        WriteLine("rnd.GetBytes(rndbytes);");
        WriteLine();
        StartBlock("for (rndbytespos = 1; rndbytespos <= 4; rndbytespos += 1)");
        WriteLine("RandomString = RandomString + rndbytes[rndbytespos].ToString();");
        EndBlock();
        WriteLine();
        WriteLine("RemotingTime = DateTime.Now;");
        WriteLine("RemotedObject = new TM" + module + "();");
        WriteLine("RemoteAtURI = (RemotingTime.Day).ToString() + (RemotingTime.Hour).ToString() + (RemotingTime.Minute).ToString() +");
        WriteLine("              (RemotingTime.Second).ToString() + '_' + RandomString.ToString();");
        WriteLine("FRemotedObject = RemotingServices.Marshal(RemotedObject, RemoteAtURI, typeof(IM" + module + "Namespace));");
        WriteLine("FRemotingURL = RemoteAtURI; // FRemotedObject.URI;");
        WriteLine();
        WriteLine("#if DEBUGMODE");
        StartBlock("if (TSrvSetting.DL >= 9)");
        WriteLine("Console.WriteLine(\"TM" + module + ".URI: \" + FRemotedObject.URI);");
        EndBlock();
        WriteLine("#endif");
        WriteLine();
        WriteLine("return FRemotingURL;");
        EndBlock();
        WriteLine();
        EndBlock();
    }

    private void CreateInstanceOfConnector(InterfaceMethodNode m, string ATypeConnector)
    {
        bool outHasBeenFound = false;
        bool firstParameter;

        foreach (ParamDeclNode p in m.Params)
        {
            if ((p.Modifiers & (Modifier.Out | Modifier.Ref)) != 0)
            {
                outHasBeenFound = true;
            }
        }

        if (!outHasBeenFound)
        {
            // simple: no need to call GetData

            String createObject = "return new T" + CSParser.GetName(m.Names) + ATypeConnector + "(";
            firstParameter = true;

            foreach (ParamDeclNode p in m.Params)
            {
                if (!firstParameter)
                {
                    createObject += ", ";
                }

                firstParameter = false;
                createObject += p.Name;
            }

            createObject += ");";
            WriteLine(createObject);
        }
        else
        {
            // the first parameters are for the constructor
            // then the out parameter is for the dataset,
            // and all the following parameters are for GetData

            WriteLine("#if DEBUGMODE");
            StartBlock("if (TSrvSetting.DL >= 9)");
            WriteLine("Console.WriteLine(this.GetType().FullName + \": Creating " +
                "T" + CSParser.GetName(m.Names) + ATypeConnector + "...\");");
            EndBlock();
            WriteLine("#endif");

            String createObject = "T" + CSParser.GetName(m.Names) + ATypeConnector + " ReturnValue = new T" + CSParser.GetName(m.Names) +
                                  ATypeConnector + "(";
            StringCollection parameters = new StringCollection();

            foreach (ParamDeclNode p in m.Params)
            {
                String parameterType = CSParser.GetName(p.Type);

                if ((p.Modifiers & (Modifier.Out | Modifier.Ref)) != 0)
                {
                    break;
                }

                parameters.Add(p.Name);
            }

            WriteLineMethodCall(createObject, parameters);

            WriteLine("#if DEBUGMODE");
            StartBlock("if (TSrvSetting.DL >= 9)");
            WriteLine("Console.WriteLine(this.GetType().FullName + \": Calling " +
                "T" + CSParser.GetName(m.Names) + ATypeConnector + ".GetData...\");");
            EndBlock();
            WriteLine("#endif");

            // find the out parameter, and use the following parameters as parameters for GetData
            String getData = "";
            outHasBeenFound = false;
            firstParameter = true;

            foreach (ParamDeclNode p in m.Params)
            {
                if (outHasBeenFound)
                {
                    if (!firstParameter)
                    {
                        getData += ", ";
                    }

                    firstParameter = false;
                    getData += p.Name;
                }

                if ((p.Modifiers & (Modifier.Out | Modifier.Ref)) != 0)
                {
                    getData += p.Name + " = ReturnValue.GetData(";
                    outHasBeenFound = true;
                }
            }

            getData += ");";
            WriteLine(getData);

            WriteLine("#if DEBUGMODE");
            StartBlock("if (TSrvSetting.DL >= 9)");
            WriteLine("Console.WriteLine(this.GetType().FullName + \": Calling " +
                "T" + CSParser.GetName(m.Names) + ATypeConnector + ".GetData finished.\");");
            EndBlock();
            WriteLine("#endif");

            WriteLine("return ReturnValue;");
        }
    }

    private void ImplementInterface(String AFullNamespace, String AInterfaceName)
    {
        // e.g. FullNamespace: Ict.Petra.Server.MPartner.Instantiator.Partner.UIConnectors
        // e.g. InterfaceNamespace: Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors
        string InterfaceNamespace = AFullNamespace.Replace("Server.", "Shared.Interfaces.").Replace("Instantiator.", "");

        // try to implement the methods defined in the interface
        InterfaceNode t = CSParser.FindInterface(CSFiles, InterfaceNamespace, AInterfaceName);

        if (t == null)
        {
            return;
        }

        foreach (InterfaceMethodNode m in CSParser.GetMethods(t))
        {
            string MethodName = CSParser.GetName(m.Names);

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

            String returnType = CSParser.GetName(m.Type);

            if (returnType == "System.Void")
            {
                returnType = "void";
            }

            WriteLine("/// generated method from interface");

            int align = (returnType + " " + MethodName).Length + 1 + ("public ").Length;
            String formattedMethod = "public " + returnType + " " + MethodName + "(";

            bool firstParameter = true;

            foreach (ParamDeclNode p in m.Params)
            {
                AddParameter(ref formattedMethod, ref firstParameter, align, p.Name, p.Modifiers, p.Type);
            }

            formattedMethod += ")";

            StartBlock(formattedMethod);

            // TODO: there is a manual exception for TReportingUIConnectorsNamespace: better do a generic way
            if (AInterfaceName.EndsWith("UIConnectorsNamespace")
                && (AInterfaceName != "IReportingUIConnectorsNamespace"))
            {
                CreateInstanceOfConnector(m, "UIConnector");
            }
            else if (AInterfaceName.EndsWith("LogicConnectorsNamespace"))
            {
                CreateInstanceOfConnector(m, "LogicConnector");
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

            EndBlock();
        }
    }

    void ImplementStaticCallConnector(string AFullNamespace)
    {
        // e.g. FullNamespace: Ict.Petra.Server.MFinance.Instantiator.AccountsPayable.WebConnectors
        // e.g. ConnectorNamespace: Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors
        string ConnectorNamespace = AFullNamespace.Replace("Instantiator.", "");

        List <CSParser>CSFiles = new List <CSParser>();
        string module = AFullNamespace.Split('.')[3];
        CSParser.GetCSFilesInProject(CSParser.ICTPath + "/Petra/Server/lib/" +
            module + "/Ict.Petra.Server." + module + ".WebConnectors.csproj",
            ref CSFiles);

        List <ClassNode>ConnectorClasses = CSParser.GetClassesInNamespace(CSFiles, ConnectorNamespace);

        foreach (ClassNode connectorClass in ConnectorClasses)
        {
            foreach (MethodNode m in CSParser.GetMethods(connectorClass))
            {
                string MethodName = CSParser.GetName(m.Names);

                String returnType = CSParser.GetName(m.Type);

                if (returnType == "System.Void")
                {
                    returnType = "void";
                }

                WriteLine("/// generated method from connector");

                int align = (returnType + " " + MethodName).Length + 1 + ("public ").Length;
                String formattedMethod = "public " + returnType + " " + MethodName + "(";
                bool firstParameter = true;
                string actualParameters = "";

                foreach (ParamDeclNode p in m.Params)
                {
                    if (!firstParameter)
                    {
                        actualParameters += ", ";
                    }

                    if ((p.Modifiers & Modifier.Out) > 0)
                    {
                        actualParameters += "out ";
                    }

                    if ((p.Modifiers & Modifier.Ref) > 0)
                    {
                        actualParameters += "ref ";
                    }

                    actualParameters += p.Name;
                    AddParameter(ref formattedMethod, ref firstParameter, align, p.Name, p.Modifiers, p.Type);
                }

                formattedMethod += ")";

                StartBlock(formattedMethod);

                // eg: return Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors.TTransactionEditWebConnector.GetDocument(ALedgerNumber, AAPNumber);
                WriteLine("return " + ConnectorNamespace + "." +
                    CSParser.GetName(connectorClass.Name) + "." +
                    MethodName + "(" + actualParameters + ");");

                EndBlock();
            }
        }
    }

    private void WriteRemotableClass(String FullNamespace, String Classname, String Namespace, Boolean HighestLevel, List <SubNamespace>children)
    {
        if (HighestLevel)
        {
            WriteLine("/// <summary>");
            WriteLine("/// " + "REMOTEABLE CLASS. " + Namespace + " Namespace (highest level).");
            WriteLine("/// </summary>");
        }

        String LocalClassname = Classname;

        if (!HighestLevel)
        {
            LocalClassname = "T" + Classname + "Namespace";
        }

        WriteLine("/// <summary>auto generated class </summary>");
        WriteLine("public class " + LocalClassname + " : MarshalByRefObject, I" + Namespace + "Namespace");
        WriteLine("{");
        Indent();
        WriteLine("#if DEBUGMODE");
        WriteLine("private DateTime FStartTime;");
        WriteLine("#endif");

        foreach (SubNamespace sn in children)
        {
            if (HighestLevel)
            {
                WriteLine("private T" + sn.Name + "Namespace F" + sn.Name + "SubNamespace;");
            }
            else
            {
                WriteLine("private T" + Namespace + sn.Name + "Namespace F" + Namespace + sn.Name + "SubNamespace;");
            }
        }

        WriteLine();
        WriteLine("/// <summary>Constructor</summary>");
        StartBlock("public " + LocalClassname + "()");
        WriteLine("#if DEBUGMODE");
        StartBlock("if (TSrvSetting.DL >= 9)");
        WriteLine("Console.WriteLine(this.GetType().FullName + \" created: Instance hash is \" + this.GetHashCode().ToString());");
        EndBlock();
        WriteLine("FStartTime = DateTime.Now;");
        WriteLine("#endif");
        EndBlock();
        WriteLine();
        WriteLine("// NOTE AutoGeneration: This destructor is only needed for debugging...");
        WriteLine("#if DEBUGMODE");
        WriteLine("/// <summary>Destructor</summary>");
        StartBlock("~" + LocalClassname + "()");
        WriteLine("#if DEBUGMODELONGRUNNINGFINALIZERS");
        WriteLine("const Int32 MAX_ITERATIONS = 100000;");
        WriteLine("System.Int32 LoopCounter;");
        WriteLine("object MyObject;");
        WriteLine("object MyObject2;");
        WriteLine("#endif");
        StartBlock("if (TSrvSetting.DL >= 9)");
        WriteLine("Console.WriteLine(this.GetType().FullName + \": Getting collected after \" + (new TimeSpan(");
        WriteLine("                                                                                DateTime.Now.Ticks -");
        WriteLine("                                                                                FStartTime.Ticks)).ToString() + \" seconds.\");");
        EndBlock();
        WriteLine("#if DEBUGMODELONGRUNNINGFINALIZERS");
        WriteLine("MyObject = new object();");
        StartBlock("if (TSrvSetting.DL >= 9)");
        WriteLine("Console.WriteLine(this.GetType().FullName + \": Now performing some longer-running stuff...\");");
        EndBlock();
        StartBlock("for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)");
        WriteLine("MyObject2 = new object();");
        WriteLine("GC.KeepAlive(MyObject);");
        EndBlock();
        StartBlock("if (TSrvSetting.DL >= 9)");
        WriteLine("Console.WriteLine(this.GetType().FullName + \": FINALIZER has run.\");");
        EndBlock();
        WriteLine("#endif");
        EndBlock();
        WriteLine("#endif");
        WriteLine();
        WriteLine();
        WriteLine();
        WriteLine("/// NOTE AutoGeneration: This function is all-important!!!");
        StartBlock("public override object InitializeLifetimeService()");
        WriteLine("return null; // make sure that the " + LocalClassname + " object exists until this AppDomain is unloaded!");
        EndBlock();
        WriteLine();

        if (children.Count == 0)
        {
            if (Namespace.EndsWith("WebConnectors"))
            {
                ImplementStaticCallConnector(FullNamespace);
            }
            else
            {
                ImplementInterface(FullNamespace, "I" + Namespace + "Namespace");
            }
        }
        else
        {
            WriteLine(
                "// NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)");
        }

        foreach (SubNamespace sn in children)
        {
            if (HighestLevel)
            {
                WriteLine("/// <summary>The '" + sn.Name + "' subnamespace contains further subnamespaces.</summary>");
                StartBlock("public I" + sn.Name + "Namespace " + sn.Name + "");
            }
            else
            {
                WriteLine("/// <summary>The '" + Namespace + sn.Name + "' subnamespace contains further subnamespaces.</summary>");
                StartBlock("public I" + Namespace + sn.Name + "Namespace " + sn.Name + "");
            }

            StartBlock("get");
            WriteLine("//");
            WriteLine("// Creates or passes a reference to an instantiator of sub-namespaces that");
            WriteLine("// reside in the '" + Namespace + "." + sn.Name + "' sub-namespace.");
            WriteLine("// A call to this function is done everytime a Client uses an object of this");
            WriteLine("// sub-namespace - this is fully transparent to the Client.");
            WriteLine("//");
            WriteLine("// @return A reference to an instantiator of sub-namespaces that reside in");
            WriteLine("//         the '" + Namespace + "." + sn.Name + "' sub-namespace");
            WriteLine("//");
            WriteLine();
            WriteLine("// accessing T" + sn.Name + "Namespace the first time? > instantiate the object");

            if (HighestLevel)
            {
                StartBlock("if (F" + sn.Name + "SubNamespace == null)");
            }
            else
            {
                StartBlock("if (F" + Namespace + sn.Name + "SubNamespace == null)");
            }

            WriteLine("// NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!");
            WriteLine("//      * for the Generator: the name of this Type ('T" + sn.Name + "Namespace') needs to come out of the XML definition,");
            WriteLine(
                "//      * The Namespace where it resides in ('Ict.Petra.Server." + Namespace + ".Instantiator." + sn.Name +
                "') should be automatically contructable.");

            if (HighestLevel)
            {
                WriteLine("F" + sn.Name + "SubNamespace = new T" + sn.Name + "Namespace();");
            }
            else
            {
                WriteLine("F" + Namespace + sn.Name + "SubNamespace = new T" + Namespace + sn.Name + "Namespace();");
            }

            EndBlock();
            WriteLine();

            if (HighestLevel)
            {
                WriteLine("return F" + sn.Name + "SubNamespace;");
            }
            else
            {
                WriteLine("return (I" + Namespace + sn.Name + "Namespace)F" + Namespace + sn.Name + "SubNamespace;");
            }

            EndBlock();
            EndBlock();
            WriteLine();
        }

        DeIndent();
        WriteLine("}");
    }

    private void WriteNamespace(String NamespaceName, String ClassName, SubNamespace sn)
    {
        WriteLine();
        StartBlock("namespace " + NamespaceName);

        WriteLine();
        WriteRemotableClass(NamespaceName,
            ClassName,
            ClassName,
            false,
            sn.Children);

        EndBlock();

        foreach (SubNamespace sn2 in sn.Children)
        {
            WriteNamespace(NamespaceName + "." + sn2.Name, ClassName + sn2.Name, sn2);
        }
    }

    private void CreateAutoHierarchy(TopNamespace tn, String AOutputPath, String AXmlFileName)
    {
        String OutputFile = AOutputPath + Path.DirectorySeparatorChar + "M" + tn.Name +
                            Path.DirectorySeparatorChar + "Instantiator.AutoHierarchy.cs";

        Console.WriteLine("working on " + OutputFile);

        OpenFile(OutputFile);
        WriteLine("/* Auto generated with nant generateGlue");
        WriteLine(" * based on " + Path.GetFullPath(AXmlFileName).Substring(Path.GetFullPath(AXmlFileName).IndexOf("csharp")));
        WriteLine(" */");
        WriteLine("//");
        WriteLine("// Contains a remotable class that instantiates an Object which gives access to");
        WriteLine("// the MPartner Namespace (from the Client's perspective).");
        WriteLine("//");
        WriteLine("// The purpose of the remotable class is to present other classes which are");
        WriteLine("// Instantiators for sub-namespaces to the Client. The instantiation of the");
        WriteLine("// sub-namespace objects is completely transparent to the Client!");
        WriteLine("// The remotable class itself gets instantiated and dynamically remoted by the");
        WriteLine("// loader class, which in turn gets called when the Client Domain is set up.");
        WriteLine("//");
        WriteLine();
        WriteLine("using System;");
        SynchronizeLines();
        WriteLine("using System.Collections;");
        WriteLine("using System.Collections.Generic;");
        WriteLine("using System.Data;");
        WriteLine("using System.Threading;");
        WriteLine("using System.Runtime.Remoting;");
        WriteLine("using System.Security.Cryptography;");
        WriteLine("using Ict.Common;");
        WriteLine("using Ict.Petra.Shared;");

        WriteLine("using Ict.Petra.Shared.Interfaces.M" + tn.Name + ';');

        foreach (SubNamespace sn in tn.SubNamespaces)
        {
            WriteLine("using Ict.Petra.Shared.Interfaces.M" + tn.Name + "." + sn.Name + ';');
        }

        foreach (SubNamespace sn in tn.SubNamespaces)
        {
            CommonNamespace.WriteUsingNamespace(this, "Ict.Petra.Shared.Interfaces.M" + tn.Name + "." + sn.Name, sn.Name, sn, sn.Children);
        }

        foreach (SubNamespace sn in tn.SubNamespaces)
        {
            WriteLine("using Ict.Petra.Server.M" + tn.Name + ".Instantiator." + sn.Name + ';');
        }

        foreach (SubNamespace sn in tn.SubNamespaces)
        {
            CommonNamespace.WriteUsingNamespace(this, "Ict.Petra.Server.M" + tn.Name + ".Instantiator." + sn.Name, sn.Name, sn, sn.Children);
        }

        foreach (SubNamespace sn in tn.SubNamespaces)
        {
            WriteLine("using Ict.Petra.Server.M" + tn.Name + "." + sn.Name + ';');
        }

        foreach (SubNamespace sn in tn.SubNamespaces)
        {
            CommonNamespace.WriteUsingNamespace(this, "Ict.Petra.Server.M" + tn.Name + "." + sn.Name, sn.Name, sn, sn.Children);
        }

        WriteLine();

        //InsertManualLines(true);

        WriteLine();
        WriteLine("namespace Ict.Petra.Server.M" + tn.Name + ".Instantiator");
        WriteLine("{");
        Indent();
        WriteLoaderClass(tn.Name);
        WriteLine();
        WriteRemotableClass("Ict.Petra.Server.M" + tn.Name + ".Instantiator",
            "TM" + tn.Name,
            "M" + tn.Name,
            true,
            tn.SubNamespaces);

        DeIndent();
        WriteLine("}");

        foreach (SubNamespace sn in tn.SubNamespaces)
        {
            WriteNamespace("Ict.Petra.Server.M" + tn.Name + ".Instantiator." + sn.Name, sn.Name, sn);
        }

        Close();
    }

    private List <CSParser>CSFiles = null;
    public void CreateFiles(List <TopNamespace>ANamespaces, String AOutputPath, String AXmlFileName)
    {
        // get the appropriate cs file
        CSFiles = new List <CSParser>();
        CSParser.GetCSFilesInProject(CSParser.ICTPath + "/Petra/Shared/lib/Interfaces/Ict.Petra.Shared.Interfaces.csproj", ref CSFiles);

        foreach (TopNamespace tn in ANamespaces)
        {
            // for testing:
//        if (tn.Name != "Reporting") continue;
            CreateAutoHierarchy(tn, AOutputPath, AXmlFileName);
        }
    }
}
}