//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Tests.HTTPRemoting.Interface;
using Tests.HTTPRemoting.Client;

namespace Ict.Testing.HTTPRemoting.Client
{
    class Client
    {
        static void Main(string[] args)
        {
            new TLogging("../../log/TestRemotingClient.log");

            try
            {
                new TAppSettingsManager("../../etc/Client.config");

                TLogging.DebugLevel = Convert.ToInt32(TAppSettingsManager.GetValue("Client.DebugLevel", "0"));

                new TClientSettings();

                // initialize the client
                new TRemoteTest();

                // need to call this as well to make the progress dialog work
                new TRemote();

                // allow self signed ssl certificate for test purposes
                ServicePointManager.ServerCertificateValidationCallback = delegate {
                    return true;
                };

                THttpConnector.InitConnection(TAppSettingsManager.GetValue("OpenPetra.HTTPServer"));

                TClientInfo.InitializeUnit();

                Catalog.Init("en-GB", "en-GB");

                SortedList <string, object>Parameters = new SortedList <string, object>();
                Parameters.Add("username", "demo");
                Parameters.Add("password", "demo");
                Parameters.Add("version", TFileVersionInfo.GetApplicationVersion().ToString());

                List <object>ResultList = THttpConnector.CallWebConnector("SessionManager", "LoginClient", Parameters, "list");
                eLoginEnum Result = (eLoginEnum)ResultList[0];

                if (Result != eLoginEnum.eLoginSucceeded)
                {
                    // failed login
                    return;
                }

                IMyUIConnector MyUIConnector = TRemoteTest.MyService.SubNamespace.MyUIConnector();
                TRemoteTest.TMyService.TMySubNamespace test = TRemoteTest.MyService.SubNamespace;

                while (true)
                {
                    try
                    {
                        TLogging.Log("before call");
                        TLogging.Log(TRemoteTest.MyService.HelloWorld("Hello World"));
                        TLogging.Log("after call");
                    }
                    catch (Exception e)
                    {
                        TLogging.Log("problem with MyService HelloWorld: " + Environment.NewLine + e.ToString());
                    }

                    try
                    {
                        DateTime DateTomorrow;
                        TLogging.Log("should show today's date: " + TRemoteTest.MyService.TestDateTime(DateTime.Today,
                                out DateTomorrow).ToShortDateString());
                        TLogging.Log("should show tomorrow's date: " + DateTomorrow.ToShortDateString());
                    }
                    catch (Exception e)
                    {
                        TLogging.Log("problem with TestDateTime: " + Environment.NewLine + e.ToString());
                    }

                    try
                    {
                        TLogging.Log(test.HelloSubWorld("Hello SubWorld"));
                    }
                    catch (Exception e)
                    {
                        TLogging.Log("problem with sub namespace HelloSubWorld: " + Environment.NewLine + e.ToString());
                    }

                    try
                    {
                        TLogging.Log(MyUIConnector.HelloWorldUIConnector());
                    }
                    catch (Exception e)
                    {
                        TLogging.Log("problem with HelloWorldUIConnector: " + Environment.NewLine + e.ToString());
                    }

                    // start long running job
                    Thread t = new Thread(() => TRemoteTest.MyService.LongRunningJob());

                    using (TProgressDialog dialog = new TProgressDialog(t))
                    {
                        if (dialog.ShowDialog() == DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    Console.WriteLine("Press ENTER to say Hello World again... ");
                    Console.WriteLine("Press CTRL-C to exit ...");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                Console.ReadLine();
            }
        }
    }
}