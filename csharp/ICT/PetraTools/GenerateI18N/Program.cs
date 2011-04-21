//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;

namespace GenerateI18N
{
class Program
{
    private static void ParseWithGettext(string AGettextApp, string APoFile, string AListOfFilesToParse)
    {
        System.Diagnostics.Process GettextProcess;
        GettextProcess = new System.Diagnostics.Process();
        GettextProcess.EnableRaisingEvents = false;
        GettextProcess.StartInfo.FileName = AGettextApp;
        GettextProcess.StartInfo.Arguments = String.Format(
            "-j --add-comments=/// --no-location --from-code=UTF-8 {0} -o \"{1}\"",
            AListOfFilesToParse, APoFile);
        GettextProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        GettextProcess.EnableRaisingEvents = true;
        try
        {
            if (!GettextProcess.Start())
            {
                throw new Exception("cannot start gettext");
            }
        }
        catch (Exception)
        {
            TLogging.Log("Cannot start external gettext program. Is it on the path?");
            TLogging.Log("Arguments: " + GettextProcess.StartInfo.Arguments);
            throw new Exception("Problem running gettext");
        }

        while ((!GettextProcess.HasExited))
        {
            Thread.Sleep(100);
        }
    }

    public static void Main(string[] args)
    {
        TAppSettingsManager settings = new TAppSettingsManager(false);

        try
        {
            if (settings.HasValue("do") && (settings.GetValue("do") == "removeDoNotTranslate"))
            {
                string doNotTranslatePath = settings.GetValue("dntFile");
                string poFilePath = settings.GetValue("poFile");

                // remove all strings from po file that are listed in the "Do Not Translate" file
                TDropUnwantedStrings.RemoveUnwantedStringsFromTranslation(doNotTranslatePath, poFilePath);
            }
            else if (settings.HasValue("file"))
            {
                TGenerateCatalogStrings.Execute(settings.GetValue("file"), null, null);
            }
            else if (settings.HasValue("filelist"))
            {
                TDataDefinitionStore store = new TDataDefinitionStore();
                Console.WriteLine("parsing " + settings.GetValue("petraxml", true));
                TDataDefinitionParser parser = new TDataDefinitionParser(settings.GetValue("petraxml", true));
                parser.ParseDocument(ref store, true, true);

                string CollectedStringsFilename = settings.GetValue("tmpPath") +
                                                  Path.DirectorySeparatorChar +
                                                  "GenerateI18N.CollectedGettext.cs";
                StreamWriter writerCollectedStringsFile = new StreamWriter(CollectedStringsFilename);

                string GettextApp = settings.GetValue("gettext");

                string filesToParseWithGettext = string.Empty;
                StreamReader readerFilelist = new StreamReader(settings.GetValue("filelist"));

                while (!readerFilelist.EndOfStream)
                {
                    string pathCodeFile = readerFilelist.ReadLine().Trim();
                    string ext = Path.GetExtension(pathCodeFile);

                    if (".cs" == ext)
                    {
                        if (TGenerateCatalogStrings.Execute(pathCodeFile, store, writerCollectedStringsFile))
                        {
                            filesToParseWithGettext += "\"" + pathCodeFile + "\" ";

                            if (filesToParseWithGettext.Length > 1500)
                            {
                                ParseWithGettext(GettextApp, settings.GetValue("poFile"), filesToParseWithGettext);
                                filesToParseWithGettext = string.Empty;
                            }
                        }
                    }
                    else if (".yml" == ext)
                    {
                        TGenerateCatalogStrings.AddTranslationUINavigation(pathCodeFile, writerCollectedStringsFile);
                    }
                    else
                    {
                        Console.WriteLine("the file " + pathCodeFile + " has an unknown extension! File ignored!");
                    }
                }

                if (filesToParseWithGettext.Length > 0)
                {
                    ParseWithGettext(GettextApp, settings.GetValue("poFile"), filesToParseWithGettext);
                }

                writerCollectedStringsFile.Close();

                // delete the file if it is empty
                if (File.ReadAllText(CollectedStringsFilename).Length == 0)
                {
                    File.Delete(CollectedStringsFilename);
                }
            }
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