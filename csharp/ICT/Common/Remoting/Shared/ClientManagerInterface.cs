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
using System.Security.Principal;
using System.Runtime.Remoting.Messaging;
using Ict.Common;

namespace Ict.Common.Remoting.Shared
{
    /**
     * This is the first interface the Client connects to
     *
     * IClientManagerInterface is the main interface for the Client.
     *
     * Its purpose is to allow connecting to and disconnecting from the PETRAServer.
     */
    public interface IClientManagerInterface : IInterface
    {
        /**
         * Calling ConnectClient creates an AppDomain and remotes objects that can
         * be called by the Client.
         *
         * See implementation of this class for more detailed description!
         *
         */

        void ConnectClient(String AUserName,
            String APassword,
            String AClientComputerName,
            String AClientIPAddress,
            System.Version AClientExeVersion,
            TClientServerConnectionType AClientServerConnectionType,
            out String AClientName,
            out System.Int32 AClientID,
            out string ACrossDomainURL,
            out Hashtable ARemotingURLs,
            out TExecutingOSEnum AServerOS,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out IPrincipal AUserInfo);


        /**
         * Calling DisconnectClient tears down the Client's AppDomain and therefore
         * kills all remoted objects for the Client that were not released before.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Boolean DisconnectClient(System.Int32 AClientID, out String ACantDisconnectReason);


        /**
         * Calling DisconnectClient tears down the Client's AppDomain and therefore
         * kills all remoted objects for the Client that were not released before.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Boolean DisconnectClient(System.Int32 AClientID, String AReason, out String ACantDisconnectReason);

        /// <summary>
        /// add a service that is offered by the appdomain, for single port remoting
        /// </summary>
        void AddCrossDomainService(string ClientID, string ObjectURI, ICrossDomainService ObjectToRemote);

        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Int32 QueueClientTaskFromClient(System.Int32 AClientID,
            String ATaskGroup,
            String ATaskCode,
            System.Int16 ATaskPriority,
            System.Int32 AExceptClientID);

        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Int32 QueueClientTaskFromClient(System.Int32 AClientID, String ATaskGroup, String ATaskCode, System.Int16 ATaskPriority);


        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Int32 QueueClientTaskFromClient(System.Int32 AClientID,
            String ATaskGroup,
            String ATaskCode,
            object ATaskParameter1,
            object ATaskParameter2,
            object ATaskParameter3,
            object ATaskParameter4,
            System.Int16 ATaskPriority,
            System.Int32 AExceptClientID);

        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Int32 QueueClientTaskFromClient(System.Int32 AClientID,
            String ATaskGroup,
            String ATaskCode,
            object ATaskParameter1,
            object ATaskParameter2,
            object ATaskParameter3,
            object ATaskParameter4,
            System.Int16 ATaskPriority);


        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Int32 QueueClientTaskFromClient(String AUserID, String ATaskGroup, String ATaskCode, System.Int16 ATaskPriority, System.Int32 AExceptClientID);

        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Int32 QueueClientTaskFromClient(String AUserID, String ATaskGroup, String ATaskCode, System.Int16 ATaskPriority);

        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Int32 QueueClientTaskFromClient(String AUserID,
            String ATaskGroup,
            String ATaskCode,
            object ATaskParameter1,
            object ATaskParameter2,
            object ATaskParameter3,
            object ATaskParameter4,
            System.Int16 ATaskPriority,
            System.Int32 AExceptClientID);


        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Int32 QueueClientTaskFromClient(String AUserID,
            String ATaskGroup,
            String ATaskCode,
            object ATaskParameter1,
            object ATaskParameter2,
            object ATaskParameter3,
            object ATaskParameter4,
            System.Int16 ATaskPriority);

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
        void AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            String AUserID,
            Int32 AProcessID);


        /**
         * The following functions are only for development purposes (note that these
         * functions can also be invoked directly from the Server's menu)
         *
         */
        System.Int32 GCGetGCGeneration(object AInspectObject);

        /// <summary>
        /// perform garbage collection
        /// </summary>
        /// <returns></returns>
        System.Int32 GCPerformGC();

        /// <summary>
        /// see how much memory is available
        /// </summary>
        /// <returns></returns>
        System.Int32 GCGetApproxMemory();
    }

    /// <summary>
    /// this services is available across domains
    /// </summary>
    public interface ICrossDomainService
    {
        /// <summary>
        /// marshal a message across domains
        /// </summary>
        IMessage Marshal(IMessage msg);
    }
}