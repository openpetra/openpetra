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
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.Security
{
    ///<summary>
    /// Reads and saves entries in the Login Log table.
    ///
    /// @Comment Calls methods that have the same name in the
    ///   Ict.Petra.Server.App.Core.Security.LoginLog Namespace to perform its
    ///   functionality!
    ///</summary>
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

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ALoginSuccesful"></param>
        /// <param name="ALoginStatusType"></param>
        /// <param name="ALoginStatus"></param>
        /// <param name="AProcessID"></param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        public static void AddLoginLogEntry(String AUserID,
            Boolean ALoginSuccesful,
            String ALoginStatusType,
            String ALoginStatus,
            out Int32 AProcessID,
            TDBTransaction ATransaction)
        {
            Ict.Petra.Server.App.Core.Security.TLoginLog.AddLoginLogEntry(AUserID, ALoginSuccesful, ALoginStatusType,
                ALoginStatus, out AProcessID, ATransaction);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ALoginSuccesful"></param>
        /// <param name="ALoginStatusType"></param>
        /// <param name="ALoginStatus"></param>
        /// <param name="AImmediateLogout"></param>
        /// <param name="AProcessID"></param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        public static void AddLoginLogEntry(String AUserID,
            Boolean ALoginSuccesful,
            String ALoginStatusType,
            String ALoginStatus,
            Boolean AImmediateLogout,
            out Int32 AProcessID,
            TDBTransaction ATransaction)
        {
            Ict.Petra.Server.App.Core.Security.TLoginLog.AddLoginLogEntry(AUserID, ALoginSuccesful, ALoginStatusType,
                ALoginStatus,
                AImmediateLogout,
                out AProcessID, ATransaction);
        }
    }
}