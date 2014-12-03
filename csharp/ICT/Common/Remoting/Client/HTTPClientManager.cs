//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Collections.Generic;
using System.Security.Principal;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// interface, to support standalone and client/server scenario
    /// </summary>
    public interface IClientManager
    {
        /// connect the client to the server
        eLoginEnum ConnectClient(String AUserName,
            String APassword,
            String AClientComputerName,
            String AClientIPAddress,
            System.Version AClientExeVersion,
            TClientServerConnectionType AClientServerConnectionType,
            out Int32 AClientID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out IPrincipal AUserInfo);

        /// <summary>
        /// disconnect
        /// </summary>
        Boolean DisconnectClient(out String ACantDisconnectReason);

        /// <summary>
        /// disconnect
        /// </summary>
        Boolean DisconnectClient(String AReason, out String ACantDisconnectReason);
    }

    /// client manager for the connection to the server via http
    public class THTTPClientManager : IClientManager
    {
        /// connect the client to the server
        public eLoginEnum ConnectClient(String AUserName,
            String APassword,
            String AClientComputerName,
            String AClientIPAddress,
            System.Version AClientExeVersion,
            TClientServerConnectionType AClientServerConnectionType,
            out Int32 AClientID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out IPrincipal AUserInfo)
        {
            AWelcomeMessage = string.Empty;
            ASystemEnabled = true;
            AUserInfo = null;
            AClientID = -1;

            THttpConnector.InitConnection(TAppSettingsManager.GetValue("OpenPetra.HTTPServer"));
            SortedList <string, object>Parameters = new SortedList <string, object>();
            Parameters.Add("username", AUserName);
            Parameters.Add("password", APassword);
            Parameters.Add("version", AClientExeVersion.ToString());

            List <object>ResultList = THttpConnector.CallWebConnector("SessionManager", "LoginClient", Parameters, "list");
            eLoginEnum Result = (eLoginEnum)ResultList[0];

            if (Result != eLoginEnum.eLoginSucceeded)
            {
                // failed login
                return Result;
            }

            AClientID = (Int32)ResultList[1];
            AWelcomeMessage = (string)ResultList[2];
            ASystemEnabled = (Boolean)ResultList[3];
            AUserInfo = (IPrincipal)ResultList[4];

            return eLoginEnum.eLoginSucceeded;
        }

        /// <summary>
        /// disconnect
        /// </summary>
        public Boolean DisconnectClient(out String ACantDisconnectReason)
        {
            THttpConnector.CallWebConnector("SessionManager", "Logout", null, "System.Boolean");
            ACantDisconnectReason = string.Empty;
            return true;
        }

        /// <summary>
        /// disconnect
        /// </summary>
        public Boolean DisconnectClient(String AReason, out String ACantDisconnectReason)
        {
            THttpConnector.CallWebConnector("SessionManager", "Logout", null, "System.Boolean");
            ACantDisconnectReason = string.Empty;
            return true;
        }

        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        public Int32 QueueClientTaskFromClient(System.Int32 AClientID,
            String ATaskGroup,
            String ATaskCode,
            object ATaskParameter1,
            object ATaskParameter2,
            object ATaskParameter3,
            object ATaskParameter4,
            System.Int16 ATaskPriority,
            System.Int32 AExceptClientID)
        {
            // TODORemoting
            return -1;
        }

        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        public Int32 QueueClientTaskFromClient(String AUserID,
            String ATaskGroup,
            String ATaskCode,
            object ATaskParameter1,
            object ATaskParameter2,
            object ATaskParameter3,
            object ATaskParameter4,
            System.Int16 ATaskPriority,
            System.Int32 AExceptClientID)
        {
            // TODORemoting
            return -1;
        }

        /// <summary>
        /// add error to the log
        /// </summary>
        /// <param name="AErrorCode"></param>
        /// <param name="AContext"></param>
        /// <param name="AMessageLine1"></param>
        /// <param name="AMessageLine2"></param>
        /// <param name="AMessageLine3"></param>
        /// <param name="AUserID"></param>
        /// <param name="AProcessID"></param>
        public void AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            String AUserID,
            Int32 AProcessID)
        {
            // TODORemoting
        }

        /**
         * The following functions are only for development purposes (note that these
         * functions can also be invoked directly from the Server's menu)
         *
         */
        public System.Int32 GCGetGCGeneration(object AInspectObject)
        {
            // TODORemoting
            return -1;
        }

        /// <summary>
        /// perform garbage collection
        /// </summary>
        /// <returns></returns>
        public System.Int32 GCPerformGC()
        {
            // TODORemoting
            return -1;
        }

        /// <summary>
        /// see how much memory is available
        /// </summary>
        /// <returns></returns>
        public System.Int32 GCGetApproxMemory()
        {
            // TODORemoting
            return -1;
        }
    }
}