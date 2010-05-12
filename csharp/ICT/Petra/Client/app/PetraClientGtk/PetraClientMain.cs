//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.GTK;
using Ict.Petra.Shared;
using Ict.Petra.Shared.DataStore.TableList;
using Ict.Petra.Shared.DataStore;
using Ict.Petra.Shared.Security;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonDialogs;
using PetraClientShutdown;

namespace PetraClientMain
{
/// <summary>
/// this class manages the startup of the client
/// </summary>
public class TPetraClientMain
{
    private static TFrmMain GMainWindow;

    /// <summary>tells whether the Login was successful, or not</summary>
    private static Boolean FLoginSuccessful;

    /// <summary>ProcessID (unique) assigned by the PetraServer</summary>
    private static Int32 FProcessID;

    /// <summary>Welcome message (passed on to the MainWindow)</summary>
    private static String FWelcomeMessage;

    /// <summary>Tells whether the Petra System is enabled, or not (passed on to the MainWindow)</summary>
    private static Boolean FSystemEnabled;
    private static TLogging FLogging;

    [DllImport("user32.dll")] private static extern int FindWindow(string classname, string windowname);
    [DllImport("user32.dll")] private static extern int SendMessage(
        int hWnd,                  // handle to destination window
        uint Msg,                   // message
        long wParam,              // first message parameter
        long lParam               // second message parameter
        );

