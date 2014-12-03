//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2014 by OM International
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
using System.Web;
using Ict.Common.DB;
using Ict.Common.Session;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// This class holds details about a connected Client.
    /// It is stored as an entry in the UClientObjects HashTable (one entry for each
    /// current Client connection).
    ///
    /// </summary>
    public class TConnectedClient
    {
        /// <summary> todoComment </summary>
        public System.Int32 FClientID;

        /// <summary> todoComment </summary>
        public String FUserID;

        /// <summary> todoComment </summary>
        public String FClientName;

        /// <summary> todoComment </summary>
        public DateTime FClientConnectionStartTime;

        private DateTime FLastActionTime;

        /// <summary> todoComment </summary>
        public DateTime FClientConnectionFinishedTime;

        /// <summary> todoComment </summary>
        public DateTime FClientDisconnectionStartTime;

        /// <summary> todoComment </summary>
        public DateTime FClientDisconnectionFinishedTime;

        /// <summary> todoComment </summary>
        public String FClientComputerName;

        /// <summary> todoComment </summary>
        public String FClientIPAddress;

        /// <summary> todoComment </summary>
        public TClientServerConnectionType FClientServerConnectionType;

        /// <summary> todoComment </summary>
        public String FAppDomainName;

        private System.Object FDisconnectClientMonitor;
        private Boolean FClientDisconnectionScheduled;

        /// <summary> todoComment </summary>
        public TSessionStatus FAppDomainStatus;

        /// <summary>
        /// access the tasks for this client
        /// </summary>
        public TClientTasksManager FTasksManager = null;

        /// <summary>
        /// access the poll client tasks for this client
        /// </summary>
        public TPollClientTasks FPollClientTasks = null;

        /// <summary>Serverassigned ID of the Client</summary>
        public System.Int32 ClientID
        {
            get
            {
                return FClientID;
            }
        }

        /// <summary>UserID for which the AppDomain was created</summary>
        public String UserID
        {
            get
            {
                return FUserID;
            }
        }

        /// <summary>Server-assigned name of the Client</summary>
        public String ClientName
        {
            get
            {
                return FClientName;
            }
        }

        /// <summary>Time when the Client connected to the Server.</summary>
        public DateTime ClientConnectionTime
        {
            get
            {
                return FClientConnectionStartTime;
            }
        }

        /// <summary>
        /// when was the last request to this session?
        /// </summary>
        public DateTime LastActionTime
        {
            get
            {
                return FLastActionTime;
            }
        }

        /// <summary>Computer name of the Client</summary>
        public String ClientComputerName
        {
            get
            {
                return FClientComputerName;
            }
        }

        /// <summary>IP Address of the Client</summary>
        public String ClientIPAddress
        {
            get
            {
                return FClientIPAddress;
            }
        }

        /// <summary>Type of the connection (eg. LAN, Remote)</summary>
        public TClientServerConnectionType ClientServerConnectionType
        {
            get
            {
                return FClientServerConnectionType;
            }
        }

        /// <summary>Serverassigned name of the Client AppDomain</summary>
        public String AppDomainName
        {
            get
            {
                return FAppDomainName;
            }

            set
            {
                FAppDomainName = value;
            }
        }

        /// <summary>Status that the Client AppDomain has</summary>
        public TSessionStatus SessionStatus
        {
            get
            {
                return FAppDomainStatus;
            }

            set
            {
                FAppDomainStatus = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public System.Object DisconnectClientMonitor
        {
            get
            {
                return FDisconnectClientMonitor;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public Boolean ClientDisconnectionScheduled
        {
            get
            {
                return FClientDisconnectionScheduled;
            }

            set
            {
                FClientDisconnectionScheduled = value;
            }
        }


        /// <summary>
        /// Initialises fields.
        ///
        /// </summary>
        /// <param name="AClientID">Server-assigned ID of the Client</param>
        /// <param name="AUserID">AUserID for which the AppDomain was created</param>
        /// <param name="AClientName">Server-assigned name of the Client</param>
        /// <param name="AClientComputerName">Computer name of the Client</param>
        /// <param name="AClientIPAddress">IP Address of the Client</param>
        /// <param name="AClientServerConnectionType">Type of the connection (eg. LAN, Remote)</param>
        /// <param name="AAppDomainName">Server-assigned name of the Client AppDomain
        /// </param>
        /// <returns>void</returns>
        public TConnectedClient(System.Int32 AClientID,
            String AUserID,
            String AClientName,
            String AClientComputerName,
            String AClientIPAddress,
            TClientServerConnectionType AClientServerConnectionType,
            String AAppDomainName)
        {
            FClientID = AClientID;
            FUserID = AUserID;
            FClientName = AClientName;
            FClientComputerName = AClientComputerName;
            FClientIPAddress = AClientIPAddress;
            FClientServerConnectionType = AClientServerConnectionType;
            FClientConnectionStartTime = DateTime.Now;
            FAppDomainName = AAppDomainName;
            FAppDomainStatus = TSessionStatus.adsConnectingLoginVerification;
            FDisconnectClientMonitor = new System.Object();
        }

        /// start the session
        public void StartSession(TDelegateTearDownAppDomain ATearDownAppDomain)
        {
            FTasksManager = new TClientTasksManager();
            FPollClientTasks = new TPollClientTasks(FTasksManager);

            FClientConnectionStartTime = DateTime.Now;

            // Start ClientStillAliveCheck Thread
            new ClientStillAliveCheck.TClientStillAliveCheck(this, FClientServerConnectionType, ATearDownAppDomain);

            SessionStatus = TSessionStatus.adsActive;
            FClientConnectionFinishedTime = DateTime.Now;
        }

        /// <summary>
        /// update on a new http request
        /// </summary>
        public void UpdateLastAccessTime()
        {
            FLastActionTime = DateTime.Now;
        }

        /// <summary>
        /// end the session, and release all resources
        /// </summary>
        public void EndSession()
        {
            if (FAppDomainStatus == TSessionStatus.adsStopped)
            {
                TLogging.Log("EndSession (for client '" + this.ClientName + "'): Session has been stopped already!");
                return;
            }

            TLogging.Log("EndSession (for client '" + this.ClientName + "'): Session is about to getting stopped!");
            
            // TODORemoting
            // release all UIConnector objects
            ClientStillAliveCheck.TClientStillAliveCheck.StopClientStillAliveCheckThread();
            
            TLogging.Log("EndSession (for client '" + this.ClientName + "'): Checking whether there is a DB connection");
            
            // close db connection
            if (DBAccess.GDBAccessObj != null)
            {
                TLogging.Log("EndSession (for client '" + this.ClientName + "'): Closing DB connection");
                DBAccess.GDBAccessObj.CloseDBConnection();
            }

            TLogging.Log("EndSession (for client '" + this.ClientName + "'): Checking whether there is a HttpSession.Current object");
            
            // clear the session object
            if (HttpContext.Current != null)
            {
                TLogging.Log("EndSession (for client '" + this.ClientName + "'): Clearing Session");
                TSession.Clear();
            }

            FClientDisconnectionFinishedTime = DateTime.Now;
            FAppDomainStatus = TSessionStatus.adsStopped;

            TLogging.LogAtLevel(1, "Session for client " + this.ClientName + " has been destroyed successfully!");
        }
    }

    /// <summary>
    /// several states for sessions
    /// </summary>
    public enum TSessionStatus
    {
        /// <summary>
        /// during login verification
        /// </summary>
        adsConnectingLoginVerification,

        /// <summary>
        /// login was successful
        /// </summary>
        adsConnectingLoginOK,

        /// <summary>
        /// session is active
        /// </summary>
        adsActive,

        /// <summary>
        /// session is not busy
        /// </summary>
        adsIdle,

        /// <summary>
        /// the db connection is being closed
        /// </summary>
        adsDisconnectingDBClosing,

        /// <summary>
        /// session has been deleted
        /// </summary>
        adsStopped
    };
}