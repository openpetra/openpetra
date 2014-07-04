//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Windows.Forms;
using System.Globalization;

using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner; // Implicit reference
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MCommon.Gui;
using Ict.Petra.Client.MConference.Gui;
using Ict.Petra.Client.MFinance.Gui.Gift;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Client.MPartner.Gui.Extracts;
using Ict.Petra.Client.MPartner.Gui.Setup;
using Ict.Petra.Client.MPersonnel.Gui;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MPartner;
using Ict.Petra.Client.MSysMan.Gui;
using SplashScreen;
using PetraClientShutdown;
using Ict.Common.Remoting.Shared;
using System.Data;

namespace Ict.Petra.Client.App.PetraClient
{
    /// <summary>
    /// Main Unit for the Petra Client application.
    ///
    /// Startup of the application begins here.
    /// </summary>
    public class TPetraClientMain
    {
        private static TSplashScreenManager FSplashScreen;

        /// <summary>tells whether the Login was successful, or not</summary>
        private static Boolean FLoginSuccessful;

        // <summary>ProcessID (unique) assigned by the PetraServer</summary>
        // TODO private static Int32 FProcessID;

        // <summary>Welcome message (passed on to the MainWindow)</summary>
        // TODO private static String FWelcomeMessage;

        // <summary>Tells whether the Petra System is enabled, or not (passed on to the MainWindow)</summary>
        // TODO private static Boolean FSystemEnabled;

        [DllImport("user32.dll")] private static extern int FindWindow(string classname, string windowname);
        [DllImport("user32.dll")] private static extern int SendMessage(
            int hWnd,                    // handle to destination window
            uint Msg,                     // message
            long wParam,                // first message parameter
            long lParam                 // second message parameter
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
                TLogging.Log(e.ToString());
                FSplashScreen.ShowMessageBox(e.Message, "Failure");
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
                TLoginForm AConnectDialog;

                // TODO: close current connection if it is open
                FLoginSuccessful = false;

                // Need to show and hide Connect Dialog before closing the Splash Screen so that it can receive input focus!
                AConnectDialog = new TLoginForm();

                // this causes a bug on Mono. see bug #590. inactive login screen
                // and it is not needed because the splashscreen is disabled for the moment anyway
                // AConnectDialog.Show();
                // AConnectDialog.Visible = false;

                // Close Splash Screen
                FSplashScreen.Close();
                FSplashScreen = null;

                // Show the Connect Dialog
                if (AConnectDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    FLoginSuccessful = false;
                }
                else
                {
                    // TODO reset any caches
                    // TODO AConnectDialog.GetReturnedParameters(out FProcessID, out FWelcomeMessage, out FSystemEnabled);

                    // get Connection Dialog out of memory
                    AConnectDialog.Dispose();

                    FLoginSuccessful = true;

                    Ict.Petra.Client.MSysMan.Gui.TUC_GeneralPreferences.InitLanguageAndCulture();
                }
            }
            catch (Exception exp)
            {
                if (FSplashScreen == null)
                {
                    MessageBox.Show("Exception caught in Method PerformLogin: " + exp.ToString());
                }
                else
                {
                    FSplashScreen.ShowMessageBox("Exception caught in Method PerformLogin: " + exp.ToString());
                }
            }
        }

