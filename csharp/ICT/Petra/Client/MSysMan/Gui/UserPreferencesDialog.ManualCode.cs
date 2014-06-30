//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MSysMan.Gui
{
    public partial class TFrmUserPreferencesDialog
    {
        private Boolean FViewMode = false;
        private Ict.Petra.Client.MSysMan.Gui.TUC_FinancePreferences ucoFinance;
        private Boolean tabPageFinanceWasSelected = false;

        /// ViewMode is a special mode where the whole window with all tabs is in a readonly mode
        public bool ViewMode {
            get
            {
                return FViewMode;
            }
            set
            {
                FViewMode = value;
            }
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            // The general tab may present a dialog that allows the user to cancel - so we need to call this first
            if (ucoGeneral.SaveGeneralTab() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            if (ucoAppearance.SaveAppearanceTab() | (tpgFinance.Enabled && tabPageFinanceWasSelected && ucoFinance.SaveFinanceTab()))
            {
                Form MainWindow = FPetraUtilsObject.GetCallerForm();
                MethodInfo method = MainWindow.GetType().GetMethod("LoadNavigationUI");

                if (method != null)
                {
                    method.Invoke(MainWindow, new object[] { true });
                }

                method = MainWindow.GetType().GetMethod("SelectSettingsFolder");

                if (method != null)
                {
                    method.Invoke(MainWindow, null);
                }
            }

            ucoPartner.SavePartnerTab();

            Close();
        }

        private void InitializeManualCode()
        {
            this.AcceptButton = btnOK;

            tabPreferences.Selected += new TabControlEventHandler(tabPreferences_Selected);
        }

        private void GetDataFromControlsManual()
        {
            ucoFinance.GetDataFromControls();
        }

        private void RunOnceOnActivationManual()
        {
            ucoGeneral.Focus();
            HookupAllInContainer(ucoGeneral);

            if (UserInfo.GUserInfo.IsInModule("PTNRUSER"))
            {
                tpgPartner.Enabled = true;
            }

            if (UserInfo.GUserInfo.IsInModule("FINANCE-1"))
            {
                // only create this object when the user has access to finance (this code would normally be generated)
                ucoFinance = new Ict.Petra.Client.MSysMan.Gui.TUC_FinancePreferences();
                ucoFinance.Name = "ucoFinance";
                ucoFinance.Dock = System.Windows.Forms.DockStyle.Fill;
                tpgFinance.Controls.Add(this.ucoFinance);

                ucoFinance.PetraUtilsObject = FPetraUtilsObject;
                ucoFinance.InitUserControl();
                FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

                tpgFinance.Enabled = true;
            }
        }

        void tabPreferences_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tpgFinance)
            {
                tabPageFinanceWasSelected = true;
            }
        }
    }
}