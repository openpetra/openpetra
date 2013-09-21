//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Text;
using System.Collections.Generic;

namespace Ict.Common.IO
{
    /// <summary>
    /// delegate to apply an operation on the file
    /// </summary>
    public delegate void ProcessFileType(string filename);

    /// Some useful functions for dealing with text files;
    /// only used for BuildTools at the moment
    public class TTextFile
    {
        /// todo: exclude directory names, e.g. CSV, see BuildTools\ProgressConverter\AnalyseProgressFiles.cs
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

        /// remove carriage return
        public static void Dos2Unix(String filename)
        {
            StreamReader sr = new StreamReader(filename);
            string lines = sr.ReadToEnd();
            Encoding oldEncoding = sr.CurrentEncoding;

            sr.Close();
            StreamWriter sw = new StreamWriter(filename, false, oldEncoding);
            sw.Write(lines.Replace("\r", ""));
            sw.Close();
        }

        /// add carriage return
        public static void Unix2Dos(String filename)
        {
            StreamReader sr = new StreamReader(filename);
            string lines = sr.ReadToEnd();
            Encoding oldEncoding = sr.CurrentEncoding;

            sr.Close();
            StreamWriter sw = new StreamWriter(filename, false, oldEncoding);
            sw.Write(lines.Replace("\r", "").Replace("\n", "\r\n"));
            sw.Close();
        }

        /// <summary>
        /// convert a text file from a given code page to Unicode
        /// </summary>
        /// <param name="AFilename"></param>
        /// <param name="AEncodingCodePage"></param>
        public static void ConvertToUnicode(String AFilename, String AEncodingCodePage)
        {
            Encoding SourceEncoding = Encoding.Default;

            try
            {
                SourceEncoding = Encoding.GetEncoding(Convert.ToInt32(AEncodingCodePage));
            }
            catch (Exception)
            {
                SourceEncoding = Encoding.GetEncoding(AEncodingCodePage);
            }

            StreamReader reader = new StreamReader(AFilename, SourceEncoding);
            string Content = reader.ReadToEnd();
            reader.Close();
            StreamWriter writer = new StreamWriter(AFilename, false, Encoding.Unicode);
            writer.Write(Content);
            writer.Close();
        }

        /// <summary>
        /// replace given strings
        /// </summary>
        private static string ReplaceStrings(string ALine, SortedList <string, string>AToReplace)
        {
            if (AToReplace == null)
            {
                return ALine;
            }

            foreach (string key in AToReplace.Keys)
            {
                ALine = ALine.Replace(key, AToReplace[key]);
            }

            return ALine;
        }

        /// <summary>
        /// check if the two text files have the same content
        /// </summary>
        public static bool SameContent(String filename1,
            String filename2,
            bool AIgnoreNewLine = true,
            SortedList <string, string>AToReplace = null,
            bool AWithLogging = false)
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

                if (AWithLogging)
                {
                    TLogging.Log("TTextFile.SameContent: the files does not exist: " +
                        (System.IO.File.Exists(filename1) ? filename2 : filename1));
                }

                return false;
            }

            sr1 = new StreamReader(filename1);
            sr2 = new StreamReader(filename2);

            if (!AIgnoreNewLine)
            {
                if (sr1.CurrentEncoding != sr2.CurrentEncoding)
                {
                    return false;
                }

                // compare the length only when you want the line endings to be the same.
                // otherwise the length would be different anyways
                if (sr1.BaseStream.Length != sr2.BaseStream.Length)
                {
                    sr1.Close();
                    sr2.Close();
                    return false;
                }

                line = ReplaceStrings(sr1.ReadToEnd(), AToReplace);
                line2 = ReplaceStrings(sr2.ReadToEnd(), AToReplace);
                sr1.Close();
                sr2.Close();
                return line == line2;
            }

            line = "start";
            line2 = "start";

            int linecounter = 0;

            while (true)
            {
                linecounter++;
                line = ReplaceStrings(sr1.ReadLine(), AToReplace);
                line2 = ReplaceStrings(sr2.ReadLine(), AToReplace);

                if (line.CompareTo(line2) != 0)
                {
                    if (AWithLogging)
                    {
                        TLogging.Log("TTextFile.SameContent: files are different, at line " + linecounter.ToString());
                        TLogging.Log("TTextFile.SameContent:  first file: " + line);
                        TLogging.Log("TTextFile.SameContent: second file: " + line2);
                    }

                    sr1.Close();
                    sr2.Close();
                    return false;
                }

                // test for end of file
                if (sr1.EndOfStream || sr2.EndOfStream)
                {
                    if (sr1.EndOfStream == sr2.EndOfStream)
                    {
                        sr1.Close();
                        sr2.Close();
                        return true;
                    }
                    else
                    {
                        if (AWithLogging)
                        {
                            TLogging.Log("TTextFile.SameContent: expected more lines; " +
                                (sr2.EndOfStream ? filename1 : filename2) +
                                " is longer than " + (sr2.EndOfStream ? filename2 : filename1));
                        }

                        sr1.Close();
                        sr2.Close();
                        return false;
                    }
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
        /// <param name="AIgnoreNewLine">should ignore line break character differences</param>
        /// <returns></returns>
        public static bool UpdateFile(String AOrigFilename, bool AIgnoreNewLine)
        {
            string NewFilename = AOrigFilename + ".new";

            if (SameContent(AOrigFilename, NewFilename, AIgnoreNewLine) == true)
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
                    TFileHelper.MoveToBackup(AOrigFilename);
                }

                System.IO.File.Move(NewFilename, AOrigFilename);
            }

            return true;
        }

        /// <summary>
        /// this will compare the original file with the file that has the same name but an extension .new additionally at the end
        /// if the files have identical content, the new file is dropped
        /// otherwise the original file is backed up, and the new file is renamed to the original file name
        ///
        /// the intention is to generate code, but not to touch it for VCS if not necessary.
        ///
        /// this overload will compare line endings as well.
        /// </summary>
        /// <param name="AOrigFilename"></param>
        /// <returns></returns>
        public static bool UpdateFile(String AOrigFilename)
        {
            return UpdateFile(AOrigFilename, false);
        }

        /// StreamReader DetectEncodingFromByteOrderMarks does not work for ANSI?
        /// therefore we have to detect the encoding by comparing the first bytes of the file
        public static Encoding GetFileEncoding(String AFilename, Encoding ADefaultEncoding = null)
        {
            FileInfo fileinfo = new FileInfo(AFilename);

            FileStream fs = fileinfo.OpenRead();

            Encoding[] UnicodeEncodings =
            {
                Encoding.BigEndianUnicode, Encoding.Unicode, Encoding.UTF8, Encoding.UTF32
            };

            foreach (Encoding testEncoding in UnicodeEncodings)
            {
                byte[] encodingpreamble = testEncoding.GetPreamble();
                byte[] filepreamble = new byte[encodingpreamble.Length];
                fs.Position = 0;
                bool equal = (fs.Read(filepreamble, 0, encodingpreamble.Length) == encodingpreamble.Length);

                for (int i = 0; equal && i < encodingpreamble.Length; i++)
                {
                    equal = (encodingpreamble[i] == filepreamble[i]);
                }

                if (equal)
                {
                    fs.Close();
                    return testEncoding;
                }
            }

            if (fs != null)
            {
                fs.Close();
            }

            if (ADefaultEncoding == null)
            {
                return Encoding.Default;
            }

            return ADefaultEncoding;
        }
    }
}