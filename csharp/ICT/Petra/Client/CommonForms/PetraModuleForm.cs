//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2011 by OM International
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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using Ict.Petra.Shared;
using Ict.Common.Controls;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// utility class for the Petra Main screen and the Petra Module Main screens
    /// </summary>
    public class TFrmPetraModuleUtils : TFrmPetraUtils
    {
        private static List <System.Type>FModuleWindows = new List <System.Type>();

        /// <summary>
        /// because of dependency cycle, we cannot directly reference the module windows
        /// therefore they need to be added by the main program, and will be generated with reflection
        /// </summary>
        /// <param name="AWindowType"></param>
        public static void AddModuleWindow(System.Type AWindowType)
        {
            FModuleWindows.Add(AWindowType);
        }

        private System.Type GetWindowClass(string AWindowTypeName)
        {
            foreach (System.Type t in FModuleWindows)
            {
                if (t.ToString() == AWindowTypeName)
                {
                    return t;
                }
            }

            throw new Exception(String.Format("cannot find window type {0}", AWindowTypeName));
        }

        private void OpenOrFocusScreen(string AClassName)
        {
            if (!TFormsList.GFormsList.ShowForm(AClassName))
            {
                System.Type windowType = GetWindowClass(AClassName);
                Object[] args = new Object[1];
                args[0] = this.FWinForm.Handle;
                Form window = (Form)Activator.CreateInstance(windowType, args);
                window.Show();
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ACallerForm">the int handle of the form that has opened this window; needed for focusing when this window is closed later</param>
        /// <param name="ATheForm"></param>
        /// <param name="AStatusBar"></param>
        public TFrmPetraModuleUtils(Form ACallerForm, IFrmPetra ATheForm, TExtStatusBarHelp AStatusBar)
            : base(ACallerForm, ATheForm, AStatusBar)
        {
        }

        /// <summary>
        /// switch to main screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenMainScreen(System.Object sender, System.EventArgs e)
        {
            TFormsList.GFormsList.ShowForm("Ict.Petra.Client.App.PetraClient.TFrmMainWindow");
        }

        /// <summary>
        /// switch to the finance screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenFinanceModule(System.Object sender, System.EventArgs e)
        {
            if (!TFormsList.GFormsList.ShowForm("Ict.Petra.Client.MFinance.Gui.TFrmFinanceMain"))
            {
                OpenOrFocusScreen("Ict.Petra.Client.MFinance.Gui.TFrmFinanceMain");
            }
        }

        /// <summary>
        /// switch to the conference screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenConferenceModule(System.Object sender, System.EventArgs e)
        {
            // TODO OpenOrFocusScreen("Ict.Petra.Client.MConference.Gui.TFrmConferenceMain");
        }

        /// <summary>
        /// switch to the Financial Development screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenFinDevModule(System.Object sender, System.EventArgs e)
        {
            // TODO OpenOrFocusScreen("Ict.Petra.Client.MFinDev.Gui.TFrmFinDevMain");
        }

        /// <summary>
        /// switch to the partner screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenPartnerModule(System.Object sender, System.EventArgs e)
        {
            OpenOrFocusScreen("Ict.Petra.Client.MPartner.Gui.TFrmPartnerMain");
        }

        /// <summary>
        /// switch to the personnel screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenPersonnelModule(System.Object sender, System.EventArgs e)
        {
            OpenOrFocusScreen("Ict.Petra.Client.MPersonnel.Gui.TFrmPersonnelMain");
        }

        /// <summary>
        /// switch to the System Manager screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenSysManModule(System.Object sender, System.EventArgs e)
        {
            // TODO OpenOrFocusScreen("Ict.Petra.Client.MSysMan.Gui.TFrmSysManMain");
        }

        /// <summary>
        /// Checks the Module Access Permissions for the current user and disables or
        /// enables the 'Petra' Menue items and the Module Buttons according to the
        /// permissions.
        ///
        /// </summary>
        /// <returns>void</returns>
        public override void InitActionState()
        {
            EnableAction("actPartnerModule", UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_PTNRADMIN)
                || UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_PTNRUSER));

            EnableAction("actPersonnelModule", UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_PERSONNEL));

            EnableAction("actFinanceModule", UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1)
                || UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE2)
                || UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE3));

            EnableAction("actConferenceModule", UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_CONFERENCE));

            EnableAction("actFinDevModule", UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVUSER));

            EnableAction("actSysManModule", UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_SYSADMIN));
        }
    }
}