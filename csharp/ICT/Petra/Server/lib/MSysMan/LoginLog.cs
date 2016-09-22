//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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

using Ict.Common.DB;

namespace Ict.Petra.Server.MSysMan.Security
{
    /// <summary>
    /// Adds records to the s_login DB Table. That DB Table contains a log of all the log-ins/log-in attempts to
    /// the system, and of log-outs from the system.
    /// </summary>
    /// <remarks>Calls methods that have the same name in the Ict.Petra.Server.App.Core.Security.LoginLog Namespace
    /// to perform its functionality!</remarks>
    public class TLoginLog
    {
        /// <summary>User login was successful.</summary>
        public const string LOGIN_STATUS_TYPE_LOGIN_SUCCESSFUL = "LOGIN_SUCCESSFUL";
        /// <summary>User login of a user that has got SYSAMIN privileges was successful.</summary>
        public const string LOGIN_STATUS_TYPE_LOGIN_SUCCESSFUL_SYSADMIN = "LOGIN_SUCCESSFUL_SYSADMIN";
        /// <summary>A login attempt was made for a User ID but the password provided was wrong.</summary>
        public const string LOGIN_STATUS_TYPE_LOGIN_ATTEMPT_PWD_WRONG = "LOGIN_ATTEMPT_PWD_WRONG";
        /// <summary>A login attempt was made for a User ID but the password provided was wrong
        /// and the permitted number of failed logins in a row got exceeded. Because of this the user
        /// account for the user got locked!</summary>
        public const string LOGIN_STATUS_TYPE_LOGIN_ATTEMPT_PWD_WRONG_ACCOUNT_GOT_LOCKED =
            "LOGIN_ATTEMPT_PWD_WRONG_ACCOUNT_GOT_LOCKED";
        /// <summary>A login attempt was made for a UserID that doesn't exist.</summary>
        public const string LOGIN_STATUS_TYPE_LOGIN_ATTEMPT_FOR_NONEXISTING_USER = "LOGIN_ATTEMPT_FOR_NONEXISTING_USER";
        /// <summary>A login attempt was made for a UserID whose user account is Locked.</summary>
        public const string LOGIN_STATUS_TYPE_LOGIN_ATTEMPT_FOR_LOCKED_USER = "LOGIN_ATTEMPT_FOR_LOCKED_USER";
        /// <summary>A login attempt was made for a UserID that is Retired.</summary>
        public const string LOGIN_STATUS_TYPE_LOGIN_ATTEMPT_FOR_RETIRED_USER = "LOGIN_ATTEMPT_FOR_RETIRED_USER";
        /// <summary>A login attempt was made while the System was Disabled.</summary>
        public const string LOGIN_STATUS_TYPE_LOGIN_ATTEMPT_WHEN_SYSTEM_WAS_DISABLED = "LOGIN_ATTEMPT_WHEN_SYSTEM_WAS_DISABLED";
        // See also constant LOGIN_STATUS_TYPE_LOGOUT in Ict.Petra.Server.App.Core.Security.TLoginLog!

        /// <summary>
        /// Adds a record to the s_login DB Table. That DB Table contains a log of all the log-ins/log-in attempts to
        /// the system, and of log-outs from the system.
        /// </summary>
        /// <param name="AUserID">UserID of the User for which a record should be written.</param>
        /// <param name="ALoginType">Type of the login/logout record. This is a hard-coded constant value
        /// (there's no 'lookup table' for it); for available values and their meaning please check program code
        /// (Ict.Petra.Server.MSysMan.Security.TLoginLog Class).</param>
        /// <param name="ALoginDetails">Details/description of the login/login attempt/logout</param>
        /// <param name="AProcessID">'Process ID'; this is a unique key and comes from a sequence (seq_login_process_id).</param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        public static void AddLoginLogEntry(String AUserID, String ALoginType, String ALoginDetails, out Int32 AProcessID,
            TDBTransaction ATransaction)
        {
            Ict.Petra.Server.App.Core.Security.TLoginLog.AddLoginLogEntry(AUserID, ALoginType,
                ALoginDetails, out AProcessID, ATransaction);
        }

        /// <summary>
        /// Records the logging-out (=disconnection) of a Client to the s_login DB Table. That DB Table contains a log
        /// of all the log-ins/log-in attempts to the system, and of log-outs from the system.
        /// </summary>
        /// <param name="AUserID">UserID of the User for which a logout should be recorded.</param>
        /// <param name="AProcessID">ProcessID of the User for which a logout should be recorded.
        /// This will need to be the number that got returned from an earlier call to
        /// <see cref="AddLoginLogEntry(string, string, string, out int, TDBTransaction)"/>!</param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        public static void RecordUserLogout(String AUserID, int AProcessID, TDBTransaction ATransaction)
        {
            new Ict.Petra.Server.App.Core.Security.TLoginLog().RecordUserLogout(AUserID, AProcessID, ATransaction);
        }
    }
}