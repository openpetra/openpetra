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

            FileStream fs;

            try
            {
                fs = fileinfo.OpenRead();
            }
            catch (Exception)
            {
                return ADefaultEncoding;  // This prevents an exception at this point.
            }                             // If your client code expected an exception, you should examine the return value instead.

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

        /// <summary>
        /// Function to detect the encoding for UTF-7, UTF-8/16/32 (bom, no bom, little or big endian), and local default codepage
        /// </summary>
        /// <param name="AFilename">Filename to open</param>
        /// <param name="AText">The content of the file using the discovered encoding.</param>
        /// <param name="AEncoding">The discovered encoding.  One of the six UTF options or the user's default ANSI code page.</param>
        /// <param name="AHasBOM">Will be set to true if the file has a BOM header</param>
        /// <param name="AIsAmbiguousUTF">Will be set to true if the text that the method outputs is ambiguous.  This will be true if
        ///  the determination of the UTF format was statistically not significant</param>
        /// <param name="ARawBytes">The raw bytes read from the file.  This can be used to display alternative text for ambiguous options</param>
        /// <param name="ATestByteCount">Number of bytes to check of the file (to save processing).
        /// Higher value is slower, but more reliable (especially UTF-8 with special characters later on may appear to be ASCII initially).
        /// If ATestByteCount = 0, then ATestByteCount becomes the length of the file (for maximum reliability).</param>
        /// <returns>True if the file was opened and read successfully.  False if the file could not be opened or if the file is empty.</returns>
        public static bool AutoDetectTextEncodingAndOpenFile(string AFilename,
            out String AText,
            out Encoding AEncoding,
            out bool AHasBOM,
            out bool AIsAmbiguousUTF,
            out byte[] ARawBytes,
            int ATestByteCount = 0)
        {
            // This algorithm is based on http://stackoverflow.com/questions/1025332/determine-a-strings-encoding-in-c-sharp
            // AlanP implemented this method May 2016
            // There are a number of similar algorithms on other pages of the internet...
            AText = string.Empty;
            AEncoding = null;
            AHasBOM = false;
            AIsAmbiguousUTF = false;
            ARawBytes = null;

            byte[] b;

            try
            {
                // This static method does not need to be closed and will Dispose of resources automatically
                b = File.ReadAllBytes(AFilename);

                if ((b == null) || (b.Length == 0))
                {
                    throw new Exception("The file is empty");
                }
            }
            catch (Exception ex)
            {
                TLogging.Log(string.Format("Could not open file: {0}   Exception message was {1}", AFilename, ex.Message));
                return false;
            }

            ARawBytes = b;

            // First check the low hanging fruit by checking if a BOM/signature exists (sourced from http://www.unicode.org/faq/utf_bom.html#bom4)
            AHasBOM = true;

            if ((b.Length >= 4) && (b[0] == 0x00) && (b[1] == 0x00) && (b[2] == 0xFE) && (b[3] == 0xFF))
            {
                AText = Encoding.GetEncoding("utf-32BE").GetString(b, 4, b.Length - 4);
                AEncoding = Encoding.GetEncoding("utf-32BE");
                return true;
            }  // UTF-32, big-endian
            else if ((b.Length >= 4) && (b[0] == 0xFF) && (b[1] == 0xFE) && (b[2] == 0x00) && (b[3] == 0x00))
            {
                AText = Encoding.UTF32.GetString(b, 4, b.Length - 4);
                AEncoding = Encoding.UTF32;
                return true;
            }  // UTF-32, little-endian
            else if ((b.Length >= 2) && (b[0] == 0xFE) && (b[1] == 0xFF))
            {
                AText = Encoding.BigEndianUnicode.GetString(b, 2, b.Length - 2);
                AEncoding = Encoding.BigEndianUnicode;
                return true;
            }  // UTF-16, big-endian
            else if ((b.Length >= 2) && (b[0] == 0xFF) && (b[1] == 0xFE))
            {
                AText = Encoding.Unicode.GetString(b, 2, b.Length - 2);
                AEncoding = Encoding.Unicode;
                return true;
            }  // UTF-16, little-endian
            else if ((b.Length >= 3) && (b[0] == 0xEF) && (b[1] == 0xBB) && (b[2] == 0xBF))
            {
                AText = Encoding.UTF8.GetString(b, 3, b.Length - 3);
                AEncoding = Encoding.UTF8;
                return true;
            }  // UTF-8
            else if ((b.Length >= 3) && (b[0] == 0x2b) && (b[1] == 0x2f) && (b[2] == 0x76))
            {
                AText = Encoding.UTF7.GetString(b, 3, b.Length - 3);
                AEncoding = Encoding.UTF7;
                return true;
            }  // UTF-7

            // So the file did NOT have a BOM
            AHasBOM = false;
            AIsAmbiguousUTF = true;

            // If the code reaches here, no BOM/signature was found, so now we need to 'taste' the file to see if can manually discover
            // the encoding. A high test byte count value is desired for UTF-8
            if ((ATestByteCount == 0) || (ATestByteCount > b.Length))
            {
                ATestByteCount = b.Length;    // ATestByteCount size can't be bigger than the filesize obviously.
            }

            // Some text files are encoded in UTF8, but have no BOM/signature. Hence the below manually checks for a UTF8 pattern.
            // This code is based off the top answer at: http://stackoverflow.com/questions/6555015/check-for-invalid-utf8
            // An alternative stricter (and terser/slower) implementation is shown at:
            //   http://stackoverflow.com/questions/1031645/how-to-detect-utf-8-in-plain-c
            // It is more useful for checking that the UTF-8 is 'well formed'
            // Using the first method, false positives should be exceedingly rare and would be either slightly malformed UTF-8
            // (which would suit our purposes anyway) or 8-bit extended ASCII/UTF-16/32 at a vanishingly long shot.
            // [Sadly UTF-8 can always be mistaken for 2-byte Chinese or other Asian characters.]
            int i = 0;
            int utfCpCount = 0;
            bool utf8 = false;

            while (i < ATestByteCount - 4)
            {
                if (b[i] <= 0x7F)
                {
                    i += 1;
                    continue;
                }     // If all characters are below 0x80, then it is valid UTF8, but UTF8 is not 'required'

                // (and therefore the text is more desirable to be treated as the AFallbackEncoding codepage of the computer).
                // Hence, there's no "utf8 = true;" code unlike the next three checks.

                if ((b[i] >= 0xC2) && (b[i] <= 0xDF) && (b[i + 1] >= 0x80) && (b[i + 1] < 0xC0))
                {
                    i += 2;
                    utf8 = true;
                    utfCpCount++;
                    continue;
                }

                if ((b[i] >= 0xE0) && (b[i] <= 0xF0) && (b[i + 1] >= 0x80) && (b[i + 1] < 0xC0) && (b[i + 2] >= 0x80) && (b[i + 2] < 0xC0))
                {
                    i += 3;
                    utf8 = true;
                    utfCpCount++;
                    continue;
                }

                if ((b[i] >= 0xF0) && (b[i] <= 0xF4) && (b[i + 1] >= 0x80) && (b[i + 1] < 0xC0) && (b[i + 2] >= 0x80) && (b[i + 2] < 0xC0)
                    && (b[i + 3] >= 0x80) && (b[i + 3] < 0xC0))
                {
                    i += 4;
                    utf8 = true;
                    utfCpCount++;
                    continue;
                }

                utf8 = false;
                break;
            }

            if (utf8 == true)
            {
                // Is ambiguous depends on statistics.  We invent our own values for this.  Provided we read the whole file the statistics are simple.
                AIsAmbiguousUTF = utfCpCount <= 2;      // ambiguous if we only find two examples?

                AText = Encoding.UTF8.GetString(b);
                AEncoding = Encoding.UTF8;
                return true;
            }

            // The next check is a heuristic attempt to detect UTF-16 without a BOM.
            // We simply look for zeroes in odd or even byte places, and if a certain threshold is reached, the code is 'probably' UTF-16.
            double threshold = 0.1; // proportion of chars step 2 which must be zeroed to be diagnosed as utf-16. 0.1 = 10%
            int count = 0;

            for (int n = 0; n < ATestByteCount; n += 2)
            {
                if (b[n] == 0)
                {
                    count++;
                }
            }

            if (((double)count) / ATestByteCount > threshold)
            {
                AText = Encoding.BigEndianUnicode.GetString(b);
                AEncoding = Encoding.BigEndianUnicode;
                AIsAmbiguousUTF = false;
                return true;
            }

            count = 0;

            for (int n = 1; n < ATestByteCount; n += 2)
            {
                if (b[n] == 0)
                {
                    count++;
                }
            }

            if (((double)count) / ATestByteCount > threshold)
            {
                AText = Encoding.Unicode.GetString(b);
                AEncoding = Encoding.Unicode;
                AIsAmbiguousUTF = false;
                return true;
            } // (little-endian)

            // If all else fails, the encoding is probably (though certainly not definitely) the user's local codepage!
            // One might present to the user a list of alternative encodings as shown here:
            //   http://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language
            // A full list can be found using Encoding.GetEncodings();

            AText = Encoding.Default.GetString(b);
            AEncoding = Encoding.Default;

            return true;
        }
    }
}