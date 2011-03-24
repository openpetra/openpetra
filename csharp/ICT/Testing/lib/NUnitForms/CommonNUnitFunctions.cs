//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2010 by OM International
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

using NUnit.Extensions.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;


using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.MFinance.Gui;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;

using Ict.Testing.NUnitPetraServer;

namespace Ict.Testing.NUnitForms
{
    /// <summary>
    /// The idea of CommonNUnitFunctions is that you can replace the test inheritace of
    /// NUnitFormTest by NUnitFormTest. So you will get a set of small helpfull routines
    /// to make testing something easier.
    /// </summary>
    public class CommonNUnitFunctions : NUnitFormTest
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// Empty Constructor ...
        /// </summary>
        public CommonNUnitFunctions()
        {
        }

        /// <summary>
        /// This "string" can be used as an public property to read in the
        /// Title of the last Message box.
        /// </summary>
        public String lastMessageTitle;

        /// <summary>
        /// This "string" can be used as an public property to read in the
        /// Message of the last Message box.
        /// </summary>
        public String lastMessageText;

        public string rootPath;

        /// <summary>
        /// The delegate to handle the message box is installed.
        /// </summary>
        /// <param name="cmd">Contains a NUnit.Extensions.Forms.MessageBoxTester.Command to
        /// insert the desired reaction.</param>
        public void WaitForMessageBox(NUnit.Extensions.Forms.MessageBoxTester.Command cmd)
        {
            lastMessageTitle = "";
            lastMessageText = "";

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);

                System.Console.WriteLine("Title: " + tester.Title);
                System.Console.WriteLine("Message: " + tester.Text);

                lastMessageTitle = tester.Title;
                lastMessageText = tester.Text;

                tester.SendCommand(cmd);
            };
        }

        /// <summary>
        /// Routine to initialize the connection to TestServer.config and TestServer.log
        /// </summary>
        public void InitServerConnection()
        {
            string strAssemblyPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString();

            string[] strArr = strAssemblyPath.Split(new char[] { '\\', '/' });
            string strTrunkRoot = "";

            for (int i = 0; i < strArr.Length - 2; ++i)
            {
                strTrunkRoot = strTrunkRoot + strArr[i] + "/";
            }

            rootPath = strTrunkRoot;

            System.Diagnostics.Debug.WriteLine("strRootPath: " + rootPath);

            string strNameLog = strTrunkRoot + "log/TestServer.log";
            string strNameConfig = strTrunkRoot + "etc/TestServer.config";

            new TLogging(strNameLog);
            TPetraServerConnector.Connect(strNameConfig);
        }
        
        public void DisconnectServerConnection()
        {
        	TPetraServerConnector.Disconnect();
        }
        
        /// <summary>
        /// Routine to load a test specific data base.
        /// </summary>
        /// <param name="strSqlFilePathFromCSharpName"></param>
        public void LoadTestDataBase(string strSqlFilePathFromCSharpName)
        {
            nant("stopPetraServer", false);
            // csharp\\ICT\\Testing\\MFinance\\GiftForm\\TestData\\withpartners.sql"
            nant("loadDatabase -D:LoadDB.file=" + rootPath + strSqlFilePathFromCSharpName, true);
            nant("startPetraServer", true);
        }

        /// <summary>
        /// Routine to start nant ...
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="ignoreError"></param>
        private void nant(String argument, bool ignoreError)
        {
            Process NantProcess = new Process();

            NantProcess.EnableRaisingEvents = false;
            NantProcess.StartInfo.FileName = "cmd"; //if you have trouble and want to check the dos box try with cmd and /k nant xxx as arguments
            NantProcess.StartInfo.Arguments = "/c nant " + argument;
            NantProcess.StartInfo.CreateNoWindow = false;
            NantProcess.StartInfo.WorkingDirectory = "../../../../..";
            NantProcess.StartInfo.UseShellExecute = true;
            NantProcess.EnableRaisingEvents = true;
            NantProcess.StartInfo.ErrorDialog = true;

            if (!NantProcess.Start())
            {
                Debug.Print("failed to start " + NantProcess.StartInfo.FileName);
            }
            else
            {
                NantProcess.WaitForExit(60000);
                Debug.Print("OS says nant process is finished");
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