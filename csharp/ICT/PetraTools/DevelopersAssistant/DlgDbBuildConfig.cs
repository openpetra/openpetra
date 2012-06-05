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
using System.IO;

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
            string dbms, dbName, port, password, location, version;
            bool isBlank;

            dbCfg.GetStoredConfiguration(Index, out dbms, out dbName, out port, out password, out isBlank, out location, out version);

            cboDBMS.SelectedIndex = BuildConfiguration.GetDBMSIndex(dbms);
            txtDBName.Text = dbName;
            txtPort.Text = port;
            txtPassword.Text = password;
            chkBlankPW.Checked = isBlank;
            txtLocation.Text = location;

            if (String.Compare(dbms, "postgresql", true) == 0)
            {
                SetPostgreSQLVersionIndex(version);
            }
            else
            {
                cboVersion.SelectedIndex = 0;
            }

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
            string version = cboVersion.Items[cboVersion.SelectedIndex].ToString();

            if (version == BuildConfiguration.DefaultString)
            {
                version = String.Empty;
            }

            if (version == "9.0")
            {
                version = String.Empty;                         // applies to PostgreSQL
            }

            string location = txtLocation.Text;
            location = location.Replace('\\', '/');

            ExitData = BuildConfiguration.MakeConfigString(
                (cboDBMS.SelectedIndex ==
                 0) ? String.Empty : BuildConfiguration.Systems[BuildConfiguration.GetDBMSIndex(cboDBMS.Items[cboDBMS.SelectedIndex].ToString())],
                txtDBName.Text,
                txtPort.Text,
                txtPassword.Text,
                chkBlankPW.Checked,
                location,
                version);
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

        private void cboDBMS_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Called in initialisation (index changes from -1 to 0) and whenever the user selects a different DBMS
            // We always have the DefaultString as the first item
            cboVersion.Items.Clear();
            cboVersion.Items.Add(BuildConfiguration.DefaultString);

            // Now handle the case where the DBMS is PostgreSQL
            if (cboDBMS.Items[cboDBMS.SelectedIndex].ToString() == "PostgreSQL")
            {
                cboVersion.Items.Add("9.1");

                // Try and build in some future-proofing by checking Program Files for later versions
                string searchPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "PostgreSQL");
                searchPath = searchPath.Replace(" (x86)", String.Empty);

                if (Directory.Exists(searchPath))
                {
                    string[] versions = Directory.GetDirectories(searchPath, "*.*", SearchOption.TopDirectoryOnly);

                    for (int i = 0; i < versions.Length; i++)
                    {
                        string version = versions[i].Substring(searchPath.Length + 1);

                        if (version == "9.0")
                        {
                            continue;
                        }

                        if (version == "9.1")
                        {
                            continue;
                        }

                        cboVersion.Items.Add(version);
                    }
                }
            }

            cboVersion.SelectedIndex = 0;
        }

        private void SetPostgreSQLVersionIndex(string ForVersion)
        {
            // Sets the selected index for a specified version string
            // Start by assuming it is the first one (which should be the DefaultString)
            cboVersion.SelectedIndex = 0;

            if (String.Compare(ForVersion, "9.0") == 0)
            {
                return;
            }

            if (ForVersion == String.Empty)
            {
                return;
            }

            for (int i = 0; i < cboVersion.Items.Count; i++)
            {
                if (String.Compare(ForVersion, cboVersion.Items[i].ToString(), true) == 0)
                {
                    cboVersion.SelectedIndex = i;
                    return;
                }
            }

            // Handle a case where the config file has some random version we do not expect!
            cboVersion.Items.Add(ForVersion);
            cboVersion.SelectedIndex = cboVersion.Items.Count - 1;
        }
    }
}