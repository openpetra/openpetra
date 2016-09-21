//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Executes a certain Task.
    /// </summary>
    public class TClientTaskInstance : TClientTaskInstanceBase
    {
        [DllImport("user32.dll")]
        private static extern int FindWindow(string classname, string windowname);
        [DllImport("user32.dll")]
        private static extern int PostMessage(
            int hWnd,                    // handle to destination window
            uint Msg,                    // message
            long wParam,                 // first message parameter
            long lParam                  // second message parameter
            );

        private const string MODALDELEGATETYPE_DBCONNBROKEN = "DBConnectionBroken";

        /// <summary></summary>
        public delegate void FastReportsPrintReportNoUi(String ReportName, String ParamStr);
        /// <summary></summary>
        public static FastReportsPrintReportNoUi FastReportsPrintReportNoUiDelegate;

        /// <summary></summary>
        public delegate int TShowExtendedMessageBox(Form AParentForm,
            String AMessage,
            String ACaption,
            String AChkOptionText,
            int AButtons,
            int ADefaultButton,
            int AIcon,
            bool AEntryOptionSelected,
            out bool AExitOptionSelected);

        /// <summary></summary>
        public static TShowExtendedMessageBox ShowExtendedMessageBox;

        /// <summary>
        /// Tells the caller whether a given Client Task should be launched on a separate Thread, or not.
        /// </summary>
        /// <remarks>If that Method returns false for a given Client Task, the processing of any other Client Tasks
        /// is suspended until the Client Task that is not to run on its own Thread finishes (this is by design
        /// and wanted behaviour)!</remarks>
        /// <returns>True if the given Client Task should be launched un a separate Thread, false if not.</returns>
        public override bool LaunchOnItsOwnThread()
        {
            string TaskGroup = FClientTaskDataRow["TaskGroup"].ToString();

            switch (TaskGroup)
            {
                case SharedConstants.CLIENTTASKGROUP_USERMESSAGE:
                    bool ModalRequested;
                    bool BlockingRequested;
                    string MessageBoxTitle;

                    ParseMessageBoxOptions(out MessageBoxTitle, out ModalRequested, out BlockingRequested);

                    if (BlockingRequested)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                case SharedConstants.CLIENTTASKGROUP_REPORT:
                    return false;

                default:
                    return true;
            }
        }

        /// <summary>
        /// Executes the Client Task.
        /// </summary>
        public override void Execute()
        {
            try
            {
                switch (FClientTaskDataRow["TaskGroup"].ToString())
                {
                    // MessageBox.Show('Executing Client Task #' + FClientTaskDataRow['TaskID'].ToString + ' in Thread.');
                    case SharedConstants.CLIENTTASKGROUP_USERMESSAGE:
                    {
                        //MessageBox.Show(SharedConstants.CLIENTTASKGROUP_USERMESSAGE + " (Client Task #" +
                        //    FClientTaskDataRow["TaskID"].ToString() + "): " + FClientTaskDataRow["TaskCode"].ToString(),
                        //    "Client #" + UserInfo.GUserInfo.ProcessID.ToString() + " received a ClientTask.");

                        ShowMessageBoxOnTopOfAllFormsAutoParsed();

                        break;
                    }

                    case SharedConstants.CLIENTTASKGROUP_CACHEREFRESH:
                    {
                        if (FClientTaskDataRow["TaskParameter1"].ToString() == "")
                        {
                            TDataCache.ReloadCacheTable(FClientTaskDataRow["TaskCode"].ToString());
                        }
                        else
                        {
                            TDataCache.ReloadCacheTable(FClientTaskDataRow["TaskCode"].ToString(), FClientTaskDataRow["TaskParameter1"]);
                        }

                        break;
                    }

                    case SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH:
                    {
                        if (FClientTaskDataRow["TaskCode"].ToString() == "All")
                        {
//                            TLogging.Log("FClientTaskDataRow[TaskCode] = All!");
                            TUserDefaults.ReloadCachedUserDefaults();
                            TUserDefaults.SaveChangedUserDefaults();
                        }
                        else
                        {
//                            TLogging.Log("FClientTaskDataRow[TaskCode] <> All, but '" + FClientTaskDataRow["TaskCode"].ToString() + "'" +
//                                "FClientTaskDataRow[TaskParameter1]: '" + FClientTaskDataRow["TaskParameter1"].ToString() + "'\r\n" +
//                                "FClientTaskDataRow[TaskParameter2]: '" + FClientTaskDataRow["TaskParameter2"].ToString() + "'\r\n" +
//                                "FClientTaskDataRow[TaskParameter3]: '" + FClientTaskDataRow["TaskParameter3"].ToString() + "'" +
//                                ((FClientTaskDataRow["TaskParameter4"] != System.DBNull.Value) ? "\r\nFClientTaskDataRow[TaskParameter4]: '" +
//                                 FClientTaskDataRow["TaskParameter4"].ToString() + "'" : "\r\nFClientTaskDataRow[TaskParameter4]: DBNull"));
                            TUserDefaults.RefreshCachedUserDefault(
                                FClientTaskDataRow["TaskParameter1"].ToString(), FClientTaskDataRow["TaskParameter2"].ToString(),
                                FClientTaskDataRow["TaskParameter3"].ToString(),
                                ((FClientTaskDataRow["TaskParameter4"] != System.DBNull.Value) ?
                                 Convert.ToInt32(FClientTaskDataRow["TaskParameter4"]) : -1));
                        }

                        break;
                    }

                    case SharedConstants.CLIENTTASKGROUP_USERINFOREFRESH:
                    {
                        TUserInfo.ReloadCachedUserInfo();
                        break;
                    }

                    case RemotingConstants.CLIENTTASKGROUP_DISCONNECT:
                    {
                        if (FClientTaskDataRow["TaskCode"].ToString() == "IMMEDIATE")
                        {
                            TLogging.Log("Client disconnected due to server disconnection request.");

                            PetraClientShutdown.Shutdown.SaveUserDefaultsAndDisconnectAndStop();
                        }

                        if (FClientTaskDataRow["TaskCode"].ToString() == "IMMEDIATE-HARDEXIT")
                        {
                            TLogging.Log(
                                "Application stopped due to server disconnection request (without saving of User Defaults or disconnection).");

                            // APPLICATION STOPS HERE !!!
                            Environment.Exit(0);
                        }

                        break;
                    }

                    case SharedConstants.CLIENTTASKGROUP_REPORT:
                    {
                        FastReportsPrintReportNoUiDelegate(FClientTaskDataRow["TaskCode"].ToString(), FClientTaskDataRow["TaskParameter1"].ToString());
                        break;
                    }

                    case SharedConstants.CLIENTTASKGROUP_DBCONNECTIONBROKEN:
                    {
                        //MessageBox.Show(SharedConstants.CLIENTTASKGROUP_DBCONNECTIONBROKEN + " (Client Task #" +
                        //    FClientTaskDataRow["TaskID"].ToString() + "): " + FClientTaskDataRow["TaskCode"].ToString(),
                        //    "Client #" + UserInfo.GUserInfo.ProcessID.ToString() + " received a ClientTask.");

                        ShowMessageBoxOnTopOfAllFormsDBConnectionBroken();

                        break;
                    }
                }
            }
            catch (Exception Exp)
            {
//              MessageBox.Show("Exception occured in TClientTaskInstance.Execute: \r\n" + Exp.ToString());
                TLogging.Log("Exception occured in TClientTaskInstance.Execute: \r\n" + Exp.ToString());
            }
        }

        private delegate void TMyShowMessageBoxDelegate();

        private void ParseMessageBoxOptions(out string AMessageBoxTitle, out bool AModalRequested, out bool ABlockingRequested)
        {
            AModalRequested = false;
            ABlockingRequested = false;
            AMessageBoxTitle = Catalog.GetString("OpenPetra Message");

            AModalRequested = FClientTaskDataRow["TaskParameter1"].ToString().ToUpper() == "MODAL";

            if (AModalRequested)
            {
                ABlockingRequested = FClientTaskDataRow["TaskParameter2"].ToString().ToUpper() == "BLOCKING";
            }

            if (FClientTaskDataRow["TaskParameter3"].ToString() != String.Empty)
            {
                AMessageBoxTitle = FClientTaskDataRow["TaskParameter3"].ToString();
            }
        }

        /// <summary>
        /// Gets round the issue that a MessageBox that gets shown from a Thread does not appear in front of all
        /// open Forms and isn't modal.
        /// </summary>
        private void ShowMessageBoxOnTopOfAllFormsAutoParsed()
        {
            string MessageBoxTitle;
            bool ModalRequested;
            bool BlockingRequested;

            ParseMessageBoxOptions(out MessageBoxTitle, out ModalRequested, out BlockingRequested);

            ShowMessageBoxOnTopOfAllForms(FClientTaskDataRow["TaskCode"].ToString(), MessageBoxTitle, ModalRequested);
        }

        private void ShowMessageBoxOnTopOfAllFormsDBConnectionBroken()
        {
            if (FClientTaskDataRow["TaskCode"].ToString() == "BROKEN")
            {
                ShowMessageBoxOnTopOfAllForms(TExceptionHelper.StrDBConnectionBroken +
                    TExceptionHelper.StrDBConnectionBrokenRemedyWhenLoggedIn1 +
                    TExceptionHelper.StrDBConnectionBrokenContactITSupport +
                    Environment.NewLine + Environment.NewLine + Catalog.GetString(String.Format(
                            TExceptionHelper.StrDBConnectionIssueDateTimeFooter, DateTime.Now)),
                    TExceptionHelper.StrDBConnectionBrokenTitle, true, MessageBoxIcon.Error,
                    MODALDELEGATETYPE_DBCONNBROKEN);
            }
            else
            {
                // Attempt to close the previously displayed message that was about the fact that the DB connection has broken before
                // displaying the new message that the DB connection has been restored in order to reduce confusion.
                int wHandle = FindWindow(null, TExceptionHelper.StrDBConnectionBrokenTitle);

                if (wHandle > 0)
                {
                    // 0x10 is the WM_CLOSE message
                    PostMessage(wHandle, 0x10, 0, 0);
                }

                ShowMessageBoxOnTopOfAllForms(
                    TExceptionHelper.StrDBConnectionBrokenRestored + TExceptionHelper.StrDBConnectionBrokenAdvice +
                    TExceptionHelper.StrDBConnectionBrokenRemedyWhenLoggedIn3 +
                    TExceptionHelper.StrDBConnectionBrokenRemedyWhenLoggedInFollowUpAction2 +
                    Environment.NewLine + Catalog.GetString(String.Format(
                            TExceptionHelper.StrDBConnectionIssueDateTimeFooter, DateTime.Now)),
                    TExceptionHelper.StrDBConnectionBrokenRecoveredTitlePrefix +
                    TExceptionHelper.StrDBConnectionBrokenTitle, true, MessageBoxIcon.Information,
                    MODALDELEGATETYPE_DBCONNBROKEN);
            }
        }

        /// <summary>
        /// Gets round the issue that a MessageBox that gets shown from a Thread does not appear in front of all
        /// open Forms and isn't modal.
        /// </summary>
        private void ShowMessageBoxOnTopOfAllForms(string AMessageBoxText = "", string AMessageBoxTitle = "",
            bool AModalRequested = false, MessageBoxIcon AMessageBoxIcon = MessageBoxIcon.None, string AModalDelegateType = "")
        {
            if (AModalRequested)
            {
                // We are requested to show the MessageBox modally and in front of all other Forms - i.e.
                // get the 'normal' MessageBox behaviour.

                // Would normally use the code below but cannot due to circular referencing.
                // Form MainMenuForm = TFormsList.GFormsList.MainMenuForm;
                Form MainMenuForm = Application.OpenForms[0];  // This gets the first ever opened Form, which is the Main Menu.

                // Since this procedure is called from a separate (background) Thread, it is
                // necessary to execute this procedure in the Thread of the GUI
                if (MainMenuForm.InvokeRequired)
                {
                    if (AModalDelegateType == String.Empty)
                    {
                        MainMenuForm.Invoke(new TMyShowMessageBoxDelegate(ShowMessageBoxOnTopOfAllFormsAutoParsed));
                    }
                    else if (AModalDelegateType == MODALDELEGATETYPE_DBCONNBROKEN)
                    {
                        MainMenuForm.Invoke(new TMyShowMessageBoxDelegate(ShowMessageBoxOnTopOfAllFormsDBConnectionBroken));
                    }
                    else
                    {
                        throw new ArgumentException(String.Format(
                                "Argument 'AModalDelegateType' got passed with value '{0}' but this isnt' supported", AModalDelegateType),
                            "AModalDelegateType");
                    }
                }
                else
                {
                    TLogging.Log(AMessageBoxText);

                    MessageBox.Show(AMessageBoxText, AMessageBoxTitle, MessageBoxButtons.OK, AMessageBoxIcon);
                }
            }
            else
            {
                TLogging.Log(AMessageBoxText);

                // We are requested to show the MessageBox non-modally, but still in front of all other Forms!
                MessageBox.Show(AMessageBoxText, AMessageBoxTitle,
                    MessageBoxButtons.OK, AMessageBoxIcon, MessageBoxDefaultButton.Button1,
                    (MessageBoxOptions)0x40000); // this is the 'MB_TOPMOST' Flag...
            }
        }
    }
}
