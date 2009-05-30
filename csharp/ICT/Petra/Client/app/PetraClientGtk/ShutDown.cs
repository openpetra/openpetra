/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank, timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Diagnostics;
using Ict.Common.GTK;
using Ict.Petra.Client.App.Core;

namespace PetraClientShutdown
{
/// <summary>
/// Description of Shutdown.
/// </summary>
public static class Shutdown
{
    /// <summary>
    /// save the user defaults and disconnect from the Petra server
    /// </summary>
    public static void SaveUserDefaultsAndDisconnect()
    {
        String CantDisconnectReason;

        try
        {
            TUserDefaults.SaveChangedUserDefaults();

            if (!Ict.Petra.Client.App.Core.ConnectionManagement.GConnectionManagement.DisconnectFromServer(out CantDisconnectReason))
            {
#if TESTMODE
                TLogging.Log("cannot disconnect: " + CantDisconnectReason);
#endif
#if  TESTMODE
#else
#if DEBUGMODE
                MessageBox.Show(CantDisconnectReason, "Error on Client Disconnection");
#endif
#endif
            }
        }
        catch (Exception Exp)
        {
#if DEBUGMODE
            MessageBox.Show("DEBUGMODE Information: Unhandled exception while disconnecting from Servers: " + "\r\n" + Exp.ToString());
#endif
        }
    }

    /// <summary>
    /// save the user defaults, disconnect from the Petra server and stop the Petra client
    /// </summary>
    public static void SaveUserDefaultsAndDisconnectAndStop()
    {
        SaveUserDefaultsAndDisconnect();
        StopPetraClient();
    }

    /// <summary>
    /// stop the server (if standalone)
    /// stop the client
    /// </summary>
    public static void StopPetraClient()
    {
        if (TClientSettings.RunAsStandalone == true)
        {
            StopServers();
        }

        // APPLICATION STOPS HERE !!!
        Environment.Exit(0);
    }

    /// <summary>
    /// Stops the PostgreSql Server and Petra Server Console
    /// </summary>
    public static void StopServers()
    {
        System.Diagnostics.Process PetraServerProcess;

        // stop the Petra server (e.g. c:\petra2\bin22\PetraServerAdminConsole.exe C:C:\petra2\ServerAdminStandalone.config Command:Stop
        try
        {
            PetraServerProcess = new System.Diagnostics.Process();
            PetraServerProcess.StartInfo.FileName = TClientSettings.Petra_Path_Bin + "/PetraServerAdminConsole.exe";
            PetraServerProcess.StartInfo.WorkingDirectory = TClientSettings.Petra_Path_Bin;
            PetraServerProcess.StartInfo.Arguments = "-C:" + TClientSettings.DelphiServerAdmin_Configfile + " -Command:Stop";
            PetraServerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            PetraServerProcess.EnableRaisingEvents = false;

            if (!PetraServerProcess.Start())
            {
#if TESTMODE
                TLogging.Log("failed to start " + PetraServerProcess.StartInfo.FileName);
#endif
                return;
            }
        }
        catch (Exception exp)
        {
#if TESTMODE
            TLogging.Log("Exception while shutting down delphi server process: " + exp.ToString());
#endif
#if  TESTMODE
#else
            MessageBox.Show("Exception while shutting down delphi server process: " + exp.ToString());
#endif
            return;
        }

        PetraServerProcess.WaitForExit(20000);

        StopPostgreSqlServer();
    }

    private static void StopPostgreSqlServer()
    {
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
#endif
#if  TESTMODE
#else
                MessageBox.Show("Exception while shutting down PostgreSql server process: " + exp.ToString());
#endif
                return;
            }
            PostgreSqlServerProcess.WaitForExit(20000);
        }
    }
}
}