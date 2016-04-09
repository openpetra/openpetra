//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       charlvj, timop
//
// Copyright 2004-2016 by OM International
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
using System.Collections.Specialized;

namespace Ict.Common
{
    /// <summary>
    /// This class contains all the commandline parameters. It takes a very easy
    /// approach for the formatting of commandline parameters that takes values.
    /// Basically it uses the format &lt;flag&gt;:&lt;value&gt;, eg -F:c:\path\to\file.txt where
    /// -F is the parameter name, and c:\path\to\file.txt is the value.
    /// </summary>
    /// <example>
    /// if (commandLineOpts.IsFlagSet("hello")) then
    /// {
    ///     Console.Writeline(commandLineOpts.GetOptValue("hello"));
    /// }
    ///</example>
    public class TCmdOpts
    {
        /// The string list of all the command line options.
        private StringCollection FList = new StringCollection();
        private Dictionary <string, string>_options = new Dictionary <string, string>();

        /// <summary>
        /// </summary>
        /// <returns>The string list of all the command line options.
        /// </returns>
        StringCollection CommandOptions
        {
            get
            {
                return FList;
            }
        }

        /// <summary>
        /// Creates a new object of this class and copies all the parameters that were
        /// passed on the command line into a string list.
        /// Uses the array of strings returned by Environment.GetCommandLineArgs
        /// </summary>
        /// <returns>void</returns>
        public TCmdOpts()
        {
            string[] list = Environment.GetCommandLineArgs();

            foreach (string s in list)
            {
                if ((FList.Count > 0) && FList[FList.Count - 1].EndsWith(":") && !s.StartsWith("-"))
                {
                    // allow space after : to allow automatic tab expansion for the filename on the Command line
                    // but also allow empty parameters, that do not use the next parameter name as a value
                    FList[FList.Count - 1] += s.Trim();
                }
                else if ((FList.Count > 0) && !s.StartsWith("-"))
                {
                    // avoid splitting path names that would need quotes otherwise
                    FList[FList.Count - 1] += " " + s.Trim();
                }
                else
                {
                    FList.Add(s.Trim());
                }
            }

            foreach (string entry in FList)
            {
                string[] res = entry.Split(new Char[] { '-' }, 2);

                if (1 < res.Length)
                {
                    string[] opt = res[1].Split(new Char[] { ':' }, 2);

                    if ((1 < opt.Length) && !_options.ContainsKey(opt[0]))
                    {
                        // Add option to dictionary for fast lockup
                        _options.Add(opt[0], opt[1]);
                    }
                }
            }
        }

        /// <summary>
        /// return all keys of available options
        /// </summary>
        /// <param name="AStartingWith">only keys that start with this string</param>
        public StringCollection GetOptKeys(string AStartingWith = "")
        {
            StringCollection Result = new StringCollection();

            foreach (string opt in FList)
            {
                if ((AStartingWith == String.Empty) || (opt.StartsWith(AStartingWith)))
                {
                    Result.Add(opt);
                }
            }

            return Result;
        }

        /// <summary>
        /// Returns the value of the given option.
        ///
        /// </summary>
        /// <param name="AOpt">The option/parameter to look for.</param>
        /// <returns>The value of the option, or throws an exception if the value was not found
        /// </returns>
        public string GetOptValue(string AOpt)
        {
            if (_options.ContainsKey(AOpt))
            {
                return _options[AOpt];
            }

            throw new Exception("missing option " + AOpt);
        }

        /// <summary>
        /// Checks if the given flag is set. <b>Note:</b> Flag can be defined as either
        /// a standalone flag (eg. --verbose or -v) or can be the name of a parameter
        /// (eg. -F in -Fc:\path\to\file.txt).
        /// </summary>
        /// <param name="flag">Flag to search for in the list of parameters.</param>
        /// <returns><code>true</code> if flag was set, otherwise <code>false</code>
        /// </returns>
        public Boolean IsFlagSet(string flag)
        {
            return _options.ContainsKey(flag);
        }
    }
}