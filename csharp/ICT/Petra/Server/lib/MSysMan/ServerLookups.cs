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
using Ict.Common.IO;
using Ict.Common.Exceptions;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using System.Security;

namespace Ict.Petra.Server.MSysMan.Application.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MSysMan.ServerLookups
    /// sub-namespace.
    /// </summary>
    public class TSysManServerLookups
    {
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
        /// Method to obtain the SMTP email server configuration settings from the OP server, called to initialize FastReport Preview email settings.
        /// </summary>
        /// <param name="ASMTPHost">Returns the name or address for the SMTP server.</param>
        /// <param name="ASMTPPort">Returns the TCP port to use. Default = 25.</param>
        /// <param name="AEnableSsl">Flags whether to use SSL. Default = true.</param>
        /// <param name="ALoginUsername">Returns a user name to use for credentials for the server.</param>
        /// <param name="ALoginPassword">Returns a matching password for credentials on the server.</param>
        /// <exception cref="ESmtpSenderInitializeException">Thrown when SmtpHost is blank or the default <see cref="TSmtpSender.SMTP_HOST_DEFAULT"/>;
        /// when SmtpPort is invalid; or when SmtpAuthenticationType is unrecognised.</exception>
        [RequireModulePermission("NONE")]
        public static void GetServerSmtpSettings(out string ASMTPHost,
            out int ASMTPPort,
            out bool AEnableSsl,
            out string ALoginUsername,
            out string ALoginPassword)
        {
            ASMTPHost = TSrvSetting.SmtpHost;
            ASMTPPort = TSrvSetting.SmtpPort;
            AEnableSsl = TSrvSetting.SmtpEnableSsl;
            ALoginUsername = null;
            ALoginPassword = null;

            // Validate the host name.  It should not be the content of an unmodified config file.
            if ((ASMTPHost == "") || ASMTPHost.EndsWith(TSmtpSender.SMTP_HOST_DEFAULT))
            {
                throw new ESmtpSenderInitializeException(String.Format(
                        "SmtpHost '{0}' not valid in Server.config file. Contact your System Administrator.", ASMTPHost));
            }

            if ((ASMTPPort < System.Net.IPEndPoint.MinPort) || (ASMTPPort > System.Net.IPEndPoint.MaxPort))
            {
                throw new ESmtpSenderInitializeException(String.Format(
                        "SmtpPort '{0}' not valid in Server.config file. Contact your System Administrator.", ASMTPPort));
            }

            // Could use a TSmtpAuthTypeEnum - but naming conventions would dictate the config file settings would be ugly: satBuiltin, satConfig etc.
            string SmtpAuth = TSrvSetting.SmtpAuthenticationType;

            switch (SmtpAuth)
            {
                case "builtin":
                    // We give the client the details of the OP Email user.
                    // The password is converted from a byte array (rather than being compiled into this DLL as plain text).
                    // The username and password are stored in different server DLL's.
                    ALoginUsername = TSmtpSender.EMAIL_USER_LOGIN_NAME;
                    ALoginPassword = Encoding.ASCII.GetString(TPasswordHelper.EmailUserPassword);
                    break;

                case "config":
                    ALoginUsername = TSrvSetting.SmtpUser;
                    ALoginPassword = TSrvSetting.SmtpPassword;
                    break;

                default:
                    throw new ESmtpSenderInitializeException(String.Format(
                        "SmtpAuthenticationType '{0}' not valid in Server.config file. Contact your System Administrator.", SmtpAuth));
            }
        }

        /// <summary>
        /// Method to obtain the SMTP email server configuration settings from the OP server, called to initialize TSmtpSender by autoemailed reports.
        /// </summary>
        /// <returns><see cref="TSmtpServerSettings"/> struct.</returns>
        /// <exception cref="ESmtpSenderInitializeException">Thrown when SmtpHost is blank or the default <see cref="TSmtpSender.SMTP_HOST_DEFAULT"/>;
        /// when SmtpPort is invalid; or when SmtpAuthenticationType is unrecognised.</exception>
        [RequireModulePermission("NONE")]
        public static Ict.Common.IO.TSmtpServerSettings GetServerSmtpSettings()
        {
            string SmtpHost;
            int SmtpPort;
            bool EnableSsl;
            string LoginUsername;
            string s;
            bool IgnoreServerCertificateValidation;

            GetServerSmtpSettings(out SmtpHost,
                out SmtpPort,
                out EnableSsl,
                out LoginUsername,
                out s);

#if USE_SECURESTRING
            var LoginPassword = new SecureString();

            foreach (char c in s)
            {
                LoginPassword.AppendChar(c);
            }

            s = null;
            IgnoreServerCertificateValidation = TSrvSetting.SmtpIgnoreServerCertificateValidation;
            return new TSmtpServerSettings(SmtpHost, SmtpPort, EnableSsl, LoginUsername, LoginPassword, IgnoreServerCertificateValidation);
#else
            IgnoreServerCertificateValidation = TSrvSetting.SmtpIgnoreServerCertificateValidation;
            return new TSmtpServerSettings(SmtpHost, SmtpPort, EnableSsl, LoginUsername, s, IgnoreServerCertificateValidation);
#endif
        }
    }
}
