//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Globalization;

namespace Ict.Common.IO
{
    /// <summary>
    /// write and read a text file in a format that Petra 2.x uses
    /// </summary>
    public class TImportExportTextFile
    {
        /// <summary>
        /// This can be used (and changed) from outside
        /// </summary>
        private string dateFormat = "dd/MM/yyyy";
        private const string QUOTE_MARKS = "\"";
        private const string SPACE = "  ";

        /// <summary>
        /// this contains the context of the text file, all Write functions write into this string,
        /// </summary>
        private StringBuilder FTextToWrite = new StringBuilder();

        /// <summary>
        /// useful when deciding if SPACE needs to be written or not (at beginning of line)
        /// </summary>
        private bool FStartOfLine = true;

        /// <summary>
        /// this contains the context of the text file, when it is being read
        /// </summary>
        private String[] FLinesToParse;

        private String FCurrentLine;
        private Int32 FCurrentLineCounter;

        /// <summary>
        /// read the current line counter
        /// </summary>
        public Int32 CurrentLineCounter
        {
            get
            {
                return FCurrentLineCounter;
            }
        }

        /// <summary>
        /// Access the current Date Format for import / export
        /// </summary>
        public string DATEFORMAT
        {
            get
            {
                return dateFormat;
            }
            set
            {
                dateFormat = value;
            }
        }
        /// <summary>
        /// read the current line
        /// </summary>
        public string CurrentLine
        {
            get
            {
                return FCurrentLine;
            }
        }

        /// <summary>
        /// initialise the stringbuilder for writing a new file
        /// </summary>
        public void StartWriting()
        {
            FTextToWrite = new StringBuilder();
            FStartOfLine = true;
        }

        /// <summary>
        /// returns the string that has been assembled during all the previous write function calls
        /// </summary>
        public string FinishWriting()
        {
            return FTextToWrite.ToString();
        }

        /// <summary>
        /// pass the lines that should be parsed
        /// </summary>
        public void InitReading(string[] ATextFileContents)
        {
            FLinesToParse = ATextFileContents;
            FCurrentLineCounter = 0;
            FCurrentLine = FLinesToParse[FCurrentLineCounter].TrimEnd();

            FCurrentLine = FCurrentLine.Replace(" ", "  ");

            string DateTimeFormat = ReadString();

            if (DateTimeFormat.ToLower() == "dmy")
            {
                dateFormat = "dd/MM/yyyy";
            }
            else if (DateTimeFormat.ToLower() == "mdy")
            {
                dateFormat = "MM/dd/yyyy";
            }
            else
            {
                throw new Exception("Unknown DateTimeFormat " + DateTimeFormat);
            }
        }

        /// <summary>
        /// is there more data to read?
        /// </summary>
        public bool EndOfFile()
        {
            if (FCurrentLineCounter >= FLinesToParse.Length)
            {
                return true;
            }

            return FCurrentLine.Length <= 0;
        }

        /// <summary>
        /// all items in the text file are either separated by newline or by SPACE
        /// </summary>
        /// <returns></returns>
        private string ReadNextStringItem()
        {
            String CurrentLineTemp = "";

            if (EndOfFile())
            {
                throw new Exception("ReadNextStringItem: there is no data anymore");
            }

            string NextStringItem = string.Empty;

            // sometimes strings go across several lines
            if (FCurrentLine[0] == '"')
            {
                bool AcrossSeveralLines = false;

                do
                {
                    if (AcrossSeveralLines)
                    {
                        FCurrentLineCounter++;
                        FCurrentLine += Environment.NewLine + FLinesToParse[FCurrentLineCounter].TrimEnd();
                    }

                    try
                    {
                        CurrentLineTemp = FCurrentLine;
                        NextStringItem = StringHelper.GetNextCSV(ref CurrentLineTemp, SPACE);
                        AcrossSeveralLines = false;
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        // the current data row is across several lines
                        AcrossSeveralLines = true;
                    }

                    if (NextStringItem == StringHelper.CSV_STRING_FORMAT_ERROR)
                    {
                        // the current data row is across several lines
                        AcrossSeveralLines = true;
                    }
                    else
                    {
                        // reading next csv was fine and all is on one line --> assign shortened line string
                        FCurrentLine = CurrentLineTemp;
                    }
                } while (AcrossSeveralLines);
            }
            else
            {
                NextStringItem = StringHelper.GetNextCSV(ref FCurrentLine, SPACE);
            }

            while (FCurrentLine.Length == 0 && FCurrentLineCounter < FLinesToParse.Length - 1)
            {
                FCurrentLineCounter++;
                FCurrentLine = FLinesToParse[FCurrentLineCounter].TrimEnd();
            }

            return NextStringItem;
        }

        /// <summary>
        /// this will check if the next item is the quoted keyword.
        /// if this is true, the parser will proceed to next item
        /// </summary>
        public bool CheckForKeyword(string AKeyword)
        {
            if (EndOfFile())
            {
                return false;
            }

            string TempString = String.Copy(FCurrentLine);
            string NextStringItem = StringHelper.GetNextCSV(ref TempString, SPACE);

            if (NextStringItem == AKeyword)
            {
                ReadNextStringItem();
                return true;
            }

            return false;
        }

        /// <summary>
        /// insert a new line
        /// </summary>
        public void WriteLine()
        {
            FTextToWrite.Append(Environment.NewLine);
            FStartOfLine = true;
        }

