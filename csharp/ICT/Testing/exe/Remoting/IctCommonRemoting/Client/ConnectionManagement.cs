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
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Security.Principal;
using System.IO;
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using System.Reflection;
using Tests.IctCommonRemoting.Interface;

namespace Tests.IctCommonRemoting.Client
{
    /// <summary>
    /// Manages the connection to the PetraServer.
    /// </summary>
    public class TConnectionManagement : TConnectionManagementBase
    {
        private String FRemotingURL_MyService;
        private IMyService FRemoteMyServiceObject;

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
            IPrincipal LocalUserInfo;

            ConnectToServer(AUserName, APassword, out AProcessID, out AWelcomeMessage, out ASystemEnabled, out AError, out LocalUserInfo);

            FRemoteMyServiceObject = (IMyService)FConnector.GetRemoteObject(FRemotingURL_MyService, typeof(IMyService));

            //
            // initialise object that holds references to all our remote object .NET Remoting Proxies
            //
            FRemote = new TRemote(FClientManager,
                FRemoteMyServiceObject);

            return true;
        }

        /// <summary>
        /// specific things for connecting all the services
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
        protected override bool ConnectClient(String AUserName,
            String APassword,
            IClientManagerInterface AClientManager,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out String AError,
            out IPrincipal AUserInfo)
        {
            try
            {
                base.ConnectClient(AUserName,
                    APassword,
                    AClientManager,
                    out AProcessID,
                    out AWelcomeMessage,
                    out ASystemEnabled,
                    out AError,
                    out AUserInfo);

                if (FRemotingURLs.ContainsKey(SharedConstantsTest.REMOTINGURL_IDENTIFIER_MYSERVICE))
                {
                    FRemotingURL_MyService = (String)FRemotingURLs[SharedConstantsTest.REMOTINGURL_IDENTIFIER_MYSERVICE];
                }

                return true;
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
    }
}