//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.IO;
using System.Collections;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.App.Core.ServerAdmin.WebConnectors
{
    ///<summary>
    /// This connector provides methods for the server admin console
    ///</summary>
    public class TServerAdminWebConnector
    {
        /// <summary>
        /// get the number of clients that have connected to this server since the last restart
        /// </summary>
        [CheckServerAdminToken]
        public static int GetClientsConnectedTotal()
        {
            return TServerManagerBase.TheServerManager.ClientsConnectedTotal;
        }

        /// <summary>
        /// get number of currently connected clients
        /// </summary>
        [CheckServerAdminToken]
        public static int GetClientsConnected()
        {
            return TServerManagerBase.TheServerManager.ClientsConnected;
        }

        /// <summary>
        /// get list of clients
        /// </summary>
        [CheckServerAdminToken]
        public static ArrayList GetClientList()
        {
            return TServerManagerBase.TheServerManager.ClientList;
        }

        /// <summary>
        /// version of the server
        /// </summary>
        [CheckServerAdminToken]
        public static String GetServerInfoVersion()
        {
            return TServerManagerBase.TheServerManager.ServerInfoVersion;
        }

        /// <summary>
        /// state of the server
        /// </summary>
        [CheckServerAdminToken]
        public static String GetServerInfoState()
        {
            return TServerManagerBase.TheServerManager.ServerInfoState;
        }

        /// <summary>
        /// how much memory is used by the server
        /// </summary>
        [CheckServerAdminToken]
        public static System.Int64 GetServerInfoMemory()
        {
            return TServerManagerBase.TheServerManager.ServerInfoMemory;
        }

        /// <summary>
        /// command to stop the server in a more controlled way than <see cref="StopServer" /> (gets all connected clients to disconnect)
        /// </summary>
        [CheckServerAdminToken]
        public static bool StopServerControlled(bool AForceAutomaticClosing)
        {
            return TServerManagerBase.TheServerManager.StopServerControlled(AForceAutomaticClosing);
        }

        /// <summary>
        /// command to stop the server unconditionally (forces 'hard' disconnection of all Clients!)
        /// </summary>
        [CheckServerAdminToken]
        public static void StopServer()
        {
            TServerManagerBase.TheServerManager.StopServer();
        }

        /// <summary>
        /// perform garbage collection on the server
        /// </summary>
        /// <returns></returns>
        [CheckServerAdminToken]
        public static System.Int64 PerformGC()
        {
            return TServerManagerBase.TheServerManager.PerformGC();
        }

        /// <summary>
        /// get a formatted list of clients
        /// </summary>
        /// <param name="AListDisconnectedClients"></param>
        /// <returns></returns>
        [CheckServerAdminToken]
        public static String FormatClientList(Boolean AListDisconnectedClients)
        {
            return TServerManagerBase.TheServerManager.FormatClientList(AListDisconnectedClients);
        }

        /// <summary>
        /// get a formatted list for sysadm (also useful for webmin???)
        /// </summary>
        /// <param name="AListDisconnectedClients"></param>
        /// <returns></returns>
        [CheckServerAdminToken]
        public static String FormatClientListSysadm(Boolean AListDisconnectedClients)
        {
            return TServerManagerBase.TheServerManager.FormatClientListSysadm(AListDisconnectedClients);
        }

        /// <summary>
        /// put a task into a queue on the server
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="ATaskGroup"></param>
        /// <param name="ATaskCode"></param>
        /// <param name="ATaskPriority"></param>
        /// <returns></returns>
        [CheckServerAdminToken]
        public static bool QueueClientTask(System.Int16 AClientID, String ATaskGroup, String ATaskCode, System.Int16 ATaskPriority)
        {
            return TServerManagerBase.TheServerManager.QueueClientTask(AClientID, ATaskGroup, ATaskCode, ATaskPriority);
        }

        /// <summary>
        /// disconnect a client
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="ACantDisconnectReason"></param>
        /// <returns></returns>
        [CheckServerAdminToken]
        public static bool DisconnectClient(System.Int16 AClientID, out String ACantDisconnectReason)
        {
            return TServerManagerBase.TheServerManager.DisconnectClient(AClientID, out ACantDisconnectReason);
        }

        /// <summary>
        /// returns a string with yml.gz data
        /// </summary>
        /// <returns></returns>
        [CheckServerAdminToken]
        public static string BackupDatabaseToYmlGZ()
        {
            return TServerManagerBase.TheServerManager.BackupDatabaseToYmlGZ();
        }

        /// <summary>
        /// restore the database from a string with yml.gz data
        /// </summary>
        /// <returns></returns>
        [CheckServerAdminToken]
        public static bool RestoreDatabaseFromYmlGZ(string AYmlGzData)
        {
            // set the client id to avoid problems with missing ClientID for InitProgressTracker during restore of database
            DomainManager.GClientID = -1;

            return TServerManagerBase.TheServerManager.RestoreDatabaseFromYmlGZ(AYmlGzData);
        }

        /// <summary>
        /// Marks all DataTables in the Cache to be no longer up-to-date (=out of sync
        /// with the data that was originally placed in the DataTable).
        /// </summary>
        [CheckServerAdminToken]
        public static void RefreshAllCachedTables()
        {
            TServerManagerBase.TheServerManager.RefreshAllCachedTables();
        }

        /// <summary>
        /// add a new user
        /// </summary>
        /// <returns></returns>
        [CheckServerAdminToken]
        public static bool AddUser(string AUserID)
        {
            return TServerManagerBase.TheServerManager.AddUser(AUserID);
        }

        /// <summary>
        /// add a new user
        /// </summary>
        /// <returns></returns>
        [CheckServerAdminToken]
        public static bool AddUser(string AUserID, string APassword = "")
        {
            return TServerManagerBase.TheServerManager.AddUser(AUserID, APassword);
        }

        /// Allows the admin console to run a timed job now
        [CheckServerAdminToken]
        public static void PerformTimedProcessingNow(string AProcessName)
        {
            TServerManagerBase.TheServerManager.PerformTimedProcessingNow(AProcessName);
        }

        /// <summary>
        /// the host name of the smtp server
        /// </summary>
        [CheckServerAdminToken]
        public static string GetSMTPServer()
        {
            return TServerManagerBase.TheServerManager.SMTPServer;
        }
    }
}