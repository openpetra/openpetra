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

using Ict.Common;

namespace Ict.Common.Remoting.Shared
{
    /// <comment>IServerAdminInterface is the main interface for Server Admin applications (like PetraServerAdminConsole.exe).
    /// Its purpose is to allow execution of certain Server functions, including disconnecting Clients and Server shutdown.
    /// </comment>
    /// <remarks>Comment Clients don't use this interface!</remarks>
    public interface IServerAdminInterface
    {
        /// <summary>DB Reconnection attempts (-1 = no connection established yet at all; 0 = none are being made).</summary>
        Int64 DBReconnectionAttemptsCounter
        {
            get;
        }

        /// <summary>
        /// DB Connection Check Interval (if this is 0 then no automatic checks are done).
        /// </summary>
        int DBConnectionCheckInterval
        {
            get;
        }

        /// <summary>SiteKey of the OpenPetra DB that the Server is connected to.</summary>
        Int64 SiteKey
        {
            get;
        }

        /// <summary>
        /// get number of connected clients including disconnected clients
        /// </summary>
        int ClientsConnectedTotal
        {
            get;
        }

        /// <summary>
        /// get number of currently connected clients
        /// </summary>
        int ClientsConnected
        {
            get;
        }

        /// <summary>
        /// get list of clients
        /// </summary>
        ArrayList ClientList
        {
            get;
        }

        /// <summary>
        /// the port the server is running on
        /// </summary>
        int IPPort
        {
            get;
        }

        /// <summary>
        /// Name of the Server configuration file
        /// </summary>
        String ConfigurationFileName
        {
            get;
        }

        /// <summary>
        /// version of the server
        /// </summary>
        String ServerInfoVersion
        {
            get;
        }

        /// <summary>
        /// state of the server
        /// </summary>
        String ServerInfoState
        {
            get;
        }

        /// <summary>
        /// how much memory is used by the server
        /// </summary>
        System.Int64 ServerInfoMemory
        {
            get;
        }

        /// <summary>
        /// command to stop the server in a more controlled way than <see cref="StopServer" /> (gets all connected clients to disconnect)
        /// </summary>
        bool StopServerControlled(bool AForceAutomaticClosing);

        /// <summary>
        /// command to stop the server unconditionally (forces 'hard' disconnection of all Clients!)
        /// </summary>
        void StopServer();

        /// <summary>
        /// perform garbage collection on the server
        /// </summary>
        /// <returns></returns>
        System.Int64 PerformGC();

        /// <summary>
        /// get a formatted list of clients
        /// </summary>
        /// <param name="AListDisconnectedClients"></param>
        /// <returns></returns>
        String FormatClientList(Boolean AListDisconnectedClients);

        /// <summary>
        /// get a formatted list for sysadm (also useful for webmin???)
        /// </summary>
        /// <param name="AListDisconnectedClients"></param>
        /// <returns></returns>
        String FormatClientListSysadm(Boolean AListDisconnectedClients);

        /// <summary>
        /// put a task into a queue on the server
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="ATaskGroup"></param>
        /// <param name="ATaskCode"></param>
        /// <param name="ATaskParameter1">Parameter #1 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskParameter2">Parameter #2 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskParameter3">Parameter #3 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskParameter4">Parameter #4 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskPriority"></param>
        /// <returns></returns>
        bool QueueClientTask(System.Int16 AClientID, String ATaskGroup, String ATaskCode, object ATaskParameter1,
            object ATaskParameter2, object ATaskParameter3, object ATaskParameter4, System.Int16 ATaskPriority);

        /// <summary>
        /// disconnect a client
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="ACantDisconnectReason"></param>
        /// <returns></returns>
        bool DisconnectClient(System.Int16 AClientID, out String ACantDisconnectReason);

        /// <summary>
        /// upgrade the database
        /// </summary>
        /// <returns>true if the database was upgraded</returns>
        bool UpgradeDatabase();

        /// <summary>
        /// Returns a string with yml.gz data.
        /// </summary>
        /// <returns></returns>
        string BackupDatabaseToYmlGZ();

        /// <summary>
        /// Restore the database from a string with yml.gz data.
        /// </summary>
        /// <returns></returns>
        bool RestoreDatabaseFromYmlGZ(string AYmlGzData);

        /// <summary>
        /// Marks all DataTables in the Cache to be no longer up-to-date (=out of sync
        /// with the data that was originally placed in the DataTable).
        /// </summary>
        void RefreshAllCachedTables();

        /// <summary>
        /// Clears (flushes) all RDMBS Connection Pools and returns the new number of DB Connections after clearing all
        /// RDMBS Connection Pools.
        /// </summary>
        int ClearConnectionPoolAndGetNumberOfDBConnections();

        /// <summary>
        /// add a new user
        /// </summary>
        /// <returns></returns>
        bool AddUser(string AUserID, string APassword = "");

        /// <summary>
        /// Whether the 'Server Timed Processing' has been set up.
        /// </summary>
        bool ServerTimedProcessingSetup
        {
            get;
        }

        /// Allows the server or admin console to run a timed job now
        void PerformTimedProcessingNow(string AProcessName);

        /// Is the process job enabled?
        bool TimedProcessingJobEnabled(string AProcessName);

        /// <summary>
        /// the host name of the smtp server
        /// </summary>
        string SmtpHost
        {
            get;
        }

        /// <summary>
        /// the daily start time for the timed processing
        /// </summary>
        string TimedProcessingDailyStartTime24Hrs
        {
            get;
        }
    }
}
