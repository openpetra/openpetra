//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ALoginStatus"></param>
        /// <param name="AProcessID"></param>
        public static void AddLoginLogEntry(String AUserID,
            String ALoginStatus,
            out Int32 AProcessID)
        {
            Ict.Petra.Server.App.Core.Security.TLoginLog.AddLoginLogEntry(AUserID,
                ALoginStatus,
                out AProcessID);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ALoginStatus"></param>
        /// <param name="AImmediateLogout"></param>
        /// <param name="AProcessID"></param>
        public static void AddLoginLogEntry(String AUserID,
            String ALoginStatus,
            Boolean AImmediateLogout,
            out Int32 AProcessID)
        {
            Ict.Petra.Server.App.Core.Security.TLoginLog.AddLoginLogEntry(AUserID,
                ALoginStatus,
                AImmediateLogout,
                out AProcessID);
        }
    }
}