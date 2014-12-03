//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Reflection;
using NamespaceHierarchy;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.CodeGeneration;
using GenerateSharedCode;
using GenerateGlue;

namespace Ict.Tools.GenerateGlue
{
    class Program
    {
        private static String sampleCall =
            "GenerateSharedCode -outputdir:..\\..\\..\\Petra\\ -TemplateDir:..\\..\\..\\inc\\template\\src\\ClientServerGlue\\";

        public static void Main(string[] args)
        {
            TCmdOpts cmd = new TCmdOpts();

            new TAppSettingsManager(false);

            TLogging.DebugLevel = TAppSettingsManager.GetInt32("debuglevel", 0);

            String OutputDir;

            if (!cmd.IsFlagSet("TemplateDir"))
            {
                Console.WriteLine("call: " + sampleCall);
                return;
            }

            if (cmd.IsFlagSet("outputdir"))
            {
                OutputDir = cmd.GetOptValue("outputdir");

                // calculate ICTPath from outputdir
                string fullOutputPath = Path.GetFullPath(OutputDir).Replace("\\", "/");

                if (!fullOutputPath.Contains("csharp/ICT"))
                {
                    Console.WriteLine("Output path must be below the csharp/ICT directory");
                }

                CSParser.ICTPath = fullOutputPath.Substring(0, fullOutputPath.IndexOf("csharp/ICT") + "csharp/ICT".Length);
            }
            else
            {
                Console.WriteLine("call: " + sampleCall);
                return;
            }

            TNamespace namespaceRoot;

            try
            {
                Console.WriteLine("parsing all cs files for namespaces...");
                namespaceRoot = TNamespace.ParseFromDirectory(OutputDir + "/Server/lib/");

                if (namespaceRoot.Children.Count < 1)
                {
                    Console.WriteLine("problems with parsing namespaces from " + OutputDir + "/Server/lib/");
                    Environment.Exit(-1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Environment.Exit(-1);
                return;
            }

            try
            {
                /*
                 * CreateInstantiators instantiators = new CreateInstantiators();
                 * instantiators.CreateFiles(namespaceRoot, OutputDir + "/Server/lib", cmd.GetOptValue("TemplateDir"));
                 * TCreateConnectors connectors = new TCreateConnectors();
                 * connectors.CreateFiles(namespaceRoot, OutputDir + "/Server/lib", cmd.GetOptValue("TemplateDir"));
                 */

                CreateInterfaces interfaces = new CreateInterfaces();
                interfaces.CreateFiles(namespaceRoot, OutputDir + "/Shared/lib/Interfaces", cmd.GetOptValue("TemplateDir"));
                GenerateClientGlue.GenerateCode(namespaceRoot, OutputDir + "/Client/app/Core/Remoteobjects", cmd.GetOptValue("TemplateDir"));
                GenerateClientGlue.GenerateConnectorCode(OutputDir + "/../Common/Remoting/Client", cmd.GetOptValue("TemplateDir"));
                GenerateServerGlue.GenerateCode(namespaceRoot, OutputDir + "/Server/app/WebService", cmd.GetOptValue("TemplateDir"));

                namespaceRoot = new TNamespace();
                TNamespace ServerAdminNamespace = new TNamespace("ServerAdmin");
                namespaceRoot.Children.Add("ServerAdmin", ServerAdminNamespace);
                TNamespace ServerAdminWebConnectorNamespace = new TNamespace("WebConnectors");
                ServerAdminNamespace.Children.Add("WebConnectors", ServerAdminWebConnectorNamespace);

                GenerateServerGlue.GenerateCode(namespaceRoot, OutputDir + "/Server/app/WebService", cmd.GetOptValue("TemplateDir"));
                GenerateClientGlue.GenerateCode(namespaceRoot, OutputDir + "/ServerAdmin/app/Core", cmd.GetOptValue("TemplateDir"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Environment.Exit(-1);
            }
        }
    }
}