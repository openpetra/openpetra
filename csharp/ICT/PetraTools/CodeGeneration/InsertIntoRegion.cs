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
using System.Collections.Specialized;
using System.IO;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Tools.CodeGeneration
{
    /// <summary>
    /// insert auto generated code into a region
    /// </summary>
    public class TInsertIntoRegion
    {
        /// <summary>
        /// modify the given file, and replace the content of the region with the new content
        /// </summary>
        /// <param name="AFilename"></param>
        /// <param name="ARegionName"></param>
        /// <param name="AInsertCode"></param>
        /// <returns></returns>
        public static bool InsertIntoRegion(string AFilename, string ARegionName, string AInsertCode)
        {
            try
            {
                StreamReader sr = new StreamReader(AFilename);
                StreamWriter sw = new StreamWriter(AFilename + ".new");

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (line.ToLower().Contains("#region " + ARegionName.ToLower()))
                    {
                        sw.WriteLine(line);
                        sw.WriteLine(AInsertCode.Replace("INDENT", "".PadLeft(line.IndexOf("#"))));

                        while (!sr.EndOfStream && !line.ToLower().Contains("#endregion"))
                        {
                            line = sr.ReadLine();
                        }
                    }

                    sw.WriteLine(line);
                }

                sr.Close();
                sw.Close();

                TTextFile.UpdateFile(AFilename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error InsertIntoRegion: " + e.Message);
                return false;
            }
            return true;
        }
    }
}