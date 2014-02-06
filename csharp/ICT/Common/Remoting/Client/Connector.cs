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
using System.Runtime.Remoting.Lifetime;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Sinks.Encryption;

using Ict.Common.Exceptions;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// The TConnector class is responsible for opening a connection to the
    /// PetraServer's ClientManager and to retrieve Remoting Proxy objects for
    /// the Server-side .NET Remoting Sponsor and several other remoted objects from
    /// the PetraServer.
    /// </summary>
    public class TConnector
    {
        /// <summary>
        /// the single instance for this client
        /// </summary>
        public static TConnector TheConnector = null;

        private String FServerIPAddr = string.Empty;
        private String FServerPort = string.Empty;
        private String FCrossDomainURL = string.Empty;
        private String FClientID = string.Empty;

        /// <summary>
        /// the minimum debuglevel which should display log messages for the connector
        /// </summary>
        protected const int CONNECTOR_LOGGING = 2;

        /// <summary>
        /// if you want to overwrite GetRemoteServerConnection, you need to be able to set this variable.
        /// can only connect once.
        /// </summary>
        protected Boolean FRemotingConfigurationSetup = false;

        /// <summary>
        /// initialize connection parameters
        /// </summary>
        public void Init(string ACrossDomainURI, string AClientID)
        {
            DetermineServerIPAddress();
            FCrossDomainURL = "tcp://" + FServerIPAddr + ":" + FServerPort + "/" + ACrossDomainURI;
            FClientID = AClientID;
            TheConnector = this;
        }

        /// <summary>
        /// Opens a .NET Remoting connection to the PetraServer's ClientManager.
        ///
        /// </summary>
        /// <param name="ConfigFile">File name of the .NET (Remoting) Configuration file</param>
        /// <param name="ARemote">.NET Remoting Proxy object for the ClientManager object
        /// </param>
        /// <returns>void</returns>
        public virtual void GetRemoteServerConnection(string ConfigFile, out IClientManagerInterface ARemote)
        {
            ARemote = null;
            try
            {
                if (!FRemotingConfigurationSetup)
                {
                    FRemotingConfigurationSetup = true;

                    if (TAppSettingsManager.HasValue("Server.Port"))
                    {
                        IChannel[] regChannels = ChannelServices.RegisteredChannels;

                        foreach (IChannel ch in regChannels)
                        {
                            ChannelServices.UnregisterChannel(ch);
                        }

                        ChannelServices.RegisterChannel(new TcpChannel(0), false);
                    }
                    else if (TAppSettingsManager.HasValue("OpenPetra.ChannelEncryption.PublicKeyfile"))
                    {
                        IClientChannelSinkProvider TCPSink = new BinaryClientFormatterSinkProvider();

                        Hashtable properties = new Hashtable();
                        properties.Add("HttpsPublicKeyXml", TAppSettingsManager.GetValue("OpenPetra.ChannelEncryption.PublicKeyfile"));

                        TCPSink.Next = new EncryptionClientSinkProvider(properties);

                        Hashtable ChannelProperties = new Hashtable();

                        TcpChannel Channel = new TcpChannel(ChannelProperties, TCPSink, null);
                        ChannelServices.RegisterChannel(Channel, false);
                    }
                    else
                    {
                        // The following call must be done only once while the application runs (otherwise a RemotingException occurs)
                        RemotingConfiguration.Configure(ConfigFile, false);
                    }
                }

                DetermineServerIPAddress();

                ARemote = (IClientManagerInterface)
                          Activator.GetObject(typeof(IClientManagerInterface),
                    String.Format("tcp://{0}:{1}/Clientmanager", FServerIPAddr, FServerPort));

                if (ARemote != null)
                {
                    if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                    {
                        TLogging.Log("GetRemoteServerConnection: connected.", TLoggingType.ToLogfile);
                    }
                }
            }
            catch (Exception exp)
            {
                TLogging.Log("Error in GetRemoteServerConnection(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a Remoting Proxy object for a Server-side object
        /// </summary>
        /// <param name="RemotingURL">The Server-assigned URL for the remote object</param>
        /// <param name="ARemoteType"></param>
        /// <returns>.NET Remoting Proxy object for the remote object</returns>
        public IInterface GetRemoteObject(string RemotingURL, Type ARemoteType)
        {
            try
            {
                if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                {
                    TLogging.Log("Connecting to: " + FCrossDomainURL + ":" + RemotingURL, TLoggingType.ToLogfile);
                }

                IInterface RemoteObject = (IInterface) new CustomProxy(FCrossDomainURL, RemotingURL, FClientID, ARemoteType).GetTransparentProxy();

                if (RemoteObject == null)
                {
                    TLogging.Log("GetRemoteObject(" + RemotingURL + ", " + ARemoteType.ToString() + "): Connection failed!", TLoggingType.ToLogfile);
                }
                else if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                {
                    TLogging.Log("GetRemoteObject: connected " + RemotingURL, TLoggingType.ToLogfile);
                }

                return RemoteObject;
            }
            catch (Exception exp)
            {
                TLogging.Log(
                    "Error in GetRemoteObject(" + RemotingURL + ", " + ARemoteType.ToString() + "), Possible reasons :-" + exp.ToString(),
                    TLoggingType.ToLogfile);
                throw;
            }
        }

        /// <summary>
        /// Determines the PetraServer's IP Address and port by parsing the .NET (Remoting) Configuration file.
        /// </summary>
        protected void DetermineServerIPAddress()
        {
            const String CLIENTMANAGERENTRY = "Ict.Common.Remoting.Shared.IClientManagerInterface";

            string strServerIPAddr = string.Empty;

            if (TAppSettingsManager.HasValue("Server.Host"))
            {
                FServerIPAddr = TAppSettingsManager.GetValue("Server.Host");
            }

            if (TAppSettingsManager.HasValue("Server.Port"))
            {
                FServerPort = TAppSettingsManager.GetValue("Server.Port");
            }

            if (FServerIPAddr == "")
            {
                // find entry for ClientManagerInterface in the RegisteredWellKnownClientTypes
                // and extract the Server IP address from it
                foreach (WellKnownClientTypeEntry entr in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
                {
                    if (entr.ObjectType.ToString() == CLIENTMANAGERENTRY)
                    {
                        int indexServerIPAddrStart = entr.ObjectUrl.IndexOf("//") + 2;
                        strServerIPAddr =
                            entr.ObjectUrl.Substring(indexServerIPAddrStart, entr.ObjectUrl.IndexOf(':',
                                    indexServerIPAddrStart) - indexServerIPAddrStart);
                        int indexPortStart = entr.ObjectUrl.IndexOf(':', indexServerIPAddrStart) + 1;
                        FServerPort = entr.ObjectUrl.Substring(indexPortStart, entr.ObjectUrl.IndexOf('/', indexPortStart) - indexPortStart);
                    }
                }

                FServerIPAddr = strServerIPAddr;
            }

            if (FServerIPAddr.Length == 0)
            {
                throw new EServerIPAddressNotFoundInConfigurationFileException(
                    "The IP Address of the PetraServer could " + "not be extracted from the .NET (Remoting) Configuration File (used '" +
                    CLIENTMANAGERENTRY + "' entry " + "to look for the IP Address)!");
            }
        }
    }

    #region EServerIPAddressNotFoundInConfigurationFileException

    /// <summary>
    /// Thrown if the IP Address of the PetraServer could not be extracted from the .NET (Remoting) Configuration File.
    /// </summary>
    public class EServerIPAddressNotFoundInConfigurationFileException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EServerIPAddressNotFoundInConfigurationFileException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EServerIPAddressNotFoundInConfigurationFileException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AConnectionString">Connection String.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EServerIPAddressNotFoundInConfigurationFileException(string AConnectionString, Exception AInnerException) : base(AConnectionString,
                                                                                                                               AInnerException)
        {
        }
    }

    #endregion
}