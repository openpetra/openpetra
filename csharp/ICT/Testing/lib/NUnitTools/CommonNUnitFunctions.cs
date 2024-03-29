﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
//
// Copyright 2004-2021 by OM International
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
using System.Data;
using System.Data.Odbc;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance.Account.Data;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Tools.DBXML;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;
using Ict.Petra.Server.MFinance.Account.Data.Access;

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
        /// load the csv file into a string
        /// </summary>
        public static string LoadCSVFileToString(string fileName)
        {
            if (!File.Exists(fileName))
            {
                fileName = rootPath + "/" +
                           fileName.Replace('\\', Path.DirectorySeparatorChar);
            }

            using (FileStream fs = new FileStream(fileName,
                       FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// load test data written in PostgreSQL syntax into MySQL
        /// </summary>
        private static void LoadTestDataMySQL(string fileName)
        {
            // parse the sql file, and for each table (are there multiple???) load the data
            string sqlfileContent = LoadCSVFileToString(fileName);

            string[] lines = sqlfileContent.Split(new char[] { '\n' });
            string currenttable = string.Empty;
            string sqlStmt = string.Empty;

            TDataDefinitionParser parser = new TDataDefinitionParser(rootPath + Path.DirectorySeparatorChar + "db" + Path.DirectorySeparatorChar + "petra.xml");
            TDataDefinitionStore store = new TDataDefinitionStore();
            TTable table = null;

            if (!parser.ParseDocument(ref store))
            {
                throw new Exception("failed to parse petra.xml");
            }

            TDBTransaction LoadTransaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("LoadTestDataMySQL");
            bool SubmissionOK = false;

            db.WriteTransaction(ref LoadTransaction,
                ref SubmissionOK,
                delegate
                {
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("--") || (line.Trim() == String.Empty))
                        {
                            continue;
                        }
                        else if (line.StartsWith("COPY "))
                        {
                            currenttable = line.Substring("COPY ".Length, line.IndexOf("(") - "COPY ".Length - 1);

                            table = store.GetTable(currenttable);

                            sqlStmt = table.PrepareSQLInsertStatement();
                        }
                        else if (line == "\\.")
                        {
                            currenttable = String.Empty;
                        }
                        else if (currenttable != String.Empty)
                        {
                            List <OdbcParameter> Parameters = table.PrepareParametersInsertStatement(line);

                            if (db.ExecuteNonQuery(sqlStmt, LoadTransaction, Parameters.ToArray()) == 0)
                            {
                                throw new Exception("failed to import line for table " + currenttable);
                            }
                        }
                    }

                    SubmissionOK = true;
                });
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
        /// <param name="ALedgerNumber">ledger that we are using currently</param>
        public static void LoadTestDataBase(string strSqlFilePathFromCSharpName, int ALedgerNumber = -1)
        {
            string tempfile = string.Empty;

            TLogging.Log("LoadTestDataBase(" + strSqlFilePathFromCSharpName + ")");

            if (ALedgerNumber != -1)
            {
                // we need to replace the ledgernumber variable in the sql file
                string sqlfileContent = LoadCSVFileToString(strSqlFilePathFromCSharpName);

                if (sqlfileContent.IndexOf("{ledgernumber}") == -1)
                {
                    throw new Exception("LoadTestDatabase: expecting variable ledgernumber in file " + strSqlFilePathFromCSharpName);
                }

                sqlfileContent = sqlfileContent.Replace("{ledgernumber}", ALedgerNumber.ToString());

                tempfile = Path.GetTempFileName();
                StreamWriter sw = new StreamWriter(tempfile);
                sw.WriteLine(sqlfileContent);
                sw.Close();
                strSqlFilePathFromCSharpName = tempfile;
            }

            if (TSrvSetting.RDMBSType == TDBType.PostgreSQL)
            {
                nant("loadDatabaseIncrement -D:file=\"" + strSqlFilePathFromCSharpName + "\"", false);
            }
            else if (TSrvSetting.RDMBSType == TDBType.MySQL)
            {
                LoadTestDataMySQL(strSqlFilePathFromCSharpName);
            }

            if (tempfile.Length > 0)
            {
                File.Delete(tempfile);
            }
        }

        /// <summary>
        /// create a new ledger, with new ledgernumber
        /// </summary>
        /// <returns>ledgernumber of new ledger</returns>
        public static int CreateNewLedger(DateTime? AStartDate = null, bool AWithILT = false)
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("NUnitFunctions.CreateNewLedger");
            ALedgerTable ledgers = null;

            db.ReadTransaction(ref ReadTransaction,
                delegate
                {
                    ledgers = ALedgerAccess.LoadAll(ReadTransaction);
                });
            db.CloseDBConnection();

            ledgers.DefaultView.Sort = ALedgerTable.GetLedgerNumberDBName() + " DESC";
            int newLedgerNumber = ((ALedgerRow)ledgers.DefaultView[0].Row).LedgerNumber + 1;

            if (newLedgerNumber < 500)
            {
                // avoiding conflicts with foreign ledgers
                newLedgerNumber = 500;
            }

            if (AStartDate == null)
            {
                AStartDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            TLogging.Log("CommonNUnitFunctions.CreateNewLedger " + newLedgerNumber.ToString());

            TVerificationResultCollection VerificationResult;
            TGLSetupWebConnector.CreateNewLedger(newLedgerNumber,
                "NUnit Test Ledger " + newLedgerNumber.ToString(),
                "99",
                "EUR",
                "USD",
                AStartDate.Value,
                12,
                1,
                8,
                AWithILT,
                out VerificationResult);

            return newLedgerNumber;
        }

        /// <summary>
        /// execute commands in the server admin console
        /// </summary>
        public static string OpenPetraServerAdminConsole(string ACommand, string AParameters = "")
        {
            Process process = new Process();

            string argument = "-Command:" + ACommand;

            if (Ict.Common.Utilities.DetermineExecutingOS() >= TExecutingOSEnum.eosWinNTOrLater)
            {
                process.StartInfo.FileName = "PetraServerAdminConsole.exe";
                process.StartInfo.Arguments = "-C:../../etc/ServerAdmin.config " + argument + " " + AParameters;
            }
            else
            {
                process.StartInfo.FileName = "mono";
                process.StartInfo.Arguments = "PetraServerAdminConsole.exe -C:../../etc/ServerAdmin.config " + argument + " " + AParameters;
            }

            process.StartInfo.WorkingDirectory = rootPath + "/delivery/bin".Replace('/', Path.DirectorySeparatorChar);
            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = false;
            process.StartInfo.ErrorDialog = false;
            process.StartInfo.RedirectStandardOutput = true;

            if (!process.Start())
            {
                TLogging.Log("failed to start " + process.StartInfo.FileName + " " + process.StartInfo.Arguments);
            }
            else
            {
                process.WaitForExit(60000);
                Debug.Print("OS says OpenPetraServerAdminConsole process is finished");
            }

            if (process.ExitCode != 0)
            {
                throw new Exception("OpenPetraServerAdminConsole did not succeed");
            }

            return process.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// start the OpenPetra server
        /// </summary>
        public static void StartOpenPetraServer(string AParameters = "")
        {
            CommonNUnitFunctions.nant("startServerBackground " + AParameters, false);
            // wait a few seconds, otherwise the client will not connect
            Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(5.0).TotalMilliseconds));
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
        public static void nant(String argument, bool ignoreError)
        {
            Process NantProcess = new Process();

            NantProcess.EnableRaisingEvents = false;

            if (Ict.Common.Utilities.DetermineExecutingOS() >= TExecutingOSEnum.eosWinNTOrLater)
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
                NantProcess.WaitForExit(120000);
                Debug.Print("OS says nant process is finished");
            }

            string nantLogFile = rootPath + Path.DirectorySeparatorChar + "nant.txt";

            if (File.Exists(nantLogFile))
            {
                try
                {
                    StreamReader sr = new StreamReader(nantLogFile);
                    TLogging.Log(sr.ReadToEnd());
                    sr.Close();
                    File.Delete(nantLogFile);
                }
                catch (System.IO.IOException)
                {
                    TLogging.Log("Problem reading log file, but continuing anyway: " + nantLogFile);
                }
            }

            if ((NantProcess.ExitCode != 0) && !ignoreError)
            {
                throw new Exception("Nant did not succeed");
            }
        }

        /// <summary>
        /// Checks that a <see cref="TVerificationResultCollection" /> is either null or that it doesn't contain
        /// any <see cref="TVerificationResult" /> items . If it isn't null and it contains items an Assert.Fail call
        /// is issued by this Method!
        /// </summary>
        /// <remarks>
        /// Can be used for 'Guard Asserts' to check that the <see cref="TVerificationResultCollection" />
        /// that is returned from server calls is null or empty.
        /// </remarks>
        /// <param name="AVerificationResult"><see cref="TVerificationResultCollection" /> reference (can be null!).</param>
        /// <param name="AMessage">String to append before the Assert message that this Method produces (optional).</param>
        public static void EnsureNullOrEmptyVerificationResult(TVerificationResultCollection AVerificationResult, string AMessage = "")
        {
            if ((AVerificationResult != null)
                && (AVerificationResult.Count > 0))
            {
                Assert.Fail(AMessage + "*** VerificationResult is NOT EMPTY *** : " +
                    AVerificationResult.BuildVerificationResultString());
            }
        }

        /// <summary>
        /// Checks that a <see cref="TVerificationResultCollection" /> is either null or that it doesn't contain
        /// any <see cref="TVerificationResult" /> items that are CriticalErrors. If it isn't null and it contains such items, an Assert.Fail
        /// call is issued by this Method!
        /// </summary>
        /// <remarks>
        /// Can be used for 'Guard Asserts' to check that the <see cref="TVerificationResultCollection" />
        /// that is returned from server calls is null or holds only non-critical <see cref="TVerificationResult" /> items.
        /// </remarks>
        /// <param name="AVerificationResult"><see cref="TVerificationResultCollection" /> reference (can be null!).</param>
        /// <param name="AMessage">String to append before the Assert message that this Method produces (optional).</param>
        public static void EnsureNullOrOnlyNonCriticalVerificationResults(TVerificationResultCollection AVerificationResult, string AMessage = "")
        {
            string VerificationResultStr;

            if ((AMessage != String.Empty)
                && (!AMessage.EndsWith(" ")))
            {
                AMessage = AMessage + " ";
            }

            if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
            {
                VerificationResultStr = AVerificationResult.BuildVerificationResultString();

                TLogging.Log(VerificationResultStr);

                Assert.Fail(AMessage + "*** TVerificationResult HAS CRITICAL ERRORS *** : " +
                    VerificationResultStr);
            }
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
