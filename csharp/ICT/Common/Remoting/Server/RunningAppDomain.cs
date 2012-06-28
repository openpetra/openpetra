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

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// This class holds details about a connected Client.
    /// It is stored as an entry in the UClientObjects HashTable (one entry for each
    /// current Client connection).
    ///
    /// </summary>
    public class TRunningAppDomain
    {
        /// <summary> todoComment </summary>
        public System.Int32 FClientID;

        /// <summary> todoComment </summary>
        public String FUserID;

        /// <summary> todoComment </summary>
        public String FClientName;

        /// <summary> todoComment </summary>
        public DateTime FClientConnectionStartTime;

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

        /// <summary> todoComment </summary>
        public String FAppDomainRemotedObjectURL;

        /// <summary> todoComment </summary>
        public IClientAppDomainConnection FClientAppDomainConnection;
        private System.Object FDisconnectClientMonitor;
        private Boolean FClientDisconnectionScheduled;

        /// <summary> todoComment </summary>
        public TAppDomainStatus FAppDomainStatus;

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

        /// <summary>Serverassigned name of the Client</summary>
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

        /// <summary>.NET Remoting URL of a Test object (for testing purposes only)</summary>
        public String AppDomainRemotedObjectURL
        {
            get
            {
                return FAppDomainRemotedObjectURL;
            }

            set
            {
                FAppDomainRemotedObjectURL = value;
            }
        }

        /// <summary>Connection object to the Client's AppDomain</summary>
        public IClientAppDomainConnection ClientAppDomainConnection
        {
            get
            {
                return FClientAppDomainConnection;
            }

            set
            {
                FClientAppDomainConnection = value;
            }
        }

        /// <summary>Status that the Client AppDomain has</summary>
        public TAppDomainStatus AppDomainStatus
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
        public TRunningAppDomain(System.Int32 AClientID,
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
            FAppDomainStatus = TAppDomainStatus.adsConnectingLoginVerification;
            FDisconnectClientMonitor = new System.Object();
        }

        /// <summary>
        /// Comment
        ///
        /// </summary>
        /// <param name="AAppDomainRemotedObjectURL">.NET Remoting URL of a Test object (for
        /// testing purposes only)</param>
        /// <param name="AClientAppDomainConnection">Object that allows a connection to the
        /// Client's AppDomain without causing a load of the Assemblies that are
        /// loaded in the Client's AppDomain into the Default AppDomain.
        /// </param>
        /// <returns>void</returns>
        public void PassInClientRemotingInfo(String AAppDomainRemotedObjectURL,
            IClientAppDomainConnection AClientAppDomainConnection)
        {
            FAppDomainRemotedObjectURL = AAppDomainRemotedObjectURL;
            FClientAppDomainConnection = AClientAppDomainConnection;
        }
    }

    /// <summary>
    /// several states for app domains
    /// </summary>
    public enum TAppDomainStatus
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
        /// app domain has been setup
        /// </summary>
        adsConnectingAppDomainSetupOK,

        /// <summary>
        /// app domain is active
        /// </summary>
        adsActive,

        /// <summary>
        /// app domain is not busy
        /// </summary>
        adsIdle,

        /// <summary>
        /// the db connection is being closed
        /// </summary>
        adsDisconnectingDBClosing,

        /// <summary>
        /// app domain is shutting down
        /// </summary>
        adsDisconnectingAppDomainUnloading,

        /// <summary>
        /// app domain has been stopped
        /// </summary>
        adsStopped
    };
}