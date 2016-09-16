//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ict.Tools.DevelopersAssistant
{
    /// <summary>
    /// Supported IDE's
    /// </summary>
    public enum TDevEnvironment
    {
        /// <summary>SharpDevelop</summary>
        SharpDevelop,

        /// <summary>Visual Studio</summary>
        VisualStudio
    };

    /// <summary>Supported IDE Solutions</summary>
    public enum TSolution
    {
        /// <summary>OpenPetra.sln</summary>
        Full,

        /// <summary>OpenPetra.Client.sln</summary>
        Client,

        /// <summary>OpenPetra.Server.sln</summary>
        Server,

        /// <summary>OpenPetra.Tools.sln</summary>
        Tools,

        /// <summary>OpenPetra.Testing.sln</summary>
        Testing
    };

    /****************************************************************************************************************************************
     *
     * The main window class for openPetra Developer's Assistant
     *
     * *************************************************************************************************************************************/

    /// <summary>
    /// The main window class for the application
    /// </summary>
    public partial class MainForm : Form
    {
        private const string STR_READY = "Ready";
        private const string STR_REVIEW_OUTPUT =
            "The most recent Nant task resulted in errors, warnings or failures. The Output tab shows where these occurred.";

        private SettingsDictionary _localSettings = null;                                   // Our settings persisted locally between sessions
        private ExternalLinksDictionary _externalLinks = null;                              // Our external links to the web
        private bool _serverIsRunning = false;                                              // Local variable holds server state
        private List <NantTask.TaskItem>_sequence = new List <NantTask.TaskItem>();         // List of tasks in the standard sequence
        private List <NantTask.TaskItem>_altSequence = new List <NantTask.TaskItem>();      // List of tasks in the alternate sequence
        private List <OutputText.ErrorItem>_warnings = new List <OutputText.ErrorItem>();   // List of positions/severities in verbose text where warnings/errors appear
        private int _currentWarning = -1;                                                   // 'Current' warning ID in _warnings list
        private string _activeAsyncNantTask = String.Empty;                                 // If not empty contains the status text for an active async task
        private string _activeAsyncBazaarTask = String.Empty;                               // If not empty contains the status text for an active async task
        private Color _normalLabelColor;                                                    // Stores the label back colour for the current visual style
        private TDevEnvironment _preferredIDE = TDevEnvironment.SharpDevelop;               // The installed development environment

        private enum TPaths
        {
            Settings,               // User settings file
            ExternalLinks           // User links file
        };

        /// <summary>
        /// Gets the current branch location as displayed as the first entry in the drop down list for branch history
        /// </summary>
        private string BranchLocation
        {
            get
            {
                if (cboBranchLocation.Items.Count == 0)
                {
                    return String.Empty;
                }
                else
                {
                    return cboBranchLocation.Items[cboBranchLocation.SelectedIndex].ToString();
                }
            }
        }

        #region Initialisation and GUI state routines

        /**************************************************************************************************************************************
         *
         * Initialisation and GUI state routines
         *
         * ***********************************************************************************************************************************/

        /// <summary>
        /// Constructor for the class
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            string appVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
            _localSettings = new SettingsDictionary(GetDataFilePath(TPaths.Settings), appVersion);
            _localSettings.Load();

            _externalLinks = new ExternalLinksDictionary(GetDataFilePath(TPaths.ExternalLinks));
            _externalLinks.Load();
            _externalLinks.PopulateListBox(lstExternalWebLinks);

            tbbSourceHistoryAllMenuItem.Font = new Font(tbbSourceHistoryAllMenuItem.Font, FontStyle.Bold);
            tbbShowSourceDifferencesAllMenuItem.Font = new Font(tbbShowSourceDifferencesAllMenuItem.Font, FontStyle.Bold);

            PopulateCombos();

            this.Text = Program.APP_TITLE;
            cboCodeGeneration.SelectedIndex = _localSettings.CodeGenerationComboID;
            cboCompilation.SelectedIndex = _localSettings.CompilationComboID;
            cboMiscellaneous.SelectedIndex = _localSettings.MiscellaneousComboID;
            cboDatabase.SelectedIndex = _localSettings.DatabaseComboID;
            cboSourceCode.SelectedIndex = _localSettings.SourceCodeComboID;
            chkAutoStartServer.Checked = _localSettings.AutoStartServer;
            chkAutoStopServer.Checked = _localSettings.AutoStopServer;
            chkCheckForUpdatesAtStartup.Checked = _localSettings.AutoCheckForUpdates;
            chkMinimizeServer.Checked = _localSettings.MinimiseServerAtStartup;
            chkTreatWarningsAsErrors.Checked = _localSettings.TreatWarningsAsErrors;
            chkCompileWinform.Checked = _localSettings.CompileWinForm;
            chkStartClientAfterGenerateWinform.Checked = _localSettings.StartClientAfterCompileWinForm;
            txtFlashAfterSeconds.Text = _localSettings.FlashAfterSeconds.ToString();

            if (_localSettings.BranchLocation != String.Empty)
            {
                cboBranchLocation.Items.Add(_localSettings.BranchLocation);

                for (int i = 1; i < 10; i++)
                {
                    string path = _localSettings.GetBranchHistory(i);

                    if (path == String.Empty)
                    {
                        break;
                    }

                    cboBranchLocation.Items.Add(path);
                }

                cboBranchLocation.SelectedIndex = 0;
            }

            if (_localSettings.YAMLLocationHistory != string.Empty)
            {
                cboYAMLHistory.Items.AddRange(_localSettings.YAMLLocationHistory.Split(','));
                cboYAMLHistory.SelectedIndex = 0;
            }

            txtLaunchpadUserName.Text = _localSettings.LaunchpadUserName;

            txtBazaarPath.Text = _localSettings.BazaarPath;
            ValidateBazaarPath();

            _sequence = ConvertStringToSequenceList(_localSettings.Sequence);
            _altSequence = ConvertStringToSequenceList(_localSettings.AltSequence);
            ShowSequence(txtSequence, _sequence);
            ShowSequence(txtAltSequence, _altSequence);
            lblVersion.Text = "Version " + appVersion;

            SetBranchDependencies();

            GetPreferredIDE();
            GetServerState();

            SetEnabledStates();

            SetToolTips();

            // Check if we were launched using commandline switches
            // If so, we execute the instruction, start a timer, which then will close us down.
            if (Program.cmdLine.StartServer)
            {
                linkLabelStartServer_LinkClicked(null, null);
                ShutdownTimer.Enabled = true;
            }
            else if (Program.cmdLine.StopServer)
            {
                linkLabelStopServer_LinkClicked(null, null);
                ShutdownTimer.Enabled = true;
            }

            _normalLabelColor = toolStripStatusLabel.BackColor;
        }

        private void ShutdownTimer_Tick(object sender, EventArgs e)
        {
            ShutdownTimer.Enabled = false;
            Close();
        }

        private void SetBranchDependencies()
        {
            lblBranchLocation.Text = (BranchLocation == String.Empty) ? "Not defined" : BranchLocation;

            BuildConfiguration dbCfg = new BuildConfiguration(BranchLocation, _localSettings);

            if (BranchLocation == String.Empty)
            {
                lblDbBuildConfig.Text = "";
                lblDatabaseName.Text = "None";
                ShowDbNameInComboText(string.Empty);
            }
            else
            {
                lblDbBuildConfig.Text = dbCfg.CurrentConfig;
                lblDatabaseName.Text = dbCfg.CurrentDBName;
                ShowDbNameInComboText(dbCfg.CurrentDBName);
            }

            dbCfg.ListAllConfigs(listDbBuildConfig, -1);        // This might add a new configuration, so we need to update our local settings
            _localSettings.DbBuildConfigurations = dbCfg.FavouriteConfigurations;
            btnRemoveDbBuildConfig.Enabled = listDbBuildConfig.Items.Count > 0;
            btnEditDbBuildConfig.Enabled = listDbBuildConfig.Items.Count > 0;
            btnSaveDbBuildConfig.Enabled = listDbBuildConfig.Items.Count > 0 && BranchLocation != String.Empty;
            SetPromoteDemoteButtons();

            if (BranchLocation == String.Empty)
            {
                txtAutoLogonUser.Text = String.Empty;
                txtAutoLogonPW.Text = String.Empty;
                txtAutoLogonAction.Text = String.Empty;
                chkUseAutoLogon.Checked = false;
                chkUseAutoLogon.Enabled = false;
            }
            else
            {
                ClientAutoLogOn calo = new ClientAutoLogOn(BranchLocation);
                txtAutoLogonUser.Text = calo.UserName;
                txtAutoLogonPW.Text = calo.Password;
                txtAutoLogonAction.Text = calo.TestAction.Replace(",", "\r\n");
                chkUseAutoLogon.Checked = (txtAutoLogonUser.Text != String.Empty);
                chkUseAutoLogon.Enabled = true;
            }
        }

        private void SetToolTips()
        {
            toolTip.SetToolTip(btnDatabase, toolTip.GetToolTip(btnDatabase) + Environment.NewLine + "Shortcut: Ctrl + D");
            toolTip.SetToolTip(btnCodeGeneration, toolTip.GetToolTip(btnCodeGeneration) + Environment.NewLine + "Shortcut: Ctrl + G");
            toolTip.SetToolTip(btnCompilation, toolTip.GetToolTip(btnCompilation) + Environment.NewLine + "Shortcut: Ctrl + I");
            toolTip.SetToolTip(btnMiscellaneous, toolTip.GetToolTip(btnMiscellaneous) + Environment.NewLine + "Shortcut: Ctrl + M");
            toolTip.SetToolTip(btnStartClient, toolTip.GetToolTip(btnStartClient) + Environment.NewLine + "Shortcut: Ctrl + O");
            toolTip.SetToolTip(linkLabelRestartServer, toolTip.GetToolTip(linkLabelRestartServer) + Environment.NewLine + "Shortcut: Ctrl + R");
            toolTip.SetToolTip(linkLabelStartServer, toolTip.GetToolTip(linkLabelStartServer) + Environment.NewLine + "Shortcut: Ctrl + S");
            toolTip.SetToolTip(btnSourceCode, toolTip.GetToolTip(btnSourceCode) + Environment.NewLine + "Shortcut: Ctrl + U");
            toolTip.SetToolTip(btnPreviewWinform, toolTip.GetToolTip(btnPreviewWinform) + Environment.NewLine + "Shortcut: Ctrl + W");
            toolTip.SetToolTip(btnGenerateWinform, toolTip.GetToolTip(btnGenerateWinform) + Environment.NewLine + "Shortcut: Ctrl + Y");
            toolTip.SetToolTip(linkLabelBazaar, toolTip.GetToolTip(linkLabelBazaar) + Environment.NewLine + "Shortcut: Ctrl + Z");
        }

        private void PopulateCombos()
        {
            // Code generation
            for (NantTask.TaskItem i = NantTask.FirstCodeGenItem; i <= NantTask.LastCodeGenItem; i++)
            {
                NantTask t = new NantTask(i);
                cboCodeGeneration.Items.Add(t.ShortDescription);
            }

            // Compile
            for (NantTask.TaskItem i = NantTask.FirstCompileItem; i <= NantTask.LastCompileItem; i++)
            {
                NantTask t = new NantTask(i);
                cboCompilation.Items.Add(t.ShortDescription);
            }

            // Misc
            for (NantTask.TaskItem i = NantTask.FirstMiscItem; i <= NantTask.LastMiscItem; i++)
            {
                NantTask t = new NantTask(i);
                cboMiscellaneous.Items.Add(t.ShortDescription);
            }

            // Database
            for (NantTask.TaskItem i = NantTask.FirstDatabaseItem; i <= NantTask.LastDatabaseItem; i++)
            {
                NantTask t = new NantTask(i);
                cboDatabase.Items.Add(t.ShortDescription);
            }

            // Source Code Control
            for (BazaarTask.TaskItem i = BazaarTask.FirstBazaarItem; i <= BazaarTask.LastBazaarItem; i++)
            {
                BazaarTask t = new BazaarTask(i);
                cboSourceCode.Items.Add(t.Description);
            }
        }

        private void SetEnabledStates()
        {
            bool bGotBranch = BranchLocation != String.Empty;
            bool bIsNantIdle = _activeAsyncNantTask == String.Empty;
            bool bIsBazaarIdle = _activeAsyncBazaarTask == String.Empty;
            bool bIsBlockingBazaarTaskSelected = false;

            if (bGotBranch && (cboSourceCode.Items.Count > 0))
            {
                // Deal with the Go button for Source Code
                BazaarTask.TaskItem curTask = (BazaarTask.TaskItem)cboSourceCode.SelectedIndex + 1;

                if ((curTask != BazaarTask.TaskItem.qdiffFile) && (curTask != BazaarTask.TaskItem.qlogFile))
                {
                    bIsBlockingBazaarTaskSelected = (new BazaarTask(curTask)).GetBazaarArgs(BranchLocation, string.Empty).Contains("--ui-mode");
                }
            }

            linkLabelStartServer.Enabled = bGotBranch && !_serverIsRunning && bIsNantIdle;
            linkLabelStopServer.Enabled = bGotBranch && _serverIsRunning && bIsNantIdle;
            linkLabelRestartServer.Enabled = bGotBranch && _serverIsRunning && bIsNantIdle;
            linkLabelYamlFile.Enabled = bGotBranch;
            linkLabelBranchLocation.Enabled = bIsNantIdle && bIsBazaarIdle;
            cboBranchLocation.Enabled = bIsNantIdle && bIsBazaarIdle;

            btnGenerateWinform.Enabled = bGotBranch && bIsNantIdle && cboYAMLHistory.Items.Count > 0;
            btnCodeGeneration.Enabled = bGotBranch && bIsNantIdle;
            btnCompilation.Enabled = bGotBranch && bIsNantIdle;
            btnMiscellaneous.Enabled = bGotBranch && bIsNantIdle;
            btnDatabase.Enabled = bGotBranch && bIsNantIdle;
            btnSourceCode.Enabled = (bGotBranch || (cboSourceCode.SelectedIndex + 1 == (int)BazaarTask.TaskItem.qbranch))
                                    && (bIsBazaarIdle || !bIsBlockingBazaarTaskSelected);
            btnStartClient.Enabled = bGotBranch && bIsNantIdle;
            btnRunSequence.Enabled = bGotBranch && bIsNantIdle && txtSequence.Text != String.Empty;
            btnRunAltSequence.Enabled = bGotBranch && bIsNantIdle && txtAltSequence.Text != String.Empty;
            btnResetClientConfig.Enabled = bGotBranch;
            btnUpdateMyClientConfig.Enabled = bGotBranch;
            btnPreviewWinform.Enabled = CanEnableYAMLPreview(btnGenerateWinform.Enabled);

            tbbGenerateSolutionFullCompile.Enabled = btnCodeGeneration.Enabled;
            tbbGenerateSolutionMinCompile.Enabled = btnCodeGeneration.Enabled;
            tbbGenerateWinForms.Enabled = btnCodeGeneration.Enabled;
            tbbGenerateGlue.Enabled = btnCodeGeneration.Enabled;

            tbbUncrustify.Enabled = btnMiscellaneous.Enabled;
            tbbRunAllTests.Enabled = btnMiscellaneous.Enabled;
            tbbRunMainNavigationScreensTests.Enabled = btnMiscellaneous.Enabled;

            tbbSourceHistoryLog.Enabled = bGotBranch;
            tbbShowSourceDifferences.Enabled = bGotBranch;
            tbbCommitSourceChanges.Enabled = bGotBranch && bIsBazaarIdle;
            tbbShelveSourceChanges.Enabled = bGotBranch && bIsBazaarIdle;
            tbbUnshelveSourceChanges.Enabled = bGotBranch && bIsBazaarIdle;

            tbbMergeSourceFromTrunk.Enabled = bGotBranch && bIsBazaarIdle;
            tbbCreateNewSourceBranch.Enabled = bIsBazaarIdle;

            tbbCreateDatabase.Enabled = btnDatabase.Enabled;
            tbbDatabaseContent.Enabled = btnDatabase.Enabled;
        }

        private void SetWarningButtons()
        {
            btnPrevWarning.Enabled = (_warnings.Count > 0 && chkVerbose.Checked);
            btnNextWarning.Enabled = (_warnings.Count > 0 && chkVerbose.Checked);
        }

        private void SetPromoteDemoteButtons()
        {
            int index = listDbBuildConfig.SelectedIndex;
            int count = listDbBuildConfig.Items.Count;

            btnPromoteFavouriteBuild.Enabled = (count > 1 && index >= 1);
            btnDemoteFavouriteBuild.Enabled = (count > 1 && index < count - 1);
        }

        private void GetServerState()
        {
            _serverIsRunning = NantExecutor.IsServerRunning();
        }

        private List <NantTask.TaskItem>ConvertStringToSequenceList(string s)
        {
            List <NantTask.TaskItem>list = new List <NantTask.TaskItem>();

            if (s != String.Empty)
            {
                string[] items = s.Split(';');

                for (int i = 0; i < items.Length; i++)
                {
                    list.Add((NantTask.TaskItem)Convert.ToInt32(items[i]));
                }
            }

            return list;
        }

        private string ConvertSequenceListToString(List <NantTask.TaskItem>list)
        {
            string s = "";

            for (int i = 0; i < list.Count; i++)
            {
                if (s != String.Empty)
                {
                    s += "; ";
                }

                s += ((int)list[i]).ToString();
            }

            return s;
        }

        private void ShowSequence(TextBox tb, List <NantTask.TaskItem>sequence)
        {
            string s = "";

            for (int i = 0; i < sequence.Count; i++)
            {
                if (s != String.Empty)
                {
                    s += "\r\n";
                }

                NantTask task = new NantTask(sequence[i]);
                s += task.Description;
            }

            tb.Text = s;
        }

        #endregion

        #region GUI event handlers

        /************************************************************************************************************************
         *
         * GUI event handlers
         *
         * *********************************************************************************************************************/

        private void linkLabelBranchLocation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            InitialiseFolderBrowserDialog(dlg);

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            if (String.Compare(dlg.SelectedPath, BranchLocation, true) == 0)
            {
                return;                                                                         // no change
            }

            string tryPath = dlg.SelectedPath;

            if (!ValidateBranchPath(ref tryPath))
            {
                // go back to the original path in the dialog
                tryPath = dlg.SelectedPath;

                // Is it empty??
                bool isEmpty = (Directory.GetDirectories(tryPath).GetLength(0) == 0 && Directory.GetFiles(tryPath).GetLength(0) == 0);

                string msg =
                    "The location that you have chosen is not a recognised OpenPetra source folder.  The file 'OpenPetra.Build' could not be found.";

                if (isEmpty)
                {
                    msg += Environment.NewLine + Environment.NewLine;
                    msg += "Do you want to create a new Launchpad branch and associate it with this folder?";

                    if (MessageBox.Show(
                            msg,
                            Program.APP_TITLE,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(
                        msg,
                        Program.APP_TITLE,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

                // So we create a new branch on Launchpad then ...
                RunCreateBazaarBranch(ref tryPath);

                if (!ValidateBranchPath(ref tryPath))
                {
                    msg =
                        "A problem occurred while creating the new branch.  There is no source code in the target location.  Maybe you cancelled the Launchpad operation?";
                    MessageBox.Show(
                        msg,
                        Program.APP_TITLE,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }
            }

            GetServerState();

            if (_serverIsRunning)
            {
                RunSimpleNantTarget(new NantTask(NantTask.TaskItem.stopPetraServer));
            }

            ChangeBranchLocation(tryPath);

            DlgNewBranchActions dlgNewBranch = new DlgNewBranchActions(this);
            dlgNewBranch.Initialise(tryPath, _localSettings, _preferredIDE);
            dlgNewBranch.ShowDialog();
        }

        private void cboBranchLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBranchLocation.SelectedIndex < 1)
            {
                return;
            }

            // The user has chosen a different branch from the history
            string path = cboBranchLocation.Items[cboBranchLocation.SelectedIndex].ToString();

            if (!ValidateBranchPath(ref path))
            {
                MessageBox.Show("The selected path has been moved and is no longer valid.", Program.APP_TITLE);
                cboBranchLocation.SelectedIndex = 0;
                return;
            }

            ChangeBranchLocation(path);
        }

        private void linkLabelYamlFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = "yaml";
            dlg.AddExtension = true;
            dlg.Filter = "YAML files|*.yaml;*.yml|All files|*.*";
            dlg.InitialDirectory = BranchLocation + "\\csharp\\ICT\\Petra\\Client";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            string s = dlg.FileName;
            int p = s.IndexOf("\\Petra\\Client\\", 0, StringComparison.InvariantCultureIgnoreCase);

            UpdateYAMLHistoryOrder(s.Substring(p + 14));

            SetEnabledStates();
        }

        private void chkCompileWinform_CheckedChanged(object sender, EventArgs e)
        {
            chkStartClientAfterGenerateWinform.Enabled = chkCompileWinform.Checked;
            chkStartClientAfterGenerateWinform.Checked = chkCompileWinform.Checked;
        }

        private void linkModifySequence_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DlgSequence dlg = new DlgSequence();

            dlg.InitializeList(_sequence);

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            _sequence = dlg.ExitSequence;
            ShowSequence(txtSequence, _sequence);
            SetEnabledStates();
        }

        private void linkModifyAltSequence_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DlgSequence dlg = new DlgSequence();

            dlg.InitializeList(_altSequence);

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            _altSequence = dlg.ExitSequence;
            ShowSequence(txtAltSequence, _altSequence);
            SetEnabledStates();
        }

        private void chkVerbose_CheckedChanged(object sender, EventArgs e)
        {
            txtOutput.Text = chkVerbose.Checked ? OutputText.VerboseOutput : OutputText.ConciseOutput;
            SetWarningButtons();
            lblWarnings.Text = GetWarningDisplayText();
        }

        private void btnNextWarning_Click(object sender, EventArgs e)
        {
            _currentWarning++;

            if (_currentWarning >= _warnings.Count)
            {
                _currentWarning = 0;
            }

            txtOutput.Focus();
            txtOutput.SelectionStart = _warnings[_currentWarning].Position;
            txtOutput.SelectionLength = _warnings[_currentWarning].SelLength;
            txtOutput.ScrollToCaret();
            lblWarnings.Text = GetWarningDisplayText();
            SetWarningButtons();
        }

        private void btnPrevWarning_Click(object sender, EventArgs e)
        {
            _currentWarning--;

            if (_currentWarning < 0)
            {
                _currentWarning = _warnings.Count - 1;
            }

            txtOutput.Focus();
            txtOutput.SelectionStart = _warnings[_currentWarning].Position;
            txtOutput.SelectionLength = _warnings[_currentWarning].SelLength;
            txtOutput.ScrollToCaret();
            lblWarnings.Text = GetWarningDisplayText();
            SetWarningButtons();
        }

        private void btnAddDbBuildConfig_Click(object sender, EventArgs e)
        {
            DlgDbBuildConfig dlg = new DlgDbBuildConfig();

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            BuildConfiguration dbCfg = new BuildConfiguration(BranchLocation, _localSettings);
            dbCfg.InsertConfig(dlg.ExitData);
            SetBranchDependencies();
            listDbBuildConfig.SelectedIndex = 0;
        }

        private void btnRemoveDbBuildConfig_Click(object sender, EventArgs e)
        {
            int index = listDbBuildConfig.SelectedIndex;

            if (index < 0)
            {
                return;
            }

            string msg = "This will remove the selected item from the saved configurations list.  Are you sure?";

            if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            BuildConfiguration dbCfg = new BuildConfiguration(BranchLocation, _localSettings);
            dbCfg.RemoveConfig(index);
            _localSettings.DbBuildConfigurations = dbCfg.FavouriteConfigurations;

            SetBranchDependencies();

            if ((--index < 0) && (listDbBuildConfig.Items.Count > 0))
            {
                index = 0;
            }

            listDbBuildConfig.SelectedIndex = index;
        }

        private void btnEditDbBuildConfig_Click(object sender, EventArgs e)
        {
            int index = listDbBuildConfig.SelectedIndex;

            if (index < 0)
            {
                return;
            }

            DlgDbBuildConfig dlg = new DlgDbBuildConfig();
            dlg.InitializeDialog(BranchLocation, listDbBuildConfig.SelectedIndex, _localSettings);

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            BuildConfiguration dbCfg = new BuildConfiguration(BranchLocation, _localSettings);
            dbCfg.EditConfig(listDbBuildConfig.SelectedIndex, dlg.ExitData);
            _localSettings.DbBuildConfigurations = dbCfg.FavouriteConfigurations;

            SetBranchDependencies();
            listDbBuildConfig.SelectedIndex = index;
        }

        private void listDbBuildConfig_DoubleClick(object sender, EventArgs e)
        {
            btnEditDbBuildConfig_Click(sender, e);
        }

        private void btnSaveDbBuildConfig_Click(object sender, EventArgs e)
        {
            if (listDbBuildConfig.SelectedIndex < 0)
            {
                return;
            }

            string msg =
                "This will save the selected details to your 'Database Build Configuration File'.  Your existing configuration will be overwritten.  Are you sure you want to do this?";

            if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            // Stop the server because we will restart it again with a different database connection
            GetServerState();

            if (_serverIsRunning)
            {
                RunSimpleNantTarget(new NantTask(NantTask.TaskItem.stopPetraServer));
            }

            //  Ok - we write the specified settings to the config file and remove the unspecified ones
            BuildConfiguration DbCfg = new BuildConfiguration(BranchLocation, _localSettings);

            if (!DbCfg.SetConfigAsDefault(listDbBuildConfig.SelectedIndex))
            {
                return;
            }

            SetBranchDependencies();

            // Optionally run initConfigFiles to get everything matched up
            msg = "Do you want to run the 'InitConfigFiles' task to initialize the other configuration files?";

            if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                RunInitConfigFiles();
            }
        }

        private void btnSaveCurrentDbBuildConfig_Click(object sender, EventArgs e)
        {
            new BuildConfiguration(BranchLocation, _localSettings);
        }

        private void chkUseAutoLogon_CheckedChanged(object sender, EventArgs e)
        {
            txtAutoLogonUser.Enabled = chkUseAutoLogon.Checked;
            txtAutoLogonPW.Enabled = chkUseAutoLogon.Checked;
            lblAutoLogonUser.Enabled = chkUseAutoLogon.Checked;
            lblAutoLogonPW.Enabled = chkUseAutoLogon.Checked;
        }

        private void btnUpdateMyClientConfig_Click(object sender, EventArgs e)
        {
            if (chkUseAutoLogon.Checked && (txtAutoLogonUser.Text == String.Empty))
            {
                string msg = "If you want to use the Auto-Logon capability, you must supply a Username.";
                MessageBox.Show(msg, Program.APP_TITLE);
                txtAutoLogonUser.Focus();
                return;
            }

            ClientAutoLogOn calo = new ClientAutoLogOn(BranchLocation);

            // Validate the format of the action text
            string[] sep =
            {
                "\r\n"
            };
            string[] items = txtAutoLogonAction.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Trim();
            }

            string s = String.Empty;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != String.Empty)
                {
                    if (s != String.Empty)
                    {
                        s += ",";
                    }

                    s += items[i];
                }
            }

            if (calo.UpdateConfig(chkUseAutoLogon.Checked, txtAutoLogonUser.Text, txtAutoLogonPW.Text, s))
            {
                // Now we will run the InitConfigFiles task, which copies the new file ready for running the client
                // By doing it here users will see the effect of their changes if they run direct from the IDE as opposed to using this app to start the client
                if (RunInitConfigFiles())
                {
                    MessageBox.Show("The update was applied successfully.", Program.APP_TITLE);
                }
                else
                {
                    MessageBox.Show(
                        @"The update was applied successfully to \inc\Template\etc\Client.config.my, but an error occurred in running the InitConfigFiles task.",
                        Program.APP_TITLE);
                }
            }
        }

        private void btnResetClientConfig_Click(object sender, EventArgs e)
        {
            string msg =
                "This action will reset all your client options (and not simply the startup options) because it will overwrite your personal copy of your entire ";

            msg += "client configuration with the latest version from the source code repository.  ";
            msg += "Usually this is a good thing because the repository copy may contain enhancements that your current personal copy is lacking.  ";
            msg += "You may notice changes in the behaviour of your client application as a result of this action.\r\n\r\n";
            msg += "Do you want to proceed?";

            if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ClientAutoLogOn calo = new ClientAutoLogOn(BranchLocation);

            if (!calo.ResetConfig())
            {
                return;
            }

            SetBranchDependencies();

            if (!RunInitConfigFiles())
            {
                msg =
                    @"Your configuration was reset successfully in \inc\Template\etc\Client.config.my, but an error occurred in running the InitConfigFiles task.  ";
                msg += "This will mean that the Open Petra Client may not rspond correctly to your changes.";
                MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lstExternalWebLinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            string url;
            string info;

            _externalLinks.GetDetails(lstExternalWebLinks.SelectedItem.ToString(), out url, out info);
            lblExternalWebLink.Text = url;
            lblWebLinkInfo.Text = info;
        }

        private void btnBrowseWeb_Click(object sender, EventArgs e)
        {
            if (lblExternalWebLink.Text != String.Empty)
            {
                System.Diagnostics.Process.Start(lblExternalWebLink.Text);
            }
        }

        private void lstExternalWebLinks_DoubleClick(object sender, EventArgs e)
        {
            btnBrowseWeb_Click(sender, e);
        }

        private void linkEditLinks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(GetDataFilePath(TPaths.ExternalLinks));
        }

        private void linkRefreshLinks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _externalLinks = new ExternalLinksDictionary(GetDataFilePath(TPaths.ExternalLinks));
            _externalLinks.Load();
            _externalLinks.PopulateListBox(lstExternalWebLinks);
        }

        private void linkSuggestedLinksUpdates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Do we have any suggestions?
            string unUsed1, unUsed2, unUsed3;

            if (_externalLinks.GetSuggestedUpdate(0, out unUsed1, out unUsed2, out unUsed3))
            {
                DlgSuggestedLinksUpdates dlg = new DlgSuggestedLinksUpdates();
                dlg.Initialise(_externalLinks);
                dlg.ShowDialog();

                if (dlg.NumberOfChanges > 0)
                {
                    // Update the GUI
                    _externalLinks.Save();

                    string msg = "Your change has been saved.";

                    if (dlg.NumberOfChanges > 1)
                    {
                        msg = string.Format("Your {0} changes have been saved.", dlg.NumberOfChanges);
                    }

                    MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK);

                    _externalLinks.PopulateListBox(lstExternalWebLinks);
                }
            }
            else
            {
                MessageBox.Show("There are no new suggestions for external links.  All your links are up to date!",
                    Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool bHaveAlertedFlashSetting = false;
        private void txtFlashAfterSeconds_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                Convert.ToUInt32(txtFlashAfterSeconds.Text);
                bHaveAlertedFlashSetting = false;
            }
            catch (Exception)
            {
                e.Cancel = true;
                tabControl.SelectedTab = OptionsPage;

                if (!bHaveAlertedFlashSetting)
                {
                    MessageBox.Show("Please enter a numeric value for the flash delay.", Program.APP_TITLE);
                }

                bHaveAlertedFlashSetting = true;
                txtFlashAfterSeconds.Focus();
                txtFlashAfterSeconds.SelectAll();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((_activeAsyncBazaarTask != String.Empty) || (_activeAsyncNantTask != String.Empty))
            {
                string msg =
                    "You have a Nant or Bazaar process that has not been terminated.  Do you want to wait until the task has been completed?";

                if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            _localSettings.AltSequence = ConvertSequenceListToString(_altSequence);
            _localSettings.AutoCheckForUpdates = chkCheckForUpdatesAtStartup.Checked;
            _localSettings.BazaarPath = txtBazaarPath.Text;
            _localSettings.BranchLocation = BranchLocation;
            _localSettings.AutoStartServer = chkAutoStartServer.Checked;
            _localSettings.AutoStopServer = chkAutoStopServer.Checked;
            _localSettings.CodeGenerationComboID = cboCodeGeneration.SelectedIndex;
            _localSettings.CompilationComboID = cboCompilation.SelectedIndex;
            _localSettings.CompileWinForm = chkCompileWinform.Checked;
            _localSettings.DatabaseComboID = cboDatabase.SelectedIndex;
            _localSettings.FlashAfterSeconds = Convert.ToUInt32(txtFlashAfterSeconds.Text);
            _localSettings.LaunchpadUserName = txtLaunchpadUserName.Text;
            _localSettings.MinimiseServerAtStartup = chkMinimizeServer.Checked;
            _localSettings.MiscellaneousComboID = cboMiscellaneous.SelectedIndex;
            _localSettings.Sequence = ConvertSequenceListToString(_sequence);
            _localSettings.SourceCodeComboID = cboSourceCode.SelectedIndex;
            _localSettings.StartClientAfterCompileWinForm = chkStartClientAfterGenerateWinform.Checked;
            _localSettings.TreatWarningsAsErrors = chkTreatWarningsAsErrors.Checked;
            _localSettings.YAMLLocationHistory = string.Join(",", cboYAMLHistory.Items.Cast <string>());

            if (WindowState == FormWindowState.Minimized)
            {
                _localSettings.WindowPosition = string.Format("{0}; {1}", RestoreBounds.Left, RestoreBounds.Top);
            }
            else
            {
                _localSettings.WindowPosition = string.Format("{0}; {1}", Left, Top);
            }

            for (int i = 1; (i < 10) && (i < cboBranchLocation.Items.Count); i++)
            {
                _localSettings.SetBranchHistoryItem(i, cboBranchLocation.Items[i].ToString());
            }

            for (int i = cboBranchLocation.Items.Count; i < 10; i++)
            {
                _localSettings.SetBranchHistoryItem(i, String.Empty);
            }

            _localSettings.ContentHeader = String.Format("; Settings file for Open Petra Developer's Assistant\r\n; Application {0}\r\n",
                lblVersion.Text);
            _localSettings.Save();
        }

        private void listDbBuildConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPromoteDemoteButtons();
        }

        private void btnPromoteFavouriteBuild_Click(object sender, EventArgs e)
        {
            BuildConfiguration DbCfg = new BuildConfiguration(BranchLocation, _localSettings);

            DbCfg.Promote(listDbBuildConfig);
            SetPromoteDemoteButtons();
        }

        private void btnDemoteFavouriteBuild_Click(object sender, EventArgs e)
        {
            BuildConfiguration DbCfg = new BuildConfiguration(BranchLocation, _localSettings);

            DbCfg.Demote(listDbBuildConfig);
            SetPromoteDemoteButtons();
        }

        /// <summary>
        /// Handler to capture keypress for keyboard accelerators
        /// </summary>
        /// <param name="message">Windows message</param>
        /// <param name="keys">Keys invoked</param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            if (tabControl.SelectedIndex == 0)
            {
                switch (keys)
                {
                    case Keys.F5:
                        btnRunSequence_Click(null, null);
                        return true;

                    case Keys.F5 | Keys.Alt:
                        btnRunAltSequence_Click(null, null);
                        return true;

                    case Keys.G | Keys.Control:
                        btnCodeGeneration_Click(null, null);
                        return true;

                    case Keys.I | Keys.Control:
                        btnCompilation_Click(null, null);
                        return true;

                    case Keys.M | Keys.Control:
                        btnMiscellaneous_Click(null, null);
                        return true;

                    case Keys.O | Keys.Control:
                        btnStartClient_Click(null, null);
                        return true;

                    case Keys.R | Keys.Control:
                        linkLabelRestartServer_LinkClicked(null, null);
                        return true;

                    case Keys.S | Keys.Control:
                        linkLabelStartServer_LinkClicked(null, null);
                        return true;

                    case Keys.U | Keys.Control:
                        btnSourceCode_Click(null, null);
                        return true;

                    case Keys.W | Keys.Control:
                        btnPreviewWinform_Click(null, null);
                        return true;

                    case Keys.Y | Keys.Control:
                        btnGenerateWinform_Click(null, null);
                        return true;

                    case Keys.Z | Keys.Control:
                        linkLabelBazaar_LinkClicked(null, null);
                        return true;
                }
            }

            if (tabControl.SelectedIndex == 1)
            {
                switch (keys)
                {
                    case Keys.D | Keys.Control:
                        btnDatabase_Click(null, null);
                        return true;
                }
            }

            return base.ProcessCmdKey(ref message, keys);
        }

        /// <summary>
        /// This is the override for the main windows message pump.  We use it to process our own user messages
        /// </summary>
        /// <param name="message">The message object to process</param>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message message)
        {
            // We just check for our magic number to see if we have been activated by another instance

            if (message.Msg == Program.UM_STOP_SERVER)
            {
                System.Diagnostics.Trace.WriteLine(String.Format("Stop server: Message received - {0}", message.Msg));
                linkLabelStopServer_LinkClicked(null, null);
            }
            else if (message.Msg == Program.UM_START_SERVER)
            {
                System.Diagnostics.Trace.WriteLine(String.Format("Start server: Message received - {0}", message.Msg));
                linkLabelStartServer_LinkClicked(null, null);
            }
            else
            {
                base.WndProc(ref message);
            }
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            // Every 10 seconds we just check that opda.txt got deleted.  Some tasks lock it until they are complete.
            // For example StartPetraClient: nant does not complete until the client window closes
            // There are two places to look ...
            string path = Path.Combine(BranchLocation, "opda.txt");

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception)
                {
                }
            }

            path = Path.Combine(BranchLocation, @"csharp\ICT\Petra\Client\opda.txt");

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception)
                {
                }
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            string windowPos = _localSettings.WindowPosition;

            if (windowPos != string.Empty)
            {
                string[] items = windowPos.Split(';');

                if (items.Length == 2)
                {
                    int l = Convert.ToInt32(items[0]);
                    int t = Convert.ToInt32(items[1]);

                    // Find the screen that contains the specified point
                    Point leftTop = new Point(l, t);
                    Rectangle workRect = Screen.GetWorkingArea(leftTop);

                    // Check if the working area of the screen completely contains our window
                    if (workRect.Contains(new Rectangle(leftTop, new Size(this.Width, this.Height))))
                    {
                        this.Left = l;
                        this.Top = t;
                    }
                }
            }

            // The window has just been shown for the first time
            string startupMessage = Program.cmdLine.StartupMessage;

            if (startupMessage != String.Empty)
            {
                switch (startupMessage)
                {
                    case "UpdateSuccess":
                        MessageBox.Show("The application was updated successfully.",
                        Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "UpdateFail":
                        MessageBox.Show(
                        "An error occurred during the update process.  The new file was not copied.  The application has not been updated.",
                        Program.APP_TITLE,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                        break;

                    default:
                        break;
                }
            }
            else if (chkCheckForUpdatesAtStartup.Checked && !ShutdownTimer.Enabled)
            {
                if (CheckForUpdates.DoCheck(BranchLocation, false))
                {
                    RunUpdaterAndClose();
                }
            }
        }

        /// <summary>
        /// Updates the link label in response to a change in the user name text
        /// </summary>
        private void txtLaunchpadUserName_TextChanged(object sender, EventArgs e)
        {
            // Update the small helper link label so it is obvious what the result is and so it can be clicked
            if (txtLaunchpadUserName.Text == String.Empty)
            {
                linkLabel_LaunchpadUrl.Text = String.Empty;
            }
            else
            {
                string branchShortName = BranchLocation.Substring(BranchLocation.LastIndexOf('\\') + 1);
                linkLabel_LaunchpadUrl.Text = String.Format("lp:/{0}/openpetraorg/{1}", txtLaunchpadUserName.Text, branchShortName);
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == OutputPage)
            {
                if (toolStripStatusLabel.BackColor == Color.Yellow)
                {
                    toolStripStatusLabel.BackColor = _normalLabelColor;

                    if (toolStripStatusLabel.Text == STR_REVIEW_OUTPUT)
                    {
                        toolStripStatusLabel.Text = _activeAsyncBazaarTask == string.Empty ? STR_READY : _activeAsyncBazaarTask;
                    }
                }
            }
        }

        private void cboSourceCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                SetEnabledStates();
            }
        }

        private void cboYAMLHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboYAMLHistory.SelectedIndex == 0)
            {
                return;
            }

            UpdateYAMLHistoryOrder(cboYAMLHistory.SelectedItem.ToString());
            btnPreviewWinform.Enabled = CanEnableYAMLPreview(btnGenerateWinform.Enabled);
        }

        #endregion

        #region Main OP actions initiated from the GUI

        /*****************************************************************************************************************************
         *
         * These are the main OpenPetra Actions initiated from the GUI
         *
         * **************************************************************************************************************************/
        private void linkLabelStartServer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OutputText.ResetOutput();
            TickTimer.Enabled = false;
            NantTask task = new NantTask(NantTask.TaskItem.startPetraServer);

            // Check if we are up to date with the server state - someone may have started it manually at a cmd window
            GetServerState();

            if (_serverIsRunning)
            {
                SetEnabledStates();
            }
            else
            {
                // Ok.  It needs starting
                RunSimpleNantTarget(task);

                txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

                if ((task.NumFailures > 0) || ((task.NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
                {
                    tabControl.SelectedTab = OutputPage;
                    chkVerbose.Checked = true;
                }

                PrepareWarnings();
            }

            TickTimer.Enabled = true;
        }

        private void linkLabelStopServer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OutputText.ResetOutput();
            TickTimer.Enabled = false;
            NantTask task = new NantTask(NantTask.TaskItem.stopPetraServer);

            // Check if we are up to date with the server state - someone may have stopped it manually at a cmd window
            GetServerState();

            if (_serverIsRunning)
            {
                // Ok.  It needs stopping
                RunSimpleNantTarget(task);

                txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

                if ((task.NumFailures > 0) || ((task.NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
                {
                    tabControl.SelectedTab = OutputPage;
                    chkVerbose.Checked = true;
                }

                PrepareWarnings();
            }
            else
            {
                SetEnabledStates();
            }

            TickTimer.Enabled = true;
        }

        private void linkLabelRestartServer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OutputText.ResetOutput();
            TickTimer.Enabled = false;

            // Check if we are up to date with the server state - someone may have stopped it manually at a cmd window
            GetServerState();

            NantTask task = new NantTask(NantTask.TaskItem.stopPetraServer);

            if (_serverIsRunning)
            {
                RunSimpleNantTarget(task);
            }

            if (task.NumFailures == 0)
            {
                task = new NantTask(NantTask.TaskItem.startPetraServer);
                RunSimpleNantTarget(task);
            }

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((task.NumFailures > 0) || ((task.NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
            }

            PrepareWarnings();
            TickTimer.Enabled = true;
        }

        private void btnCodeGeneration_Click(object sender, EventArgs e)
        {
            RunCodeGenerationTask((NantTask.TaskItem)(cboCodeGeneration.SelectedIndex + NantTask.FirstCodeGenItem));
        }

        private void RunCodeGenerationTask(NantTask.TaskItem ACodeGenerationTask)
        {
            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            TickTimer.Enabled = false;
            NantTask task = new NantTask(ACodeGenerationTask);

            if (chkAutoStopServer.Checked && (ACodeGenerationTask == NantTask.TaskItem.generateSolution))
            {
                // This is a case where we need to auto-stop the server first
                GetServerState();

                if (_serverIsRunning)
                {
                    NantTask subTask = new NantTask(NantTask.TaskItem.stopPetraServer);
                    RunSimpleNantTarget(subTask);

                    if (subTask.NumFailures > 0)
                    {
                        DoFinishActions(subTask.NumFailures, subTask.NumWarnings, dtStart);
                        TickTimer.Enabled = true;
                        return;
                    }
                }
            }

            // Now we are ready to perform the original task
            if (ACodeGenerationTask == NantTask.TaskItem.generateSolutionNoCompile)
            {
                Environment.SetEnvironmentVariable("OPDA_StopServer", (_localSettings.DoPreBuildOnIctCommon) ? "1" : null);
                Environment.SetEnvironmentVariable("OPDA_StartServer", (_localSettings.DoPostBuildOnPetraClient) ? "1" : null);
            }

            string branchLocation = PrepareAsyncNantTask(task);

            // Define an async task to run nant and parse the output file
            Task asyncTask = new Task(() => RunSimpleNantTarget(task, branchLocation, false));
            // Define a continuation task that runs in the GUI thread that reports the nant output
            Task reportTask = asyncTask.ContinueWith((x) =>
                {
                    Environment.SetEnvironmentVariable("OPDA_StopServer", null);
                    Environment.SetEnvironmentVariable("OPDA_StartServer", null);

                    DoFinishActions(task.NumFailures, task.NumWarnings, dtStart);
                    FinishAsyncNantTask();
                    TickTimer.Enabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());

            // Start the async task and then quit this method
            asyncTask.Start();
        }

        private void btnCompilation_Click(object sender, EventArgs e)
        {
            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            TickTimer.Enabled = false;
            NantTask task = new NantTask(cboCompilation.Items[cboCompilation.SelectedIndex].ToString());

            if (chkAutoStopServer.Checked && ((task.Item == NantTask.TaskItem.compile) || (task.Item == NantTask.TaskItem.quickCompileServer)))
            {
                // This is a case where we need to auto-stop the server first
                GetServerState();

                if (_serverIsRunning)
                {
                    NantTask subTask = new NantTask(NantTask.TaskItem.stopPetraServer);
                    RunSimpleNantTarget(subTask);

                    if (subTask.NumFailures > 0)
                    {
                        DoFinishActions(subTask.NumFailures, subTask.NumWarnings, dtStart);
                        TickTimer.Enabled = true;
                        return;
                    }
                }
            }

            // Now we are ready to perform the original task
            string branchLocation = PrepareAsyncNantTask(task);

            // Define an async task to run nant and parse the output file
            Task asyncTask = new Task(() => RunSimpleNantTarget(task, branchLocation, false));
            // Define a continuation task that runs in the GUI thread that reports the nant output
            Task reportTask = asyncTask.ContinueWith((x) =>
                {
                    DoFinishActions(task.NumFailures, task.NumWarnings, dtStart);
                    FinishAsyncNantTask();
                    TickTimer.Enabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());

            // Start the async task and then quit this method
            asyncTask.Start();
        }

        private void btnMiscellaneous_Click(object sender, EventArgs e)
        {
            RunMiscellaneousTask((NantTask.TaskItem)(cboMiscellaneous.SelectedIndex + NantTask.FirstMiscItem));
        }

        private void RunMiscellaneousTask(NantTask.TaskItem AMiscellaneousTask)
        {
            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            TickTimer.Enabled = false;
            NantTask task = new NantTask(AMiscellaneousTask);
            string workingFolder = BranchLocation;

            if (AMiscellaneousTask == NantTask.TaskItem.uncrustify)
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "Select a folder to Uncrustify";
                dlg.SelectedPath = BranchLocation;
                dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                dlg.ShowNewFolderButton = false;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                // check the selection is based on teh current branch
                if (!dlg.SelectedPath.StartsWith(BranchLocation, StringComparison.InvariantCultureIgnoreCase))
                {
                    MessageBox.Show("You must choose a folder within the current branch.", Program.APP_TITLE);
                    return;
                }

                // check that the folder contains a .build file
                string[] files = Directory.GetFiles(dlg.SelectedPath, "*.build", SearchOption.TopDirectoryOnly);

                if (files.Length == 0)
                {
                    MessageBox.Show("The selected folder cannot be Uncrustified.  You must choose a folder that contains a BUILD file.",
                        Program.APP_TITLE);
                    return;
                }

                // reset the start time
                dtStart = DateTime.UtcNow;

                // Ready to run - overriding the usual root location with the specified folder
                workingFolder = dlg.SelectedPath;
            }
            else if ((AMiscellaneousTask == NantTask.TaskItem.test) || (AMiscellaneousTask == NantTask.TaskItem.testWithoutDisplay)
                     || (AMiscellaneousTask == NantTask.TaskItem.mainNavigationTests))
            {
                if (!CanRunTestOnCurrentDatabase())
                {
                    return;
                }

                // reset the start time
                dtStart = DateTime.UtcNow;
            }

            // Now we are ready to perform the original task
            PrepareAsyncNantTask(task);

            // Define an async task to run nant and parse the output file
            Task asyncTask = new Task(() => RunSimpleNantTarget(task, workingFolder, false));
            // Define a continuation task that runs in the GUI thread that reports the nant output
            Task reportTask = asyncTask.ContinueWith((x) =>
                {
                    DoFinishActions(task.NumFailures, task.NumWarnings, dtStart);
                    FinishAsyncNantTask();
                    TickTimer.Enabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());

            // Start the async task and then quit this method
            asyncTask.Start();
        }

        private void btnSourceCode_Click(object sender, EventArgs e)
        {
            RunBazaarTask((BazaarTask.TaskItem)cboSourceCode.SelectedIndex + 1);
        }

        private void RunBazaarTask(BazaarTask.TaskItem ABazaarTask)
        {
            BazaarTask task = new BazaarTask(ABazaarTask);

            if ((task.Item != BazaarTask.TaskItem.winexplore) && (txtBazaarPath.Text == String.Empty))
            {
                string msg = "The Assistant cannot find the installed location for Bazaar.  It was expected to be in the Program Files (86) folder.";
                msg += "  You can select the 'Options' tab and manually browse to the file bzrw.exe.  Then try this action again.";
                MessageBox.Show(msg, Program.APP_TITLE);
                return;
            }

            if (task.Item == BazaarTask.TaskItem.qbranch)
            {
                // Special code to handle qbranch because we need two actions for this
                string tryPath = String.Empty;

                if (!RunCreateBazaarBranch(ref tryPath))
                {
                    return;
                }

                if (ValidateBranchPath(ref tryPath))
                {
                    string msg = "The branch was created successfully.  Do you want to set this as the current branch?";

                    if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        GetServerState();

                        if (_serverIsRunning)
                        {
                            RunSimpleNantTarget(new NantTask(NantTask.TaskItem.stopPetraServer));
                        }

                        ChangeBranchLocation(tryPath);

                        DlgNewBranchActions dlgNewBranch = new DlgNewBranchActions(this);
                        dlgNewBranch.Initialise(tryPath, _localSettings, _preferredIDE);
                        dlgNewBranch.ShowDialog();
                    }
                }
                else
                {
                    string msg =
                        "A problem occurred while creating the new branch.  There is no source code in the target location.";
                    MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                // A standard Bazaar or winexplore command is simple...
                string cmd = task.GetBazaarCommand();
                System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo(cmd);
                si.Arguments = task.GetBazaarArgs(BranchLocation, linkLabel_LaunchpadUrl.Text);
                si.UseShellExecute = false;     // OS needs this because we access environment variables
                si.WorkingDirectory = BranchLocation;
                bool waitForExit = (si.Arguments.IndexOf("--ui-mode") > 0);
                string errMsg = "The Assistant failed to launch the specified command.  The system error message was: {0}";

                if (!waitForExit)
                {
                    // Launch the task synchronously and quit immediately
                    try
                    {
                        Process p = Process.Start(si);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(errMsg, ex.Message), Program.APP_TITLE);
                    }
                    return;
                }

                // We want to run asynchronously while we wait for the task to complete
                _activeAsyncBazaarTask = task.StatusText;
                toolStripStatusLabel.Text = _activeAsyncBazaarTask;
                toolStripStatusLabel.BackColor = _normalLabelColor;
                toolStripProgressBar.Visible = true;
                SetEnabledStates();

                // Define an async task to run Bazaar
                Task asyncTask = new Task(() =>
                    {
                        try
                        {
                            Process p = Process.Start(si);
                            p.WaitForExit();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(String.Format(errMsg, ex.Message), Program.APP_TITLE);
                        }
                    });
                // Define a continuation task that runs in the GUI thread and restores the GUI elements
                Task cleanupTask = asyncTask.ContinueWith((x) =>
                    {
                        bool isNantActive = _activeAsyncNantTask != String.Empty;

                        _activeAsyncBazaarTask = String.Empty;
                        toolStripStatusLabel.Text = (isNantActive) ? _activeAsyncNantTask : STR_READY;
                        toolStripProgressBar.Visible = isNantActive;
                        SetEnabledStates();
                    }, TaskScheduler.FromCurrentSynchronizationContext());

                // Start the async task and then quit this method
                asyncTask.Start();
            }
        }

        private void btnDatabase_Click(object sender, EventArgs e)
        {
            RunDatabaseTask((NantTask.TaskItem)(cboDatabase.SelectedIndex + NantTask.FirstDatabaseItem));
        }

        private void RunDatabaseTask(NantTask.TaskItem ADatabaseTask)
        {
            if ((ADatabaseTask == NantTask.TaskItem.resetDatabase) || (ADatabaseTask == NantTask.TaskItem.recreateDatabase))
            {
                if (!CanResetCurrentDatabase())
                {
                    return;
                }
            }

            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            TickTimer.Enabled = false;

            if (chkAutoStopServer.Checked && (ADatabaseTask == NantTask.TaskItem.recreateDatabase))
            {
                // This is a case where we need to auto-stop the server first
                GetServerState();

                if (_serverIsRunning)
                {
                    NantTask subTask = new NantTask(NantTask.TaskItem.stopPetraServer);
                    RunSimpleNantTarget(subTask);

                    if (subTask.NumFailures > 0)
                    {
                        DoFinishActions(subTask.NumFailures, subTask.NumWarnings, dtStart);
                        TickTimer.Enabled = true;
                        return;
                    }
                }
            }

            // Now we are ready to perform the original task
            NantTask task = new NantTask(ADatabaseTask);
            string workingFolder = PrepareAsyncNantTask(task);

            // Define an async task to run nant and parse the output file
            Task asyncTask = new Task(() => RunSimpleNantTarget(task, workingFolder, false));
            // Define a continuation task that runs in the GUI thread that reports the nant output
            Task reportTask = asyncTask.ContinueWith((x) =>
                {
                    DoFinishActions(task.NumFailures, task.NumWarnings, dtStart);
                    FinishAsyncNantTask();
                    TickTimer.Enabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());

            // Start the async task and then quit this method
            asyncTask.Start();
        }

        private void btnStartClient_Click(object sender, EventArgs e)
        {
            OutputText.ResetOutput();
            TickTimer.Enabled = false;
            DateTime dtStart = DateTime.UtcNow;
            NantTask task = new NantTask(NantTask.TaskItem.startPetraClient);
            int NumFailures = 0;
            int NumWarnings = 0;

            if (chkAutoStartServer.Checked)
            {
                // This is a case where we need to auto-start the server first
                GetServerState();

                if (!_serverIsRunning)
                {
                    NantTask subTask = new NantTask(NantTask.TaskItem.startPetraServer);
                    RunSimpleNantTarget(subTask);
                    NumFailures = subTask.NumFailures;
                }
            }

            // Now we are ready to perform the original task
            if (NumFailures == 0)
            {
                RunSimpleNantTarget(task);
                NumFailures = task.NumFailures;
                NumWarnings = task.NumWarnings;
            }

            DoFinishActions(NumFailures, NumWarnings, dtStart);

            TickTimer.Enabled = true;
        }

        private void btnGenerateWinform_Click(object sender, EventArgs e)
        {
            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            TickTimer.Enabled = false;
            int NumFailures = 0;
            int NumWarnings = 0;

            RunGenerateWinform(ref NumFailures, ref NumWarnings);
            SetEnabledStates();

            DoFinishActions(NumFailures, NumWarnings, dtStart);

            TickTimer.Enabled = true;
        }

        private void btnPreviewWinform_Click(object sender, EventArgs e)
        {
            OutputText.ResetOutput();
            TickTimer.Enabled = false;
            int NumFailures = 0;
            int NumWarnings = 0;

            RunPreviewWinform(ref NumFailures, ref NumWarnings);
            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
            }

            PrepareWarnings();
            TickTimer.Enabled = true;
        }

        private void btnRunSequence_Click(object sender, EventArgs e)
        {
            RunSequence(_sequence);
        }

        private void btnRunAltSequence_Click(object sender, EventArgs e)
        {
            RunSequence(_altSequence);
        }

        private void btnBrowseBazaar_Click(object sender, EventArgs e)
        {
            string x86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)", EnvironmentVariableTarget.Process);

            if (x86 == null)
            {
                x86 = Environment.GetEnvironmentVariable("ProgramFiles", EnvironmentVariableTarget.Process);
            }

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Browse for the Bazaar Explorer Application";
            dlg.FileName = "bzrw.exe";
            dlg.Filter = "Applications|*.exe";
            dlg.CheckFileExists = true;

            if (x86 != null)
            {
                dlg.InitialDirectory = x86;
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            txtBazaarPath.Text = dlg.FileName;
            linkLabelBazaar.Enabled = true;
        }

        private void linkLabelBazaar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Launch Bazaar using <Path-to-Bazaar> explorer <branch-location>
            System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo(txtBazaarPath.Text);
            si.Arguments = String.Format("explorer \"{0}\"", BranchLocation);
            si.WorkingDirectory = BranchLocation;

            try
            {
                System.Diagnostics.Process.Start(si);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Assistant failed to launch the Bazaar Explorer.  The system error message was: " + ex.Message, Program.APP_TITLE);
            }
        }

        private void btnAdvancedOptions_Click(object sender, EventArgs e)
        {
            DlgAdvancedOptions dlg = new DlgAdvancedOptions();

            dlg.chkDoPreBuild.Checked = _localSettings.DoPreBuildOnIctCommon;
            dlg.chkDoPostBuild.Checked = _localSettings.DoPostBuildOnPetraClient;

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            _localSettings.DoPreBuildOnIctCommon = dlg.chkDoPreBuild.Checked;
            _localSettings.DoPostBuildOnPetraClient = dlg.chkDoPostBuild.Checked;
        }

        private void linkLabel_LaunchpadUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // We almost know the correct URL for Launchpad
            string url = linkLabel_LaunchpadUrl.Text;

            if (url != String.Empty)
            {
                url = url.Replace("lp:", "https://code.launchpad.net");
                System.Diagnostics.Process.Start(url);
            }
        }

        private void btnCheckForUpdates_Click(object sender, EventArgs e)
        {
            // The Check For Updates button has been clicked
            if (CheckForUpdates.DoCheck(BranchLocation, true))
            {
                RunUpdaterAndClose();
            }
        }

        #region Toolbar Buttons

        private void tbbGenerateSolutionFullCompile_Click(object sender, EventArgs e)
        {
            RunCodeGenerationTask(NantTask.TaskItem.generateSolution);
        }

        private void tbbGenerateSolutionMinCompile_Click(object sender, EventArgs e)
        {
            RunCodeGenerationTask(NantTask.TaskItem.minimalGenerateSolution);
        }

        private void tbbGenerateWinForms_Click(object sender, EventArgs e)
        {
            RunCodeGenerationTask(NantTask.TaskItem.generateWinforms);
        }

        private void tbbGenerateGlue_Click(object sender, EventArgs e)
        {
            RunCodeGenerationTask(NantTask.TaskItem.generateGlue);
        }

        private void tbbUncrustify_Click(object sender, EventArgs e)
        {
            RunMiscellaneousTask(NantTask.TaskItem.uncrustify);
        }

        private void tbbRunAllTests_Click(object sender, EventArgs e)
        {
            RunMiscellaneousTask(NantTask.TaskItem.test);
        }

        private void tbbRunMainNavigationScreensTests_Click(object sender, EventArgs e)
        {
            RunMiscellaneousTask(NantTask.TaskItem.mainNavigationTests);
        }

        private void tbbSourceHistoryLog_ButtonClick(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qlog);
        }

        private void tbbSourceHistoryAllMenuItem_Click(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qlog);
        }

        private void tbbSourceHistoryFileMenuItem_Click(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qlogFile);
        }

        private void tbbShowSourceDifferences_ButtonClick(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qdiff);
        }

        private void tbbShowSourceDifferencesAllMenuItem_Click(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qdiff);
        }

        private void tbbShowSourceDifferencesFileMenuItem_Click(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qdiffFile);
        }

        private void tbbCommitSourceChanges_Click(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qcommit);
        }

        private void tbbShelveSourceChanges_Click(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qshelve);
        }

        private void tbbUnshelveSourceChanges_Click(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qunshelve);
        }

        private void tbbMergeSourceFromTrunk_Click(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qmerge);
        }

        private void tbbCreateNewSourceBranch_Click(object sender, EventArgs e)
        {
            RunBazaarTask(BazaarTask.TaskItem.qbranch);
        }

        private void tbbCreateDatabase_Click(object sender, EventArgs e)
        {
            RunDatabaseTask(NantTask.TaskItem.recreateDatabase);
        }

        private void tbbDatabaseContent_Click(object sender, EventArgs e)
        {
            RunDatabaseTask(NantTask.TaskItem.resetDatabase);
        }

        #endregion

        #endregion

        #region Helper functions

        /******************************************************************************************************************************************
        *
        * Helper functions
        *
        * ****************************************************************************************************************************************/

        // Generic method that runs most tasks.
        // It reads and parses the log file for errors/warnings and returns the number of errors and warnings found.
        // It handles the display of the splash dialog and the text that will end up in the output window
        private void RunSimpleNantTarget(NantTask Task)
        {
            RunSimpleNantTarget(Task, BranchLocation, true);
        }

        // Generic method that runs most tasks.
        // It reads and parses the log file for errors/warnings and returns the number of errors and warnings found.
        // It handles the display of the splash dialog and the text that will end up in the output window
        // Use this override when the method has been called asynchronously.  In that case the splash dialog is not displayed.
        private void RunSimpleNantTarget(NantTask Task, string WorkingFolder, bool ShowSplashScreen)
        {
            // Basic routine that runs a simple target with no parameters
            ProgressDialog dlg = new ProgressDialog();

            dlg.lblStatus.Text = Task.StatusText;

            if (ShowSplashScreen)
            {
                dlg.Show();
            }

            Task.NumFailures = 0;
            Task.NumWarnings = 0;
            OutputText.AppendText(OutputText.OutputStream.Both, String.Format("~~~~~~~~~~~~~~~~ {0} ...\r\n", Task.LogText));
            dlg.Refresh();

            bool bOk;

            switch (Task.Item)
            {
                case NantTask.TaskItem.startPetraServer:
                    bOk = NantExecutor.StartServer(WorkingFolder, chkMinimizeServer.Checked);
                    break;

                case NantTask.TaskItem.stopPetraServer:
                    bOk = NantExecutor.StopServer(WorkingFolder);
                    break;

                case NantTask.TaskItem.runAdminConsole:
                    bOk = NantExecutor.RunServerAdminConsole(WorkingFolder, String.Empty);
                    break;

                case NantTask.TaskItem.refreshCachedTables:
                    bOk = NantExecutor.RunServerAdminConsole(WorkingFolder, "-Command:RefreshAllCachedTables");
                    break;

                default:
                    bOk = NantExecutor.RunGenericNantTarget(WorkingFolder, Task.TargetName);
                    break;
            }

            if (bOk)
            {
                // It ran successfully - let us check the output ...
                OutputText.AddLogFileOutput(WorkingFolder + @"\opda.txt", Task);

                if ((Task.Item == NantTask.TaskItem.startPetraServer) || (Task.Item == NantTask.TaskItem.stopPetraServer))
                {
                    bool taskComplete = true;

                    do
                    {
                        GetServerState();

                        if (Task.Item == NantTask.TaskItem.stopPetraServer)
                        {
                            if (_serverIsRunning)
                            {
                                string msg =
                                    "The Assistant failed to stop the server.  This can happen if it was not the Assistant that started it.  ";
                                msg += "Please stop the server manually by closing the application window in the Taskbar.  Then close this message.";
                                MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                taskComplete = false;
                            }
                            else
                            {
                                taskComplete = true;
                            }
                        }
                    } while (!taskComplete);

                    SetEnabledStates();
                }
            }
            else
            {
                Task.NumFailures++;
            }

            dlg.Close();
        }

        // Generic method to run generateWinform because it is in a different disk location to all the rest and takes additional parameters.
        private void RunGenerateWinform(ref int NumFailures, ref int NumWarnings)
        {
            NantTask task = new NantTask(NantTask.TaskItem.generateWinform);

            ProgressDialog dlg = new ProgressDialog();

            dlg.lblStatus.Text = task.StatusText;
            dlg.Show();

            OutputText.AppendText(OutputText.OutputStream.Both, String.Format("~~~~~~~~~~~~~~~~ {0} ...\r\n", task.LogText));
            dlg.Refresh();

            if (NantExecutor.RunGenerateWinform(BranchLocation, cboYAMLHistory.SelectedItem.ToString()))
            {
                // It ran successfully - let us check the output ...
                OutputText.AddLogFileOutput(BranchLocation + @"\csharp\ICT\Petra\Client\opda.txt", task);
                NumFailures += task.NumFailures;
                NumWarnings += task.NumWarnings;
            }
            else
            {
                NumFailures++;
            }

            if ((NumFailures == 0) && ((NumWarnings == 0) || !chkTreatWarningsAsErrors.Checked) && chkCompileWinform.Checked)
            {
                RunNestedTask(NantTask.TaskItem.quickCompileClient, dlg, ref NumFailures, ref NumWarnings);

                if ((NumFailures == 0) && ((NumWarnings == 0) || !chkTreatWarningsAsErrors.Checked) && chkStartClientAfterGenerateWinform.Checked)
                {
                    GetServerState();

                    if (!_serverIsRunning)
                    {
                        RunNestedTask(NantTask.TaskItem.startPetraServer, dlg, ref NumFailures, ref NumWarnings);
                    }

                    RunNestedTask(NantTask.TaskItem.startPetraClient, dlg, ref NumFailures, ref NumWarnings);
                }
            }

            dlg.Close();
        }

        /// <summary>
        /// Method that will run a task inside the context of another task
        /// </summary>
        /// <param name="NestedTask">Task to run</param>
        /// <param name="SplashDialog">Reference to the splash dialog that the parent task launched</param>
        /// <param name="NumFailures">Ongoing number of failures</param>
        /// <param name="NumWarnings">Ongoing number of errors/warnings</param>
        private void RunNestedTask(NantTask.TaskItem NestedTask, ProgressDialog SplashDialog, ref int NumFailures, ref int NumWarnings)
        {
            NantTask nestedTask = new NantTask(NestedTask);

            OutputText.AppendText(OutputText.OutputStream.Both, String.Format("~~~~~~~~~~~~~~~~ {0} ...\r\n", nestedTask.LogText));
            SplashDialog.lblStatus.Text = nestedTask.StatusText;
            SplashDialog.lblStatus.Refresh();

            if (NestedTask == NantTask.TaskItem.startPetraServer)
            {
                if (NantExecutor.StartServer(BranchLocation, chkMinimizeServer.Checked))
                {
                    // It ran successfully - let us check the output ...
                    OutputText.AddLogFileOutput(BranchLocation + @"\opda.txt", nestedTask);
                    NumFailures += nestedTask.NumFailures;
                    NumWarnings += nestedTask.NumWarnings;
                    GetServerState();
                    SetEnabledStates();
                }
                else
                {
                    NumFailures++;
                }
            }
            else
            {
                if (NantExecutor.RunGenericNantTarget(BranchLocation, nestedTask.TargetName))
                {
                    // It ran successfully - let us check the output ...
                    OutputText.AddLogFileOutput(BranchLocation + @"\opda.txt", nestedTask);
                    NumFailures += nestedTask.NumFailures;
                    NumWarnings += nestedTask.NumWarnings;
                }
                else
                {
                    NumFailures++;
                }
            }
        }

        // Generic method to run previewWinform because it is in a different disk location to all the rest and takes additional parameters.
        private void RunPreviewWinform(ref int NumFailures, ref int NumWarnings)
        {
            NantTask task = new NantTask(NantTask.TaskItem.previewWinform);

            ProgressDialog dlg = new ProgressDialog();

            dlg.lblStatus.Text = task.StatusText;
            dlg.Show();

            OutputText.AppendText(OutputText.OutputStream.Both, String.Format("~~~~~~~~~~~~~~~~ {0} ...\r\n", task.LogText));
            dlg.Refresh();

            if (NantExecutor.RunPreviewWinform(BranchLocation, cboYAMLHistory.SelectedItem.ToString()))
            {
                // It ran successfully - let us check the output ...
                OutputText.AddLogFileOutput(BranchLocation + @"\csharp\ICT\Petra\Client\opda.txt", task);
            }
            else
            {
                NumFailures++;
            }

            dlg.Close();
        }

        // Generic method to run a sequence of tasks
        private void RunSequence(List <NantTask.TaskItem>Sequence)
        {
            if (Sequence.Contains(NantTask.TaskItem.recreateDatabase) || Sequence.Contains(NantTask.TaskItem.resetDatabase))
            {
                if (!CanResetCurrentDatabase())
                {
                    return;
                }
            }

            if (Sequence.Contains(NantTask.TaskItem.test) || Sequence.Contains(NantTask.TaskItem.testClient)
                || Sequence.Contains(NantTask.TaskItem.testWithoutDisplay))
            {
                if (!CanRunTestOnCurrentDatabase())
                {
                    return;
                }
            }

            DateTime dtStart = DateTime.UtcNow;

            OutputText.ResetOutput();
            TickTimer.Enabled = false;
            bool bShowOutputTab = false;

            for (int i = 0; i < Sequence.Count; i++)
            {
                NantTask task = new NantTask(Sequence[i]);
                int NumFailures = 0;
                int NumWarnings = 0;

                switch (task.Item)
                {
                    case NantTask.TaskItem.generateWinform:

                        if (btnGenerateWinform.Enabled)
                        {
                            RunGenerateWinform(ref NumFailures, ref NumWarnings);
                        }
                        else
                        {
                            OutputText.AppendText(OutputText.OutputStream.Both,
                                "\r\n\r\n~~~~~~~~~ Cannot generate Winform: No YAML file specified!\r\n\r\n");
                            NumFailures++;
                        }

                        break;

                    case NantTask.TaskItem.startPetraServer:
                        GetServerState();

                        if (!_serverIsRunning)
                        {
                            RunSimpleNantTarget(task);
                            NumFailures += task.NumFailures;
                            NumWarnings += task.NumWarnings;
                        }
                        else
                        {
                            OutputText.AppendText(OutputText.OutputStream.Both,
                                "\r\n\r\n~~~~~~~~~ Skipping 'start server'.  The server is already running.\r\n\r\n");
                        }

                        break;

                    case NantTask.TaskItem.stopPetraServer:
                        GetServerState();

                        if (_serverIsRunning)
                        {
                            RunSimpleNantTarget(task);
                            NumFailures += task.NumFailures;
                            NumWarnings += task.NumWarnings;
                        }
                        else
                        {
                            OutputText.AppendText(OutputText.OutputStream.Both,
                                "\r\n\r\n~~~~~~~~~ Skipping 'stop server'.  The server is already not running.\r\n\r\n");
                        }

                        break;

                    default:
                        RunSimpleNantTarget(task);
                        NumFailures += task.NumFailures;
                        NumWarnings += task.NumWarnings;
                        break;
                }

                if ((NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
                {
                    bShowOutputTab = true;
                }

                if (NumFailures > 0)
                {
                    break;
                }
            }

            DoFinishActions(bShowOutputTab, dtStart);

            TickTimer.Enabled = true;
        }

        /// <summary>
        /// This method runs all the additional actions requested by the user after creating a new branch
        /// </summary>
        /// <param name="Dlg">Reference to the additional actions dialog</param>
        public void RunAdditionalNewBranchActions(DlgNewBranchActions Dlg)
        {
            OutputText.ResetOutput();
            TickTimer.Enabled = false;

            int numActions = 0;

            bool DoGenerateSolution =
                Dlg.WasExistingBranch ? (_localSettings.EBA_GenerateSolutionOption > 0) : (_localSettings.NBA_GenerateSolutionOption > 0);
            bool DoCreateMyConfigurations =
                Dlg.WasExistingBranch ? _localSettings.EBA_CreateMyConfigurations : _localSettings.NBA_CreateMyConfigurations;
            bool DoInitialiseDatabase = Dlg.WasExistingBranch ? _localSettings.EBA_InitialiseDatabase : _localSettings.NBA_InitialiseDatabase;
            bool DoLaunchIDE = Dlg.WasExistingBranch ? _localSettings.EBA_LaunchIDE : _localSettings.NBA_LaunchIDE;
            bool DoStartClient = Dlg.WasExistingBranch ? _localSettings.EBA_StartClient : _localSettings.NBA_StartClient;

            numActions += (DoGenerateSolution) ? 1 : 0;
            numActions += (DoCreateMyConfigurations) ? 1 : 0;
            numActions += (DoInitialiseDatabase) ? 1 : 0;
            numActions += (DoLaunchIDE) ? 1 : 0;
            numActions += (DoStartClient) ? 1 : 0;

            if (numActions == 0)
            {
                return;
            }

            int maxAction = numActions + 1;
            int curAction = 1;
            string actionMsg;
            NantTask task = null;
            int NumFailures = 0;
            int NumWarnings = 0;
            DateTime dtStart = DateTime.UtcNow;

            if (DoGenerateSolution)
            {
                actionMsg = "Generating the solution ...";
                Dlg.SetProgressAndStatus(curAction, maxAction, actionMsg);
                OutputText.AppendText(OutputText.OutputStream.Verbose, "[opda]: " + actionMsg + Environment.NewLine);

                int generateOption = Dlg.WasExistingBranch ? _localSettings.EBA_GenerateSolutionOption : _localSettings.NBA_GenerateSolutionOption;

                if (generateOption == 1)
                {
                    task = new NantTask(NantTask.TaskItem.generateSolution);
                }
                else
                {
                    task = new NantTask(NantTask.TaskItem.minimalGenerateSolution);
                }

                RunSimpleNantTarget(task, BranchLocation, false);
                NumFailures += task.NumFailures;
                NumWarnings += task.NumWarnings;
                curAction++;
            }

            if ((NumFailures == 0) && DoCreateMyConfigurations)
            {
                actionMsg = "Creating .my configuration files ...";
                Dlg.SetProgressAndStatus(curAction, maxAction, actionMsg);
                OutputText.AppendText(OutputText.OutputStream.Verbose, "[opda]: " + actionMsg + Environment.NewLine);

                if (!CreateMyConfigFiles())
                {
                    NumWarnings++;
                }

                curAction++;
            }

            if ((NumFailures == 0) && DoInitialiseDatabase)
            {
                actionMsg = "Initialising the database configuration ...";
                Dlg.SetProgressAndStatus(curAction, maxAction, actionMsg);
                OutputText.AppendText(OutputText.OutputStream.Verbose, "[opda]: " + actionMsg + Environment.NewLine);

                BuildConfiguration DbCfg = new BuildConfiguration(BranchLocation, _localSettings);
                bool bSuccess;

                if (Dlg.WasExistingBranch)
                {
                    bSuccess = DbCfg.SetConfigAsDefault(_localSettings.EBA_DatabaseConfiguration);
                }
                else
                {
                    bSuccess = DbCfg.SetConfigAsDefault(_localSettings.NBA_DatabaseConfiguration);
                }

                if (!bSuccess)
                {
                    OutputText.AppendText(OutputText.OutputStream.Verbose,
                        "[opda]: " + "Warning: an error occurred while initialising the database configuration." + Environment.NewLine);
                    NumWarnings++;
                }

                task = new NantTask(NantTask.TaskItem.initConfigFiles);
                RunSimpleNantTarget(task, BranchLocation, false);

                NumFailures += task.NumFailures;
                NumWarnings = task.NumWarnings;

                curAction++;
            }

            if ((NumFailures == 0) && DoLaunchIDE)
            {
                actionMsg = "Starting the IDE ...";
                Dlg.SetProgressAndStatus(curAction, maxAction, actionMsg);
                OutputText.AppendText(OutputText.OutputStream.Verbose, "[opda]: " + actionMsg + Environment.NewLine);

                string sln = "OpenPetra.sln";

                int slnID = Dlg.WasExistingBranch ? _localSettings.EBA_IDESolution : _localSettings.NBA_IDESolution;

                if (slnID == (int)TSolution.Client)
                {
                    sln = "OpenPetra.Client.sln";
                }
                else if (slnID == (int)TSolution.Server)
                {
                    sln = "OpenPetra.Server.sln";
                }
                else if (slnID == (int)TSolution.Tools)
                {
                    sln = "OpenPetra.Tools.sln";
                }
                else if (slnID == (int)TSolution.Testing)
                {
                    sln = "OpenPetra.Testing.sln";
                }

                Process p = new Process();
                ProcessStartInfo si = null;

                if (_preferredIDE == TDevEnvironment.SharpDevelop)
                {
                    si = new ProcessStartInfo(Path.Combine(BranchLocation, "delivery", "projects", "sharpdevelop4", sln));
                }
                else if (_preferredIDE == TDevEnvironment.VisualStudio)
                {
                    si = new ProcessStartInfo(Path.Combine(BranchLocation, "delivery", "projects", "vs2010", sln));
                }
                else
                {
                    throw new NotImplementedException("Unexpected IDE enum value");
                }

                p.StartInfo = si;
                p.Start();
                curAction++;
            }

            if ((NumFailures == 0) && DoStartClient)
            {
                actionMsg = "Starting the server and client ...";
                Dlg.SetProgressAndStatus(curAction, maxAction, actionMsg);
                OutputText.AppendText(OutputText.OutputStream.Verbose, "[opda]: " + actionMsg + Environment.NewLine);

                GetServerState();

                if (!_serverIsRunning)
                {
                    task = new NantTask(NantTask.TaskItem.startPetraServer);
                    RunSimpleNantTarget(task, BranchLocation, false);
                    NumFailures += task.NumFailures;
                }

                if (NumFailures == 0)
                {
                    task = new NantTask(NantTask.TaskItem.startPetraClient);
                    RunSimpleNantTarget(task, BranchLocation, false);
                    NumFailures += task.NumFailures;
                }
            }

            DoFinishActions(NumFailures, NumWarnings, dtStart);
            SetBranchDependencies();
            TickTimer.Enabled = true;
        }

        private bool RunInitConfigFiles()
        {
            bool ret = true;
            NantTask task = new NantTask(NantTask.TaskItem.initConfigFiles);

            RunSimpleNantTarget(task);

            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if ((task.NumFailures > 0) || ((task.NumWarnings > 0) && chkTreatWarningsAsErrors.Checked))
            {
                tabControl.SelectedTab = OutputPage;
                chkVerbose.Checked = true;
                ret = false;
            }

            PrepareWarnings();
            return ret;
        }

        private string PrepareAsyncNantTask(NantTask Task)
        {
            _activeAsyncNantTask = Task.StatusText;
            toolStripStatusLabel.Text = _activeAsyncNantTask;
            toolStripStatusLabel.BackColor = _normalLabelColor;
            toolStripProgressBar.Visible = true;
            SetEnabledStates();

            return BranchLocation;
        }

        private void FinishAsyncNantTask()
        {
            _activeAsyncNantTask = string.Empty;
            toolStripProgressBar.Visible = _activeAsyncBazaarTask != string.Empty;
            SetEnabledStates();
        }

        private void DoFinishActions(int NumFailures, int NumWarnings, DateTime DtStart)
        {
            bool bShowOutputTab = (NumFailures > 0) || ((NumWarnings > 0) && chkTreatWarningsAsErrors.Checked);

            DoFinishActions(bShowOutputTab, DtStart);
        }

        private void DoFinishActions(bool ShowOutputTab, DateTime DtStart)
        {
            txtOutput.Text = (chkVerbose.Checked) ? OutputText.VerboseOutput : OutputText.ConciseOutput;

            if (ShowOutputTab)
            {
                if (_activeAsyncNantTask == String.Empty)
                {
                    tabControl.SelectedTab = OutputPage;
                    toolStripStatusLabel.Text = (_activeAsyncBazaarTask == string.Empty) ? STR_READY : _activeAsyncBazaarTask;
                }
                else
                {
                    toolStripStatusLabel.Text = STR_REVIEW_OUTPUT;
                    toolStripStatusLabel.BackColor = Color.Yellow;
                }

                chkVerbose.Checked = true;
            }
            else
            {
                toolStripStatusLabel.Text = (_activeAsyncBazaarTask == string.Empty) ? STR_READY : _activeAsyncBazaarTask;
            }

            PrepareWarnings();

            if ((DateTime.UtcNow - DtStart > TimeSpan.FromSeconds(Convert.ToUInt32(txtFlashAfterSeconds.Text))) && !Focused)
            {
                FlashWindow.Flash(this, 5);
            }
        }

        // Call this method at the end of a task or sequence of tasks to initialise the verbose output warnings handler.
        // It sets up the list of positions where warnings occur, initialises the button states and initialises the text label.
        private void PrepareWarnings()
        {
            _warnings = OutputText.FindWarnings();
            _currentWarning = -1;
            lblWarnings.Text = String.Format("{0} failed, {1} errors/warnings", OutputText.ErrorCount, OutputText.WarningCount);
            SetWarningButtons();

            if (btnNextWarning.Enabled)
            {
                btnNextWarning_Click(null, null);
            }
        }

        private void ValidateBazaarPath()
        {
            if (txtBazaarPath.Text != String.Empty)
            {
                if (File.Exists(txtBazaarPath.Text))
                {
                    // All is good
                    linkLabelBazaar.Enabled = true;
                    return;
                }
                else
                {
                    // Did it get moved??
                    txtBazaarPath.Text = String.Empty;
                }
            }

            if (txtBazaarPath.Text == String.Empty)
            {
                // We will try and find it
                string x86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)", EnvironmentVariableTarget.Process);

                if (x86 == null)
                {
                    x86 = Environment.GetEnvironmentVariable("ProgramFiles", EnvironmentVariableTarget.Process);
                }

                if (x86 == null)
                {
                    return;
                }

                try
                {
                    string[] tryPath = Directory.GetFiles(Path.Combine(x86, "Bazaar"), "bzrw.exe", SearchOption.AllDirectories);

                    if ((tryPath == null) || (tryPath.Length < 1))
                    {
                        return;
                    }

                    txtBazaarPath.Text = tryPath[0];
                    linkLabelBazaar.Enabled = true;
                }
                catch (Exception ex)
                {
                    string msg = "The Assistant could not verify the location of bzrw.exe - the Windows executable for the Bazaar Explorer.  ";
                    msg += String.Format("The Assistant searched the folders beneath '{0}' but the following error was generated: {1}",
                        x86,
                        ex.Message);
                    msg += (Environment.NewLine + Environment.NewLine);
                    msg +=
                        "You should select the Options Tab on the Assistant main window and click the small browse button to manually locate the bzrw.exe file.  ";
                    msg += "This will prevent this message from being displayed again.";
                    MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBazaarPath.Text = String.Empty;
                }
            }
        }

        private bool RunCreateBazaarBranch(ref string LocalFolderLocation)
        {
            // Standard method to create a new branch.  This can be called from the link to change the branch location
            //   or from the Source Code drop down option
            if (LocalFolderLocation == String.Empty)
            {
                // we need to find out where to create the branch on the file system...
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                InitialiseFolderBrowserDialog(dlg);

                DialogResult result = dlg.ShowDialog();

                if (result != DialogResult.OK)
                {
                    return false;
                }

                LocalFolderLocation = dlg.SelectedPath;

                if ((Directory.GetDirectories(LocalFolderLocation).GetLength(0) > 0) || (Directory.GetFiles(LocalFolderLocation).GetLength(0) > 0))
                {
                    MessageBox.Show("The folder that you selected is not empty.  Please choose an empty folder.",
                        Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            BazaarTask task = new BazaarTask(BazaarTask.TaskItem.qbranch);
            string cmd = task.GetBazaarCommand();

            if (txtLaunchpadUserName.Text == String.Empty)
            {
                // We need to know the user's ID on Launchpad
                DlgLaunchpadUserName dlgLaunchpad = new DlgLaunchpadUserName();
                dlgLaunchpad.BranchName = LocalFolderLocation.Substring(LocalFolderLocation.LastIndexOf('\\') + 1);

                if (dlgLaunchpad.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }

                txtLaunchpadUserName.Text = dlgLaunchpad.txtLaunchpadUserName.Text;
            }

            string newLaunchpadUrl = linkLabel_LaunchpadUrl.Text.Substring(0, linkLabel_LaunchpadUrl.Text.LastIndexOf('/') + 1);
            newLaunchpadUrl += LocalFolderLocation.Substring(LocalFolderLocation.LastIndexOf('\\') + 1);

            System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo(cmd);
            si.Arguments = task.GetBazaarArgs(LocalFolderLocation, newLaunchpadUrl);
            si.UseShellExecute = false;

            try
            {
                Process p = Process.Start(si);
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Assistant failed to launch the first qbranch command.  The system error message was: " + ex.Message,
                    Program.APP_TITLE);
                return false;
            }

            // Now we have to do it again with qbranch2
            task = new BazaarTask(BazaarTask.TaskItem.qbranch2);
            si = new System.Diagnostics.ProcessStartInfo(cmd);
            si.Arguments = task.GetBazaarArgs(LocalFolderLocation, newLaunchpadUrl);
            si.UseShellExecute = false;

            try
            {
                Process p = Process.Start(si);
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Assistant failed to launch the second qbranch command.  The system error message was: " + ex.Message,
                    Program.APP_TITLE);
                return false;
            }

            return true;
        }

        private string GetWarningDisplayText()
        {
            if (chkVerbose.Checked)
            {
                return String.Format("{0} failed, {1} errors/warnings : Showing {2} of {3}",
                    OutputText.ErrorCount,
                    OutputText.WarningCount,
                    _currentWarning + 1,
                    _warnings.Count);
            }
            else
            {
                return String.Format("{0} failed, {1} errors/warnings", OutputText.ErrorCount, OutputText.WarningCount);
            }
        }

        private string GetDataFilePath(TPaths PathItem)
        {
            string path = String.Empty;
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            switch (PathItem)
            {
                case TPaths.Settings:
                    path = Path.Combine(appDataPath, @"OM_International\DevelopersAssistant.ini");
                    break;

                case TPaths.ExternalLinks:
                    path = Path.Combine(appDataPath, @"OM_International\OPDAExternalLinks.ini");
                    break;

                default:
                    throw new InvalidEnumArgumentException("");
            }

            return path;
        }

        private void RunUpdaterAndClose()
        {
            // get the path to the Updater application
            string exePathInBranch = Path.Combine(BranchLocation, "delivery/bin/Ict.Tools.DevelopersAssistantUpdater.exe");

            if (!File.Exists(exePathInBranch))
            {
                // Unlikely but I suppose possible
                MessageBox.Show(String.Format("Could not find the Updater executable: {0}",
                        exePathInBranch), Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // My path will be the argument for the Updater - it is me that needs to be updated!!
            string myPath = Application.ExecutablePath;

            ProcessStartInfo si = new ProcessStartInfo();
            si.FileName = exePathInBranch;
            si.Arguments = "\"" + myPath + "\"";
            si.WindowStyle = ProcessWindowStyle.Hidden;

            // Start the Updater application and close me down
            Process p = new Process();
            p.StartInfo = si;

            if (p.Start())
            {
                Close();
            }
            else
            {
                MessageBox.Show(String.Format("Failed to launch the Updater executable: {0}",
                        exePathInBranch), Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool ValidateBranchPath(ref string PathToValidate)
        {
            if ((PathToValidate == String.Empty) || !Directory.Exists(PathToValidate))
            {
                return false;
            }

            bool bValidChoice = true;

            if (!File.Exists(PathToValidate + @"\OpenPetra.Build"))
            {
                PathToValidate = Path.Combine(PathToValidate, "trunk");

                if (!File.Exists(PathToValidate + @"\OpenPetra.Build"))
                {
                    bValidChoice = false;
                }
            }

            return bValidChoice;
        }

        /// <summary>
        /// Populates the branch location history drop down list, placing the new location at the top of the list
        /// </summary>
        /// <param name="NewBranchLocation">New Location</param>
        private void RePopulateBranchComboBox(string NewBranchLocation)
        {
            // The drop down list always starts with the new location as the first item
            List <string>newList = new List <string>();
            newList.Add(NewBranchLocation);

            // Now add all the items from the combo, ignoring the current one and any that no longer exist
            // This will ensure that the items in the list are ordered starting with the most recently used.
            for (int i = 0; i < cboBranchLocation.Items.Count; i++)
            {
                string path = cboBranchLocation.Items[i].ToString();

                if (ValidateBranchPath(ref path) && (path != NewBranchLocation))
                {
                    newList.Add(path);
                }
            }

            // Now we use our working list to populate the comboBox itself
            cboBranchLocation.Items.Clear();

            for (int i = 0; i < newList.Count; i++)
            {
                cboBranchLocation.Items.Add(newList[i]);

                if (i == 0)
                {
                    // This won't trigger a new call to this method because we ignore any index selection less than 1
                    cboBranchLocation.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Initialises the Folder Browser dialog including the initial folder and the title
        /// </summary>
        /// <param name="Dlg"></param>
        private void InitialiseFolderBrowserDialog(FolderBrowserDialog Dlg)
        {
            Dlg.ShowNewFolderButton = true;
            Dlg.Description = "Choose a new location of your working branch";

            // seems you cannot set RootFolder and SelectedPath at the same time
            // FolderBrowserDialog ignores SelectedPath property if RootFolder has been set
            // dlg.RootFolder = Environment.SpecialFolder.MyComputer;

            if (BranchLocation != String.Empty)
            {
                Dlg.SelectedPath = BranchLocation;
            }
            else
            {
                string path = Environment.CurrentDirectory;

                if (path.Replace('\\', '/').EndsWith("/delivery/bin"))
                {
                    path = path.Substring(0, path.Length - "/delivery/bin".Length);
                }

                Dlg.SelectedPath = path;
            }
        }

        /// <summary>
        /// Runs the actions necessary when the branch location changes
        /// </summary>
        /// <param name="NewLocation">Full path to the new location</param>
        private void ChangeBranchLocation(string NewLocation)
        {
            if (chkAutoStopServer.Checked)
            {
                // This is a case where we need to auto-stop the server first
                GetServerState();

                if (_serverIsRunning)
                {
                    RunSimpleNantTarget(new NantTask(NantTask.TaskItem.stopPetraServer));
                }
            }

            // These are our standard steps when initialising a new branch location
            RePopulateBranchComboBox(NewLocation);

            SetBranchDependencies();
            SetEnabledStates();
        }

        private void GetPreferredIDE()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".sln");
            object o = key.GetValue(null);

            if ((o != null) && o.ToString().Contains("VisualStudio"))
            {
                _preferredIDE = TDevEnvironment.VisualStudio;
            }
        }

        private bool CreateMyConfigFiles()
        {
            string pathPrefix = Path.Combine(BranchLocation, "inc", "template", "etc");

            try
            {
                // Delete any .old files
                string file = Path.Combine(pathPrefix, "Client.config.old.my");

                if (File.Exists(file))
                {
                    OutputText.AppendText(OutputText.OutputStream.Verbose, "[opda]: Deleting " + file + Environment.NewLine);
                    File.Delete(file);
                }

                file = Path.Combine(pathPrefix, "Server-postgresql.config.old.my");

                if (File.Exists(file))
                {
                    OutputText.AppendText(OutputText.OutputStream.Verbose, "[opda]: Deleting " + file + Environment.NewLine);
                    File.Delete(file);
                }

                // Rename any .my files that already exist
                file = Path.Combine(pathPrefix, "Client.config.my");

                if (File.Exists(file))
                {
                    OutputText.AppendText(OutputText.OutputStream.Verbose, "[opda]: Renaming " + file + Environment.NewLine);
                    File.Move(file, Path.Combine(pathPrefix, "Client.config.old.my"));
                }

                file = Path.Combine(pathPrefix, "Server-postgresql.config.my");

                if (File.Exists(file))
                {
                    OutputText.AppendText(OutputText.OutputStream.Verbose, "[opda]: Renaming " + file + Environment.NewLine);
                    File.Move(file, Path.Combine(pathPrefix, "Server-postgresql.config.old.my"));
                }

                // Create new .my files
                file = Path.Combine(pathPrefix, "Client.config");
                OutputText.AppendText(OutputText.OutputStream.Verbose, "[opda]: Copying " + file + " to Client.config.my" + Environment.NewLine);
                File.Copy(file, Path.Combine(pathPrefix, "Client.config.my"));
                file = Path.Combine(pathPrefix, "Server-postgresql.config._my");
                OutputText.AppendText(OutputText.OutputStream.Verbose,
                    "[opda]: Copying " + file + " to Server-postgresql.config.my" + Environment.NewLine);
                File.Copy(file, Path.Combine(pathPrefix, "Server-postgresql.config.my"));
            }
            catch (Exception ex)
            {
                OutputText.AppendText(
                    OutputText.OutputStream.Verbose,
                    "[opda]: Warning: The 'Create .my configuration files' action failed.  The message from the file system was: " + ex.Message +
                    Environment.NewLine);
                return false;
            }

            return true;
        }

        private bool CanResetCurrentDatabase()
        {
            BuildConfiguration dbBldConfig = new BuildConfiguration(BranchLocation, _localSettings);
            string dbName = dbBldConfig.CurrentDBName;

            if (String.Compare(dbName, "nantTest", true) != 0)
            {
                // We don't mind nantTest, but any other database name (including demo) gives rise to a warning
                string msg = String.Format("Warning!  Your current database is '{0}'.  " +
                    "If you proceed you will lose all the information in this database." +
                    "\r\nAre you sure that you want to continue?", dbName);

                if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CanRunTestOnCurrentDatabase()
        {
            BuildConfiguration dbBldConfig = new BuildConfiguration(BranchLocation, _localSettings);
            string dbName = dbBldConfig.CurrentDBName;

            if (String.Compare(dbName, "nantTest", true) != 0)
            {
                string msg = String.Format(
                    "You are about to run tests using the '{0}' database.  This is a friendly reminder that all the data in the database will be lost.\r\n\r\n",
                    dbName);
                msg +=
                    "Click OK to continue, or 'Cancel' if you want to select a different database for testing (by moving to the 'database' tab of the Assistant).\r\n\r\n";
                msg += "The best advice is to create a specific database for testing purposes using PgAdmin or equivalent.\r\n\r\n";
                msg += "Tip: if you call your test database 'nantTest', it will be used without displaying this message!";

                if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Cancel)
                {
                    return false;
                }
            }

            return true;
        }

        private void UpdateYAMLHistoryOrder(string Filename)
        {
            List <string>locations = _localSettings.YAMLLocationHistory.Split(',').ToList();

            if (locations.Contains(Filename))
            {
                locations.Remove(Filename);
            }

            locations.Insert(0, Filename);

            while (locations.Count > 10)
            {
                locations.RemoveAt(10);
            }

            cboYAMLHistory.Items.Clear();
            cboYAMLHistory.Text = "";
            cboYAMLHistory.Items.AddRange(locations.ToArray());
            cboYAMLHistory.SelectedIndex = 0;
        }

        private void ShowDbNameInComboText(string DbName)
        {
            for (int i = 0; i < cboDatabase.Items.Count; i++)
            {
                string item = (string)cboDatabase.Items[i];
                int posBrace = item.IndexOf('(');

                if (posBrace == -1)
                {
                    // Not yet got a db in parentheses
                    item += DbName.Length == 0 ? "" : " ";
                    posBrace = item.Length;
                }

                // This is the text up to the parenthesis
                string prefix = item.Substring(0, posBrace);

                if (DbName.Length > 0)
                {
                    // add the db name as suffix
                    cboDatabase.Items[i] = string.Format("{0}({1})", prefix, DbName);
                }
                else
                {
                    cboDatabase.Items[i] = prefix;
                }
            }

            // And do the tooltips for the toolbar buttons
            if (DbName.Length > 0)
            {
                tbbCreateDatabase.ToolTipText = "Create a New Database as " + DbName;
                tbbDatabaseContent.ToolTipText = "Reset the Database Content of " + DbName;
            }
            else
            {
                tbbCreateDatabase.ToolTipText = "Create a New Database";
                tbbDatabaseContent.ToolTipText = "Reset the Database Content";
            }
        }

        private bool CanEnableYAMLPreview(bool CanGenerateWinForm)
        {
            if (CanGenerateWinForm)
            {
                string path = Path.Combine(BranchLocation, Path.Combine(@"csharp\ICT\petra\client", cboYAMLHistory.SelectedItem.ToString()));
                path = path.Replace(".yaml", "-generated.cs");
                return File.Exists(path);
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}