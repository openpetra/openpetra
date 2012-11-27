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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Tests.IctCommonRemoting.Interface;
using Tests.IctCommonRemoting.Client;

namespace Ict.Testing.IctCommonRemoting.Client
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
                TConnectionManagementBase.ConnectorType = typeof(TConnector);
                TConnectionManagementBase.GConnectionManagement = new TConnectionManagement();

                TClientInfo.InitializeUnit();

                Catalog.Init("en-GB", "en-GB");

                string error;
                ConnectToTestServer("DEMO", "DEMO", out error);

                IMyUIConnector MyUIConnector = TRemote.MyService.SubNamespace.MyUIConnector();
                TEnsureKeepAlive.Register(MyUIConnector);
                IMySubNamespace test = TRemote.MyService.SubNamespace;

                while (true)
                {
                    try
                    {
                        TLogging.Log(TRemote.MyService.HelloWorld("Hello World"));
                    }
                    catch (Exception e)
                    {
                        TLogging.Log("problem with MyService HelloWorld: " + Environment.NewLine + e.ToString());
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

        static private bool ConnectToTestServer(String AUserName, String APassWord, out String AError)
        {
            bool ReturnValue = false;

            AError = "";
            try
            {
                int ProcessID;
                string WelcomeMessage;
                bool SystemEnabled;

                ReturnValue = ((TConnectionManagement)TConnectionManagement.GConnectionManagement).ConnectToServer(AUserName, APassWord,
                    out ProcessID,
                    out WelcomeMessage,
                    out SystemEnabled,
                    out AError);
            }
            catch (EClientVersionMismatchException exp)
            {
                TLogging.Log(exp.Message + "Petra Client/Server Program Version Mismatch!");
                return false;
            }
            catch (ELoginFailedServerTooBusyException)
            {
                TLogging.Log("The PetraServer is too busy to accept the Login request.");

                return false;
            }
            catch (EServerConnectionServerNotReachableException)
            {
                TLogging.Log("The PetraServer cannot be reached!");

                return false;
            }
            catch (EServerConnectionGeneralException exp)
            {
                TLogging.Log(
                    Catalog.GetString("An error occurred while trying to connect to the OpenPetra Server!") + Environment.NewLine + exp.ToString(), TLoggingType.ToLogfile);

                return false;
            }
            catch (Exception exp)
            {
                TLogging.Log(
                    Catalog.GetString("An error occurred while trying to connect to the OpenPetra Server!") + Environment.NewLine + exp.ToString(), TLoggingType.ToLogfile);

                return false;
            }

            // TODO: exception for authentication failure
            // TODO: exception for retired user
            return ReturnValue;
        }
    }
}