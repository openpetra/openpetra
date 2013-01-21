//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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
using System;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

using NUnit.Framework;
using NUnit.Framework.Constraints;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;

namespace Ict.Testing.NUnitTools
{
    /// <summary>
    /// a set of small helpfull routines to make testing something easier.
    /// </summary>
    public class CommonNUnitFunctions
    {
        /// <summary>
        /// This is the central path of the complete tree
        /// </summary>
        public static string rootPath;

        /// <summary>
        /// get the root path of this OpenPetra directory
        /// </summary>
        public static void InitRootPath()
        {
            string strAssemblyPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString();

            string[] strArr = strAssemblyPath.Split(new char[] { '\\', '/' });
            rootPath = "";

            //This value is correct for tests run within SharpDevelop, need one less for NUnit standalone
            int numPathElementsForRoot = strArr.Length - 2;

            for (int i = 0; i < numPathElementsForRoot; ++i)
            {
                //  Check for last value of i to allow for NUnit
                if (i == (numPathElementsForRoot - 1))  //last value of i
                {
                    //If "OpenPetra.build" not in rootPath directory then move down one more directory
                    if (!File.Exists(rootPath + "OpenPetra.build"))
                    {
                        rootPath += strArr[i] + "/";
                    }
                }
                else
                {
                    rootPath += strArr[i] + "/";
                }
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public static string LoadCSVFileToString(string fileName)
        {
            using (FileStream fs = new FileStream(rootPath + "/" +
                       fileName.Replace('\\', Path.DirectorySeparatorChar),
                       FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Resets the data base to its initial value ...
        /// </summary>
        public static void ResetDatabase()
        {
            nant("resetDatabase", false);
        }

        /// <summary>
        /// Routine to load a test specific data base.
        /// </summary>
        /// <param name="strSqlFilePathFromCSharpName">A filename starting from the root.
        /// (csharp\\ICT\\Testing\\...\\filename.sql)</param>
        public static void LoadTestDataBase(string strSqlFilePathFromCSharpName)
        {
            TLogging.Log("LoadTestDataBase file: " + strSqlFilePathFromCSharpName);
            //nant("stopPetraServer", true);
            // csharp\\ICT\\Testing\\...\\filename.sql"
            //  + " >C:\\report.txt"
            nant("loadDatabaseIncrement -D:file=" + strSqlFilePathFromCSharpName, false);
            //nant("startPetraServer", true);
        }

        /// <summary>
        /// Actually this setting shall be done manually.
        /// </summary>
        private static string pathAndFileNameToNantExe = "nant";

        /// <summary>
        /// Routine to start nant ...
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="ignoreError"></param>
        private static void nant(String argument, bool ignoreError)
        {
            Process NantProcess = new Process();

            NantProcess.EnableRaisingEvents = false;

            if (Ict.Common.Utilities.DetermineExecutingOS() == TExecutingOSEnum.eosWinNTOrLater)
            {
                NantProcess.StartInfo.FileName = "cmd";
                NantProcess.StartInfo.Arguments = "/c " + pathAndFileNameToNantExe + " " + argument + " -logfile:nant.txt";
            }
            else
            {
                NantProcess.StartInfo.FileName = pathAndFileNameToNantExe;
                NantProcess.StartInfo.Arguments = argument.Replace("\\", "/") + " -logfile:nant.txt";
            }

            NantProcess.StartInfo.WorkingDirectory = rootPath;
            NantProcess.StartInfo.UseShellExecute = true;
            NantProcess.EnableRaisingEvents = true;
            NantProcess.StartInfo.ErrorDialog = true;

            if (!NantProcess.Start())
            {
                TLogging.Log("failed to start " + NantProcess.StartInfo.FileName);
            }
            else
            {
                NantProcess.WaitForExit(60000);
                Debug.Print("OS says nant process is finished");
            }

            string nantLogFile = rootPath + Path.DirectorySeparatorChar + "nant.txt";

            if (!File.Exists(nantLogFile))
            {
                FileStream fs = File.Create(nantLogFile);
                fs.Close();
            }

            StreamReader sr = new StreamReader(nantLogFile);
            TLogging.Log(sr.ReadToEnd());
            sr.Close();
            File.Delete(rootPath + Path.DirectorySeparatorChar + "nant.txt");
        }
    }

    /// <summary>
    /// This converter finds the different date substrings in a string like a message.
    /// Actually the common date format looks like "dd-MMM-yyyy" and so we are dealing
    /// with strings like "This Date is valid from 17-JAN-2009 to 21-FEB-2010".
    /// </summary>

    public class DateConverter
    {
        MatchCollection matchCollection;
        Regex regex;

        /// <summary>
        /// ...
        /// </summary>
        public DateConverter()
        {
            regex = new Regex("[0-3][0-9]-" +
                "(JAN|FEB|MAR|APR|MAI|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-[0-2][0-9][0-9][0-9]");
        }

        /// <summary>
        /// Gets the n'th date value from a string. Refering to our example string
        /// "This Date is valid from 17-JAN-2009 to 21-FEB-2010" the first value is
        /// "17-JAN-2009" (converted to date format) and the second value is "21-FEB-2010"
        /// </summary>
        /// <param name="inputString">The string which shall be searched for date entries</param>
        /// <param name="n">The number of the hit which shall be searched</param>
        /// <returns></returns>
        public DateTime GetNthDate(String inputString, int n)
        {
            matchCollection = regex.Matches(inputString);

            if (n >= 0)
            {
                if (n < matchCollection.Count)
                {
                    return Convert.ToDateTime(matchCollection[n].Value);
                }
                else
                {
                    // Enforce a failed test if the date does not exist!
                    Assert.Less(n, matchCollection.Count,
                        "This date match does not exist!");
                    return DateTime.MinValue;
                }
            }
            else
            {
                // Enforce a failed test if the date cannot not exist!
                Assert.GreaterOrEqual(n, 0, "Invalid date position requested");
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// We have to create own strings in an apropriate date format.
        /// </summary>
        /// <param name="dateTime">Date which shall be converted.</param>
        /// <returns></returns>
        public String GetDateString(DateTime dateTime)
        {
            return dateTime.ToString("dd-MMM-yyyy");
        }
    }
}