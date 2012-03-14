//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ict.Tools.DevelopersAssistant
{
    /// <summary>
    /// Dialog that displays the fields for editing the database build config file
    /// </summary>
    public partial class DlgDbBuildConfig : Form
    {
        /// <summary>
        /// The exit data specified when the user clicked the OK button
        /// </summary>
        public string ExitData = String.Empty;

        /// <summary>
        /// Constructor for the class
        /// </summary>
        public DlgDbBuildConfig()
        {
            InitializeComponent();

            for (int i = 0; i < BuildConfiguration.Systems.Length; i++)
            {
                cboDBMS.Items.Add(BuildConfiguration.Systems[i]);
            }

            cboDBMS.SelectedIndex = 0;
        }

        /// <summary>
        /// Call this before ShowDialog to initialise the dialog with entry values to be edited
        /// </summary>
        /// <param name="BranchLocation">The path to the active branch</param>
        /// <param name="Index">The index of the favourite to be edited</param>
        /// <param name="LocalSettings">A reference to the local settings object used to persist personal preferences</param>
        public void InitializeDialog(string BranchLocation, int Index, SettingsDictionary LocalSettings)
        {
            BuildConfiguration dbCfg = new BuildConfiguration(BranchLocation, LocalSettings);
            string dbms, dbName, port, password, location;
            bool isBlank;

            dbCfg.GetStoredConfiguration(Index, out dbms, out dbName, out port, out password, out isBlank, out location);

            cboDBMS.SelectedIndex = BuildConfiguration.GetDBMSIndex(dbms);
            txtDBName.Text = dbName;
            txtPort.Text = port;
            txtPassword.Text = password;
            chkBlankPW.Checked = isBlank;
            txtLocation.Text = location;

            SetEnabledStates();
        }

        /// <summary>
        /// Call this to set the enabled states of the checkbox and textbox
        /// </summary>
        public void SetEnabledStates()
        {
            txtPassword_TextChanged(null, null);
            chkBlankPW_Click(null, null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string s = txtLocation.Text;

            s = s.Replace('\\', '/');
            ExitData = BuildConfiguration.MakeConfigString(
                (cboDBMS.SelectedIndex == 0) ? String.Empty : cboDBMS.Items[cboDBMS.SelectedIndex].ToString(),
                txtDBName.Text,
                txtPort.Text,
                txtPassword.Text,
                chkBlankPW.Checked,
                s);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            chkBlankPW.Enabled = txtPassword.Text == String.Empty;

            if (!chkBlankPW.Enabled)
            {
                chkBlankPW.Checked = false;
            }
        }

        private void chkBlankPW_Click(object sender, EventArgs e)
        {
            txtPassword.Enabled = (!chkBlankPW.Checked);
        }
    }
}