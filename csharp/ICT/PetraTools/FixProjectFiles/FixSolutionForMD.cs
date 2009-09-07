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
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Tools.CodeGeneration;

namespace FixProjectFiles
{
/// <summary>
/// it seems that MonoDevelop/mdtool does not understand the SharpDevelop solution files because of ordering of ActiveCfg and Build
/// </summary>
public class TFixSolutionForMD
{
    /// <summary>
    /// make sure that there is first ActiveCfg, then Build
    /// </summary>
    /// <param name="AFilename"></param>
    public static void FixSolution(string AFilename)
    {
        StreamReader reader = new StreamReader(AFilename);
        StreamWriter writer = new StreamWriter(AFilename + ".new");

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();

            if (line.StartsWith("Microsoft Visual Studio Solution File"))
            {
                // monodevelop has problems with format 10.00 of vs 2008
                writer.WriteLine("Microsoft Visual Studio Solution File, Format Version 9.00");
            }
            else if (line.StartsWith("# Visual Studio "))
            {
                writer.WriteLine("# Visual Studio 2005");
            }
            else if (line.Contains("Any CPU.ActiveCfg"))
            {
                // correct order
                writer.WriteLine(line);
                line = reader.ReadLine();
                writer.WriteLine(line);
            }
            else if (line.Contains("Any CPU.Build"))
            {
                // wrong order, this will not work for md tool
                writer.WriteLine(reader.ReadLine());
                writer.WriteLine(line);
            }
            else
            {
                writer.WriteLine(line.Replace("/", "\\"));
            }
        }

        reader.Close();
        writer.Close();

        if (TTextFile.UpdateFile(AFilename))
        {
            Console.WriteLine("wrote file: " + AFilename);
        }
    }
}
}