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
using System.Xml;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;
using Ict.Tools.CodeGeneration.Winforms;

namespace GenerateWinForms
{
class Program
{
    private static void ProcessFile(string filename)
    {
        ProcessXAML processor = new ProcessXAML(filename);

        processor.AddWriter("navigation", typeof(TWinFormsWriter));
        processor.AddWriter("edit", typeof(TWinFormsWriter));
        processor.AddWriter("report", typeof(TWinFormsWriter));
        processor.AddWriter("browse", typeof(TWinFormsWriter));

        //processor.AddWriter("browse", typeof(TWinFormsWriter));
        // could add instead of TWinformsWriter: TGtkWriter
        processor.ProcessDocument();
    }

    public static void Main(string[] args)
    {
        try
        {
            TAppSettingsManager opts = new TAppSettingsManager(false);

            if (!opts.HasValue("ymlfile"))
            {
                Console.WriteLine("call: GenerateWinForms -ymlfile:c:\\test.yaml -petraxml:petra.xml");
                Console.Write("Press any key to continue . . . ");
                Console.ReadLine();
                Environment.Exit(-1);
                return;
            }

            TControlGenerator.FPetraXMLStore = new TDataDefinitionStore();
            Console.WriteLine("parsing " + opts.GetValue("petraxml", true));
            TDataDefinitionParser parser = new TDataDefinitionParser(opts.GetValue("petraxml", true));
            parser.ParseDocument(ref TControlGenerator.FPetraXMLStore, true, true);

            string ymlfileParam = opts.GetValue("ymlfile", true);

            if (ymlfileParam.Contains(","))
            {
                StringCollection collection = StringHelper.StrSplit(ymlfileParam, ",");

                foreach (string file in collection)
                {
                    ProcessFile(file);
                }
            }
            else if (System.IO.Directory.Exists(ymlfileParam))
            {
                foreach (string file in System.IO.Directory.GetFiles(ymlfileParam, "*.yaml"))
                {
                    ProcessFile(file);
                }
            }
            else
            {
                ProcessFile(ymlfileParam);
            }
        }
        catch (Exception e)
        {
            string commandline = "";

            foreach (string s in args)
            {
                commandline += s + " ";
            }

            Console.WriteLine("Problem while processing " + commandline);
            Console.WriteLine(e.GetType().ToString() + ": " + e.Message);

            if (e.InnerException != null)
            {
                Console.WriteLine(e.InnerException.GetType().ToString() + ": " + e.InnerException.Message);
            }

            Console.WriteLine(e.StackTrace);
            Environment.Exit(-1);
        }
    }
}
}