    /// <summary>
    /// Loads read-only Client settings (from .NET Configuration File and Command
    /// Line)
    ///
    /// </summary>
    /// <returns>true if settings could be loaded, otherwise false.
    /// </returns>
    public static bool LoadClientSettings()
    {
        try
        {
            new TClientSettings();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Failure");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Display the Login Dialog, and get the permissions of the user that just has logged in
    /// </summary>
    /// <returns>void</returns>
    public static void PerformLogin()
    {
        try
        {
            TLoginForm ConnectDialog;

            // TODO: close current connection if it is open
            FLoginSuccessful = false;

            // Need to show and hide Connect Dialog before closing the Splash Screen so that it can receive input focus!
            ConnectDialog = new TLoginForm();

            // Show the Connect Dialog
            if (ConnectDialog.Run() != (int)Gtk.ResponseType.Accept)
            {
                FLoginSuccessful = false;
            }
            else
            {
                // TODO reset any caches
                ConnectDialog.GetReturnedParameters(out FProcessID, out FWelcomeMessage, out FSystemEnabled);

                // get Connection Dialog out of memory
                ConnectDialog.Destroy();

                FLoginSuccessful = true;
            }
        }
        catch (Exception exp)
        {
            MessageBox.Show("Exception caught in Method PerformLogin: " + exp.ToString());
        }
    }

    /// <summary>
    /// standalone must start their own server
    /// </summary>
    /// <returns></returns>
    public static Boolean StartServer()
    {
        Boolean ReturnValue;

        System.Diagnostics.Process PetraServerProcess;
        ReturnValue = true;

        if (!StartPostgreSqlServer())
        {
            return false;
        }

        // start the Petra server (e.g. c:\petra2\bin22\PetraServerConsole.exe C:C:\petra2\ServerStandalone.config RunWithoutMenu:true
        try
        {
            PetraServerProcess = new System.Diagnostics.Process();
            PetraServerProcess.EnableRaisingEvents = false;
            PetraServerProcess.StartInfo.FileName = TClientSettings.Petra_Path_Bin + "/PetraServerConsole.exe";
            PetraServerProcess.StartInfo.WorkingDirectory = TClientSettings.Petra_Path_Bin;
            PetraServerProcess.StartInfo.Arguments = "-C:" + TClientSettings.DelphiServer_Configfile + " -RunWithoutMenu:true";
            PetraServerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            PetraServerProcess.EnableRaisingEvents = false;

            if (PetraServerProcess.Start())
            {
            }
            // Include(UDelphiServerProcess.Exited, DelphiServerProcessEnd);
            else
            {
#if TESTMODE
                TLogging.Log("failed to start " + PetraServerProcess.StartInfo.FileName);
#endif
#if  TESTMODE
#else
                MessageBox.Show("failed to start " + PetraServerProcess.StartInfo.FileName);
#endif
                return false;
            }
        }
        catch (Exception exp)
        {
#if TESTMODE
            TLogging.Log("Exception while starting PetraServer process: " + exp.ToString());
#endif
#if  TESTMODE
#else
            MessageBox.Show("Exception while starting PetraServer process: " + exp.ToString());
#endif
            return false;
        }

        // We can't minimize the command window for the PostgreSql server directly after starting.
        // So we do it here.
        MinimizePostgreSqlWindow();

        return ReturnValue;
    }

    /// <summary>
    /// start up the client
    /// </summary>
    public static void StartUp()
    {
        Gtk.Application.Init();
        ExceptionHandlingUnit.GApplicationShutdownCallback = Shutdown.SaveUserDefaultsAndDisconnectAndStop;

        InitialiseClasses();

        if (!LoadClientSettings())
        {
            Environment.Exit(0);
        }

        FLogging = new TLogging(TClientSettings.PathTemp + "/PetraClient.log");

        /*
         * Show Petra Login screen.
         * Connection to the PetraServer is established in here as well.
         */
        try
        {
            PerformLogin();
        }
        catch (Exception)
        {
#if TESTMODE
            // in Testmode, if no connection, just stop here
            Environment.Exit(0);
#else
            throw;
#endif
        }

        // call this once; this table is used for the GUI, to figure out the field length etc.
        TPetraDataStore.FillSortedListTables();

        if (FLoginSuccessful)
        {
            try
            {
                if (TClientSettings.RunAsStandalone == true)
                {
                    ProcessReminders.StartStandaloneRemindersProcessing();
                }

                // This loads the Main Window of Petra
                GMainWindow = new TFrmMain(
                    System.Reflection.Assembly.GetEntryAssembly().GetName().Version,
                    FProcessID, FWelcomeMessage, FSystemEnabled);
                GMainWindow.ShowAll();

                Gtk.Application.Run();
            }
            finally
            {
                /*
                 * This code gets executed only after the Main Window of Petra has
                 * closed.
                 */
                Shutdown.SaveUserDefaultsAndDisconnect();
            }
        }
        else
        {
            // No successful login

            // APPLICATION STOPS IN THIS PROCEDURE !!!
            Shutdown.StopPetraClient();
        }

        // APPLICATION STOPS IN THIS PROCEDURE !!!
        Shutdown.StopPetraClient();
    }

    /// <summary>
    /// Start the PostgreSql database.
    /// If user has admin rights then start as a service.
    /// If user has no admin rights then start as an executible.
    /// </summary>
    /// <returns>true if startup was successful</returns>
    private static bool StartPostgreSqlServer()
    {
        System.Diagnostics.Process PostgreSqlServerProcess;

        if (TClientSettings.RunAsStandalone)
        {
            // start the PostgreSql server as exe(e.g. c:\Program Files\Postgres\8.3\bin\pg_ctl.exe -D C:\petra2\db23_pg start
            try
            {
                PostgreSqlServerProcess = new System.Diagnostics.Process();
                PostgreSqlServerProcess.StartInfo.FileName = "\"" + TClientSettings.PostgreSql_BaseDir + "\\bin\\pg_ctl.exe\"";
                PostgreSqlServerProcess.StartInfo.Arguments = "-D " + TClientSettings.PostgreSql_DataDir + " start";
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

                if (PostgreSqlServerProcess.Start())
                {
                }
                else
                {
#if TESTMODE
                    TLogging.Log("failed to start " + PostgreSqlServerProcess.StartInfo.FileName);
#endif
#if  TESTMODE
#else
                    MessageBox.Show("failed to start " + PostgreSqlServerProcess.StartInfo.FileName);
#endif
                    return false;
                }
            }
            catch (Exception exp)
            {
#if TESTMODE
                TLogging.Log("Exception while starting PostgreSql process: " + exp.ToString());
#endif
#if  TESTMODE
#else
                MessageBox.Show("Exception while starting PostgreSql process: " + exp.ToString());
#endif
                return false;
            }
            PostgreSqlServerProcess.WaitForExit(20000);

            return true;
        }

        return true;
    }

    /// <summary>
    /// Checks if the current user has Administrator rights
    /// </summary>
    /// <returns>true if the user has Admin rights</returns>
    public static bool HasAdministratorPrivileges()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);

        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    /// <summary>
    /// Perform necessary initializations of Classes
    /// </summary>
    /// <returns>void</returns>
    private static void InitialiseClasses()
    {
        TAppSettingsManager.InitializeUnit();
        TConnectionManagement.InitializeUnit();
        TStaticDataTables.InitializeUnit();
        TDataCache.InitializeUnit();
        TClientInfo.InitializeUnit();
        TSystemDefaults.InitializeUnit();
        TCacheableTablesManager.InitializeUnit();

        // I18N: assign proper font which helps to read asian characters
        // this is the first place where it is called, and we need to initialize the TAppSettingsManager
        TAppSettingsManager.InitFontI18N();
    }

    /// <summary>
    /// Minimize the command window that starts the PostgreSql database.
    /// </summary>
    private static void MinimizePostgreSqlWindow()
    {
        int wHandle = 0;

        String WindowName = TClientSettings.PostgreSql_BaseDir + "\\bin\\pg_ctl.exe";

        wHandle = FindWindow(null, WindowName);

        if (wHandle > 0)
        {
            // 0xF020 is the WM_MINIMIZE message
            SendMessage(wHandle, 0x0112, 0xF020, 0);
        }
    }
}
}