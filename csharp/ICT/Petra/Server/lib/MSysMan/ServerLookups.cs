//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, berndr, timop
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
using System.Data;
using System.Text;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.Application.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MSysMan.ServerLookups
    /// sub-namespace.
    /// </summary>
    public class TSysManServerLookups
    {
        // This is the password for the IUSROPEMAIL user.  If authentication is required by the EMail server so that clients can send emails from
        // connections on the public internet, we can tell the client to authenticate using these credentials.
        // That way they do not need to supply their own login credentials which we would have to store somewhere.
        // The sysadmin for the servers needs to create this user with low privileges accessible by the mail server (locally or using Active Directory).
        // The password must be set to 'never expires' and 'cannot be changed'.
        // Note that the password is not stored in this file as text and it is never exposed to a client.
        // Password is ....
        private static byte[] EmailUserPassword = new byte[] {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
        };

        /// <summary>
        /// Retrieves the current database version
        /// </summary>
        /// <param name="APetraDBVersion">Current database version</param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static System.Boolean GetDBVersion(out System.String APetraDBVersion)
        {
            TDBTransaction ReadTransaction = null;

            APetraDBVersion = "Cannot retrieve DB version";
            TLogging.LogAtLevel(9, "TSysManServerLookups.GetDatabaseVersion called!");

            SSystemDefaultsTable SystemDefaultsDT = new SSystemDefaultsTable();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                delegate
                {
                    // Load data
                    SystemDefaultsDT = SSystemDefaultsAccess.LoadByPrimaryKey("CurrentDatabaseVersion", ReadTransaction);
                });

            if (SystemDefaultsDT.Rows.Count < 1)
            {
                throw new EOPAppException(
                    "TSysManServerLookups.GetDBVersion: s_system_defaults DB Table is empty; this is unexpected and can lead to sever malfunction of OpenPetra. Contact your Support Team.");
            }

            SSystemDefaultsRow sysrow = SystemDefaultsDT.Rows[0] as SSystemDefaultsRow;

            if (sysrow == null)
            {
                throw new EOPAppException(
                    "TSysManServerLookups.GetDBVersion: s_system_defaults DB Table is empty; this is unexpected and can lead to sever malfunction of OpenPetra. Contact your Support Team.");
            }

            APetraDBVersion = sysrow.DefaultValue;

            return true;
        }

        /// <summary>
        /// Retrieves a list of all installed Patches
        /// </summary>
        /// <param name="APatchLogDT">The installed patches</param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static System.Boolean GetInstalledPatches(out Ict.Petra.Shared.MSysMan.Data.SPatchLogTable APatchLogDT)
        {
            SPatchLogTable TmpTable = new SPatchLogTable();

            APatchLogDT = new SPatchLogTable();
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            TLogging.LogAtLevel(9, "TSysManServerLookups.GetInstalledPatches called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                // Load data
                TmpTable = SPatchLogAccess.LoadAll(ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TSysManServerLookups.GetInstalledPatches: committed own transaction.");
                }
            }

            /* Sort the data...
             */
            TmpTable.DefaultView.Sort = SPatchLogTable.GetDateRunDBName() + " DESC, " +
                                        SPatchLogTable.GetPatchNameDBName() + " DESC";

            /* ...and put it in the output table.
             */
            for (int Counter = 0; Counter < TmpTable.DefaultView.Count; ++Counter)
            {
                TLogging.LogAtLevel(7, "Patch: " + TmpTable.DefaultView[Counter][0]);
                APatchLogDT.ImportRow(TmpTable.DefaultView[Counter].Row);
            }

            return true;
        }

        /// <summary>
        /// Method to obtain the Smtp Configuration settings from the OP server that has been set up to act as an Email server
        /// </summary>
        /// <param name="ASMTPHost">The host address.  Default is empty string (not set).
        /// If the name contains 'example.org' the parameter also returns an empty string</param>
        /// <param name="ASMTPPort">Default return value is 25.</param>
        /// <param name="AEnableSsl">Default return value id 'false'.</param>
        /// <param name="ALoginUsername">Returns a user name to use for credentials for the server, or null if no credentials are required</param>
        /// <param name="ALoginPassword">Returns a matching password for credentials on the server, or null if no credentials are required</param>
        [RequireModulePermission("NONE")]
        public static void GetServerSmtpSettings(out string ASMTPHost,
            out int ASMTPPort,
            out bool AEnableSsl,
            out string ALoginUsername,
            out string ALoginPassword)
        {
            ASMTPHost = TAppSettingsManager.GetValue("SmtpHost", "");
            ASMTPPort = TAppSettingsManager.GetInt32("SmtpPort", 25);
            AEnableSsl = TAppSettingsManager.GetBoolean("SmtpEnableSsl", false);
            ALoginUsername = null;
            ALoginPassword = null;

            // Validate the host name.  It should not be the content of an unmodified config file.
            if (ASMTPHost.Contains("example.org"))
            {
                ASMTPHost = string.Empty;
                return;
            }

            if (TAppSettingsManager.GetBoolean("SmtpRequireCredentials", false) == true)
            {
                // We give the client the details of the OP Email user.
                // The password is converted from a byte array (rather than being compiled into this DLL as plain text).
                // The username and password are stored in different server DLL's.
                ALoginUsername = MSysManConstants.EMAIL_USER_LOGIN_NAME;
                ALoginPassword = Encoding.ASCII.GetString(EmailUserPassword);
            }
        }
    }
}
