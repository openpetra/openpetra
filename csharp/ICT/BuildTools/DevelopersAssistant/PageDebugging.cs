//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ict.Tools.DevelopersAssistant
{
    /// <summary>
    /// The user control that sits on the Debugging tab page
    /// </summary>
    public partial class PageDebugging : UserControl
    {
        DebugLevelDetails _currentDetails = null;
        string _branchLocation = string.Empty;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PageDebugging()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Public method that can fill the content of the grid that shows what the various debug levels do
        /// </summary>
        public void PopulateDebugLevelGrid()
        {
            // populate the grid with a static list accessed by means of a static method
            dataGridViewDebug.DataSource = DebugLevels.GetDebugLevelList();

            // define how the grid should look
            dataGridViewDebug.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewDebug.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewDebug.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewDebug.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// This method is called when the debugging tab is selected
        /// </summary>
        public void OnDebuggingTabSelected(string ABranchLocation)
        {
            _branchLocation = ABranchLocation;
            dataGridViewDebug.AutoResizeColumns();

            if (_branchLocation == string.Empty)
            {
                _currentDetails = null;
                nudClientDebugLevel.Value = 0;
                nudServerDebugLevel.Value = 0;
                chkLevelSetByBuildConfig.Checked = false;
                btnApplyDebugLevels.Enabled = false;
                linkLabelResetDebugging.Enabled = false;
                return;
            }

            this.ParentForm.Cursor = Cursors.WaitCursor;

            _currentDetails = new DebugLevelDetails(ABranchLocation);

            nudClientDebugLevel.Value = _currentDetails.ClientDebugLevel;
            nudServerDebugLevel.Value = _currentDetails.ServerDebugLevel;
            chkLevelSetByBuildConfig.Checked = _currentDetails.IsSetByBuildConfig;
            btnApplyDebugLevels.Enabled = true;
            linkLabelResetDebugging.Enabled = true;

            this.ParentForm.Cursor = Cursors.Default;
        }

        /// <summary>
        /// This method is called when a tab other than the debugging tab is selected.  The previously selected tab could be any tab.
        /// </summary>
        public void OnDebuggingTabDeselected()
        {
            if (_currentDetails == null)
            {
                // We were not the previously selected tab.
                return;
            }

            int clientLevel = Convert.ToInt16(nudClientDebugLevel.Value);
            int serverLevel = Convert.ToInt16(nudServerDebugLevel.Value);

            if (_currentDetails.HasChanges(clientLevel, serverLevel))
            {
                if (MessageBox.Show("You have changed the debug logging settings.  Do you want to apply the changes?",
                        Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnApplyDebugLevels_Click(null, null);
                }
            }

            _currentDetails = null;
        }

        private void chkLevelSetByBuildConfig_CheckedChanged(object sender, EventArgs e)
        {
            nudClientDebugLevel.Enabled = !chkLevelSetByBuildConfig.Checked;

            if (chkLevelSetByBuildConfig.Checked)
            {
                nudClientDebugLevel.Value = nudServerDebugLevel.Value;
            }
        }

        private void btnApplyDebugLevels_Click(object sender, EventArgs e)
        {
            int clientLevel = Convert.ToInt16(nudClientDebugLevel.Value);
            int serverLevel = Convert.ToInt16(nudServerDebugLevel.Value);

            SetNewDebugLevels(chkLevelSetByBuildConfig.Checked, clientLevel, serverLevel);

            if (sender != null)
            {
                // reload the current details if the button was clicked but not if we chose 'Apply' after changing the tab
                OnDebuggingTabSelected(_branchLocation);
            }
        }

        private void linkLabelResetDebugging_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetNewDebugLevels(true, 0, 0);

            // reload the current details if the button was clicked but not if we chose 'Apply' after changing the tab
            OnDebuggingTabSelected(_branchLocation);
        }

        private void SetNewDebugLevels(bool UseBuildConfig, int ClientLevel, int ServerLevel)
        {
            if (_currentDetails.SaveChanges(UseBuildConfig, ClientLevel, ServerLevel))
            {
                // If the server is running we can change the level 'live'

                // Show a success message
                string msg =
                    "The configuration files have been successfully updated.  The changes will take effect the next time that you start the client/server.";
                MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void nudServerDebugLevel_ValueChanged(object sender, EventArgs e)
        {
            if (chkLevelSetByBuildConfig.Checked)
            {
                nudClientDebugLevel.Value = nudServerDebugLevel.Value;
            }
        }
    }
}