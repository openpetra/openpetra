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
using System.IO;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;

namespace GenerateI18N
{
class Program
{
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
            else if (settings.HasValue("solution"))
            {
                TDataDefinitionStore store = new TDataDefinitionStore();
                Console.WriteLine("parsing " + settings.GetValue("petraxml", true));
                TDataDefinitionParser parser = new TDataDefinitionParser(settings.GetValue("petraxml", true));
                parser.ParseDocument(ref store, true, true);

                string solutionFilename = settings.GetValue("solution");
                string GettextFilename = Path.GetDirectoryName(solutionFilename) +
                                         Path.DirectorySeparatorChar +
                                         Path.GetFileNameWithoutExtension(solutionFilename) +
                                         ".CollectedGettext.cs";
                StreamWriter writerGettextFile = new StreamWriter(GettextFilename);

                TCSProjTools projTools = new TCSProjTools();
                StringCollection pathsProjectFiles = projTools.LoadGUIDsFromSolution(solutionFilename);

                foreach (string pathProjectFile in pathsProjectFiles)
                {
                    Console.WriteLine(pathProjectFile);
                    StringCollection codeFilePaths = TCSProjTools.LoadCodeFilesFromProject(pathProjectFile);

                    foreach (string pathCodeFile in codeFilePaths)
                    {
                        TGenerateCatalogStrings.Execute(pathCodeFile, store, writerGettextFile);
                    }
                }

                if (settings.GetValue("solution").Contains("Client.sln"))
                {
                    TGenerateCatalogStrings.AddTranslationUINavigation(settings.GetValue("UINavigation.File"), writerGettextFile);
                }

                writerGettextFile.Close();

                // delete the file if it is empty
                if (File.ReadAllText(GettextFilename).Length == 0)
                {
                    File.Delete(GettextFilename);
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