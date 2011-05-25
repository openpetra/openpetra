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

namespace FixProjectFiles
{
class Program
{
    public static void Main(string[] args)
    {
        new TAppSettingsManager(false);

        try
        {
            TFixProjectReferences FixReferences = new TFixProjectReferences();
            string SolutionPath = TAppSettingsManager.GetValue("solutionpath", true);
            StringCollection SolutionFiles = StringHelper.StrSplit(TAppSettingsManager.GetValue("solutions", true), ",");

            foreach (string solutionFilename in SolutionFiles)
            {
                TFixSolutionForMD.FixSolution(SolutionPath + Path.DirectorySeparatorChar + solutionFilename + ".sln");
                FixReferences.FixAllProjectFiles(SolutionPath + Path.DirectorySeparatorChar + solutionFilename + ".sln");
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