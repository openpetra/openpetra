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
            public int Severity;
            public int Position;
        }

        static private string _conciseOutput = "";
        static private string _verboseOutput = "";

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
        /// <param name="NumFailed">Return the number of failures</param>
        /// <param name="NumWarnings">Return the number of warnings</param>
        static public void AddLogFileOutput(string path, ref int NumFailed, ref int NumWarnings)
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
                ParseOutput(result, out NumSucceeded, ref NumFailed, ref NumWarnings);

                AppendText(OutputStream.Verbose, result + "\r\n");
                AppendText(OutputStream.Both,
                    String.Format("~~~~~~~ {0} succeeded, {1} failed, {2} warnings\r\n\r\n", NumSucceeded, NumFailed, NumWarnings));

                File.Delete(path);
            }
        }

        /// <summary>
        /// Parse the complete verbose output text looking for errors and warnings
        /// </summary>
        /// <returns>Return a list of error item structs that are the cursor positions for 'warning ' or 'BUILD FAILED' in the verbose output</returns>
        public static List <ErrorItem>FindWarnings()
        {
            List <ErrorItem>list = new List <ErrorItem>();
            int pWarning = 0;
            int pError = 0;

            while (pWarning >= 0 || pError >= 0)
            {
                ErrorItem ei = new ErrorItem();
                bool bAddWarning = false;
                bool bAddError = false;

                if (pWarning >= 0)
                {
                    pWarning = _verboseOutput.IndexOf("warning ", pWarning, StringComparison.InvariantCultureIgnoreCase);
                }

                if (pError >= 0)
                {
                    pError = _verboseOutput.IndexOf("BUILD FAILED", pError, StringComparison.InvariantCultureIgnoreCase);
                }

                if ((pWarning >= 0) && (pError >= 0))
                {
                    // we have both
                    if (pWarning < pError)
                    {
                        bAddWarning = true;
                    }
                    else
                    {
                        bAddError = true;
                    }
                }
                else if (pWarning >= 0)
                {
                    // just warnings left
                    bAddWarning = true;
                }
                else if (pError >= 0)
                {
                    // just errors left
                    bAddError = true;
                }

                if (bAddError)
                {
                    ei.Position = pError++;
                    ei.Severity = 2;
                    list.Add(ei);
                }

                if (bAddWarning)
                {
                    ei.Position = pWarning++;
                    ei.Severity = 1;
                    list.Add(ei);
                }
            }

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

        // Parse the text, looking for specific strings that indicate success, errors or warnings
        static private void ParseOutput(string TextToParse, out int NumSucceeded, ref int NumFailed, ref int NumWarnings)
        {
            NumSucceeded = 0;

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

            p = 0;

            while (p >= 0)
            {
                p = TextToParse.IndexOf("BUILD FAILED", p, StringComparison.InvariantCultureIgnoreCase);

                if (p > 0)
                {
                    NumFailed++;
                    p++;
                }
            }

            p = 0;

            while (p >= 0)
            {
                p = TextToParse.IndexOf("warning ", p, StringComparison.InvariantCultureIgnoreCase);

                if (p > 0)
                {
                    NumWarnings++;
                    p++;
                }
            }
        }
    }
}