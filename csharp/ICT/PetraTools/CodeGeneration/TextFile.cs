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
using System.IO;

namespace Ict.Tools.CodeGeneration
{
    public delegate void ProcessFileType(string filename);

    /// Some useful functions for dealing with text files;
    /// only used for PetraTools at the moment
    public class TTextFile
    {
        // todo: exclude directory names, e.g. CSV, see PetraTools\ProgressConverter\AnalyseProgressFiles.cs
        public static void RecurseFilesAndDirectories(string APath, string AExt, ProcessFileType func)
        {
            string[] listOfDirectories = System.IO.Directory.GetDirectories(APath);

            foreach (string directoryName in listOfDirectories)
            {
                // recursive call
                Console.WriteLine(directoryName);
                RecurseFilesAndDirectories(directoryName, AExt, func);
            }

            string[] listOfFiles = System.IO.Directory.GetFiles(APath, AExt);

            foreach (string fileName in listOfFiles)
            {
                func(fileName);
            }
        }

        public static bool SameContent(String filename1, String filename2)
        {
            StreamReader sr1;
            StreamReader sr2;
            String line;
            String line2;

            if (((!System.IO.File.Exists(filename1))) || ((!System.IO.File.Exists(filename2))))
            {
                if (((!System.IO.File.Exists(filename1))) && ((!System.IO.File.Exists(filename2))))
                {
                    return true;
                }

                return false;
            }

            sr1 = new StreamReader(filename1);
            sr2 = new StreamReader(filename2);

            /* don't compare the length, there might be different line endings
             * if (sr1.BaseStream.Length <> sr2.BaseStream.Length) then
             * begin
             * sr1.Close();
             * sr2.Close();
             * result := false;
             * exit;
             * end;
             */
            line = "start";
            line2 = "start";

            while (true)
            {
                line = sr1.ReadLine();
                line2 = sr2.ReadLine();

                // test for end of file
                if ((sr1.Peek() < 0) && (sr2.Peek() < 0))
                {
                    sr1.Close();
                    sr2.Close();
                    return true;
                }

                if (((sr1.Peek() < 0) && (sr2.Peek() >= 0)) || ((sr1.Peek() >= 0) && (sr2.Peek() < 0)) || (line.CompareTo(line2) != 0))
                {
                    sr1.Close();
                    sr2.Close();
                    return false;
                }
            }
        }

        /// <summary>
        /// this will compare the original file with the file that has the same name but an extension .new additionally at the end
        /// if the files have identical content, the new file is dropped
        /// otherwise the original file is backed up, and the new file is renamed to the original file name
        ///
        /// the intention is to generate code, but not to touch it for VCS if not necessary
        /// </summary>
        /// <param name="AOrigFilename">the original name of the file</param>
        /// <returns></returns>
        public static bool UpdateFile(String AOrigFilename)
        {
            string NewFilename = AOrigFilename + ".new";

            if (SameContent(AOrigFilename, NewFilename) == true)
            {
                System.IO.File.Delete(NewFilename);
                return false;
            }
            else
            {
                if (System.IO.File.Exists(AOrigFilename))
                {
                    System.IO.File.SetAttributes(AOrigFilename, FileAttributes.Normal);
                }

                if (System.IO.File.Exists(AOrigFilename))
                {
                    // create backup of original file
                    int backupnr = 0;

                    while (File.Exists(AOrigFilename + "." + backupnr.ToString() + ".bak"))
                    {
                        backupnr++;
                    }

                    File.Copy(AOrigFilename, AOrigFilename + "." + backupnr.ToString() + ".bak");

                    // delete original file
                    System.IO.File.Delete(AOrigFilename);
                }

                System.IO.File.Move(NewFilename, AOrigFilename);
            }

            return true;
        }
    }
}