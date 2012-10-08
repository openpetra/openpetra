//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Threading;
using Ict.Common;
using ICSharpCode.SharpZipLib.GZip;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// a memory conscious class to parse CSV files
    /// </summary>
    public class CSVFile
    {
        private StreamReader FReader;
        private char FSeparator;
        private int FCurrentPosition;
        private int FCurrentLength;
        private int FColumnCount;

        /// <summary>
        /// the current line
        /// </summary>
        public string FCurrentLine;
        /// <summary>
        /// list of the values for the current row
        /// </summary>
        public string[] FCurrentRow;
        /// <summary>
        /// the current line. as it is seen in the file, not considering logical lines (which can go across several lines)
        /// </summary>
        public long FRealLineCounter;

        /// <summary>
        /// constructor
        /// </summary>
        public CSVFile(StreamReader AReader, int AColumnCount, char ASeparator)
        {
            FReader = AReader;
            FColumnCount = AColumnCount;
            FSeparator = ASeparator;
            FRealLineCounter = 0;
        }

        private bool GetNextCSV()
        {
            FCurrentLength = 0;

            if (FCurrentPosition > FCurrentLine.Length)
            {
                return false;
            }

            bool escape = false;
            int position = FCurrentPosition;

            if (FCurrentLine[position] != FSeparator)
            {
                if (FCurrentLine[position] == '"')
                {
                    int QuotedStringLength = (StringHelper.FindMatchingQuote(FCurrentLine, position) - position);

                    position += QuotedStringLength + 2;
                    FCurrentLength += QuotedStringLength + 2;
                }
                else
                {
                    while (position < FCurrentLine.Length)
                    {
                        if (escape)
                        {
                            escape = false;
                        }
                        else
                        {
                            if (FCurrentLine[position] == '\\')
                            {
                                escape = true;
                                position++;
                                FCurrentLength++;
                            }
                        }

                        position++;
                        FCurrentLength++;

                        if (!escape && (position < FCurrentLine.Length) && (FCurrentLine[position] == FSeparator))
                        {
                            // found the next separator
                            break;
                        }
                    }
                }
            }

            return true;
        }

        private string GetCurrentValue()
        {
            string value;

            if (FCurrentPosition == FCurrentLine.Length)
            {
                // line ends with the separator. empty value at the end
                value = string.Empty;
            }
            else if (FCurrentLine[FCurrentPosition] == '"')
            {
                value = FCurrentLine.Substring(FCurrentPosition + 1, FCurrentLength - 2);
            }
            else
            {
                value = FCurrentLine.Substring(FCurrentPosition, FCurrentLength);
            }

            if (value.Contains("\"\""))
            {
                value = value.Replace("\"\"", "\"");
            }

            if (TLogging.DebugLevel >= 20)
            {
                TLogging.Log("parsed value " + value);
            }

            return value;
        }

        /// <summary>
        /// get the next row
        /// </summary>
        /// <returns></returns>
        public bool GetNextRow()
        {
            if (FReader.EndOfStream)
            {
                return false;
            }

            FCurrentRow = new string[FColumnCount];

            FCurrentLine = FReader.ReadLine();
            FRealLineCounter++;
            FCurrentPosition = 0;
            int CurrentColumn = 0;

            while (true)
            {
                try
                {
                    if (!GetNextCSV())
                    {
                        break;
                    }

                    if (CurrentColumn >= FColumnCount)
                    {
                        throw new Exception(
                            String.Format("Line {0}: Invalid number of columns, should be {1} but there are more columns.",
                                FRealLineCounter,
                                FColumnCount));
                    }

                    FCurrentRow[CurrentColumn] = GetCurrentValue();
                    FCurrentPosition += FCurrentLength + 1;
                    CurrentColumn++;
                }
                catch (System.IndexOutOfRangeException)
                {
                    // could use StringBuilder, but not sure if that helps here enough
                    FCurrentLine += "\n" + FReader.ReadLine();
                    FRealLineCounter++;

                    if (TLogging.DebugLevel == 10)
                    {
                        TLogging.Log("adding next line: " + FCurrentLine.ToString());
                    }
                }
            }

            // last line has just a dot
            if ((CurrentColumn != 1) && (CurrentColumn != FColumnCount))
            {
                throw new Exception(
                    String.Format("Line {0}: Invalid number of columns, should be {1} but there are only {2} columns.",
                        FRealLineCounter,
                        FColumnCount,
                        CurrentColumn));
            }

            return true;
        }

        private static string delim = "\t";

        /// <summary>
        /// concatenate a string using the tabulator as delimiter. slightly different from StringHelper.StrMerge
        /// </summary>
        /// <param name="l">the string array containing the strings that should be concatenated</param>
        /// <returns>a string with the concatenated strings from the string array</returns>
        public static StringBuilder StrMergeSpecial(String[] l)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i <= l.Length - 1; i += 1)
            {
                if (i != 0)
                {
                    builder.Append(delim);
                }

                StringBuilder sb = new StringBuilder(l[i]);

                if (l[i].Contains("\\"))
                {
                    // avoid Postgresql load error: ERROR:  invalid byte sequence for encoding "UTF8": 0x80
                    sb.Replace("\\", "\\\\");
                    sb.Replace("\\\\N", "\\N");
                }

                // if the element already contains the delimiter, do something about it.
                // strsplit and getNextCSV have to revert it
                if (l[i].Contains(delim))
                {
                    sb.Replace(delim, "\\t");
                }

                builder.Append(sb.ToString());
            }

            return builder;
        }
    }

    /// <summary>
    /// parse the dump file from Progress which is basically a CSV file
    /// </summary>
    public class TParseProgressCSV
    {
        private static Encoding ProgressFileEncoding;

        /// <summary>
        /// init the codepage/encoding for the Progress CSV files
        /// </summary>
        public static void InitProgressCodePage()
        {
            string ProgressCodepage =
                TAppSettingsManager.GetValue("CodePage", Environment.GetEnvironmentVariable("PROGRESS_CP"));

            try
            {
                ProgressFileEncoding = Encoding.GetEncoding(Convert.ToInt32(ProgressCodepage));
            }
            catch
            {
                ProgressFileEncoding = Encoding.GetEncoding(ProgressCodepage);
            }
        }

        private Stream fs;
        private GZipInputStream gzipStream;
        private StreamReader MyReader;
        private CSVFile csvfile;
        private int FColumnCount;

        /// <summary>
        /// parse a CSV file that was dumped by Progress.
        /// that is the fastest way of dumping the data, but it is not ready for being imported into PostgreSQL.
        /// </summary>
        /// <param name="AInputFileDGz">the path to a gzipped csv file</param>
        /// <param name="AColumnCount">for checking the number of columns</param>
        public TParseProgressCSV(string AInputFileDGz, int AColumnCount)
        {
            fs = new FileStream(AInputFileDGz, FileMode.Open, FileAccess.Read);
            gzipStream = new GZipInputStream(fs);
            MyReader = new StreamReader(gzipStream, ProgressFileEncoding);

            csvfile = new CSVFile(MyReader, AColumnCount, ' ');
            FColumnCount = AColumnCount;
        }

        /// <summary>
        /// read the next (logical) row, ie. record of data
        /// </summary>
        /// <returns></returns>
        public string[] ReadNextRow()
        {
            try
            {
                if (csvfile.GetNextRow())
                {
                    if (csvfile.FCurrentRow[0] == ".")
                    {
                        // we have parsed all the data
                        return null;
                    }

                    for (int countColumn = 0; countColumn < FColumnCount; countColumn++)
                    {
                        if (csvfile.FCurrentRow[countColumn].Contains("\n")
                            || csvfile.FCurrentRow[countColumn].Contains("\r"))
                        {
                            csvfile.FCurrentRow[countColumn] = csvfile.FCurrentRow[countColumn].Replace("\n", "\\n").Replace("\r", "\\r");
                        }
                        else
                        {
                            if (csvfile.FCurrentRow[countColumn] == "?")
                            {
                                // NULL
                                csvfile.FCurrentRow[countColumn] = "\\N";
                            }
                        }

                        if (TLogging.DebugLevel == 10)
                        {
                            TLogging.Log("Parsed value: " + csvfile.FCurrentRow[countColumn]);
                        }
                    }

                    if ((TLogging.DebugLevel > 0) && (csvfile.FRealLineCounter % 500000 == 0))
                    {
                        TLogging.Log(csvfile.FRealLineCounter.ToString() + " " + (GC.GetTotalMemory(false) / 1024 / 1024).ToString() + " MB");
                    }

                    return csvfile.FCurrentRow;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                TLogging.Log(csvfile.FCurrentLine);
                TLogging.Log("Problem parsing file, in line " + csvfile.FRealLineCounter.ToString());
                throw;
            }
        }
    }
}