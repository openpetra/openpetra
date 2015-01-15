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
using System.Threading;
using System.Windows.Forms;

namespace SplashScreen
{
    /// <summary>todoComment</summary>
    public delegate void TSplashScreenCallback(out string APetraVersion, out string AInstallationKind, out string ACustomText);

    /// <summary>
    /// Class for handling the Splash Screen at application startup
    /// </summary>
    public class TSplashScreenManager
    {
        private volatile frmSplashScreen FSplashScreenForm = null;
        private TSplashScreenCallback FSplashScreenCallback;

        private void LaunchSplashScreen()
        {
            // deactivate for the moment, it crashes sometimes
            //FSplashScreenForm = new frmSplashScreen();
            //FSplashScreenForm.Show();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ACallback"></param>
        public TSplashScreenManager(TSplashScreenCallback ACallback)
        {
            FSplashScreenCallback = ACallback;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void Show()
        {
            // Show Splash Screen - in a new Thread, so that it can be painted properly
            Thread SplashThread = new Thread(new ThreadStart(LaunchSplashScreen));

            SplashThread.SetApartmentState(ApartmentState.STA);
            SplashThread.Start();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void Close()
        {
            if (FSplashScreenForm != null)
            {
                // Calling Form.Close would cause exception in DebugMode: System.InvalidOperationException
                // you cannot call Form.Close across threads (it was created in the other thread?)
                FSplashScreenForm.PleaseClose();
                FSplashScreenForm = null;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void UpdateTexts()
        {
            if (FSplashScreenForm == null)
            {
                return;
            }

            string PetraVersion, InstallationKind, CustomText;

            if (FSplashScreenCallback != null)
            {
                FSplashScreenCallback(out PetraVersion, out InstallationKind, out CustomText);
                FSplashScreenForm.SetTexts(PetraVersion, InstallationKind, CustomText);
            }
        }

        /// <summary>todoComment</summary>
        public string ProgressText
        {
            get
            {
                if (FSplashScreenForm == null)
                {
                    return "";
                }

                return FSplashScreenForm.ProgressText;
            }

            set
            {
                if (FSplashScreenForm != null)
                {
                    FSplashScreenForm.ProgressText = value;
                }
            }
        }

        /// <summary>todoComment</summary>
        public bool Visible
        {
            get
            {
                if (FSplashScreenForm == null)
                {
                    return false;
                }

                return FSplashScreenForm.Visible;
            }

            set
            {
                if (!value && (FSplashScreenForm != null))
                {
                    FSplashScreenForm.Visible = false;
                }
                else
                {
                    Show();
                }
            }
        }


        #region MessageBox Helper

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AMessage"></param>
        public void ShowMessageBox(string AMessage)
        {
            if (FSplashScreenForm != null)
            {
                FSplashScreenForm.ShowMessageBox(AMessage);
            }
            else
            {
                MessageBox.Show(AMessage);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AMessage"></param>
        /// <param name="ATitle"></param>
        public void ShowMessageBox(string AMessage, string ATitle)
        {
            if (FSplashScreenForm != null)
            {
                FSplashScreenForm.ShowMessageBox(AMessage, ATitle);
            }
            else
            {
                MessageBox.Show(AMessage, ATitle);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AMessage"></param>
        /// <param name="ATitle"></param>
        /// <param name="AMessageBoxIcon"></param>
        public void ShowMessageBox(string AMessage, string ATitle, MessageBoxIcon AMessageBoxIcon)
        {
            if (FSplashScreenForm != null)
            {
                FSplashScreenForm.ShowMessageBox(AMessage, ATitle, AMessageBoxIcon);
            }
            else
            {
                MessageBox.Show(AMessage, ATitle, MessageBoxButtons.OK, AMessageBoxIcon);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AMessage"></param>
        /// <param name="ATitle"></param>
        /// <param name="AMessageBoxButtons"></param>
        /// <returns></returns>
        public DialogResult ShowMessageBox(string AMessage, string ATitle, MessageBoxButtons AMessageBoxButtons)
        {
            if (FSplashScreenForm != null)
            {
                FSplashScreenForm.ShowMessageBox(AMessage, ATitle, AMessageBoxButtons);
                return FSplashScreenForm.MessageBoxDialogResult;
            }
            else
            {
                return MessageBox.Show(AMessage, ATitle, AMessageBoxButtons);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AMessage"></param>
        /// <param name="ATitle"></param>
        /// <param name="AMessageBoxButtons"></param>
        /// <param name="AMessageBoxIcon"></param>
        /// <returns></returns>
        public DialogResult ShowMessageBox(string AMessage, string ATitle,
            MessageBoxButtons AMessageBoxButtons, MessageBoxIcon AMessageBoxIcon)
        {
            if (FSplashScreenForm != null)
            {
                FSplashScreenForm.ShowMessageBox(AMessage, ATitle, AMessageBoxButtons, AMessageBoxIcon);
                return FSplashScreenForm.MessageBoxDialogResult;
            }
            else
            {
                return MessageBox.Show(AMessage, ATitle, AMessageBoxButtons, AMessageBoxIcon);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AMessage"></param>
        /// <param name="ATitle"></param>
        /// <param name="AMessageBoxButtons"></param>
        /// <param name="AMessageBoxIcon"></param>
        /// <param name="AMessageBoxDefaultButton"></param>
        /// <returns></returns>
        public DialogResult ShowMessageBox(string AMessage, string ATitle,
            MessageBoxButtons AMessageBoxButtons, MessageBoxIcon AMessageBoxIcon,
            MessageBoxDefaultButton AMessageBoxDefaultButton)
        {
            if (FSplashScreenForm != null)
            {
                FSplashScreenForm.ShowMessageBox(AMessage, ATitle, AMessageBoxButtons, AMessageBoxIcon, AMessageBoxDefaultButton);
                return FSplashScreenForm.MessageBoxDialogResult;
            }
            else
            {
                return MessageBox.Show(AMessage, ATitle, AMessageBoxButtons, AMessageBoxIcon, AMessageBoxDefaultButton);
            }
        }

        #endregion
    }
}