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
using System.Diagnostics;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;

namespace PetraClientShutdown
{
    /// <summary>
    /// Handles various aspects of the shutdown of the PetraClient.
    /// </summary>
    public static class Shutdown
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public static void SaveUserDefaultsAndDisconnect()
        {
            String CantDisconnectReason;

            try
            {
                TUserDefaults.SaveChangedUserDefaults();

                if (!Ict.Petra.Client.App.Core.TConnectionManagement.GConnectionManagement.DisconnectFromServer(out CantDisconnectReason))
                {
#if TESTMODE
                    TLogging.Log("cannot disconnect: " + CantDisconnectReason);
#endif
#if  TESTMODE
#else
                    if (TLogging.DebugLevel > 0)
                    {
                        MessageBox.Show(CantDisconnectReason, "Error on Client Disconnection");
                    }
#endif
                }
            }
            catch (Exception Exp)
            {
                if (TLogging.DebugLevel > 0)
                {
                    MessageBox.Show("DEBUG Information: Unhandled exception while disconnecting from Servers: " + "\r\n" + Exp.ToString());
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void SaveUserDefaultsAndDisconnectAndStop()
        {
            StopPetraClient(true);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void StopPetraClient(bool ASaveUserDefaultsAndDisconnect, bool ARestart = false,
            bool AShowRestartMessageToUser = false, string ARestartReason = "")
        {
            if (ARestart
                && AShowRestartMessageToUser)
            {
                MessageBox.Show(String.Format(AppCoreResourcestrings.StrOpenPetraClientNeedsToBeRestarted, ARestartReason),
                    AppCoreResourcestrings.StrOpenPetraClientNeedsToBeRestartedTitle, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (ASaveUserDefaultsAndDisconnect)
            {
                Shutdown.SaveUserDefaultsAndDisconnect();
            }

            if (TClientSettings.RunAsStandalone == true)
            {
                StopServers();
            }

            if (ARestart)
            {
                // APPLICATION STOPS AND IMMEDIATELY RE-STARTS HERE !!!
                Application.Restart();
            }
            else
            {
                // APPLICATION STOPS HERE !!!
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Stops the Petra Server Console
        /// </summary>
        public static void StopServers()
        {
            StopPostgreSqlServer();
        }

        private static void StopPostgreSqlServer()
        {
#if TODO
            System.Diagnostics.Process PostgreSqlServerProcess;

            if (TClientSettings.RunAsStandalone)
            {
                // stop the PostgreSql server (e.g. c:\Program Files\Postgres\8.3\bin\pg_ctl.exe -D C:\petra2\db23_pg stop
                try
                {
                    PostgreSqlServerProcess = new System.Diagnostics.Process();
                    PostgreSqlServerProcess.StartInfo.FileName = "\"" + TClientSettings.PostgreSql_BaseDir + "\\bin\\pg_ctl.exe\"";
                    PostgreSqlServerProcess.StartInfo.Arguments = "-D " + TClientSettings.PostgreSql_DataDir + " stop";
                    PostgreSqlServerProcess.StartInfo.WorkingDirectory = TClientSettings.PostgreSql_DataDir;
                    PostgreSqlServerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    PostgreSqlServerProcess.EnableRaisingEvents = false;
                    PostgreSqlServerProcess.StartInfo.UseShellExecute = false;

                    System.Security.SecureString MyPassword = new System.Security.SecureString();
                    String Pwd = "petra";

                    foreach (char c in Pwd)
                    {
                        MyPassword.AppendChar(c);
                    }

                    PostgreSqlServerProcess.StartInfo.Password = MyPassword;
                    PostgreSqlServerProcess.StartInfo.UserName = "petrapostgresqluser";

                    if (!PostgreSqlServerProcess.Start())
                    {
#if TESTMODE
                        TLogging.Log("failed to start " + PostgreSqlServerProcess.StartInfo.FileName);
#endif
                        return;
                    }
                }
                catch (Exception exp)
                {
#if TESTMODE
                    TLogging.Log("Exception while shutting down PostgreSql server process: " + exp.ToString());
#else
                    MessageBox.Show("Exception while shutting down PostgreSql server process: " + exp.ToString());
#endif
                    return;
                }
                PostgreSqlServerProcess.WaitForExit(20000);
            }
#endif
        }
    }
}