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
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// The TClientManagerCallForwarder class forwards calls from within a Client's
    /// AppDomain to the ClientManager, which is running in the Default AppDomain.
    /// </summary>
    /// <remarks>This class is necessary because of flaws of mono's
    ///   implementation of AppDomains (as of mono Version 1.1.13.8).
    ///   These flaws lead to errors if an Interface to the ClientManager is passed
    ///   to the TClientDomainManager constuctor via .NET Remoting and calls to
    ///   functions that are defined in the interface are made. Errors included
    ///   'Server for uri 'ClientManager' not found'  and 'NullReferenceException' as
    ///   soon as a function is called on the ClientManager Object.
    ///   The workaround that I found is this unit and class: it can be passed as
    ///   an Object in the TClientDomainManager constuctor instead of an Interface.
    ///   The ClientManager itself cannot be passed as an Object - this would lead
    ///   to circular references! [christiank]
    /// </remarks>
    public class TClientManagerCallForwarder : MarshalByRefObject
    {
        private IClientManagerInterface FClientManagerRef;

        /// <summary>
        /// ClientManager in the default appdomain
        /// </summary>
        public IClientManagerInterface ClientManagerRef
        {
            get
            {
                return FClientManagerRef;
            }
        }


        /// <summary>
        /// Makes a call to the QueueClientTaskFromClient function in TClientManager.
        ///
        /// Please see the implementation of this function in TClientManager for details.
        ///
        /// </summary>
        /// <returns>void</returns>
        public Int32 QueueClientTaskFromClient(System.Int32 AClientID,
            String ATaskGroup,
            String ATaskCode,
            System.Object ATaskParameter1,
            System.Object ATaskParameter2,
            System.Object ATaskParameter3,
            System.Object ATaskParameter4,
            System.Int16 ATaskPriority,
            System.Int32 AExceptClientID)
        {
            return FClientManagerRef.QueueClientTaskFromClient(AClientID,
                ATaskGroup,
                ATaskCode,
                ATaskParameter1,
                ATaskParameter2,
                ATaskParameter3,
                ATaskParameter4,
                ATaskPriority,
                AExceptClientID);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="ATaskGroup"></param>
        /// <param name="ATaskCode"></param>
        /// <param name="ATaskParameter1"></param>
        /// <param name="ATaskParameter2"></param>
        /// <param name="ATaskParameter3"></param>
        /// <param name="ATaskParameter4"></param>
        /// <param name="ATaskPriority"></param>
        /// <returns></returns>
        public Int32 QueueClientTaskFromClient(System.Int32 AClientID,
            String ATaskGroup,
            String ATaskCode,
            System.Object ATaskParameter1,
            System.Object ATaskParameter2,
            System.Object ATaskParameter3,
            System.Object ATaskParameter4,
            System.Int16 ATaskPriority)
        {
            return QueueClientTaskFromClient(AClientID,
                ATaskGroup,
                ATaskCode,
                ATaskParameter1,
                ATaskParameter2,
                ATaskParameter3,
                ATaskParameter4,
                ATaskPriority,
                -1);
        }

        /// <summary>
        /// add an error to log
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
            FClientManagerRef.AddErrorLogEntry(AErrorCode, AContext, AMessageLine1, AMessageLine2, AMessageLine3, AUserID, AProcessID);
        }

        /// <summary>
        /// Assigns the passed in reference to the TClientManager Object to an internal
        /// variable, which is later used to make calls to this object.
        ///
        /// </summary>
        /// <param name="AClientManagerRef">Reference to the TClientManager Object
        /// </param>
        /// <returns>void</returns>
        public TClientManagerCallForwarder(IClientManagerInterface AClientManagerRef) : base()
        {
            FClientManagerRef = AClientManagerRef;
        }

        /// <summary>
        /// Makes a call to the DisconnectClient function in TClientManager.
        ///
        /// Please see the implementation of this function in TClientManager for details.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DisconnectClient(System.Int16 AClientID, String AReason)
        {
            string CantDisconnectReason;

            // Need to call the correct overloaded version of DisconnectClient to let AReason have an effect!
            FClientManagerRef.DisconnectClient(AClientID, AReason, out CantDisconnectReason);
        }

        /// <summary>
        /// add a service that is offered by the appdomain, for single port remoting
        /// </summary>
        public void AddCrossDomainService(string ClientID, string ObjectURI, ICrossDomainService ObjectToRemote)
        {
            FClientManagerRef.AddCrossDomainService(ClientID, ObjectURI, ObjectToRemote);
        }

        /// <summary>
        /// make sure that the TClientManagerProxy object exists until the Server stops!
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            // make sure that the TClientManagerProxy object exists until the Server stops!
            return null;
        }

        /// <summary>
        /// Makes a call to the QueueClientTaskFromClient function in TClientManager.
        ///
        /// Please see the implementation of this function in TClientManager for details.
        ///
        /// </summary>
        /// <returns>void</returns>
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
            return FClientManagerRef.QueueClientTaskFromClient(AUserID,
                ATaskGroup,
                ATaskCode,
                ATaskParameter1,
                ATaskParameter2,
                ATaskParameter3,
                ATaskParameter4,
                ATaskPriority,
                AExceptClientID);
        }
    }
}