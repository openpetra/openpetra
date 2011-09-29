//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters;
using Ict.Common;
using Ict.Petra.Shared.Interfaces.ServerAdminInterface;
using Ict.Petra.Shared.RemotingSinks.Encryption;

namespace Ict.Petra.ServerAdmin.App.Core
{
    /// <summary>
    /// The TConnector class is responsible for opening a connection to the
    /// PetraServer's ServerManager.
    /// </summary>
    public class TConnector
    {
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
                if (TAppSettingsManager.HasValue("Server.IPBasePort"))
                {
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
                        typeof(Ict.Petra.Shared.Interfaces.ServerAdminInterface.IServerAdminInterface),
                        String.Format("tcp://localhost:{0}/Servermanager", TAppSettingsManager.GetValue("Server.IPBasePort")));
                }
                else
                {
                    RemotingConfiguration.Configure(ConfigFile, false);
                }

                iRemote = (IServerAdminInterface)(Ict.Common.TRemotingHelper.GetObject(typeof(IServerAdminInterface)));

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
    }
}