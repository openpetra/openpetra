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
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Security.Principal;
using System.IO;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Shared;
using System.Reflection;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// Manages the connection to the PetraServer.
    /// </summary>
    public class TConnectionManagementBase
    {
        /// <summary>
        /// static instance of this class
        /// </summary>
        public static TConnectionManagementBase GConnectionManagement = null;

        /// <summary>
        /// set to typeof(TConnector)
        /// </summary>
        public static Type ConnectorType = typeof(TConnector);

        /// <summary>
        /// keeps the connection to the server
        /// </summary>
        protected TConnector FConnector;

        /// <summary>
        /// the client manager
        /// </summary>
        protected IClientManagerInterface FClientManager;

        private String FClientName;
        private Int32 FClientID;
        private TExecutingOSEnum FServerOS;
        private String FRemotingURL_PollClientTasks;
        private IPollClientTasksInterface FRemotePollClientTasks;
        private TEnsureKeepAlive FKeepAlive;
        private TPollClientTasks FPollClientTasks;

        /// <summary>
        /// the remoting object
        /// </summary>
        protected TRemoteBase FRemote;

        /// <summary>
        /// the urls of the services
        /// </summary>
        protected Hashtable FRemotingURLs;

        /// <summary>
        /// we will always contact the server on this URL
        /// </summary>
        protected string FCrossDomainURI;

        /// <summary>todoComment</summary>
        public String ClientName
        {
            get
            {
                return FClientName;
            }
        }

        /// <summary>todoComment</summary>
        public Int32 ClientID
        {
            get
            {
                return FClientID;
            }
        }

        /// <summary>todoComment</summary>
        public TExecutingOSEnum ServerOS
        {
            get
            {
                return FServerOS;
            }
        }

        /// <summary>todoComment</summary>
        public TEnsureKeepAlive KeepAlive
        {
            get
            {
                return FKeepAlive;
            }
        }

        /// <summary>todoComment</summary>
        public TRemoteBase RemoteObjects
        {
            get
            {
                return FRemote;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public bool ConnectToServer(String AUserName,
            String APassword,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out String AError,
            out IPrincipal AUserInfo)
        {
            AError = "";
            String ConnectionError;

            if (FConnector == null)
            {
                FConnector = (TConnector)Activator.CreateInstance(ConnectorType);
            }

            try
            {
                if (TAppSettingsManager.ConfigFileName.Length > 0)
                {
                    // connect to the PetraServer's ClientManager
                    FConnector.GetRemoteServerConnection(TAppSettingsManager.ConfigFileName, out FClientManager);
                }
                else
                {
                    // connect to the PetraServer's ClientManager
                    FConnector.GetRemoteServerConnection(Environment.GetCommandLineArgs()[0] + ".config", out FClientManager);
                }

                // register Client session at the PetraServer
                bool ReturnValue = ConnectClient(AUserName, APassword, FClientManager,
                    out AProcessID,
                    out AWelcomeMessage,
                    out ASystemEnabled,
                    out ConnectionError,
                    out AUserInfo);
                TRemoteBase.ClientManager = FClientManager;

                if (!ReturnValue)
                {
                    AError = ConnectionError;
                    return ReturnValue;
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                throw new EServerConnectionServerNotReachableException();
            }
            catch (EDBConnectionNotEstablishedException exp)
            {
                if (exp.Message.IndexOf("Access denied") != -1)
                {
                    // Prevent passing out stack trace in case the PetraServer cannot connect
                    // a Client (to make this happen, somebody would have tampered with the
                    // DB Password decryption routines...)
                    throw new EServerConnectionGeneralException("PetraServer misconfiguration!");
                }
                else
                {
                    throw;
                }
            }
            catch (EClientVersionMismatchException)
            {
                throw;
            }
            catch (ELoginFailedServerTooBusyException)
            {
                throw;
            }
            catch (Exception exp)
            {
                TLogging.Log(exp.ToString() + Environment.NewLine + exp.StackTrace, TLoggingType.ToLogfile);
                throw new EServerConnectionGeneralException(exp.ToString());
            }

            //
            // acquire .NET Remoting Proxy objects for remoted Server objects
            //

            FRemotePollClientTasks =
                (IPollClientTasksInterface)FConnector.GetRemoteObject(FRemotingURL_PollClientTasks, typeof(IPollClientTasksInterface));

            //
            // start the KeepAlive Thread (which needs to run as long as the Client is running)
            //
            FKeepAlive = new TEnsureKeepAlive();

            //
            // start the PollClientTasks Thread (which needs to run as long as the Client is running)
            //
            FPollClientTasks = new TPollClientTasks(FClientID, FRemotePollClientTasks);

            //
            // initialise object that holds references to all our remote object .NET Remoting Proxies
            //
            FRemote = new TRemoteBase(FClientManager);
            return true;
        }

        /// <summary>
        /// connect the client
        /// </summary>
        /// <param name="AUserName"></param>
        /// <param name="APassword"></param>
        /// <param name="AClientManager"></param>
        /// <param name="AProcessID"></param>
        /// <param name="AWelcomeMessage"></param>
        /// <param name="ASystemEnabled"></param>
        /// <param name="AError"></param>
        /// <param name="AUserInfo"></param>
        /// <returns></returns>
        virtual protected bool ConnectClient(String AUserName,
            String APassword,
            IClientManagerInterface AClientManager,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out String AError,
            out IPrincipal AUserInfo)
        {
            AError = "";
            ASystemEnabled = false;
            AWelcomeMessage = "";
            AProcessID = -1;
            AUserInfo = null;

            try
            {
                AClientManager.ConnectClient(AUserName, APassword,
                    TClientInfo.ClientComputerName,
                    TClientInfo.ClientIPAddress,
                    new Version(TClientInfo.ClientAssemblyVersion),
                    DetermineClientServerConnectionType(),
                    out FClientName,
                    out FClientID,
                    out FCrossDomainURI,
                    out FRemotingURLs,
                    out FServerOS,
                    out AProcessID,
                    out AWelcomeMessage,
                    out ASystemEnabled,
                    out AUserInfo);

                if (FRemotingURLs.ContainsKey(RemotingConstants.REMOTINGURL_IDENTIFIER_POLLCLIENTTASKS))
                {
                    FRemotingURL_PollClientTasks = (String)FRemotingURLs[RemotingConstants.REMOTINGURL_IDENTIFIER_POLLCLIENTTASKS];
                }

                FConnector.Init(FCrossDomainURI, FClientID.ToString());

                return true;
            }
            catch (EUserNotExistantException exp)
            {
                AError = exp.Message;
                return false;
            }
            catch (EUserRetiredException exp)
            {
                AError = exp.Message;
                return false;
            }
            catch (EAccessDeniedException exp)
            {
                AError = exp.Message;
                return false;
            }
            catch (EUserRecordLockedException exp)
            {
                AError = exp.Message;
                return false;
            }
            catch (ESystemDisabledException exp)
            {
                AError = exp.Message;
                return false;
            }
            catch (EClientVersionMismatchException)
            {
                throw;
            }
            catch (ELoginFailedServerTooBusyException)
            {
                throw;
            }
            catch (Exception exp)
            {
                TLogging.Log(exp.ToString() + Environment.NewLine + exp.StackTrace, TLoggingType.ToLogfile);
                throw;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACantDisconnectReason"></param>
        /// <returns></returns>
        public Boolean DisconnectFromServer(out String ACantDisconnectReason)
        {
            Boolean ReturnValue = false;

            ACantDisconnectReason = "";
            try
            {
                if (FKeepAlive != null)
                {
                    TEnsureKeepAlive.StopKeepAlive();
                }

                if (FPollClientTasks != null)
                {
                    FPollClientTasks.StopPollClientTasks();
                    RemotingServices.Disconnect((MarshalByRefObject)FRemotePollClientTasks);
                }

                if (FRemote != null)
                {
                    ReturnValue = TRemoteBase.ClientManager.DisconnectClient(FClientID, out ACantDisconnectReason);
                    TRemoteBase.Disconnect();
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                throw;
            }
            catch (System.Runtime.Remoting.RemotingException)
            {
                throw;
            }
            return ReturnValue;
        }

        private TClientServerConnectionType DetermineClientServerConnectionType()
        {
            TClientServerConnectionType ReturnValue;

            if (TClientSettings.RunAsRemote)
            {
                ReturnValue = TClientServerConnectionType.csctRemote;
            }
            else if (TClientSettings.RunAsStandalone)
            {
                ReturnValue = TClientServerConnectionType.csctLocal;
            }
            else
            {
                ReturnValue = TClientServerConnectionType.csctLAN;
            }

            return ReturnValue;
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class EServerConnectionServerNotReachableException : ApplicationException
    {
        #region EServerConnectionServerNotReachableException

        /// <summary>
        /// constructor
        /// </summary>
        public EServerConnectionServerNotReachableException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public EServerConnectionServerNotReachableException(String msg) : base(msg)
        {
        }

        #endregion
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class EServerConnectionGeneralException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EServerConnectionGeneralException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public EServerConnectionGeneralException(String msg) : base(msg)
        {
        }
    }
}