//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Security.Principal;
using System.IO;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.Interfaces.MConference;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.Interfaces.MPersonnel;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.Interfaces.MReporting;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Reflection;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Shared.MSysMan;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Manages the connection to the PetraServer.
    /// </summary>
    public class TConnectionManagement
    {
        /// <summary>
        /// static instance of this class
        /// </summary>
        public static TConnectionManagement GConnectionManagement = new TConnectionManagement();

        private TConnector FConnector;
        private IClientManagerInterface FClientManager;
        private String FClientName;
        private Int32 FClientID;
        private Int16 FRemotingPort;
        private TExecutingOSEnum FServerOS;
        private String FRemotingURL_PollClientTasks;
        private String FRemotingURL_MConference;
        private String FRemotingURL_MPartner;
        private String FRemotingURL_MPersonnel;
        private String FRemotingURL_MCommon;
        private String FRemotingURL_MFinance;
        private String FRemotingURL_MReporting;
        private String FRemotingURL_MSysMan;
        private IPollClientTasksInterface FRemotePollClientTasks;
        private IMCommonNamespace FRemoteCommonObjects;
        private IMConferenceNamespace FRemoteConferenceObjects;
        private IMPartnerNamespace FRemotePartnerObjects;
        private IMPersonnelNamespace FRemotePersonnelObjects;
        private IMFinanceNamespace FRemoteFinanceObjects;
        private IMReportingNamespace FRemoteReportingObjects;
        private IMSysManNamespace FRemoteSysManObjects;
        private TEnsureKeepAlive FKeepAlive;
        private TPollClientTasks FPollClientTasks;
        private TRemote FRemote;

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
        public String ServerIPAddr
        {
            get
            {
                return Get_ServerIPAddr();
            }
        }

        /// <summary>todoComment</summary>
        public System.Int16 ServerIPPort
        {
            get
            {
                return Get_ServerIPPort();
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
        public TRemote RemoteObjects
        {
            get
            {
                return FRemote;
            }
        }

        /// <summary>todoComment</summary>
        public String Get_ServerIPAddr()
        {
            return FConnector.ServerIPAddr;
        }

        /// <summary>todoComment</summary>
        public System.Int16 Get_ServerIPPort()
        {
            return FConnector.ServerIPPort;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserName"></param>
        /// <param name="APassword"></param>
        /// <param name="AProcessID"></param>
        /// <param name="AWelcomeMessage"></param>
        /// <param name="ASystemEnabled"></param>
        /// <param name="AError"></param>
        /// <returns></returns>
        public bool ConnectToServer(String AUserName,
            String APassword,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out String AError)
        {
            AError = "";
            bool ReturnValue;
            String ConnectionError;

            if (FConnector == null)
            {
                FConnector = new TConnector();
            }

            try
            {
                if (TClientSettings.ConfigurationFile == "")
                {
                    // connect to the PetraServer's ClientManager
                    FConnector.GetRemoteServerConnection(Environment.GetCommandLineArgs()[0] + ".config", out FClientManager);
                }
                else
                {
                    // connect to the PetraServer's ClientManager
                    FConnector.GetRemoteServerConnection(TClientSettings.ConfigurationFile, out FClientManager);
                }

                // register Client session at the PetraServer
                ReturnValue = ConnectClient(AUserName, APassword, FClientManager,
                    out AProcessID,
                    out AWelcomeMessage,
                    out ASystemEnabled,
                    out ConnectionError);
                TRemote.ClientManager = FClientManager;

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
                    throw exp;
                }
            }
            catch (EClientVersionMismatchException exp)
            {
                throw exp;
            }
            catch (ELoginFailedServerTooBusyException exp)
            {
                throw exp;
            }
            catch (Exception exp)
            {
                TLogging.Log(exp.ToString() + Environment.NewLine + exp.StackTrace, TLoggingType.ToLogfile);
                throw new EServerConnectionGeneralException(exp.ToString());
            }

            //
            // acquire .NET Remoting Proxy objects for remoted Server objects
            //
            FConnector.ServerIPPort = FRemotingPort;

            // FConnector.GetRemoteServerSponsor(FRemotingURL_ServerSponsor, out FRemoteSponsor);
            FConnector.GetRemotePollClientTasks(FRemotingURL_PollClientTasks, out FRemotePollClientTasks);
            FConnector.GetRemoteMConferenceObject(FRemotingURL_MConference, out FRemoteConferenceObjects);
            FConnector.GetRemoteMPersonnelObject(FRemotingURL_MPersonnel, out FRemotePersonnelObjects);
            FConnector.GetRemoteMCommonObject(FRemotingURL_MCommon, out FRemoteCommonObjects);
            FConnector.GetRemoteMPartnerObject(FRemotingURL_MPartner, out FRemotePartnerObjects);
            FConnector.GetRemoteMFinanceObject(FRemotingURL_MFinance, out FRemoteFinanceObjects);
            FConnector.GetRemoteMReportingObject(FRemotingURL_MReporting, out FRemoteReportingObjects);
            FConnector.GetRemoteMSysManObject(FRemotingURL_MSysMan, out FRemoteSysManObjects);

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
            FRemote = new TRemote(FClientManager,
                FRemoteCommonObjects,
                FRemoteConferenceObjects,
                FRemotePartnerObjects,
                FRemotePersonnelObjects,
                FRemoteFinanceObjects,
                FRemoteReportingObjects,
                FRemoteSysManObjects);
            return ReturnValue;
        }

        private bool ConnectClient(String AUserName,
            String APassword,
            IClientManagerInterface AClientManager,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out String AError)
        {
            AError = "";
            ASystemEnabled = false;
            AWelcomeMessage = "";
            AProcessID = -1;
            bool ReturnValue;
            Hashtable ARemotingURLs;
            try
            {
                IPrincipal LocalUserInfo;

                AClientManager.ConnectClient(AUserName, APassword,
                    TClientInfo.ClientComputerName,
                    TClientInfo.ClientIPAddress,
                    new Version(TClientInfo.ClientAssemblyVersion),
                    DetermineClientServerConnectionType(),
                    out FClientName,
                    out FClientID,
                    out FRemotingPort,
                    out ARemotingURLs,
                    out FServerOS,
                    out AProcessID,
                    out AWelcomeMessage,
                    out ASystemEnabled,
                    out LocalUserInfo);

                Ict.Petra.Shared.UserInfo.GUserInfo = (TPetraPrincipal)LocalUserInfo;

                if (ARemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_POLLCLIENTTASKS))
                {
                    FRemotingURL_PollClientTasks = (String)ARemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_POLLCLIENTTASKS];
                }

                if (ARemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MSYSMAN))
                {
                    FRemotingURL_MSysMan = (String)ARemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MSYSMAN];
                }

                if (ARemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MCOMMON))
                {
                    FRemotingURL_MCommon = (String)ARemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MCOMMON];
                }

                if (ARemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MCONFERENCE))
                {
                    FRemotingURL_MConference = (String)ARemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MCONFERENCE];
                }

                if (ARemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MPARTNER))
                {
                    FRemotingURL_MPartner = (String)ARemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MPARTNER];
                }

                if (ARemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MPERSONNEL))
                {
                    FRemotingURL_MPersonnel = (String)ARemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MPERSONNEL];
                }

                if (ARemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MFINANCE))
                {
                    FRemotingURL_MFinance = (String)ARemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MFINANCE];
                }

                if (ARemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MREPORTING))
                {
                    FRemotingURL_MReporting = (String)ARemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MREPORTING];
                }

                ReturnValue = true;
            }
            catch (EUserNotExistantException exp)
            {
                AError = exp.Message;
                ReturnValue = false;
            }
            catch (EUserRetiredException exp)
            {
                AError = exp.Message;
                ReturnValue = false;
            }
            catch (EAccessDeniedException exp)
            {
                AError = exp.Message;
                ReturnValue = false;
            }
            catch (EUserRecordLockedException exp)
            {
                AError = exp.Message;
                ReturnValue = false;
            }
            catch (ESystemDisabledException exp)
            {
                AError = exp.Message;
                ReturnValue = false;
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
                throw exp;
            }
            return ReturnValue;
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
                }

                if (FRemote != null)
                {
                    ReturnValue = TRemote.ClientManager.DisconnectClient(FClientID, out ACantDisconnectReason);
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