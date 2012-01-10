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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// The TConnector class is responsible for opening a connection to the
    /// PetraServer's ClientManager and to retrieve Remoting Proxy objects for
    /// the Server-side .NET Remoting Sponsor and several other remoted objects from
    /// the PetraServer.
    /// </summary>
    public class TConnectorBase
    {
        private String FServerIPAddr = "";
        private System.Int16 FServerIPPort;

        /// <summary>
        /// if you want to overwrite GetRemoteServerConnection, you need to be able to set this variable.
        /// can only connect once.
        /// </summary>
        protected Boolean FRemotingConfigurationSetup;

        /// <summary>todoComment</summary>
        public String ServerIPAddr
        {
            get
            {
                return FServerIPAddr;
            }
        }

        /// <summary>todoComment</summary>
        public System.Int16 ServerIPPort
        {
            get
            {
                return FServerIPPort;
            }

            set
            {
                FServerIPPort = value;
            }
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

                    if (TAppSettingsManager.HasValue("Remote.Port"))
                    {
                        IChannel[] regChannels = ChannelServices.RegisteredChannels;

                        foreach (IChannel ch in regChannels)
                        {
                            ChannelServices.UnregisterChannel(ch);
                        }

                        ChannelServices.RegisterChannel(new TcpChannel(0), false);

                        ARemote = (IClientManagerInterface)
                                  Activator.GetObject(typeof(Ict.Common.Remoting.Shared.IClientManagerInterface),
                            String.Format("tcp://{0}:{1}/Clientmanager",
                                TAppSettingsManager.GetValue("Remote.Host"),
                                TAppSettingsManager.GetValue("Remote.Port")));
                    }
                    else
                    {
                        // The following call must be done only once while the application runs (otherwise a RemotingException occurs)
                        RemotingConfiguration.Configure(ConfigFile, false);
                        ARemote = (IClientManagerInterface)
                                  TRemotingHelper.GetObject(typeof(IClientManagerInterface));
                    }
                }

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
        /// Retrieves a Remoting Proxy object for the Server-side TPollClientTasks object
        ///
        /// </summary>
        /// <param name="RemotingURL">The Server-assigned URL for the Remoting Sponsor object</param>
        /// <param name="ARemote">.NET Remoting Proxy object for the Remoting Sponsor object
        /// </param>
        /// <returns>void</returns>
        public void GetRemotePollClientTasks(string RemotingURL, out IPollClientTasksInterface ARemote)
        {
            string strTCP;
            string strServer;

            ARemote = null;
            strServer = null;
#if DEBUGMODE
            TLogging.Log("Entering GetRemotePollClientTasks()...", TLoggingType.ToLogfile);
#endif
            try
            {
                strServer = DetermineServerIPAddress() + ':' + FServerIPPort.ToString();
                strTCP = (("tcp://" + strServer) + '/' + RemotingURL);
#if DEBUGMODE
                TLogging.Log("Connecting to: " + strTCP, TLoggingType.ToLogfile);
#endif
                ARemote = (IPollClientTasksInterface)RemotingServices.Connect(typeof(IPollClientTasksInterface), strTCP);

                if (ARemote == null)
                {
                    TLogging.Log("GetRemotePollClientTasks: Connection failed!", TLoggingType.ToLogfile);
                }
                else
                {
#if DEBUGMODE
                    TLogging.Log("GetRemotePollClientTasks: connected.", TLoggingType.ToLogfile);
#endif
                }
            }
            catch (Exception exp)
            {
                TLogging.Log("Error in GetRemotePollClientTasks(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                throw;
            }
        }

        /// <summary>
        /// Determines the PetraServer's IP Address by parsing the .NET (Remoting)
        /// Configuration file.
        ///
        /// </summary>
        /// <returns>The IP Address of the PetraServer that we connect to
        /// </returns>
        protected String DetermineServerIPAddress()
        {
            const String CLIENTMANAGERENTRY = "Ict.Common.Remoting.Shared.IClientManagerInterface";

            System.Int16 strServerIPAddrStart;
            string strServerIPAddr = "";

            if (TAppSettingsManager.HasValue("Remote.Host"))
            {
                FServerIPAddr = TAppSettingsManager.GetValue("Remote.Host");
            }

            if (FServerIPAddr == "")
            {
                // find entry for ClientManagerInterface in the RegisteredWellKnownClientTypes
                // and extract the Server IP address from it
                foreach (WellKnownClientTypeEntry entr in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
                {
                    if (entr.ObjectType.ToString() == CLIENTMANAGERENTRY)
                    {
                        strServerIPAddrStart = (short)(entr.ObjectUrl.IndexOf("//") + 2);
                        strServerIPAddr =
                            entr.ObjectUrl.Substring(strServerIPAddrStart, (entr.ObjectUrl.IndexOf(':', strServerIPAddrStart) - strServerIPAddrStart));
                    }
                }

                FServerIPAddr = strServerIPAddr;
            }

            if (FServerIPAddr == "")
            {
                throw new ServerIPAddressNotFoundInConfigurationFileException(
                    "The IP Address of the PetraServer could " + "not be extracted from the .NET (Remoting) Configuration File (used '" +
                    CLIENTMANAGERENTRY + "' entry " + "to look for the IP Address)!");
            }

            return FServerIPAddr;
        }
    }

    /// <summary>
    /// Thrown if the IP Address of the PetraServer could not be extracted from the .NET (Remoting) Configuration File.
    /// </summary>
    public class ServerIPAddressNotFoundInConfigurationFileException : ApplicationException
    {
        #region ServerIPAddressNotFoundInConfigurationFileException

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="inExp"></param>
        public ServerIPAddressNotFoundInConfigurationFileException(string strConnectionString, Exception inExp) : base(strConnectionString, inExp)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ServerIPAddressNotFoundInConfigurationFileException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AMessage"></param>
        public ServerIPAddressNotFoundInConfigurationFileException(string AMessage) : base(AMessage)
        {
        }

        #endregion
    }
}