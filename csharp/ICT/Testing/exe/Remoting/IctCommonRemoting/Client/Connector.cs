//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Collections;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Tests.IctCommonRemoting.Interface;

namespace Tests.IctCommonRemoting.Client
{
    /// <summary>
    /// The TConnector class is responsible for opening a connection to the
    /// PetraServer's ClientManager and to retrieve Remoting Proxy objects for
    /// the Server-side .NET Remoting Sponsor and several other remoted objects from
    /// the PetraServer.
    /// </summary>
    public class TConnector : TConnectorBase
    {
        /// <summary>
        /// Opens a .NET Remoting connection to the Test Server's ClientManager.
        ///
        /// </summary>
        /// <param name="ConfigFile">not used, we have hardcoded the connection</param>
        /// <param name="ARemote">.NET Remoting Proxy object for the ClientManager object
        /// </param>
        /// <returns>void</returns>
        public override void GetRemoteServerConnection(string ConfigFile, out IClientManagerInterface ARemote)
        {
            ARemote = null;
            try
            {
                if (!FRemotingConfigurationSetup)
                {
                    // The following call must be done only once while the application runs (otherwise a RemotingException occurs)
                    IClientChannelSinkProvider TCPSink = new BinaryClientFormatterSinkProvider();

                    Hashtable ChannelProperties = new Hashtable();

                    TcpChannel Channel = new TcpChannel(ChannelProperties, TCPSink, null);
                    ChannelServices.RegisterChannel(Channel, false);

                    RemotingConfiguration.RegisterWellKnownClientType(
                        typeof(IClientManagerInterface),
                        String.Format("tcp://localhost:{0}/Clientmanager", 9000));

                    FRemotingConfigurationSetup = true;
                }

                ARemote = (IClientManagerInterface)TRemotingHelper.GetObject(typeof(IClientManagerInterface));

                if (ARemote == null)
                {
                    // do nothing
                }
                else
                {
#if DEBUGMODE
                    TLogging.Log("GetRemoteServerConnection: connected.", TLoggingType.ToLogfile);
#endif
                }
            }
            catch (Exception exp)
            {
                TLogging.Log("Error in GetRemoteServerConnection(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a Remoting Proxy object for the Server-side MyService namespace
        ///
        /// @comment The MyService Namespace holds client-instantiable objects for the
        /// Petra Common Module.
        ///
        /// </summary>
        /// <param name="RemotingURL">The Server-assigned URL for the MyService namespace object</param>
        /// <param name="ARemote">.NET Remoting Proxy object for the MyService namespace object
        /// </param>
        /// <returns>void</returns>
        public void GetRemoteMyServiceObject(string RemotingURL, out IMyService ARemote)
        {
            string strTCP;
            string strServer;

            ARemote = null;
            strServer = null;
#if DEBUGMODE
            TLogging.Log("Entering GetRemoteMyServiceObject()...", TLoggingType.ToLogfile);
#endif
            try
            {
                strServer = DetermineServerIPAddress() + ':' + ServerIPPort.ToString();
                strTCP = (("tcp://" + strServer) + '/' + RemotingURL);
#if DEBUGMODE
                TLogging.Log("Connecting to: " + strTCP, TLoggingType.ToLogfile);
#endif
                ARemote = (IMyService)RemotingServices.Connect(typeof(IMyService), strTCP);

                if (ARemote == null)
                {
                    TLogging.Log("GetRemoteMyServiceObject: Connection failed!", TLoggingType.ToLogfile);
                }
                else
                {
#if DEBUGMODE
                    TLogging.Log("GetRemoteMyServiceObject: connected.", TLoggingType.ToLogfile);
#endif
                }
            }
            catch (Exception exp)
            {
                TLogging.Log("Error in GetRemoteMyServiceObject(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                throw;
            }
        }
    }
}