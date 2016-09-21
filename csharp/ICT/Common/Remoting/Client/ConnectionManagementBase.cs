//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2016 by OM International
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
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Principal;

using Ict.Common;
using Ict.Common.DB.Exceptions;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Shared;

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

        private Int32 FClientID;
        private TPollClientTasks FPollClientTasks;
        private IClientManager FClientManager;

        /// <summary>
        /// the urls of the services
        /// </summary>
        protected Hashtable FRemotingURLs;

        /// <summary>todoComment</summary>
        public Int32 ClientID
        {
            get
            {
                return FClientID;
            }
        }

        /// <summary>
        /// Connect to the server and authenticate the user
        /// </summary>
        public eLoginEnum ConnectToServer(String AUserName,
            String APassword,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out String AError,
            out IPrincipal AUserInfo)
        {
            AError = "";
            String ConnectionError;
            AUserInfo = null;
            ASystemEnabled = false;
            AWelcomeMessage = string.Empty;
            AProcessID = -1;

            try
            {
                FClientManager = TConnectionHelper.Connect();

                // register Client session at the PetraServer
                eLoginEnum ReturnValue = ConnectClient(AUserName, APassword,
                    out AProcessID,
                    out AWelcomeMessage,
                    out ASystemEnabled,
                    out ConnectionError,
                    out AUserInfo);

                if (ReturnValue != eLoginEnum.eLoginSucceeded)
                {
                    AError = ConnectionError;
                    return ReturnValue;
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                return eLoginEnum.eLoginServerNotReachable;
            }
            catch (Exception exp)
            {
                TLogging.Log(exp.ToString() + Environment.NewLine + exp.StackTrace, TLoggingType.ToLogfile);
                throw new EServerConnectionGeneralException(exp.ToString());
            }

            if (TClientSettings.RunAsStandalone)
            {
                FPollClientTasks = null;
            }
            else
            {
                //
                // start the PollClientTasks Thread (which needs to run as long as the Client is running)
                //
                FPollClientTasks = new TPollClientTasks(FClientID);
            }

            return eLoginEnum.eLoginSucceeded;
        }

        /// <summary>
        /// connect the client
        /// </summary>
        /// <param name="AUserName"></param>
        /// <param name="APassword"></param>
        /// <param name="AProcessID"></param>
        /// <param name="AWelcomeMessage"></param>
        /// <param name="ASystemEnabled"></param>
        /// <param name="AError"></param>
        /// <param name="AUserInfo"></param>
        /// <returns></returns>
        virtual protected eLoginEnum ConnectClient(String AUserName,
            String APassword,
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
                eLoginEnum result = FClientManager.ConnectClient(AUserName, APassword,
                    TClientInfo.ClientComputerName,
                    TClientInfo.ClientIPAddress,
                    new Version(TClientInfo.ClientAssemblyVersion),
                    DetermineClientServerConnectionType(),
                    out FClientID,
                    out AWelcomeMessage,
                    out ASystemEnabled,
                    out AUserInfo);

                if (result != eLoginEnum.eLoginSucceeded)
                {
                    AError = result.ToString();
                }

                return result;
            }
            catch (Exception Exc)
            {
                if (TExceptionHelper.IsExceptionCausedByUnavailableDBConnectionClientSide(Exc))
                {
                    TExceptionHelper.ShowExceptionCausedByUnavailableDBConnectionMessage(true);

                    AError = Exc.Message;
                    return eLoginEnum.eLoginFailedForUnspecifiedError;
                }

                TLogging.Log(Exc.ToString() + Environment.NewLine + Exc.StackTrace, TLoggingType.ToLogfile);
                AError = Exc.Message;
                return eLoginEnum.eLoginFailedForUnspecifiedError;
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
                if (FPollClientTasks != null)
                {
                    FPollClientTasks.StopPollClientTasks();
                }

                ReturnValue = FClientManager.DisconnectClient(out ACantDisconnectReason);
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
    public class EServerConnectionGeneralException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EServerConnectionGeneralException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EServerConnectionGeneralException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EServerConnectionGeneralException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }
}
