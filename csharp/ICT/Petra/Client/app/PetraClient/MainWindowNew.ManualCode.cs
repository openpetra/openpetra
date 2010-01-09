/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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
using System.Xml;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.App.PetraClient
{
    public partial class TFrmMainWindowNew
    {
        private void InitializeManualCode()
        {
            LoadNavigationUI();

            // leave out 'Revision' and 'Build'
            this.Text = "OpenPetra.org " +
                        System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString(3);
        }

        /// <summary>
        /// checks if the user has access to the navigation node
        /// </summary>
        private bool HasAccessPermission(XmlNode ANode, string AUserId)
        {
            // TODO: if this node belongs to a ledger, check if the user has access permission
            // TODO: if this is an action node, eg. opens a screen, check the static function that tells RequiredPermissions of the screen
            return true;
        }

        private void LoadNavigationUI()
        {
            TAppSettingsManager opts = new TAppSettingsManager();
            TYml2Xml parser = new TYml2Xml(opts.GetValue("UINavigation.File"));
            XmlDocument UINavigation = parser.ParseYML2XML();

            XmlNode OpenPetraNode = UINavigation.FirstChild.NextSibling.FirstChild;
            XmlNode SearchBoxesNode = OpenPetraNode.FirstChild;
            XmlNode MainMenuNode = SearchBoxesNode.NextSibling;
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            while (DepartmentNode != null)
            {
                lstFolders.AddFolder(DepartmentNode, UserInfo.GUserInfo.UserID, HasAccessPermission);

                DepartmentNode = DepartmentNode.NextSibling;
            }

            lstFolders.Dashboard = this.dsbContent;
            lstFolders.Statusbar = this.stbMain;
            lstFolders.SelectFolder(0);
        }

        private void ExitManualCode()
        {
            // make sure the application exits; also important for alternate navigation style main window
            this.Hide();
            Environment.Exit(0);
        }

        /// the main navigation form
        public static Form MainForm = null;

        private void SwitchToClassicNavigation(object sender, EventArgs e)
        {
            MainForm = this;

            if (TFrmMainWindow.MainForm == null)
            {
                TFrmMainWindow.MainForm = new TFrmMainWindow(IntPtr.Zero);
            }

            this.Hide();
            TFrmMainWindow.MainForm.Show();
        }
    }
}