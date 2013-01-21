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
using Ict.Common;

namespace Ict.Tools.ConvertExtToCSV
{
    class Program
    {
        public static void Main(string[] args)
        {
            new TAppSettingsManager(false);

            try
            {
                string filename = TAppSettingsManager.GetValue("extfile");
                string newCSVFilename = Path.GetDirectoryName(filename) +
                                        Path.DirectorySeparatorChar +
                                        Path.GetFileNameWithoutExtension(filename) +
                                        ".csv";

                StreamReader sr = new StreamReader(filename);
                StreamWriter sw = new StreamWriter(newCSVFilename);

                string newCSVLine = String.Empty;
                bool recordData = false;
                bool cleanFileEnd = false;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (line.StartsWith("\"APPL-FORM\""))
                    {
                        recordData = false;
                    }

                    if (line.Trim() == "\"END\"")
                    {
                        sw.WriteLine(newCSVLine);
                        newCSVLine = string.Empty;
                        recordData = false;
                    }

                    if (recordData)
                    {
                        while (line.Length > 0)
                        {
                            try
                            {
                                newCSVLine = StringHelper.AddCSV(newCSVLine, StringHelper.GetNextCSV(ref line, "  "));
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                // we have the case that a string contains new line characters, so the ending quote cannot be found in the current line
                                line += " " + sr.ReadLine();
                            }
                        }
                    }

                    if (line.Trim() == "\"PARTNER\"")
                    {
                        recordData = true;
                    }

                    if (line.Trim() == "\"END\"  \"FORMS\"")
                    {
                        recordData = true;
                    }

                    if (line.Trim() == "0  \"FINISH\"")
                    {
                        cleanFileEnd = true;
                    }
                }

                if (!cleanFileEnd)
                {
                    Console.WriteLine("Your file " + filename + " is broken, it does not have the correct finish line. Please export again");
                }

                Console.WriteLine("File " + newCSVFilename + " has been written.");

                sw.Close();
                sr.Close();
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