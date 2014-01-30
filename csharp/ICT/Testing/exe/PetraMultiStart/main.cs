//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timotheusp, ChristianK (C# translation, adaption to OpenPetra)
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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Xml;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.ServerAdmin.App.Core;

namespace PetraMultiStart
{
/// <summary>
/// Main Class of the Commandline Program.
/// </summary>
public class main
{
    private static TXMLParser parser;
    private static System.Random rnd;
    private static IServerAdminInterface TRemote;
    private static Ict.Petra.ServerAdmin.App.Core.TConnector TheConnector;

    /// <summary>
    /// Returns milliseconds from a XML Value.
    /// </summary>
    /// <param name="AXMLValue"></param>
    /// <returns></returns>
    public static Int64 GetMilliSeconds(double AXMLValue)
    {
        Int64 ReturnValue;

        if (AXMLValue >= 1)
        {
            ReturnValue = Convert.ToInt64(AXMLValue * 60 * 1000);
        }
        else
        {
            ReturnValue = Convert.ToInt64(AXMLValue * 100 * 1000);
        }

        return ReturnValue;
    }

    /// <summary>
    /// Kill all instances of a Process by name
    /// </summary>
    /// <param name="name">name of the process to kill</param>
    /// <example>
    /// KillAllProcesses( "Outlook" );
    /// </example>
    public static void KillAllProcesses(String name)
    {
        Process[] processes = Process.GetProcessesByName(name);

        foreach (Process p in processes)
        {
            Console.WriteLine("{0}: try killing {1}", DateTime.Now.ToLongTimeString(), p.MainWindowTitle);
            p.Kill();
        }
    }

    /// <summary>
    /// Returns the duration of a break. The duration will be fixed if there is no 'random' Attribute
    /// and will be randomised by the amount specified in the 'random' Attribute if it is specified.
    /// </summary>
    /// <param name="ACursor"></param>
    /// <returns>Duration of a Break.</returns>
    public static Int64 RandomBreak(XmlNode ACursor)
    {
        Int64 ReturnValue;
        Int64 time;
        Int64 deviation;

        time = GetMilliSeconds(TXMLParser.GetDoubleAttribute(ACursor, "time"));

        deviation = GetMilliSeconds(TXMLParser.GetDoubleAttribute(ACursor, "random"));

        if (deviation > 0)
        {
            ReturnValue = rnd.Next((Int32)(time - deviation), (Int32)(time + deviation));
        }
        else
        {
            ReturnValue = time;
        }

        return ReturnValue;
    }

    /// <summary>
    /// Tells whether the Server is still running.
    /// </summary>
    /// <returns></returns>
    [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", 
     Justification="We use the following server call ONLY for an 'side effect' but since we are inquiring a Property its value must be assigned to a variable. [christiank]", 
     MessageId="Tmp")]
    public static bool ServerStillRunning()
    {
        bool ReturnValue = true;

        try
        {
            // We use the following server call ONLY for an 'side effect' - namely when it throws an Exception! 
            int Tmp = TRemote.ClientsConnected;  // Causes: CA1804:RemoveUnusedLocals (but is suppressed for that reason with the SuppressMessage Attribute!)
        }
        catch (Exception exp)
        {
            System.Console.WriteLine("{0}: Exception: {1}", DateTime.Now.ToLongTimeString(), exp.Message);
            System.Console.WriteLine("{0}: Stopping the test", DateTime.Now.ToLongTimeString());

            ReturnValue = false;
        }

        return ReturnValue;
    }

    /// <summary>
    /// Executes the tests (main method of this executable!).
    /// </summary>
    public static void RunTest()
    {
        XmlNode startNode;
        XmlNode curGroup;
        Thread groupThread;
        TestGroup myGroup;
        String testcase;

        new TAppSettingsManager(true);

        testcase = TAppSettingsManager.GetValue("testcase");
        Global.StartClientID = TAppSettingsManager.GetInt16("startclientid");

        rnd = new System.Random(DateTime.Now.Millisecond);     // Init

        try
        {
            parser = new TXMLParser(TAppSettingsManager.GetValue("testscript"), false);
            startNode = parser.GetDocument().DocumentElement;
        }
        catch (Exception E)
        {
            System.Console.WriteLine("{0}: trouble in RunTest", DateTime.Now.ToLongTimeString());
            System.Console.WriteLine("{0}: {1}", DateTime.Now.ToLongTimeString(), E.Message);

            return;
        }

        new TLogging(@"..\..\log\PetraMultiStart.log");

        TheConnector = new Ict.Petra.ServerAdmin.App.Core.TConnector();
        TheConnector.GetServerConnection(TAppSettingsManager.ConfigFileName, out TRemote);

        CreateTestUsers();

        if (startNode.Name.ToLower() == "tests")
        {
            startNode = startNode.FirstChild;

            while ((startNode != null) && (startNode.Name.ToLower() == "test") && (TXMLParser.GetAttribute(startNode, "name") != testcase))
            {
                startNode = startNode.NextSibling;
            }
        }

        if (startNode == null)
        {
            Console.WriteLine("{0}: cannot find testcase {1}", DateTime.Now.ToLongTimeString(), testcase);

            return;
        }

        while (true)
        {
            // restart the whole test scenario

            if (startNode.Name.ToLower() == "test")
            {
                Global.Filename = TXMLParser.GetAttribute(startNode, "app");

                // kill instances of previous test
                KillAllProcesses(Global.Filename.Substring(0, Global.Filename.IndexOf('.')));

                Global.Configfile = TXMLParser.GetAttribute(startNode, "config");
                curGroup = startNode.FirstChild;

                while ((curGroup != null) && (curGroup.Name == "clientgroup"))
                {
                    if (TXMLParser.GetBoolAttribute(curGroup, "active", true) != false)
                    {
                        myGroup = new TestGroup(curGroup);
                        groupThread = new Thread(myGroup.Run);
                        groupThread.Start();
                    }

                    curGroup = curGroup.NextSibling;
                }
            }

            Thread.CurrentThread.Join();
            System.Console.WriteLine("{0}: All threads have stopped", DateTime.Now.ToLongTimeString());

            if (TXMLParser.GetBoolAttribute(startNode, "loop", true) == false)
            {
                return;
            }

            Thread.Sleep(5 * 60 * 1000);

            // wait for 5 minutes before restarting
        }
    }

    /// <summary>
    /// Creates Test Users. No error occurs if these Test Users already exist,
    /// </summary>
    private static void CreateTestUsers()
    {
        const int NUMBEROFTESTUSERS = 40;

        int UsersCreated = 0;

        Console.WriteLine("Setting up " + NUMBEROFTESTUSERS.ToString() + " Test Users (if necessary)...");

        for (int Counter = 0; Counter < NUMBEROFTESTUSERS; Counter++)
        {
            if (TRemote.AddUser("TESTUSER" + Counter.ToString(), "test"))
            {
                UsersCreated++;
            }
        }

        Console.WriteLine("Finished setting up Test Users (" + UsersCreated.ToString() + " users created.)");
    }
}
}