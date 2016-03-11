//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ict.Tools.DevelopersAssistant
{
    /// <summary>
    /// Dialog that is shown on creation of a new branch that contains options for setting up the code
    /// </summary>
    public partial class DlgNewBranchActions : Form
    {
        private SettingsDictionary _localSettings = null;
        private MainForm _mainForm = null;

        private bool _restoreStartClientCheck = true;
        private bool _restoreOpenIDECheck = true;

        private bool _wasExistingBranch = false;

        /// <summary>
        /// Gets a value that indicates if the branch already had a generated solution when the dialog was created.
        /// </summary>
        public bool WasExistingBranch
        {
            get
            {
                return _wasExistingBranch;
            }
        }

        #region Public methods

        /// <summary>
        /// Constructor
        /// </summary>
        public DlgNewBranchActions(MainForm ParentForm)
        {
            InitializeComponent();
            _mainForm = ParentForm;
        }

        /// <summary>
        /// Initialise the dialog
        /// </summary>
        /// <param name="BranchLocation">Current branch location</param>
        /// <param name="LocalSettings">A reference to the local settings object</param>
        /// <param name="PreferredIDE">The user's preferred IDE (obtained from the registry)</param>
        public void Initialise(string BranchLocation, SettingsDictionary LocalSettings, TDevEnvironment PreferredIDE)
        {
            // Step 1:  Is this an existing branch?
            bool gotSolutions = false;
            bool gotExes = false;

            string searchPath = Path.Combine(BranchLocation, "delivery", "projects", "vs2010");

            if (Directory.Exists(searchPath))
            {
                string[] solutionFiles = Directory.GetFiles(searchPath, "*.sln");
                gotSolutions = solutionFiles.Length >= 5;
            }

            searchPath = Path.Combine(BranchLocation, "delivery", "bin");

            if (Directory.Exists(searchPath))
            {
                string[] exeFiles = Directory.GetFiles(searchPath, "Petra*.exe");
                gotExes = exeFiles.Length >= 4;
            }

            _wasExistingBranch = gotSolutions && gotExes;

            if (_wasExistingBranch)
            {
                this.Text = this.Text.Replace(" a New ", " an Existing ");
            }

            // Step 2: initialise the control settings based on our saved settings from last time.
            // There are separate settings for New and Existing
            chkGenerateSolution.Checked = _wasExistingBranch ? LocalSettings.EBA_GenerateSolutionOption >
                                          0 : LocalSettings.NBA_GenerateSolutionOption > 0;
            optFullCompile.Checked = _wasExistingBranch ? LocalSettings.EBA_GenerateSolutionOption <= 1 : LocalSettings.NBA_GenerateSolutionOption <=
                                     1;
            optMinimalCompile.Checked = _wasExistingBranch ? LocalSettings.EBA_GenerateSolutionOption >=
                                        2 : LocalSettings.NBA_GenerateSolutionOption >= 2;
            chkCreateConfigFiles.Checked = _wasExistingBranch ? LocalSettings.EBA_CreateMyConfigurations : LocalSettings.NBA_CreateMyConfigurations;
            chkInitDatabase.Checked = _wasExistingBranch ? LocalSettings.EBA_InitialiseDatabase : LocalSettings.NBA_InitialiseDatabase;
            chkOpenIDE.Checked = _wasExistingBranch ? LocalSettings.EBA_LaunchIDE : LocalSettings.NBA_LaunchIDE;
            chkStartServerAndClient.Checked = _wasExistingBranch ? LocalSettings.EBA_StartClient : LocalSettings.NBA_StartClient;

            chkOpenIDE.Text = "Open the solution in the " + (PreferredIDE == TDevEnvironment.VisualStudio ? "Visual Studio IDE" : "SharpDevelop IDE");

            // Step 3. Initialise the database combo box
            BuildConfiguration DbCfg = new BuildConfiguration(string.Empty, LocalSettings);

            for (int i = 0;; i++)
            {
                string dbms;
                string dbName;
                string port;
                string location;
                string unused1;
                bool unused2;
                string unused3;

                if (DbCfg.GetStoredConfiguration(i, out dbms, out dbName, out port, out unused1, out unused2, out location, out unused3))
                {
                    string portString = port == string.Empty ? port : " on port " + port;
                    string s = string.Format("{0}: {1}{2} [{3}]", dbms, dbName, portString, location == string.Empty ? "local" : location);
                    cboInitialDatabase.Items.Add(s);
                }
                else
                {
                    break;
                }
            }

            int dbIndex = _wasExistingBranch ? LocalSettings.EBA_DatabaseConfiguration : LocalSettings.NBA_DatabaseConfiguration;

            if ((cboInitialDatabase.Items.Count > 0) && (dbIndex < cboInitialDatabase.Items.Count))
            {
                cboInitialDatabase.SelectedIndex = dbIndex;
            }

            // Step 4.  Initialise the solutions combo box
            for (TSolution sln = TSolution.Full; sln <= TSolution.Testing; sln++)
            {
                cboSolution.Items.Add(sln);
            }

            int slnIndex = _wasExistingBranch ? LocalSettings.EBA_IDESolution : LocalSettings.NBA_IDESolution;

            if ((slnIndex >= (int)TSolution.Full) && (slnIndex <= (int)TSolution.Testing))
            {
                cboSolution.SelectedIndex = slnIndex;
            }

            lblStatus.Visible = false;
            progressBar.Visible = false;

            // Finally, keep a reference to the settings so we can save them on exit
            _localSettings = LocalSettings;
        }

        /// <summary>
        /// Call this to set the progress bar value and the status text
        /// </summary>
        /// <param name="ProgressValue">Value to set</param>
        /// <param name="MaxProgressValue">Maximum value</param>
        /// <param name="Status">Status text</param>
        public void SetProgressAndStatus(int ProgressValue, int MaxProgressValue, string Status)
        {
            lblStatus.Visible = true;
            progressBar.Visible = true;

            // Set the label text
            lblStatus.Text = Status;

            // Set the max progress value on the first call
            if (progressBar.Maximum != MaxProgressValue)
            {
                progressBar.Maximum = MaxProgressValue;
            }

            // Ensure that the value is not greater than the max
            int v = Math.Min(ProgressValue, MaxProgressValue);

            // This kludge is required to overcome Windows 'smoothing' by setting the value to less than the current value
            if (v < MaxProgressValue)
            {
                progressBar.Value = v + 1;
            }

            // So now set the value DOWN to what we want
            progressBar.Value = v;

            lblStatus.Refresh();
            progressBar.Refresh();
            Application.DoEvents();
            System.Threading.Thread.Sleep(2000);
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// Gets the selected settings
        /// </summary>
        private void GetSettings()
        {
            if (_wasExistingBranch)
            {
                if (chkGenerateSolution.Checked)
                {
                    _localSettings.EBA_GenerateSolutionOption = optFullCompile.Checked ? 1 : 2;
                }
                else
                {
                    _localSettings.EBA_GenerateSolutionOption = 0;
                }

                _localSettings.EBA_CreateMyConfigurations = chkCreateConfigFiles.Checked;
                _localSettings.EBA_InitialiseDatabase = chkInitDatabase.Checked;
                _localSettings.EBA_DatabaseConfiguration = cboInitialDatabase.SelectedIndex;
                _localSettings.EBA_LaunchIDE = chkOpenIDE.Checked;
                _localSettings.EBA_IDESolution = cboSolution.SelectedIndex;
                _localSettings.EBA_StartClient = chkStartServerAndClient.Checked;
            }
            else
            {
                if (chkGenerateSolution.Checked)
                {
                    _localSettings.NBA_GenerateSolutionOption = optFullCompile.Checked ? 1 : 2;
                }
                else
                {
                    _localSettings.NBA_GenerateSolutionOption = 0;
                }

                _localSettings.NBA_CreateMyConfigurations = chkCreateConfigFiles.Checked;
                _localSettings.NBA_InitialiseDatabase = chkInitDatabase.Checked;
                _localSettings.NBA_DatabaseConfiguration = cboInitialDatabase.SelectedIndex;
                _localSettings.NBA_LaunchIDE = chkOpenIDE.Checked;
                _localSettings.NBA_IDESolution = cboSolution.SelectedIndex;
                _localSettings.NBA_StartClient = chkStartServerAndClient.Checked;
            }
        }

        private void CompileOptionChanged()
        {
            if (_wasExistingBranch)
            {
                return;
            }

            if (optFullCompile.Checked)
            {
                chkStartServerAndClient.Checked = _restoreStartClientCheck;
            }
            else
            {
                _restoreStartClientCheck = chkStartServerAndClient.Checked;
                chkStartServerAndClient.Checked = false;
            }

            SetEnabledStates();
        }

        private void SetEnabledStates()
        {
            optFullCompile.Enabled = chkGenerateSolution.Checked;
            optMinimalCompile.Enabled = chkGenerateSolution.Checked;
            chkOpenIDE.Enabled = chkGenerateSolution.Checked || _wasExistingBranch;
            chkStartServerAndClient.Enabled = (chkGenerateSolution.Checked && optFullCompile.Checked) || _wasExistingBranch;
        }

        #endregion

        #region Control events

        private void DlgNewBranchActions_Shown(object sender, EventArgs e)
        {
            // Position the dialog towards the bottom right of the main screen
            this.Left = _mainForm.Right - this.Width - 40;
            this.Top = _mainForm.Bottom - this.Height - 30;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnCancel.Enabled = false;

            // Put this screen's settings into our local settings dictionary
            GetSettings();

            // Run the actions synchronously on the main form.  Our modal dialog will remain.
            _mainForm.RunAdditionalNewBranchActions(this);

            // All actions completed, so we close and return control to the main screen
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void chkGenerateSolution_CheckedChanged(object sender, EventArgs e)
        {
            if (_wasExistingBranch)
            {
                return;
            }

            if (chkGenerateSolution.Checked)
            {
                chkOpenIDE.Checked = _restoreOpenIDECheck;
                chkStartServerAndClient.Checked = _restoreStartClientCheck;
            }
            else
            {
                _restoreOpenIDECheck = chkOpenIDE.Checked;
                _restoreStartClientCheck = chkStartServerAndClient.Checked;
                chkOpenIDE.Checked = false;
                chkStartServerAndClient.Checked = false;
            }

            SetEnabledStates();
        }

        private void optFullCompile_CheckedChanged(object sender, EventArgs e)
        {
            if (optFullCompile.Checked)
            {
                CompileOptionChanged();
            }
        }

        private void optMinimalCompile_CheckedChanged(object sender, EventArgs e)
        {
            if (optMinimalCompile.Checked)
            {
                CompileOptionChanged();
            }
        }

        #endregion
    }
}