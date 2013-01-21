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
using System.Collections;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using Tests.IctCommonRemoting.Interface;
using Tests.IctCommonRemoting.Server;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Sinks.Encryption;

namespace Ict.Testing.IctCommonRemoting.Server
{
    class Server
    {
        static private TServerManager TheServerManager;
        static private TCrossDomainMarshaller TheCrossDomainMarshaller;

        /// <summary>
        /// Starts the Test Server.
        /// </summary>
        /// <returns>void</returns>
        static private void StartupTestServer()
        {
            try
            {
                new TAppSettingsManager("../../etc/Server.config");

                TheServerManager = new TServerManager();

                // Ensure Logging and an 'ordered cooperative shutdown' in case of an Unhandled Exception
                TheServerManager.HookupProperShutdownProcessing();

                Console.WriteLine();
                TLogging.Log(TheServerManager.ServerInfoVersion);

                EstablishRemoting();

                Thread.Sleep(50);

                // Display information that the Server is ready to accept .NET Remoting requests
                TLogging.Log(TheServerManager.ServerInfoState);

                //
                // Server startup done.
                // From now on just listen on .NET Remoting Framework object invocations or on
                // menu commands...
                //

                // All exceptions that are raised from various parts of the Server are handled below.
                // Note: The Server stops after handling these exceptions!!!
                RunInBackground();

                // THE VERY END OF THE SERVER :(
            }
            catch (System.Net.Sockets.SocketException exp)
            {
                TLogging.Log(
                    Environment.NewLine + "Unable to start the Server: The IP Port " + TSrvSetting.IPBasePort.ToString() +
                    " is being used by a different instance of the Server or some other application." + Environment.NewLine + exp.ToString());
            }
            catch (System.Runtime.Remoting.RemotingException exp)
            {
                System.Diagnostics.Debug.WriteLine(exp.ToString());
                TLogging.Log(Environment.NewLine + "Exception occured while setting up Remoting Framework:" + Environment.NewLine + exp.ToString());
            }
            catch (ApplicationException exp)
            {
                // This Exception is used if no more messages shall be done ...
                TLogging.Log(exp.ToString());
            }
            catch (Exception exp)
            {
                TLogging.Log(Environment.NewLine + "Exception occured:" + Environment.NewLine + exp.ToString());
            }
        }

        /// Remote the remoteable objects
        static private void EstablishRemoting()
        {
            try
            {
                BinaryServerFormatterSinkProvider TCPSink = new BinaryServerFormatterSinkProvider();
                TCPSink.TypeFilterLevel = TypeFilterLevel.Low;
                IServerChannelSinkProvider EncryptionSink = TCPSink;

                if (TAppSettingsManager.GetValue("Server.ChannelEncryption.PrivateKeyfile", "", false).Length > 0)
                {
                    EncryptionSink = new EncryptionServerSinkProvider();
                    EncryptionSink.Next = TCPSink;
                }

                Hashtable ChannelProperties = new Hashtable();
                ChannelProperties.Add("port", TSrvSetting.IPBasePort);

                TcpChannel Channel = new TcpChannel(ChannelProperties, null, EncryptionSink);
                ChannelServices.RegisterChannel(Channel, false);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(Tests.IctCommonRemoting.Server.TServerManager),
                    "Servermanager", WellKnownObjectMode.Singleton);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(Ict.Common.Remoting.Server.TClientManager),
                    "Clientmanager", WellKnownObjectMode.Singleton);

                TheCrossDomainMarshaller = new TCrossDomainMarshaller();
                RemotingServices.Marshal(TheCrossDomainMarshaller, TClientManager.CROSSDOMAINURL);

                // register the services, independent of the appdomain which is per client
            }
            catch (RemotingException rex)
            {
                if (rex.Message.IndexOf("SocketException") > 1)
                {
                    TLogging.Log("A SocketException has been thrown.");
                    TLogging.Log("Most probably problem is that the address port is used twice!");
                    throw new ApplicationException();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        static private void RunInBackground()
        {
            do
            {
                /*
                 * Infinite loop - ON PURPOSE !
                 * Server can only be shutdown from PetraServerAdminConsole...
                 */

                /*
                 * Server main Thread goes to sleep and never needs to wake up again -
                 * the PetraServer is only accessed through .NET Remoting and has no
                 * interaction with the Console anymore.
                 */
                Thread.Sleep(Timeout.Infinite);
            } while (!(false));
        }

        static void Main(string[] args)
        {
            try
            {
                StartupTestServer();
                Console.WriteLine("Press ENTER to exit ...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}