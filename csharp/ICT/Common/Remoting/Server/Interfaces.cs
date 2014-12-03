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
using System.Security.Principal;
using Ict.Common;
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
        /// add a new user
        /// </summary>
        bool AddUser(string AUserID, string APassword = "");

        /// <summary>
        /// authenticate a user
        /// </summary>
        IPrincipal PerformUserAuthentication(string AUserName, string APassword,
            out Boolean ASystemEnabled);
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
    /// an interface for retrieving a welcome message from the databse
    /// </summary>
    public interface IMaintenanceLogonMessage
    {
        /// <summary>
        /// get a welcome message
        /// </summary>
        string GetLogonMessage(IPrincipal UserInfo, Boolean AReturnEnglishIfNotFound);
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