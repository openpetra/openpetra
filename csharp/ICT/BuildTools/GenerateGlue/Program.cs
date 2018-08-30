//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2018 by OM International
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

            if (cmd.IsFlagSet("plugin"))
            {
                OutputDir = cmd.GetOptValue("plugindir");

                // calculate ICTPath from outputdir
                string fullOutputPath = Path.GetFullPath(OutputDir).Replace("\\", "/");

                if (!fullOutputPath.Contains("csharp/ICT"))
                {
                    Console.WriteLine("Output path must be below the csharp/ICT directory");
                }

                CSParser.ICTPath = fullOutputPath.Substring(0, fullOutputPath.IndexOf("csharp/ICT") + "csharp/ICT".Length);
                GenerateGlueForPlugin(cmd, OutputDir);
            }
            else if (cmd.IsFlagSet("outputdir"))
            {
                OutputDir = cmd.GetOptValue("outputdir");

                // calculate ICTPath from outputdir
                string fullOutputPath = Path.GetFullPath(OutputDir).Replace("\\", "/");

                if (!fullOutputPath.Contains("csharp/ICT"))
                {
                    Console.WriteLine("Output path must be below the csharp/ICT directory");
                }

                CSParser.ICTPath = fullOutputPath.Substring(0, fullOutputPath.IndexOf("csharp/ICT") + "csharp/ICT".Length);

                GenerateGlueForOpenPetraCore(cmd, OutputDir);
            }
            else
            {
                Console.WriteLine("call: " + sampleCall);
                return;
            }
        }

        private static void GenerateGlueForOpenPetraCore(TCmdOpts ACmd, string AOutputDir)
        {
            TNamespace namespaceRoot;

            try
            {
                Console.WriteLine("parsing all cs files for namespaces...");
                namespaceRoot = TNamespace.ParseFromDirectory(AOutputDir + "/Server/lib/");

                if (namespaceRoot.Children.Count < 1)
                {
                    Console.WriteLine("problems with parsing namespaces from " + AOutputDir + "/Server/lib/");
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
                GenerateServerGlue.GenerateCode(namespaceRoot, AOutputDir + "/Server/app/WebService", ACmd.GetOptValue("TemplateDir"));

                namespaceRoot = new TNamespace();
                TNamespace ServerAdminNamespace = new TNamespace("ServerAdmin");
                namespaceRoot.Children.Add("ServerAdmin", ServerAdminNamespace);
                TNamespace ServerAdminWebConnectorNamespace = new TNamespace("WebConnectors");
                ServerAdminNamespace.Children.Add("WebConnectors", ServerAdminWebConnectorNamespace);

                GenerateServerGlue.GenerateCode(namespaceRoot, AOutputDir + "/Server/app/WebService", ACmd.GetOptValue("TemplateDir"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Environment.Exit(-1);
            }
        }

        private static void GenerateGlueForPlugin(TCmdOpts ACmd, string AOutputDir)
        {
            TNamespace namespaceRoot;

            AOutputDir = AOutputDir.Replace("\\", "/");

            try
            {
                Console.WriteLine("parsing plugin cs files for namespaces...");
                namespaceRoot = TNamespace.ParseFromDirectory(AOutputDir + "/Server/");

                if (namespaceRoot.Children.Count < 1)
                {
                    Console.WriteLine("There are no connectors in " + AOutputDir + "/Server/");
                    return;
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
                GenerateServerGlue.GenerateCode(namespaceRoot, AOutputDir + "/Server", ACmd.GetOptValue("TemplateDir"));
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