        /// <summary>
        /// start the Petra server
        /// </summary>
        /// <returns></returns>
        public static Boolean StartServer()
        {
            System.Diagnostics.Process PetraServerProcess;

            // start the Petra server (e.g. c:\petra2\bin22\PetraServerConsole.exe C:C:\petra2\ServerStandalone.config RunWithoutMenu:true
            try
            {
                FSplashScreen.ProgressText = Catalog.GetString("Starting OpenPetra Server...");
                PetraServerProcess = new System.Diagnostics.Process();
                PetraServerProcess.EnableRaisingEvents = false;
                PetraServerProcess.StartInfo.FileName = TClientSettings.Petra_Path_Bin + "/PetraServerConsole.exe";
                PetraServerProcess.StartInfo.WorkingDirectory = TClientSettings.Petra_Path_Bin;
                PetraServerProcess.StartInfo.Arguments = "-C:\"" + TClientSettings.PetraServer_Configfile + "\" -RunWithoutMenu:true";
                PetraServerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                PetraServerProcess.EnableRaisingEvents = false;

                if (!PetraServerProcess.Start())
                {
#if TESTMODE
                    TLogging.Log("failed to start " + PetraServerProcess.StartInfo.FileName);
#endif
#if  TESTMODE
#else
                    FSplashScreen.ShowMessageBox("failed to start " + PetraServerProcess.StartInfo.FileName);
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
                FSplashScreen.ShowMessageBox("Exception while starting OpenPetra Server process: " + exp.ToString());
#endif
                return false;
            }
#if TODO
            // We can't minimize the command window for the PostgreSql server directly after starting.
            // So we do it here.
            MinimizePostgreSqlWindow();
#endif
            return true;
        }

        /// <summary>
        /// Gets called from the Splash Screen to provide information about the Petra installation.
        /// </summary>
        /// <returns>void</returns>
        private static void SplashScreenInfoCallback(out string APetraVersion, out string AInstallationKind, out string ACustomText)
        {
            APetraVersion = TClientInfo.ClientAssemblyVersion;
            AInstallationKind = TClientInfo.InstallationKind;
            ACustomText = TClientSettings.CustomStartupMessage;
        }

        /// <summary>
        /// this is usually only used for remote clients; standalone clients are patched with a windows installer program
        /// </summary>
        private static void CheckForPatches()
        {
            FSplashScreen.ProgressText = "Running checks that are specific to Remote Installation...";

            // todo: check whether the user has SYSADMIN rights; should not be required
            // todo: check whether the user has write access to the bin directory
            // check whether the user has access to the server and the Petra patches directory
            if ((TClientSettings.Petra_Path_RemotePatches.Length > 0)
                && !(TClientSettings.Petra_Path_RemotePatches.ToLower().StartsWith("http://")
                     || TClientSettings.Petra_Path_RemotePatches.ToLower().StartsWith("https://"))
                && !System.IO.Directory.Exists(TClientSettings.Petra_Path_RemotePatches))
            {
                FSplashScreen.ShowMessageBox(
                    String.Format(
                        Catalog.GetString(
                            "Please make sure that you have logged in to your network drive\nand can access the directory\n{0}\nIf this is the case and you still get this message,\nyou might use an IP address rather than a hostname for the server.\nPlease ask your local System Administrator for help."),
                        TClientSettings.Petra_Path_RemotePatches),
                    Catalog.GetString("Cannot check for patches"));
            }

            // check whether there is a patch available; if this is a remote version, try to download a patch from the server
            TPatchTools patchTools = new TPatchTools(Path.GetFullPath(TClientSettings.Petra_Path_Bin + Path.DirectorySeparatorChar + ".."),
                TClientSettings.Petra_Path_Bin,
                TPatchTools.OPENPETRA_VERSIONPREFIX,
                TClientSettings.PathTemp,
                "",
                TClientSettings.Petra_Path_Patches,
                TClientSettings.Petra_Path_RemotePatches);

            string PatchStatusMessage;

            // TODO: run this only if necessary. seem adding cost centre does not update the cache?
            TDataCache.ClearAllCaches();

            if (patchTools.CheckForRecentPatch(false, out PatchStatusMessage))
            {
                // todo: display a list of all patches that will be installed? or confusing with different builds?
                if (FSplashScreen.ShowMessageBox(String.Format(Catalog.GetString("There is a new patch available: {0}" +
                                ".\r\nThe currently installed version is {1}" +
                                ".\r\nThe patch will be installed to directory '{2}'.\r\nDo you want to install now?"),
                            patchTools.GetLatestPatchVersion(), patchTools.GetCurrentPatchVersion(), TClientSettings.Petra_Path_Bin),
                        String.Format(Catalog.GetString("Install new OpenPetra patch")), MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    // reset the caches in IsolatedStorage. This can help if things have changed drastically in the database
                    // TODO: run this also after the software has been reinstalled with the InnoSetup installer? Remember the current patch number in the IsolatedStorage?
                    TDataCache.ClearAllCaches();

                    // create the temp directory; using the Petra tmp directory, so that we don't need to change the drive in the batch file
                    string TempPath = TClientSettings.PathTemp + Path.DirectorySeparatorChar + "petrapatch";
                    Directory.CreateDirectory(TempPath);

                    // check for newer patchtool
                    patchTools.CopyLatestPatchProgram(TempPath);

                    string PatchToolExe = TempPath + Path.DirectorySeparatorChar + "Ict.Tools.PatchTool.exe";

                    if (!File.Exists(PatchToolExe))
                    {
                        TLogging.Log("cannot find file " + PatchToolExe);
                    }

                    // need to stop petra client, start the patch in temppath, restart Petra client
                    Process PatchProcess = new System.Diagnostics.Process();
                    PatchProcess.EnableRaisingEvents = false;
                    PatchProcess.StartInfo.FileName = PatchToolExe;
                    PatchProcess.StartInfo.Arguments = "-action:patchRemote " +
                                                       "-ClientConfig:\"" + Path.GetFullPath(
                        TAppSettingsManager.ConfigFileName) + "\" " +
                                                       "-OpenPetra.Path.Patches:\"" + Path.GetFullPath(
                        TClientSettings.Petra_Path_Bin + "/../patches30") + "\" " +
                                                       "-OpenPetra.PathTemp:\"" + Path.GetFullPath(
                        TClientSettings.Petra_Path_Bin + "/../tmp30") + "\" " +
                                                       "-OpenPetra.Path:\"" + Path.GetFullPath(
                        TClientSettings.Petra_Path_Bin + Path.DirectorySeparatorChar + "..") + "\" " +
                                                       "-OpenPetra.Path.Bin:\"" + Path.GetFullPath(
                        TClientSettings.Petra_Path_Bin) + "\"";
                    PatchProcess.Start();

                    // Application stops here !!!
                    Environment.Exit(0);
                }
            }
            else
            {
                if (PatchStatusMessage != String.Empty)
                {
                    FSplashScreen.ShowMessageBox(PatchStatusMessage, "");
                }
            }
        }

        /// <summary>
        /// start the client
        /// </summary>
        public static void StartUp()
        {
            // for the moment default to english, because translations are not fully supported, and the layout does not adjust
            string UsersLanguageCode = "en-EN";
            string UsersCultureCode = CultureInfo.CurrentCulture.Name;

            try
            {
                new TAppSettingsManager();

                ExceptionHandling.GApplicationShutdownCallback = Shutdown.SaveUserDefaultsAndDisconnectAndStop;

                TLogging Logger = new TLogging(TClientSettings.GetPathLog() + Path.DirectorySeparatorChar + "PetraClient.log");
                String LogFileMsg;

                if (!Logger.CanWriteLogFile(out LogFileMsg))
                {
                    MessageBox.Show(LogFileMsg, Catalog.GetString("Failed to open logfile"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                Catalog.Init();
#if DEBUG
                TApplicationVCSInfo.DetermineApplicationVCSInfo();
#endif
                // Register Types that can throw Error Codes (Ict.Common.CommonErrorCodes is automatically added)
                ErrorCodeInventory.RegisteredTypes.Add(new Ict.Petra.Shared.PetraErrorCodes().GetType());
                ErrorCodeInventory.RegisteredTypes.Add(new Ict.Common.Verification.TStringChecks().GetType());

                // Initialize the client
                TClientTasksQueue.ClientTasksInstanceType = typeof(TClientTaskInstance);
                TConnectionManagementBase.ConnectorType = typeof(TConnector);
                TConnectionManagementBase.GConnectionManagement = new TConnectionManagement();


//            System.Windows.Forms.MessageBox.Show(ErrorCodes.GetErrorInfo("GENC.00001V").ShortDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00001V").FullDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00001V").Category.ToString("G") + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00001V").HelpID);
//            System.Windows.Forms.MessageBox.Show(ErrorCodes.GetErrorInfo("GENC.00002V").ShortDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00002V").FullDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00002V").ErrorMessageText + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00002V").ErrorMessageTitle + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00002V").Category.ToString("G") + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00002V").HelpID);
//            System.Windows.Forms.MessageBox.Show(ErrorCodes.GetErrorInfo("GEN.00004E").ShortDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GEN.00004E").FullDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GEN.00004E").Category.ToString("G") + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GEN.00004E").HelpID);
//            System.Windows.Forms.MessageBox.Show(ErrorCodes.GetErrorInfo("PARTN.00005V").ShortDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("PARTN.00005V").FullDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("PARTN.00005V").Category.ToString("G") + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("PARTN.00005V").HelpID);
//            System.Windows.Forms.MessageBox.Show(ErrorCodes.GetErrorInfo("GENC.00017V").ShortDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00017V").FullDescription + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00017V").Category.ToString("G") + Environment.NewLine + Environment.NewLine +
//                                                 ErrorCodes.GetErrorInfo("GENC.00017V").HelpID);

//MessageBox.Show(ErrorCodes.GetErrorInfo(ERR_EMAILADDRESSINVALID).ShortDescription);

                // TODO another Catalog.Init("org", "./locale") for organisation specific words?
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
            }

            /* Show Splash Screen.
             * This is non-blocking since it is done in a separate Thread, that means
             * that the startup procedure continues while the Splash Screen is initialised and shown!!! */
            FSplashScreen = new TSplashScreenManager(new TSplashScreenCallback(SplashScreenInfoCallback));
            FSplashScreen.Show();

            /*
             * IMPORTANT: Always use FSplashScreen.ShowMessageBox instead of MessageBox.Show
             * as long as the Splash Screen is displayed to show the MessageBox on the correct
             * Thread and in front of the Splash Screen!!!
             */

            try
            {
                InitialiseClasses();
            }
            catch (Exception e)
            {
                FSplashScreen.Close();
                TLogging.Log(e.ToString());
                MessageBox.Show(e.Message);
                Shutdown.StopPetraClient();
            }

            if (!LoadClientSettings())
            {
                Environment.Exit(0);
            }

            /*
             *  Initialise Application Help
             */
            Ict.Common.HelpLauncher.LocalHTMLHelp = TClientSettings.LocalHTMLHelp;

            if (TClientSettings.LocalHTMLHelp)
            {
                Ict.Common.HelpLauncher.HelpHTMLBaseURL = TClientSettings.HTMLHelpBaseURLLocal;
            }
            else
            {
                Ict.Common.HelpLauncher.HelpHTMLBaseURL = TClientSettings.HTMLHelpBaseURLOnInternet;

                if (Ict.Common.HelpLauncher.HelpHTMLBaseURL.EndsWith("/"))
                {
                    Ict.Common.HelpLauncher.HelpHTMLBaseURL = Ict.Common.HelpLauncher.HelpHTMLBaseURL.Substring(0,
                        Ict.Common.HelpLauncher.HelpHTMLBaseURL.Length - 1);
                }
            }

            Ict.Common.HelpLauncher.DetermineHelpTopic += new Ict.Common.HelpLauncher.TDetermineHelpTopic(
                Ict.Petra.Client.App.Core.THelpContext.DetermineHelpTopic);

            /*
             * Specific information about this Petra installation can only be shown in the
             * Splash Screen after Client settings are loaded (done in LoadClientSettings).
             */
            FSplashScreen.UpdateTexts();

            // only do automatic patch installation on remote situation
            // needs to be done before login, because the login connects to the updated server, and could go wrong because of changed interfaces
            if (TClientSettings.RunAsRemote == true)
            {
                try
                {
                    CheckForPatches();
                }
                catch (Exception e)
                {
                    TLogging.Log("Problem during checking for patches: " + e.Message);
                    TLogging.Log(e.StackTrace);
                }
            }

            if (TClientSettings.RunAsStandalone == true)
            {
                FSplashScreen.ProgressText = "Starting OpenPetra Server Environment...";

                if (!StartServer())
                {
                    Environment.Exit(0);
                }
            }

            FSplashScreen.ProgressText = "Connecting to your OpenPetra.org Server...";

            /*
             * Show Petra Login screen.
             * Connections to PetraServer are established in here as well.
             */
            try
            {
                PerformLogin();
            }
            catch (Exception)
            {
#if TESTMODE
                // in Testmode, if no connection to applink, just stop here
                Environment.Exit(0);
#endif
#if  TESTMODE
#else
                throw;
#endif
            }

            if (FLoginSuccessful)
            {
                try
                {
                    // Set Application Help language to the User's preferred language
                    TRemote.MSysMan.Maintenance.WebConnectors.GetLanguageAndCulture(ref UsersLanguageCode, ref UsersCultureCode);

                    if (UsersLanguageCode != String.Empty)
                    {
                        Ict.Common.HelpLauncher.HelpLanguage = UsersLanguageCode;
                    }

                    if (TClientSettings.RunAsStandalone == true)
                    {
                        ProcessReminders.StartStandaloneRemindersProcessing();
                    }

                    DataTable CurrencyFormatTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.CurrencyCodeList);

                    StringHelper.CurrencyFormatTable = CurrencyFormatTable;

                    // This loads the Main Window of Petra
                    Form MainWindow;

                    MainWindow = new TFrmMainWindowNew(null);

                    // TODO: user defined constructor with more details
                    //                    FProcessID, FWelcomeMessage, FSystemEnabled);

                    Application.Run(MainWindow);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    TLogging.Log(e.Message);
                    TLogging.Log(e.StackTrace);
                }
                finally
                {
                    /*
                     * This code gets executed only after the Main Window of Petra has
                     * closed.
                     * At the moment, we will never get here, since we call Environment.Exit in the MainWindow (both old and new navigation)
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

#if TODO
// TODO: should this go to the server side? will we have Postgresql at all on the client?
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
                    FSplashScreen.ProgressText = "Starting PostgreSql Server...";
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
                        FSplashScreen.ShowMessageBox("failed to start " + PostgreSqlServerProcess.StartInfo.FileName);
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
                    FSplashScreen.ShowMessageBox("Exception while starting PostgreSql process: " + exp.ToString());
#endif
                    return false;
                }
                PostgreSqlServerProcess.WaitForExit(20000);

                return true;
            }

            return true;
        }
#endif



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
            TClientInfo.InitializeUnit();
            TCacheableTablesManager.InitializeUnit();
            new TIconCache();

            // Set up Delegates for forwarding of calls to Screens in various Assemblies
            TCommonScreensForwarding.OpenPartnerFindScreen = @TPartnerFindScreenManager.OpenModalForm;
            TCommonScreensForwarding.OpenPartnerFindByBankDetailsScreen = @TPartnerFindScreenManager.OpenModalForm;
            TCommonScreensForwarding.OpenBankFindDialog = @TBankFindDialogManager.OpenModalForm;
            TCommonScreensForwarding.OpenConferenceFindScreen = @TConferenceFindScreenManager.OpenModalForm;
            TCommonScreensForwarding.OpenEventFindScreen = @TEventFindScreenManager.OpenModalForm;
            TCommonScreensForwarding.OpenExtractFindScreen = @TExtractFindScreenManager.OpenModalForm;
            TCommonScreensForwarding.OpenExtractMasterScreen = @TExtractMasterScreenManager.OpenForm;
            TCommonScreensForwarding.OpenDonorRecipientHistoryScreen = @TDonorRecipientHistoryScreenManager.OpenForm;
            TCommonScreensForwarding.OpenExtractMasterScreenHidden = @TExtractMasterScreenManager.OpenFormHidden;
            TCommonScreensForwarding.OpenRangeFindScreen = @TPostcodeRangeSetupManager.OpenModalForm;
            TCommonScreensForwarding.OpenOccupationCodeFindScreen = @TOccupationCodeSetupManager.OpenModalForm;
            TCommonScreensForwarding.OpenGetMergeDataDialog = @TGetMergeDataManager.OpenModalForm;
            TCommonScreensForwarding.OpenPrintPartnerDialog = @TPrintPartnerModal.OpenModalForm;

            // Set up Delegate for the opening of Forms from the Main Menu
            Ict.Common.Controls.TLstTasks.OpenNewOrExistingForm = @Ict.Petra.Client.CommonForms.TFormsList.OpenNewOrExistingForm;

            // Set up Delegate for the retrieval of the list of Currencies from the Cache
            Ict.Common.Controls.TTxtCurrencyTextBox.RetrieveCurrencyList = @Ict.Petra.Client.CommonControls.TControlExtensions.RetrieveCurrencyList;

            // Set up Delegate for the set-up of various Colours of all SourceGrid DataGrid instances from UserDefaults
            Ict.Common.Controls.TSgrdDataGrid.SetColourInformation = @SetDataGridColoursFromUserDefaults;

            // Set up Delegate for the set-up of various Colours of all Filter and Find instances from UserDefaults
            Ict.Petra.Client.CommonControls.TUcoFilterAndFind.SetColourInformation = @SetFilterFindColoursFromUserDefaults;

            // Set up Data Validation Delegates
            TSharedValidationHelper.SharedGetDataDelegate = @TServerLookup.TMCommon.GetData;
            TSharedValidationControlHelper.SharedGetDateVerificationResultDelegate = @TtxtPetraDate.GetDateVerificationResult;
            TSharedPartnerValidationHelper.VerifyPartnerDelegate = @TServerLookup.TMPartner.VerifyPartner;
            TSharedFinanceValidationHelper.GetValidPostingDateRangeDelegate = @TServerLookup.TMFinance.GetCurrentPostingRangeDates;
            TSharedFinanceValidationHelper.GetValidPeriodDatesDelegate = @TServerLookup.TMFinance.GetCurrentPeriodDates;

            // Set up Delegates for retrieval of cacheable tables when called from Shared directories on client side
            TSharedDataCache.TMCommon.GetCacheableCommonTableDelegate = @TDataCache.TMCommon.GetCacheableCommonTable;

            TSharedDataCache.TMFinance.GetCacheableFinanceTableDelegate = @TDataCache.TMFinance.GetCacheableFinanceTable;

            TSharedDataCache.TMPartner.GetCacheablePartnerTableDelegate = @TDataCache.TMPartner.GetCacheablePartnerTable;
            TSharedDataCache.TMPartner.GetCacheableMailingTableDelegate = @TDataCache.TMPartner.GetCacheableMailingTable;
            TSharedDataCache.TMPartner.GetCacheableSubscriptionsTableDelegate = @TDataCache.TMPartner.GetCacheableSubscriptionsTable;

            TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate = @TDataCache.TMPersonnel.GetCacheablePersonnelTable;
            TSharedDataCache.TMPersonnel.GetCacheableUnitsTableDelegate = @TDataCache.TMPersonnel.GetCacheableUnitsTable;

            TSharedDataCache.TMConference.GetCacheableConferenceTableDelegate = @TDataCache.TMConference.GetCacheableConferenceTable;

            TSharedDataCache.TMSysMan.GetCacheableSysManTableDelegate = @TDataCache.TMSysMan.GetCacheableSysManTable;

            // I18N: assign proper font which helps to read asian characters
            // this is the first place where it is called, and we need to initialize the TAppSettingsManager
            TAppSettingsManager.InitFontI18N();

            TCommonControlsHelper.SetInactiveIdentifier += delegate {
                return SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS;
            };
        }

        /// <summary>
        /// Sets up various Colours of all SourceGrid DataGrid instances from UserDefaults.
        /// </summary>
        /// <returns>void</returns>
        private static TSgrdDataGrid.ColourInformation SetDataGridColoursFromUserDefaults()
        {
            string SelectionColourUserDefault;

            TSgrdDataGrid.ColourInformation ReturnValue = new TSgrdDataGrid.ColourInformation();

            // Note: The UserDefaults store the colours as HTML representations of colours. Example: "#FFFFFF" = System.Drawing.Color.White
            ReturnValue.BackColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_BACKGROUND,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.White)));
            ReturnValue.CellBackgroundColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_CELLBACKGROUND,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.White)));

            ReturnValue.AlternatingBackgroundColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_ALTERNATE,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(230, 230, 230))));
            ReturnValue.GridLinesColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_GRIDLINES,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(211, 211, 211))));

            // The UserDefault for the Selection colour stores a decimal Alpha value appended to the HTML representation of the colour
            // because the Selection needs to be transparent to a certain degree in order to let the data of a selected Grid Row shine through!
            // Example: "#00FFAA;50": A=140 (decimal 140), R=15 (hex 0F), G=255 (hex FF), B=170 (hex AA)
            SelectionColourUserDefault = TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_GRID_SELECTION, String.Empty);

            if (SelectionColourUserDefault.Length > 0)
            {
                ReturnValue.SelectionColour = System.Drawing.ColorTranslator.FromHtml(SelectionColourUserDefault.Split(';')[0]);
                ReturnValue.SelectionColour = System.Drawing.Color.FromArgb(Convert.ToInt32(SelectionColourUserDefault.Split(
                            ';')[1]), ReturnValue.SelectionColour);
            }
            else
            {
                // No UserDefault for the Selection in the DB; use a hard-coded default
                ReturnValue.SelectionColour =
                    System.Drawing.Color.FromArgb(120, System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Highlight));
            }

            return ReturnValue;
        }

        /// <summary>
        /// Sets up various Colours of all Filter and Find instances from UserDefaults.
        /// </summary>
        /// <returns>void</returns>
        private static TUcoFilterAndFind.ColourInformation SetFilterFindColoursFromUserDefaults()
        {
            TUcoFilterAndFind.ColourInformation ReturnValue = new TUcoFilterAndFind.ColourInformation();

            ReturnValue.FilterColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_FILTER_PANEL,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightBlue)));

            ReturnValue.FindColour = System.Drawing.ColorTranslator.FromHtml(
                TUserDefaults.GetStringDefault(TUserDefaults.NamedDefaults.COLOUR_FIND_PANEL,
                    System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.BurlyWood)));

            return ReturnValue;
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