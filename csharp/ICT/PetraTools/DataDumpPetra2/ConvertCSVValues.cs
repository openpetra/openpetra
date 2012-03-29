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

            if (value.IndexOf("\"\"") != -1)
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

            if (CurrentColumn != FColumnCount)
            {
                throw new Exception(
                    String.Format("Line {0}: Invalid number of columns, should be {1} but there are only {2} columns.",
                        FRealLineCounter,
                        FColumnCount,
                        CurrentColumn));
            }

            return true;
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

        /// <summary>
        /// parse a CSV file that was dumped by Progress.
        /// that is the fastest way of dumping the data, but it is not ready for being imported into PostgreSQL.
        /// </summary>
        /// <param name="AInputFileDGz">the path to a gzipped csv file</param>
        /// <param name="AColumnCount">for checking the number of columns</param>
        public static List <string[]>ParseFile(string AInputFileDGz, int AColumnCount)
        {
            System.IO.Stream fs = new FileStream(AInputFileDGz, FileMode.Open, FileAccess.Read);
            GZipInputStream gzipStream = new GZipInputStream(fs);
            StreamReader MyReader = new StreamReader(gzipStream, ProgressFileEncoding);

            List <string[]>Result = new List <string[]>();

            CSVFile csvfile = new CSVFile(MyReader, AColumnCount, ' ');

            try
            {
                while (csvfile.GetNextRow())
                {
                    if (csvfile.FCurrentRow[0] == ".")
                    {
                        // we have parsed all the data
                        break;
                    }

                    for (int countColumn = 0; countColumn < AColumnCount; countColumn++)
                    {
                        if ((csvfile.FCurrentRow[countColumn].IndexOf('\n') != -1)
                            || (csvfile.FCurrentRow[countColumn].IndexOf('\r') != -1))
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

                    // this adds up to a lot of memory!!!
                    // TODO: should we store this in a sqlite db, instead of holding it in memory? also useful if we need p_person again, etc
                    Result.Add(csvfile.FCurrentRow);

                    if ((TLogging.DebugLevel > 0) && (csvfile.FRealLineCounter % 500000 == 0))
                    {
                        TLogging.Log(csvfile.FRealLineCounter.ToString() + " " + (GC.GetTotalMemory(false) / 1024 / 1024).ToString() + " MB");
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log(csvfile.FCurrentLine);
                TLogging.Log("Problem parsing file, in line " + csvfile.FRealLineCounter.ToString());
                throw e;
            }

            return Result;
        }

        private static string ReplaceKommaQuotes(string InputString)
        {
            int startCopyIndex = 0;
            string lineWithoutDoubleQuotes = "";

            int StringLength = InputString.Length - 1;
            bool IsString = false;

            for (int Counter = 0; Counter < StringLength; ++Counter)
            {
                if (InputString[Counter] == '\"')
                {
                    if (!IsString)
                    {
                        IsString = true;

                        if ((Counter > 0)
                            && (InputString[Counter - 1] == ','))
                        {
                            // We have ," replace it with KOMMAQUOTES%%
                            lineWithoutDoubleQuotes = lineWithoutDoubleQuotes +
                                                      InputString.Substring(startCopyIndex, Counter - 1 - startCopyIndex) +
                                                      "KOMMAQUOTES%%";
                            startCopyIndex = Counter + 1;
                        }
                    }
                    else
                    {
                        // check if " is end of string or part of string
                        if (InputString[Counter + 1] == '\"')
                        {
                            // We have a double quote as part of the string
                            Counter++;
                        }
                        else
                        {
                            IsString = false;
                        }
                    }
                }
            }

            lineWithoutDoubleQuotes = lineWithoutDoubleQuotes +
                                      InputString.Substring(startCopyIndex, InputString.Length - startCopyIndex);

            return lineWithoutDoubleQuotes;
        }
    }
}