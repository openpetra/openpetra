//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Ict.Tools.DevelopersAssistant
{
    /**************************************************************************************************************************************
     *
     * This is another class that has static methods.  There is just one Output object in two forms - concise and verbose.
     * The class has methods that reset the output string(s), parse the Nant output, construct the entire output string(s)
     *    and parse the verbose string for instances of warnings.
     * The class automatically deletes log files after they have been read and parsed, so no log files get picked up by source code control.
     *
     * ***********************************************************************************************************************************/
    class OutputText
    {
        /// <summary>
        /// The output log stream enumerations
        /// </summary>
        public enum OutputStream
        {
            Concise,
            Verbose,
            Both
        }

        /// <summary>
        /// The attributes of an error item
        /// </summary>
        public struct ErrorItem
        {
            public int Position;
            public int SelLength;
        }

        static Comparison <ErrorItem>ErrorItemComparisonDelegate = OnErrorItemSort;

        static private string _conciseOutput = "";
        static private string _verboseOutput = "";

        static public int WarningCount {
            get; private set;
        }
        static public int ErrorCount {
            get; private set;
        }

        /// <summary>
        /// Gets the concise output string
        /// </summary>
        static public string ConciseOutput {
            get
            {
                return _conciseOutput;
            }
        }
        /// <summary>
        /// Gets the verbose output string
        /// </summary>
        static public string VerboseOutput {
            get
            {
                return _verboseOutput;
            }
        }

        /// <summary>
        /// Reset concise and verbose strings to empty
        /// </summary>
        static public void ResetOutput()
        {
            _conciseOutput = String.Empty;
            _verboseOutput = String.Empty;
            WarningCount = 0;
            ErrorCount = 0;
        }

        /// <summary>
        /// Append a string to one or both of the output strings
        /// </summary>
        /// <param name="o">One of the output enumerations to append text to</param>
        /// <param name="s">The text to append</param>
        static public void AppendText(OutputStream o, string s)
        {
            if ((o == OutputStream.Concise) || (o == OutputStream.Both))
            {
                _conciseOutput += s;
            }

            if ((o == OutputStream.Verbose) || (o == OutputStream.Both))
            {
                _verboseOutput += s;
            }
        }

        //
        //
        /// <summary>
        /// The main call to read and parse a log file, given its path.
        /// </summary>
        /// <param name="path">Path to the logfile to parse and append the result to the output streams</param>
        /// <param name="NumFailures">Return the number of failed builds</param>
        /// <param name="NumWarnings">Return the number of warnings and/or errors</param>
        static public void AddLogFileOutput(string path, ref int NumFailures, ref int NumWarnings)
        {
            string result = ReadStreamFile(path);

            if (result == String.Empty)
            {
                AppendText(OutputStream.Both, "\r\n~~~~ Warning!!! No output log file was found for this action.");
                NumWarnings++;
            }
            else
            {
                int NumSucceeded;
                ParseOutput(result, out NumSucceeded, ref NumFailures, ref NumWarnings);

                AppendText(OutputStream.Verbose, result + "\r\n");
                AppendText(OutputStream.Both,
                    String.Format("~~~~~~~ {0} succeeded, {1} failed, {2} warning(s) or error(s)\r\n\r\n", NumSucceeded, NumFailures, NumWarnings));

                File.Delete(path);
            }
        }

        public static int OnErrorItemSort(ErrorItem item1, ErrorItem item2)
        {
            return (item1.Position > item2.Position) ? 1 : (item1.Position == item2.Position) ? 0 : -1;
        }

        /// <summary>
        /// Parse the complete verbose output text looking for errors and warnings
        /// </summary>
        /// <returns>Returns a sorted list of error item structs that are the cursor positions for 'warning ' or ' error' in the verbose output</returns>
        public static List <ErrorItem>FindWarnings()
        {
            List <ErrorItem>list = new List <ErrorItem>();
            string[] candidates =
            {
                "BUILD FAILED", "error", "warning", "exception"
            };
            bool bIsValid = false;
            int itemID = 0;

            foreach (string lookFor in candidates)
            {
                int p = 0;

                while (p >= 0)
                {
                    p = _verboseOutput.IndexOf(lookFor, p, StringComparison.InvariantCultureIgnoreCase);

                    if (p >= 0)
                    {
                        bIsValid = true;

                        if ((itemID == 1) || (itemID == 2))
                        {
                            // error and warning must not be plural because this is just a repeat of what we know already
                            bIsValid =
                                (_verboseOutput.Substring(p + lookFor.Length, 3).CompareTo("(s)") != 0
                                 && _verboseOutput.Substring(p + lookFor.Length, 1).CompareTo("s") != 0
                                 && _verboseOutput.Substring(p - 2, 8).CompareTo("s_error_") != 0
                                 && _verboseOutput.Substring(p - 1, 12).CompareTo("\\ErrorLog.cs") != 0
                                 && _verboseOutput.Substring(p - 1, 22).CompareTo("\\ErrorCodeInventory.cs") != 0
                                 && _verboseOutput.Substring(p - 1, 20).CompareTo("\\ErrorCodesHelper.cs") != 0
                                 && _verboseOutput.Substring(p - 1, 14).CompareTo("\\ErrorCodes.cs") != 0);
                        }
                        else if (itemID == 3)
                        {
                            // exception must not be ExceptionDetailsDialog or ExceptionLogFileDialog-
                            bIsValid =
                                (_verboseOutput.LastIndexOf('\\', p, 24) == -1
                                 && _verboseOutput.IndexOf("DetailsDialog", p, 24) == -1
                                 && _verboseOutput.IndexOf("LogFileDialog", p, 24) == -1);
                        }

                        if (bIsValid)
                        {
                            ErrorItem ei = new ErrorItem();
                            ei.Position = p;
                            ei.SelLength = lookFor.Length;
                            list.Add(ei);

                            if (itemID == 0)
                            {
                                ErrorCount++;
                            }
                            else
                            {
                                WarningCount++;
                            }
                        }

                        p++;
                    }
                }

                itemID++;
            }

            // Put everything in order of occurrence in the text
            list.Sort(ErrorItemComparisonDelegate);
            return list;
        }

        // Read the log file and return its content
        static private string ReadStreamFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        // Parse the text, looking for specific strings that indicate success, failure, errors, exceptions or warnings
        static private void ParseOutput(string TextToParse, out int NumSucceeded, ref int NumFailures, ref int NumWarnings)
        {
            NumSucceeded = 0;

            // We note the number of successes
            int p = 0;

            while (p >= 0)
            {
                p = TextToParse.IndexOf("BUILD SUCCEEDED", p, StringComparison.InvariantCultureIgnoreCase);

                if (p > 0)
                {
                    NumSucceeded++;
                    p++;
                }
            }

            // We note the number of failed builds
            p = 0;

            while (p >= 0)
            {
                p = TextToParse.IndexOf("BUILD FAILED", p, StringComparison.InvariantCultureIgnoreCase);

                if (p > 0)
                {
                    NumFailures++;
                    p++;
                }
            }

            // Finally we note the number of 'suspicious' entries
            string[] candidates =
            {
                "error", "warning", "exception"
            };
            int itemID = 0;

            foreach (string lookFor in candidates)
            {
                p = 0;

                while (p >= 0)
                {
                    p = TextToParse.IndexOf(lookFor, p, StringComparison.InvariantCultureIgnoreCase);

                    if (p > 0)
                    {
                        if ((itemID == 0) || (itemID == 1))
                        {
                            // error and warning must not be plural.  We also need to ignore s_error_log
                            if ((TextToParse.Substring(p + lookFor.Length, 3).CompareTo("(s)") != 0)
                                && (TextToParse.Substring(p + lookFor.Length, 1).CompareTo("s") != 0)
                                && (TextToParse.Substring(p - 2, 8).CompareTo("s_error_") != 0)
                                && (TextToParse.Substring(p - 1, 12).CompareTo("\\ErrorLog.cs") != 0)
                                && (TextToParse.Substring(p - 1, 22).CompareTo("\\ErrorCodeInventory.cs") != 0)
                                && (TextToParse.Substring(p - 1, 20).CompareTo("\\ErrorCodesHelper.cs") != 0)
                                && (TextToParse.Substring(p - 1, 14).CompareTo("\\ErrorCodes.cs") != 0))
                            {
                                NumWarnings++;
                            }
                        }
                        else if (itemID == 2)
                        {
                            // exception must not refer to ExceptionDetailsDialog or ExceptionLogFileDialog
                            if ((TextToParse.LastIndexOf('\\', p, 24) == -1)
                                && (TextToParse.IndexOf("DetailsDialog", p, 24) == -1)
                                && (TextToParse.IndexOf("LogFileDialog", p, 24) == -1))
                            {
                                NumWarnings++;
                            }
                        }
                        else
                        {
                            NumWarnings++;
                        }

                        p++;
                    }
                }

                itemID++;
            }
        }
    }
}