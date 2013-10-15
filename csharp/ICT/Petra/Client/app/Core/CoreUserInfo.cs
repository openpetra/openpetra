//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.DB;
using System.Windows.Forms;
using Ict.Common.DB.Exceptions;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Holds User Information (particularly security-related) in a global variable
    /// and allows refreshing of this information.
    /// </summary>
    public class TUserInfo
    {
        /// <summary>
        /// Causes TUserInfo to immediately reload the cached UserInformation.
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void ReloadCachedUserInfo()
        {
            String ErrorText = "";

            try
            {
                Ict.Petra.Shared.UserInfo.GUserInfo = TRemote.MSysMan.Security.UserManager.WebConnectors.ReloadCachedUserInfo();
            }
            catch (EDBConnectionNotAvailableException Exp)
            {
                // don't raise this Exception; this is a background thing that doesn't need to be brought to
                // the User's attention. Once he logs out and in again he will have the correct UserInfo...

                if (Exp.InnerException is OdbcException)
                {
                    if (((OdbcException)(Exp.InnerException)).Errors[0].NativeError == -210005)                      // Progress Error: "Failure getting table lock on table PUB.s_user_table_access_permission"
                    {
                        ErrorText =
                            "ReloadCachedUserInfo: couldn't reload UserInfo because of an Exclusive-Lock on the DB Table (non-critical error).";
                    }
                    else
                    {
                        ErrorText =
                            "ReloadCachedUserInfo: couldn't reload UserInfo because of an ODBC error reported by the DB (non-critical error).";
                    }
                }
                else
                {
                    ErrorText = "ReloadCachedUserInfo: couldn't reload UserInfo because of an unkonwn DB problem (non-critical error).";
                }
            }

            if (ErrorText != "")
            {
                TLogging.Log(ErrorText);
            }
        }

        /// <summary>
        /// Queues a ClientTask for reloading of the UserInfo for all connected Clients
        /// with a certain UserID.
        ///
        /// </summary>
        /// <param name="AUserID">UserID for which the ClientTask should be queued
        /// </param>
        /// <returns>void</returns>
        public static void SignalReloadCachedUserInfo(String AUserID)
        {
            TRemote.MSysMan.Security.UserManager.WebConnectors.SignalReloadCachedUserInfo(AUserID);
        }
    }
}