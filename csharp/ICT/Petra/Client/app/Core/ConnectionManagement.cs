//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2013 by OM International
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
using System.Security.Principal;
using System.IO;
using Ict.Common;
using Ict.Common.DB.Exceptions;
using Ict.Common.Exceptions;
using Ict.Common.IO;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Reflection;
using Ict.Petra.Shared.MSysMan;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Manages the connection to the PetraServer.
    /// </summary>
    public class TConnectionManagement : TConnectionManagementBase
    {
        /// <summary>
        /// Connect to the server and authenticate the user
        /// </summary>
        public eLoginEnum ConnectToServer(String AUserName,
            String APassword,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out String AError)
        {
            IPrincipal LocalUserInfo;

            eLoginEnum Result = ConnectToServer(AUserName,
                APassword,
                out AProcessID,
                out AWelcomeMessage,
                out ASystemEnabled,
                out AError,
                out LocalUserInfo);

            if (Result != eLoginEnum.eLoginSucceeded)
            {
                return Result;
            }

            Ict.Petra.Shared.UserInfo.GUserInfo = (TPetraPrincipal)LocalUserInfo;
            Ict.Common.IO.TDlgSelectCSVSeparator.UserID = ((TPetraPrincipal)LocalUserInfo).UserID;

            //
            // initialise object that holds references to all our remote object .NET Remoting Proxies
            //
            new TRemote();

            return eLoginEnum.eLoginSucceeded;
        }

        /// <summary>
        /// specific things for connecting all the services
        /// </summary>
        /// <param name="AUserName"></param>
        /// <param name="APassword"></param>
        /// <param name="AProcessID"></param>
        /// <param name="AWelcomeMessage"></param>
        /// <param name="ASystemEnabled"></param>
        /// <param name="AError"></param>
        /// <param name="AUserInfo"></param>
        /// <returns></returns>
        protected override eLoginEnum ConnectClient(String AUserName,
            String APassword,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out String AError,
            out IPrincipal AUserInfo)
        {
            try
            {
                eLoginEnum result = base.ConnectClient(AUserName,
                    APassword,
                    out AProcessID,
                    out AWelcomeMessage,
                    out ASystemEnabled,
                    out AError,
                    out AUserInfo);

                if (result != eLoginEnum.eLoginSucceeded)
                {
                    return result;
                }

                Ict.Petra.Shared.UserInfo.GUserInfo = (TPetraPrincipal)AUserInfo;

                return result;
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