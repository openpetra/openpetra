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
using System.Security.Principal;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// an interface for the user authentication
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Adds a new user
        /// </summary>
        bool AddUser(string AUserID, string APassword = "");

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        IPrincipal PerformUserAuthentication(string AUserName, string APassword,
            string AClientComputerName, string AClientIPAddress,
            out Boolean ASystemEnabled,
            TDBTransaction ATransaction);

        /// <summary>
        /// Call this Method when a log-in is attempted for a non-existing user (!) so that the time that is spent on
        /// 'authenticating' them is as long as is spent on authenticating existing users. This is done so that an attacker
        /// that tries to perform user authentication with 'username guessing' cannot easily tell that the user doesn't exist by
        /// checking the time in which the server returns an error (this is an attack vector called 'timing attack')!
        /// </summary>
        void SimulatePasswordAuthenticationForNonExistingUser();
    }

    /// <summary>
    /// for saving and loading the database
    /// </summary>
    public interface IImportExportManager
    {
        /// <summary>
        /// BackupDatabaseToYmlGZ
        /// </summary>
        string BackupDatabaseToYmlGZ();

        /// <summary>
        /// RestoreDatabaseFromYmlGZ
        /// </summary>
        bool RestoreDatabaseFromYmlGZ(string AYmlGzData);
    }

    /// <summary>
    /// for updating the database
    /// </summary>
    public interface IDBUpgrades
    {
        /// <summary>
        /// UpgradeDatabase
        /// </summary>
        bool UpgradeDatabase();
    }

    /// <summary>
    /// an interface for logging to the database
    /// </summary>
    public interface IErrorLog
    {
        /// <summary>
        /// add an error log
        /// </summary>
        void AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3);

        /// <summary>
        /// add an error log
        /// </summary>
        void AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            String AUserID,
            Int32 AProcessID);
    }

    /// <summary>
    /// an interface for login logging to the database
    /// </summary>
    public interface ILoginLog
    {
        /// <summary>
        /// Records the logging-out (=disconnection) of a Client.
        /// </summary>
        /// <param name="AUserID">UserID of the User for which a logout should be recorded.</param>
        /// <param name="AProcessID">ProcessID of the User for which a logout should be recorded.
        /// This will need to be the number that got returned from an earlier call to
        /// AddLoginLogEntry(string, bool, string, bool, out int, TDBTransaction)!</param>
        /// <param name="ATransaction">Either an instantiated DB Transaction, or null. In the latter case
        /// a separate DB Connection gets opened, a DB Transaction on that separate DB Connection gets started,
        /// then committed/rolled back and the separate DB Connection gets closed. This is needed when this Method
        /// gets called from Method 'Ict.Common.Remoting.Server.TDisconnectClientThread.StartClientDisconnection()'!</param>
        void RecordUserLogout(String AUserID, int AProcessID, TDBTransaction ATransaction);
    }

    /// <summary>
    /// an interface for retrieving a welcome message from the databse
    /// </summary>
    public interface IMaintenanceLogonMessage
    {
        /// <summary>
        /// get a welcome message
        /// </summary>
        string GetLogonMessage(IPrincipal UserInfo, Boolean AReturnEnglishIfNotFound, TDBTransaction ATransaction);
    }

    /// an interface for system defaults cache
    public interface ISystemDefaultsCache
    {
        /// <summary>
        /// get boolean default value
        /// </summary>
        bool GetBooleanDefault(String AKey, bool ADefault);

        /// <summary>
        /// get int default
        /// </summary>
        System.Int64 GetInt64Default(String AKey);
    }
}