        /// <summary>
        /// insert a string. will use quotes
        /// </summary>
        /// <param name="AValue"></param>
        public void Write(string AValue)
        {
            if (!FStartOfLine)
            {
                FTextToWrite.Append(SPACE);
            }

            FTextToWrite.Append(QUOTE_MARKS);
            FTextToWrite.Append(AValue.Replace("\"", "'"));
            FTextToWrite.Append(QUOTE_MARKS);

            FStartOfLine = false;
        }

        /// <summary>
        /// read a string. strip off the quotes.
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            string NextItem = ReadNextStringItem();

            if (NextItem.StartsWith(QUOTE_MARKS))
            {
                NextItem = NextItem.Substring(QUOTE_MARKS.Length, NextItem.Length - 2 * QUOTE_MARKS.Length);
            }

            if (NextItem == "?")
            {
                return "";
            }

            return NextItem;
        }

        /// <summary>
        /// write a boolean value
        /// </summary>
        /// <param name="AValue"></param>
        public void Write(bool AValue)
        {
            if (!FStartOfLine)
            {
                FTextToWrite.Append(SPACE);
            }

            FTextToWrite.Append(AValue ? "yes" : "no");

            FStartOfLine = false;
        }

        /// <summary>
        /// read a boolean value
        /// </summary>
        public bool ReadBoolean()
        {
            string NextItem = ReadNextStringItem();

            return NextItem == "yes";
        }

        /// <summary>
        ///  write an Int64 value
        /// </summary>
        /// <param name="AValue"></param>
        public void Write(Int64 AValue)
        {
            if (!FStartOfLine)
            {
                FTextToWrite.Append(SPACE);
            }

            FTextToWrite.Append(AValue.ToString());

            FStartOfLine = false;
        }

        /// <summary>
        /// read an Int64 value
        /// </summary>
        public Int64 ReadInt64()
        {
            return Convert.ToInt64(ReadNextStringItem());
        }

        /// <summary>
        /// read an Int64 value
        /// </summary>
        public Int64 ? ReadNullableInt64()
        {
            string s = ReadNextStringItem();

            if (s == "?")
            {
                return new Nullable <Int64>();
            }

            return Convert.ToInt64(s);
        }

        /// <summary>
        /// write an Int32 value
        /// </summary>
        /// <param name="AValue"></param>
        private void Write(Int32 AValue)
        {
            if (!FStartOfLine)
            {
                FTextToWrite.Append(SPACE);
            }

            FTextToWrite.Append(AValue.ToString());

            FStartOfLine = false;
        }

        /// <summary>
        /// read an Int32 value
        /// </summary>
        public Int32 ReadInt32()
        {
            string s = ReadNextStringItem();

            return Convert.ToInt32(s);
        }

        /// <summary>
        /// read an Int32 value
        /// </summary>
        public Int32 ? ReadNullableInt32()
        {
            string s = ReadNextStringItem();

            if (s == "?")
            {
                return new Nullable <Int32>();
            }

            return Convert.ToInt32(s);
        }

        /// <summary>
        /// write a decimal value
        /// </summary>
        /// <param name="AValue"></param>
        public void Write(Decimal AValue)
        {
            if (!FStartOfLine)
            {
                FTextToWrite.Append(SPACE);
            }

            // no thousands separator, dot for decimal separator
            FTextToWrite.Append(AValue.ToString(CultureInfo.InvariantCulture));

            FStartOfLine = false;
        }

        /// <summary>
        /// read a Decimal value
        /// </summary>
        public decimal ReadDecimal()
        {
            string s = ReadNextStringItem();

            return Convert.ToDecimal(s, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// write a date. if null, write questionmark
        /// </summary>
        /// <param name="AValue"></param>
        public void Write(DateTime ? AValue)
        {
            if (!FStartOfLine)
            {
                FTextToWrite.Append(SPACE);
            }

            if (!AValue.HasValue)
            {
                FTextToWrite.Append(QUOTE_MARKS);
                FTextToWrite.Append("?");
                FTextToWrite.Append(QUOTE_MARKS);
            }
            else
            {
                FTextToWrite.Append(QUOTE_MARKS);
                FTextToWrite.Append(AValue.Value.ToString(dateFormat));
                FTextToWrite.Append(QUOTE_MARKS);
            }

            FStartOfLine = false;
        }

        /// <summary>
        /// read a DateTime value that can be null
        /// </summary>
        public DateTime ? ReadNullableDate()
        {
            string NextItem = ReadString();

            if (NextItem.Length == 0)
            {
                return new Nullable <DateTime>();
            }

            if (NextItem.Length != dateFormat.Length)
            {
                // eg. pm_special_need.s_date_created_d is stored with just two digits for the year
                return DateTime.ParseExact(NextItem, dateFormat.Replace("yyyy", "yy"), CultureInfo.InvariantCulture);
            }

            return DateTime.ParseExact(NextItem, dateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// write a date
        /// </summary>
        /// <param name="AValue"></param>
        public void Write(DateTime AValue)
        {
            if (!FStartOfLine)
            {
                FTextToWrite.Append(SPACE);
            }

            FTextToWrite.Append(QUOTE_MARKS);
            FTextToWrite.Append(AValue.ToString(dateFormat));
            FTextToWrite.Append(QUOTE_MARKS);

            FStartOfLine = false;
        }

        /// <summary>
        /// read a DateTime value
        /// </summary>
        public DateTime ReadDate()
        {
            string NextItem = ReadString();

            return DateTime.ParseExact(NextItem, dateFormat, CultureInfo.InvariantCulture);
        }
    }
}