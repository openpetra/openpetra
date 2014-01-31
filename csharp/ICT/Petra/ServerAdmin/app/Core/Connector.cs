//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Sinks.Encryption;

namespace Ict.Petra.ServerAdmin.App.Core
{
    /// <summary>
    /// The TConnector class is responsible for opening a connection to the
    /// PetraServer's ServerManager.
    /// </summary>
    public class TConnector
    {
        private String FServerIPAddr = string.Empty;
        private String FServerPort = string.Empty;

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ConfigFile"></param>
        /// <param name="iRemote"></param>
        public void GetServerConnection(string ConfigFile, out IServerAdminInterface iRemote)
        {
            iRemote = null;
            try
            {
                if (TAppSettingsManager.HasValue("Server.Port"))
                {
                    DetermineServerIPAddress();

                    IClientChannelSinkProvider TCPSink = new BinaryClientFormatterSinkProvider();

                    if (TAppSettingsManager.HasValue("Server.ChannelEncryption.PublicKeyfile"))
                    {
                        Hashtable properties = new Hashtable();
                        properties.Add("FilePublicKeyXml", TAppSettingsManager.GetValue("Server.ChannelEncryption.PublicKeyfile"));

                        TCPSink.Next = new EncryptionClientSinkProvider(properties, null);
                    }

                    Hashtable ChannelProperties = new Hashtable();

                    TcpChannel Channel = new TcpChannel(ChannelProperties, TCPSink, null);
                    ChannelServices.RegisterChannel(Channel, false);

                    RemotingConfiguration.RegisterWellKnownClientType(
                        typeof(IServerAdminInterface),
                        String.Format("tcp://{0}:{1}/Servermanager", FServerIPAddr, FServerPort));
                }
                else
                {
                    RemotingConfiguration.Configure(ConfigFile, false);

                    DetermineServerIPAddress();
                }

                iRemote = (IServerAdminInterface)
                          Activator.GetObject(typeof(IServerAdminInterface),
                    String.Format("tcp://{0}:{1}/Servermanager", FServerIPAddr, FServerPort));

                if ((iRemote != null) && (TLogging.DebugLevel > 0))
                {
                    TLogging.Log(("GetServerConnection: connected."));
                }
            }
            catch (Exception exp)
            {
                TLogging.Log(("Error in GetServerConnection(), Possible reasons :-" + exp.ToString()));
                throw;
            }
        }

        /// <summary>
        /// Determines the PetraServer's IP Address and port by parsing the .NET (Remoting) Configuration file.
        /// </summary>
        protected void DetermineServerIPAddress()
        {
            const String SERVERMANAGERENTRY = "Ict.Common.Remoting.Shared.IServerAdminInterface";

            FServerIPAddr = "localhost";

            if (TAppSettingsManager.HasValue("Server.Host"))
            {
                FServerIPAddr = TAppSettingsManager.GetValue("Server.Host");
            }

            if (TAppSettingsManager.HasValue("Server.Port"))
            {
                FServerPort = TAppSettingsManager.GetValue("Server.Port");
            }

            if (FServerPort == "")
            {
                // find entry for ClientManagerInterface in the RegisteredWellKnownClientTypes
                // and extract the Server IP address from it
                foreach (WellKnownClientTypeEntry entr in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
                {
                    if (entr.ObjectType.ToString() == SERVERMANAGERENTRY)
                    {
                        int indexServerIPAddrStart = entr.ObjectUrl.IndexOf("//") + 2;
                        FServerIPAddr =
                            entr.ObjectUrl.Substring(indexServerIPAddrStart, entr.ObjectUrl.IndexOf(':',
                                    indexServerIPAddrStart) - indexServerIPAddrStart);
                        int indexPortStart = entr.ObjectUrl.IndexOf(':', indexServerIPAddrStart) + 1;
                        FServerPort = entr.ObjectUrl.Substring(indexPortStart, entr.ObjectUrl.IndexOf('/', indexPortStart) - indexPortStart);
                    }
                }
            }

            if (FServerPort.Length == 0)
            {
                throw new EServerIPAddressNotFoundInConfigurationFileException(
                    "The IP Address of the PetraServer could " + "not be extracted from the .NET (Remoting) Configuration File (used '" +
                    SERVERMANAGERENTRY + "' entry " + "to look for the IP Address)!");
            }
        }
    }
}