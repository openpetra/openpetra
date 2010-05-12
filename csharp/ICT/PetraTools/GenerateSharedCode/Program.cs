//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using Ict.Tools.CodeGeneration;

namespace GenerateSharedCode
{
class Program
{
    private static String sampleCall =
        "GenerateSharedCode -xmlfile:..\\..\\..\\Petra\\Definitions\\NamespaceHierarchy.xml -outputdir:..\\..\\..\\Petra\\";

    public static void Main(string[] args)
    {
        TCmdOpts cmd = new TCmdOpts();

        String XmlFileName, OutputDir;

        if (cmd.IsFlagSet("xmlfile"))
        {
            XmlFileName = cmd.GetOptValue("xmlfile");
        }
        else
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

        List <TopNamespace>namespaces;

        // Preferred approach in .NET 2.0:
        // ->  returns a Strongly Typed List of Type 'TopNamespace'.
        namespaces = SubNamespace.ReadFromFile(XmlFileName);

        if (namespaces.Count < 1)
        {
            Console.WriteLine("problems with reading " + XmlFileName);
            return;
        }

        try
        {
            CreateInterfaces interfaces = new CreateInterfaces();
            interfaces.CreateFiles(namespaces, OutputDir + "\\Shared\\lib\\Interfaces", XmlFileName);
            CreateInstantiators instantiators = new CreateInstantiators();
            instantiators.CreateFiles(namespaces, OutputDir + "\\Server\\lib\\", XmlFileName);

            //		  TestAnalysis("U:/delphi.net/ICT.Patch228/Petra/Server/_bin/Debug/Ict.Petra.Server.MPartner.Instantiator.dll",
            //		                "Ict.Petra.Server.MPartner.Instantiator.Partner.TPartnerUIConnectorsNamespace");
            //		  TestAnalysisAll("U:/delphi.net/ICT.Patch228/Petra/Server/_bin/Debug/Ict.Petra.Server.MPartner.UIConnectors.dll");
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