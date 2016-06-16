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
namespace Ict.Tools.DevelopersAssistant
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.TaskPage = new System.Windows.Forms.TabPage();
            this.cboBranchLocation = new System.Windows.Forms.ComboBox();
            this.chkTreatWarningsAsErrors = new System.Windows.Forms.CheckBox();
            this.linkLabelBazaar = new System.Windows.Forms.LinkLabel();
            this.linkLabelBranchLocation = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.grpMultiple = new System.Windows.Forms.GroupBox();
            this.btnRunAltSequence = new System.Windows.Forms.Button();
            this.linkModifyAltSequence = new System.Windows.Forms.LinkLabel();
            this.txtAltSequence = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnRunSequence = new System.Windows.Forms.Button();
            this.linkModifySequence = new System.Windows.Forms.LinkLabel();
            this.txtSequence = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.grpSingle = new System.Windows.Forms.GroupBox();
            this.lblDatabaseName = new System.Windows.Forms.Label();
            this.btnSourceCode = new System.Windows.Forms.Button();
            this.cboSourceCode = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.btnPreviewWinform = new System.Windows.Forms.Button();
            this.chkStartClientAfterGenerateWinform = new System.Windows.Forms.CheckBox();
            this.chkCompileWinform = new System.Windows.Forms.CheckBox();
            this.btnCompilation = new System.Windows.Forms.Button();
            this.btnCodeGeneration = new System.Windows.Forms.Button();
            this.cboCompilation = new System.Windows.Forms.ComboBox();
            this.cboCodeGeneration = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.linkLabelRestartServer = new System.Windows.Forms.LinkLabel();
            this.linkLabelStopServer = new System.Windows.Forms.LinkLabel();
            this.linkLabelStartServer = new System.Windows.Forms.LinkLabel();
            this.linkLabelYamlFile = new System.Windows.Forms.LinkLabel();
            this.btnMiscellaneous = new System.Windows.Forms.Button();
            this.cboMiscellaneous = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnStartClient = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGenerateWinform = new System.Windows.Forms.Button();
            this.cboYAMLHistory = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DatabasePage = new System.Windows.Forms.TabPage();
            this.lblBranchLocation = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDemoteFavouriteBuild = new System.Windows.Forms.Button();
            this.btnPromoteFavouriteBuild = new System.Windows.Forms.Button();
            this.btnEditDbBuildConfig = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btnRemoveDbBuildConfig = new System.Windows.Forms.Button();
            this.btnAddDbBuildConfig = new System.Windows.Forms.Button();
            this.lblDbBuildConfig = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.listDbBuildConfig = new System.Windows.Forms.ListBox();
            this.btnSaveDbBuildConfig = new System.Windows.Forms.Button();
            this.btnDatabase = new System.Windows.Forms.Button();
            this.cboDatabase = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.OutputPage = new System.Windows.Forms.TabPage();
            this.btnNextWarning = new System.Windows.Forms.Button();
            this.btnPrevWarning = new System.Windows.Forms.Button();
            this.lblWarnings = new System.Windows.Forms.Label();
            this.chkVerbose = new System.Windows.Forms.CheckBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.ExternalPage = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.linkSuggestedLinksUpdates = new System.Windows.Forms.LinkLabel();
            this.linkRefreshLinks = new System.Windows.Forms.LinkLabel();
            this.linkEditLinks = new System.Windows.Forms.LinkLabel();
            this.lblExternalWebLink = new System.Windows.Forms.Label();
            this.btnBrowseWeb = new System.Windows.Forms.Button();
            this.lblWebLinkInfo = new System.Windows.Forms.Label();
            this.lstExternalWebLinks = new System.Windows.Forms.ListBox();
            this.OptionsPage = new System.Windows.Forms.TabPage();
            this.chkCheckForUpdatesAtStartup = new System.Windows.Forms.CheckBox();
            this.btnCheckForUpdates = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.linkLabel_LaunchpadUrl = new System.Windows.Forms.LinkLabel();
            this.label20 = new System.Windows.Forms.Label();
            this.txtLaunchpadUserName = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.btnAdvancedOptions = new System.Windows.Forms.Button();
            this.btnBrowseBazaar = new System.Windows.Forms.Button();
            this.txtBazaarPath = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtFlashAfterSeconds = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnResetClientConfig = new System.Windows.Forms.Button();
            this.chkUseAutoLogon = new System.Windows.Forms.CheckBox();
            this.btnUpdateMyClientConfig = new System.Windows.Forms.Button();
            this.txtAutoLogonAction = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtAutoLogonPW = new System.Windows.Forms.TextBox();
            this.lblAutoLogonPW = new System.Windows.Forms.Label();
            this.txtAutoLogonUser = new System.Windows.Forms.TextBox();
            this.lblAutoLogonUser = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkMinimizeServer = new System.Windows.Forms.CheckBox();
            this.chkAutoStopServer = new System.Windows.Forms.CheckBox();
            this.chkAutoStartServer = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ShutdownTimer = new System.Windows.Forms.Timer(this.components);
            this.TickTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.tbbGenerateSolutionFullCompile = new System.Windows.Forms.ToolStripButton();
            this.tbbGenerateSolutionMinCompile = new System.Windows.Forms.ToolStripButton();
            this.tbbGenerateWinForms = new System.Windows.Forms.ToolStripButton();
            this.tbbGenerateGlue = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbUncrustify = new System.Windows.Forms.ToolStripButton();
            this.tbbRunAllTests = new System.Windows.Forms.ToolStripButton();
            this.tbbRunMainNavigationScreensTests = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbSourceHistoryLog = new System.Windows.Forms.ToolStripSplitButton();
            this.tbbSourceHistoryAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbbSourceHistoryFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbbShowSourceDifferences = new System.Windows.Forms.ToolStripSplitButton();
            this.tbbShowSourceDifferencesAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbbShowSourceDifferencesFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbbCommitSourceChanges = new System.Windows.Forms.ToolStripButton();
            this.tbbShelveSourceChanges = new System.Windows.Forms.ToolStripButton();
            this.tbbUnshelveSourceChanges = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbMergeSourceFromTrunk = new System.Windows.Forms.ToolStripButton();
            this.tbbCreateNewSourceBranch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbCreateDatabase = new System.Windows.Forms.ToolStripButton();
            this.tbbDatabaseContent = new System.Windows.Forms.ToolStripButton();
            this.tabControl.SuspendLayout();
            this.TaskPage.SuspendLayout();
            this.grpMultiple.SuspendLayout();
            this.grpSingle.SuspendLayout();
            this.DatabasePage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.OutputPage.SuspendLayout();
            this.ExternalPage.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.OptionsPage.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // tabControl
            //
            this.tabControl.Controls.Add(this.TaskPage);
            this.tabControl.Controls.Add(this.DatabasePage);
            this.tabControl.Controls.Add(this.OutputPage);
            this.tabControl.Controls.Add(this.ExternalPage);
            this.tabControl.Controls.Add(this.OptionsPage);
            this.tabControl.Location = new System.Drawing.Point(12, 44);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(741, 482);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            //
            // TaskPage
            //
            this.TaskPage.Controls.Add(this.cboBranchLocation);
            this.TaskPage.Controls.Add(this.chkTreatWarningsAsErrors);
            this.TaskPage.Controls.Add(this.linkLabelBazaar);
            this.TaskPage.Controls.Add(this.linkLabelBranchLocation);
            this.TaskPage.Controls.Add(this.label1);
            this.TaskPage.Controls.Add(this.grpMultiple);
            this.TaskPage.Controls.Add(this.grpSingle);
            this.TaskPage.Location = new System.Drawing.Point(4, 22);
            this.TaskPage.Name = "TaskPage";
            this.TaskPage.Padding = new System.Windows.Forms.Padding(3);
            this.TaskPage.Size = new System.Drawing.Size(733, 456);
            this.TaskPage.TabIndex = 0;
            this.TaskPage.Text = "Tasks";
            this.TaskPage.UseVisualStyleBackColor = true;
            //
            // cboBranchLocation
            //
            this.cboBranchLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBranchLocation.FormattingEnabled = true;
            this.cboBranchLocation.Location = new System.Drawing.Point(93, 9);
            this.cboBranchLocation.Name = "cboBranchLocation";
            this.cboBranchLocation.Size = new System.Drawing.Size(615, 21);
            this.cboBranchLocation.TabIndex = 1;
            this.cboBranchLocation.SelectedIndexChanged += new System.EventHandler(this.cboBranchLocation_SelectedIndexChanged);
            //
            // chkTreatWarningsAsErrors
            //
            this.chkTreatWarningsAsErrors.AutoSize = true;
            this.chkTreatWarningsAsErrors.Location = new System.Drawing.Point(15, 433);
            this.chkTreatWarningsAsErrors.Name = "chkTreatWarningsAsErrors";
            this.chkTreatWarningsAsErrors.Size = new System.Drawing.Size(196, 17);
            this.chkTreatWarningsAsErrors.TabIndex = 6;
            this.chkTreatWarningsAsErrors.Text = "Treat errors and warnings as failures";
            this.toolTip.SetToolTip(this.chkTreatWarningsAsErrors,
                "Check this to be alerted when warnings or errors occur.  Un-check to be alerted t" +
                "o errors only.");
            this.chkTreatWarningsAsErrors.UseVisualStyleBackColor = true;
            //
            // linkLabelBazaar
            //
            this.linkLabelBazaar.AutoSize = true;
            this.linkLabelBazaar.Enabled = false;
            this.linkLabelBazaar.Location = new System.Drawing.Point(548, 34);
            this.linkLabelBazaar.Name = "linkLabelBazaar";
            this.linkLabelBazaar.Size = new System.Drawing.Size(134, 13);
            this.linkLabelBazaar.TabIndex = 3;
            this.linkLabelBazaar.TabStop = true;
            this.linkLabelBazaar.Text = "Open Bazaar Explorer here";
            this.toolTip.SetToolTip(this.linkLabelBazaar, "Bazaar is the source code control system used by Open Petra developers.");
            this.linkLabelBazaar.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBazaar_LinkClicked);
            //
            // linkLabelBranchLocation
            //
            this.linkLabelBranchLocation.AutoSize = true;
            this.linkLabelBranchLocation.Location = new System.Drawing.Point(90, 34);
            this.linkLabelBranchLocation.Name = "linkLabelBranchLocation";
            this.linkLabelBranchLocation.Size = new System.Drawing.Size(138, 13);
            this.linkLabelBranchLocation.TabIndex = 2;
            this.linkLabelBranchLocation.TabStop = true;
            this.linkLabelBranchLocation.Text = "Change the branch location";
            this.toolTip.SetToolTip(this.linkLabelBranchLocation, "Set the location of you woking branch on the local file system.");
            this.linkLabelBranchLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(
                this.linkLabelBranchLocation_LinkClicked);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Branch location";
            //
            // grpMultiple
            //
            this.grpMultiple.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grpMultiple.AutoSize = true;
            this.grpMultiple.Controls.Add(this.btnRunAltSequence);
            this.grpMultiple.Controls.Add(this.linkModifyAltSequence);
            this.grpMultiple.Controls.Add(this.txtAltSequence);
            this.grpMultiple.Controls.Add(this.label10);
            this.grpMultiple.Controls.Add(this.btnRunSequence);
            this.grpMultiple.Controls.Add(this.linkModifySequence);
            this.grpMultiple.Controls.Add(this.txtSequence);
            this.grpMultiple.Controls.Add(this.label9);
            this.grpMultiple.Location = new System.Drawing.Point(405, 52);
            this.grpMultiple.Name = "grpMultiple";
            this.grpMultiple.Size = new System.Drawing.Size(322, 398);
            this.grpMultiple.TabIndex = 5;
            this.grpMultiple.TabStop = false;
            this.grpMultiple.Text = "Multiple Tasks";
            //
            // btnRunAltSequence
            //
            this.btnRunAltSequence.Location = new System.Drawing.Point(203, 353);
            this.btnRunAltSequence.Name = "btnRunAltSequence";
            this.btnRunAltSequence.Size = new System.Drawing.Size(100, 23);
            this.btnRunAltSequence.TabIndex = 7;
            this.btnRunAltSequence.Text = "Run Sequence";
            this.toolTip.SetToolTip(this.btnRunAltSequence, "Run the alternate sequence listed above.");
            this.btnRunAltSequence.UseVisualStyleBackColor = true;
            this.btnRunAltSequence.Click += new System.EventHandler(this.btnRunAltSequence_Click);
            //
            // linkModifyAltSequence
            //
            this.linkModifyAltSequence.AutoSize = true;
            this.linkModifyAltSequence.Location = new System.Drawing.Point(32, 358);
            this.linkModifyAltSequence.Name = "linkModifyAltSequence";
            this.linkModifyAltSequence.Size = new System.Drawing.Size(88, 13);
            this.linkModifyAltSequence.TabIndex = 6;
            this.linkModifyAltSequence.TabStop = true;
            this.linkModifyAltSequence.Text = "Modify sequence";
            this.toolTip.SetToolTip(this.linkModifyAltSequence, "Open a dialog to modify the alternate sequence above.");
            this.linkModifyAltSequence.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(
                this.linkModifyAltSequence_LinkClicked);
            //
            // txtAltSequence
            //
            this.txtAltSequence.Location = new System.Drawing.Point(17, 232);
            this.txtAltSequence.Multiline = true;
            this.txtAltSequence.Name = "txtAltSequence";
            this.txtAltSequence.ReadOnly = true;
            this.txtAltSequence.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAltSequence.Size = new System.Drawing.Size(286, 115);
            this.txtAltSequence.TabIndex = 5;
            //
            // label10
            //
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 216);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(140, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Alternate Sequence (Alt+F5)";
            //
            // btnRunSequence
            //
            this.btnRunSequence.Location = new System.Drawing.Point(203, 160);
            this.btnRunSequence.Name = "btnRunSequence";
            this.btnRunSequence.Size = new System.Drawing.Size(100, 23);
            this.btnRunSequence.TabIndex = 3;
            this.btnRunSequence.Text = "Run Sequence";
            this.toolTip.SetToolTip(this.btnRunSequence, "Run the standard sequence listed above.");
            this.btnRunSequence.UseVisualStyleBackColor = true;
            this.btnRunSequence.Click += new System.EventHandler(this.btnRunSequence_Click);
            //
            // linkModifySequence
            //
            this.linkModifySequence.AutoSize = true;
            this.linkModifySequence.Location = new System.Drawing.Point(32, 165);
            this.linkModifySequence.Name = "linkModifySequence";
            this.linkModifySequence.Size = new System.Drawing.Size(88, 13);
            this.linkModifySequence.TabIndex = 2;
            this.linkModifySequence.TabStop = true;
            this.linkModifySequence.Text = "Modify sequence";
            this.toolTip.SetToolTip(this.linkModifySequence, "Open a dialog to modify the standard sequence above.");
            this.linkModifySequence.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkModifySequence_LinkClicked);
            //
            // txtSequence
            //
            this.txtSequence.Location = new System.Drawing.Point(17, 39);
            this.txtSequence.Multiline = true;
            this.txtSequence.Name = "txtSequence";
            this.txtSequence.ReadOnly = true;
            this.txtSequence.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSequence.Size = new System.Drawing.Size(286, 115);
            this.txtSequence.TabIndex = 1;
            //
            // label9
            //
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Standard sequence (F5)";
            //
            // grpSingle
            //
            this.grpSingle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grpSingle.Controls.Add(this.lblDatabaseName);
            this.grpSingle.Controls.Add(this.btnSourceCode);
            this.grpSingle.Controls.Add(this.cboSourceCode);
            this.grpSingle.Controls.Add(this.label18);
            this.grpSingle.Controls.Add(this.btnPreviewWinform);
            this.grpSingle.Controls.Add(this.chkStartClientAfterGenerateWinform);
            this.grpSingle.Controls.Add(this.chkCompileWinform);
            this.grpSingle.Controls.Add(this.btnCompilation);
            this.grpSingle.Controls.Add(this.btnCodeGeneration);
            this.grpSingle.Controls.Add(this.cboCompilation);
            this.grpSingle.Controls.Add(this.cboCodeGeneration);
            this.grpSingle.Controls.Add(this.label8);
            this.grpSingle.Controls.Add(this.label5);
            this.grpSingle.Controls.Add(this.linkLabelRestartServer);
            this.grpSingle.Controls.Add(this.linkLabelStopServer);
            this.grpSingle.Controls.Add(this.linkLabelStartServer);
            this.grpSingle.Controls.Add(this.linkLabelYamlFile);
            this.grpSingle.Controls.Add(this.btnMiscellaneous);
            this.grpSingle.Controls.Add(this.cboMiscellaneous);
            this.grpSingle.Controls.Add(this.label6);
            this.grpSingle.Controls.Add(this.btnStartClient);
            this.grpSingle.Controls.Add(this.label4);
            this.grpSingle.Controls.Add(this.label3);
            this.grpSingle.Controls.Add(this.btnGenerateWinform);
            this.grpSingle.Controls.Add(this.cboYAMLHistory);
            this.grpSingle.Controls.Add(this.label2);
            this.grpSingle.Font = new System.Drawing.Font("Microsoft Sans Serif",
                8.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.grpSingle.Location = new System.Drawing.Point(6, 52);
            this.grpSingle.Name = "grpSingle";
            this.grpSingle.Size = new System.Drawing.Size(390, 375);
            this.grpSingle.TabIndex = 4;
            this.grpSingle.TabStop = false;
            this.grpSingle.Text = "Individual Tasks";
            //
            // lblDatabaseName
            //
            this.lblDatabaseName.AutoSize = true;
            this.lblDatabaseName.Location = new System.Drawing.Point(211, 25);
            this.lblDatabaseName.Name = "lblDatabaseName";
            this.lblDatabaseName.Size = new System.Drawing.Size(33, 13);
            this.lblDatabaseName.TabIndex = 1;
            this.lblDatabaseName.Text = "None";
            //
            // btnSourceCode
            //
            this.btnSourceCode.Location = new System.Drawing.Point(352, 305);
            this.btnSourceCode.Name = "btnSourceCode";
            this.btnSourceCode.Size = new System.Drawing.Size(32, 23);
            this.btnSourceCode.TabIndex = 23;
            this.btnSourceCode.Text = "Go";
            this.toolTip.SetToolTip(this.btnSourceCode, "Run the source code task specified in the adjacent list.");
            this.btnSourceCode.UseVisualStyleBackColor = true;
            this.btnSourceCode.Click += new System.EventHandler(this.btnSourceCode_Click);
            //
            // cboSourceCode
            //
            this.cboSourceCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSourceCode.FormattingEnabled = true;
            this.cboSourceCode.Location = new System.Drawing.Point(97, 306);
            this.cboSourceCode.Name = "cboSourceCode";
            this.cboSourceCode.Size = new System.Drawing.Size(249, 21);
            this.cboSourceCode.TabIndex = 22;
            this.cboSourceCode.SelectedIndexChanged += new System.EventHandler(this.cboSourceCode_SelectedIndexChanged);
            //
            // label18
            //
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 309);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 13);
            this.label18.TabIndex = 21;
            this.label18.Text = "Source code";
            //
            // btnPreviewWinform
            //
            this.btnPreviewWinform.Location = new System.Drawing.Point(9, 142);
            this.btnPreviewWinform.Name = "btnPreviewWinform";
            this.btnPreviewWinform.Size = new System.Drawing.Size(58, 23);
            this.btnPreviewWinform.TabIndex = 11;
            this.btnPreviewWinform.Text = "Preview";
            this.toolTip.SetToolTip(this.btnPreviewWinform, "Preview the Windows Form");
            this.btnPreviewWinform.UseVisualStyleBackColor = true;
            this.btnPreviewWinform.Click += new System.EventHandler(this.btnPreviewWinform_Click);
            //
            // chkStartClientAfterGenerateWinform
            //
            this.chkStartClientAfterGenerateWinform.AutoSize = true;
            this.chkStartClientAfterGenerateWinform.Checked = true;
            this.chkStartClientAfterGenerateWinform.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartClientAfterGenerateWinform.Location = new System.Drawing.Point(150, 148);
            this.chkStartClientAfterGenerateWinform.Name = "chkStartClientAfterGenerateWinform";
            this.chkStartClientAfterGenerateWinform.Size = new System.Drawing.Size(156, 17);
            this.chkStartClientAfterGenerateWinform.TabIndex = 9;
            this.chkStartClientAfterGenerateWinform.Text = "Start client after compilation";
            this.toolTip.SetToolTip(this.chkStartClientAfterGenerateWinform, "Start the Open Petra client application on successful compilation.");
            this.chkStartClientAfterGenerateWinform.UseVisualStyleBackColor = true;
            //
            // chkCompileWinform
            //
            this.chkCompileWinform.AutoSize = true;
            this.chkCompileWinform.Checked = true;
            this.chkCompileWinform.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompileWinform.Location = new System.Drawing.Point(150, 125);
            this.chkCompileWinform.Name = "chkCompileWinform";
            this.chkCompileWinform.Size = new System.Drawing.Size(181, 17);
            this.chkCompileWinform.TabIndex = 8;
            this.chkCompileWinform.Text = "Compile after generating the form";
            this.toolTip.SetToolTip(this.chkCompileWinform, "Compile the c# code that was generated from the YAML (assuming no errors occurred" +
                ").");
            this.chkCompileWinform.UseVisualStyleBackColor = true;
            this.chkCompileWinform.CheckedChanged += new System.EventHandler(this.chkCompileWinform_CheckedChanged);
            //
            // btnCompilation
            //
            this.btnCompilation.Location = new System.Drawing.Point(352, 225);
            this.btnCompilation.Name = "btnCompilation";
            this.btnCompilation.Size = new System.Drawing.Size(32, 23);
            this.btnCompilation.TabIndex = 17;
            this.btnCompilation.Text = "Go";
            this.toolTip.SetToolTip(this.btnCompilation, "Run the compilation task specified in the adjacent list.");
            this.btnCompilation.UseVisualStyleBackColor = true;
            this.btnCompilation.Click += new System.EventHandler(this.btnCompilation_Click);
            //
            // btnCodeGeneration
            //
            this.btnCodeGeneration.Location = new System.Drawing.Point(352, 185);
            this.btnCodeGeneration.Name = "btnCodeGeneration";
            this.btnCodeGeneration.Size = new System.Drawing.Size(32, 23);
            this.btnCodeGeneration.TabIndex = 14;
            this.btnCodeGeneration.Text = "Go";
            this.toolTip.SetToolTip(this.btnCodeGeneration, "Run the code generation task specified in the adjacent list.");
            this.btnCodeGeneration.UseVisualStyleBackColor = true;
            this.btnCodeGeneration.Click += new System.EventHandler(this.btnCodeGeneration_Click);
            //
            // cboCompilation
            //
            this.cboCompilation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCompilation.FormattingEnabled = true;
            this.cboCompilation.Location = new System.Drawing.Point(97, 226);
            this.cboCompilation.Name = "cboCompilation";
            this.cboCompilation.Size = new System.Drawing.Size(249, 21);
            this.cboCompilation.TabIndex = 16;
            //
            // cboCodeGeneration
            //
            this.cboCodeGeneration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCodeGeneration.FormattingEnabled = true;
            this.cboCodeGeneration.Location = new System.Drawing.Point(97, 186);
            this.cboCodeGeneration.Name = "cboCodeGeneration";
            this.cboCodeGeneration.Size = new System.Drawing.Size(249, 21);
            this.cboCodeGeneration.TabIndex = 13;
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Compilation";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Code generation";
            //
            // linkLabelRestartServer
            //
            this.linkLabelRestartServer.AutoSize = true;
            this.linkLabelRestartServer.Location = new System.Drawing.Point(246, 48);
            this.linkLabelRestartServer.Name = "linkLabelRestartServer";
            this.linkLabelRestartServer.Size = new System.Drawing.Size(76, 13);
            this.linkLabelRestartServer.TabIndex = 4;
            this.linkLabelRestartServer.TabStop = true;
            this.linkLabelRestartServer.Text = "Re-start server";
            this.toolTip.SetToolTip(this.linkLabelRestartServer, "Restart the Open Petra server (and refresh all tables).");
            this.linkLabelRestartServer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(
                this.linkLabelRestartServer_LinkClicked);
            //
            // linkLabelStopServer
            //
            this.linkLabelStopServer.AutoSize = true;
            this.linkLabelStopServer.Location = new System.Drawing.Point(139, 48);
            this.linkLabelStopServer.Name = "linkLabelStopServer";
            this.linkLabelStopServer.Size = new System.Drawing.Size(61, 13);
            this.linkLabelStopServer.TabIndex = 3;
            this.linkLabelStopServer.TabStop = true;
            this.linkLabelStopServer.Text = "Stop server";
            this.toolTip.SetToolTip(this.linkLabelStopServer, "Stop the Open Petra server.");
            this.linkLabelStopServer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelStopServer_LinkClicked);
            //
            // linkLabelStartServer
            //
            this.linkLabelStartServer.AutoSize = true;
            this.linkLabelStartServer.Location = new System.Drawing.Point(38, 48);
            this.linkLabelStartServer.Name = "linkLabelStartServer";
            this.linkLabelStartServer.Size = new System.Drawing.Size(61, 13);
            this.linkLabelStartServer.TabIndex = 2;
            this.linkLabelStartServer.TabStop = true;
            this.linkLabelStartServer.Text = "Start server";
            this.toolTip.SetToolTip(this.linkLabelStartServer, "Start the Open Petra server application.");
            this.linkLabelStartServer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelStartServer_LinkClicked);
            //
            // linkLabelYamlFile
            //
            this.linkLabelYamlFile.AutoSize = true;
            this.linkLabelYamlFile.Location = new System.Drawing.Point(6, 122);
            this.linkLabelYamlFile.Name = "linkLabelYamlFile";
            this.linkLabelYamlFile.Size = new System.Drawing.Size(104, 13);
            this.linkLabelYamlFile.TabIndex = 7;
            this.linkLabelYamlFile.TabStop = true;
            this.linkLabelYamlFile.Text = "Change the filename";
            this.toolTip.SetToolTip(this.linkLabelYamlFile, "Select the working YAML file from which to generate Windows Forms code");
            this.linkLabelYamlFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelYamlFile_LinkClicked);
            //
            // btnMiscellaneous
            //
            this.btnMiscellaneous.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMiscellaneous.Location = new System.Drawing.Point(352, 265);
            this.btnMiscellaneous.Name = "btnMiscellaneous";
            this.btnMiscellaneous.Size = new System.Drawing.Size(32, 23);
            this.btnMiscellaneous.TabIndex = 20;
            this.btnMiscellaneous.Text = "Go";
            this.toolTip.SetToolTip(this.btnMiscellaneous, "Run the miscellaneous task specified in the adjacent list.");
            this.btnMiscellaneous.UseVisualStyleBackColor = true;
            this.btnMiscellaneous.Click += new System.EventHandler(this.btnMiscellaneous_Click);
            //
            // cboMiscellaneous
            //
            this.cboMiscellaneous.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.cboMiscellaneous.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMiscellaneous.FormattingEnabled = true;
            this.cboMiscellaneous.Location = new System.Drawing.Point(97, 266);
            this.cboMiscellaneous.Name = "cboMiscellaneous";
            this.cboMiscellaneous.Size = new System.Drawing.Size(249, 21);
            this.cboMiscellaneous.TabIndex = 19;
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 269);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Miscellaneous";
            //
            // btnStartClient
            //
            this.btnStartClient.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartClient.Location = new System.Drawing.Point(352, 342);
            this.btnStartClient.Name = "btnStartClient";
            this.btnStartClient.Size = new System.Drawing.Size(32, 23);
            this.btnStartClient.TabIndex = 25;
            this.btnStartClient.Text = "Go";
            this.toolTip.SetToolTip(this.btnStartClient, "Start the Open Petra client application.");
            this.btnStartClient.UseVisualStyleBackColor = true;
            this.btnStartClient.Click += new System.EventHandler(this.btnStartClient_Click);
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 347);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Start Open Petra Client";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif",
                8.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(6, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Petra Server    - connecting to database:";
            //
            // btnGenerateWinform
            //
            this.btnGenerateWinform.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateWinform.Location = new System.Drawing.Point(352, 142);
            this.btnGenerateWinform.Name = "btnGenerateWinform";
            this.btnGenerateWinform.Size = new System.Drawing.Size(32, 23);
            this.btnGenerateWinform.TabIndex = 10;
            this.btnGenerateWinform.Text = "Go";
            this.toolTip.SetToolTip(this.btnGenerateWinform, "Generate the Windows form code using the specified YAML file as the source.");
            this.btnGenerateWinform.UseVisualStyleBackColor = true;
            this.btnGenerateWinform.Click += new System.EventHandler(this.btnGenerateWinform_Click);
            //
            // cboYAMLHistory
            //
            this.cboYAMLHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYAMLHistory.FormattingEnabled = true;
            this.cboYAMLHistory.Location = new System.Drawing.Point(7, 99);
            this.cboYAMLHistory.Name = "cboYAMLHistory";
            this.cboYAMLHistory.Size = new System.Drawing.Size(377, 21);
            this.cboYAMLHistory.TabIndex = 26;
            this.cboYAMLHistory.SelectedIndexChanged += new System.EventHandler(this.cboYAMLHistory_SelectedIndexChanged);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Generate a Windows Form from YAML";
            //
            // DatabasePage
            //
            this.DatabasePage.Controls.Add(this.lblBranchLocation);
            this.DatabasePage.Controls.Add(this.label16);
            this.DatabasePage.Controls.Add(this.groupBox2);
            this.DatabasePage.Controls.Add(this.btnDatabase);
            this.DatabasePage.Controls.Add(this.cboDatabase);
            this.DatabasePage.Controls.Add(this.label7);
            this.DatabasePage.Location = new System.Drawing.Point(4, 22);
            this.DatabasePage.Name = "DatabasePage";
            this.DatabasePage.Size = new System.Drawing.Size(733, 456);
            this.DatabasePage.TabIndex = 3;
            this.DatabasePage.Text = "Database";
            this.DatabasePage.UseVisualStyleBackColor = true;
            //
            // lblBranchLocation
            //
            this.lblBranchLocation.AutoSize = true;
            this.lblBranchLocation.Location = new System.Drawing.Point(129, 20);
            this.lblBranchLocation.Name = "lblBranchLocation";
            this.lblBranchLocation.Size = new System.Drawing.Size(41, 13);
            this.lblBranchLocation.TabIndex = 1;
            this.lblBranchLocation.Text = "label13";
            //
            // label16
            //
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(29, 20);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(94, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "Branch Location:  ";
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.btnDemoteFavouriteBuild);
            this.groupBox2.Controls.Add(this.btnPromoteFavouriteBuild);
            this.groupBox2.Controls.Add(this.btnEditDbBuildConfig);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.btnRemoveDbBuildConfig);
            this.groupBox2.Controls.Add(this.btnAddDbBuildConfig);
            this.groupBox2.Controls.Add(this.lblDbBuildConfig);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.listDbBuildConfig);
            this.groupBox2.Controls.Add(this.btnSaveDbBuildConfig);
            this.groupBox2.Location = new System.Drawing.Point(3, 49);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(727, 288);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Build Configuration";
            //
            // btnDemoteFavouriteBuild
            //
            this.btnDemoteFavouriteBuild.Location = new System.Drawing.Point(345, 249);
            this.btnDemoteFavouriteBuild.Name = "btnDemoteFavouriteBuild";
            this.btnDemoteFavouriteBuild.Size = new System.Drawing.Size(59, 23);
            this.btnDemoteFavouriteBuild.TabIndex = 8;
            this.btnDemoteFavouriteBuild.Text = "Demote";
            this.toolTip.SetToolTip(this.btnDemoteFavouriteBuild, "Move the highlighted row down the list");
            this.btnDemoteFavouriteBuild.UseVisualStyleBackColor = true;
            this.btnDemoteFavouriteBuild.Click += new System.EventHandler(this.btnDemoteFavouriteBuild_Click);
            //
            // btnPromoteFavouriteBuild
            //
            this.btnPromoteFavouriteBuild.Location = new System.Drawing.Point(279, 249);
            this.btnPromoteFavouriteBuild.Name = "btnPromoteFavouriteBuild";
            this.btnPromoteFavouriteBuild.Size = new System.Drawing.Size(59, 23);
            this.btnPromoteFavouriteBuild.TabIndex = 7;
            this.btnPromoteFavouriteBuild.Text = "Promote";
            this.toolTip.SetToolTip(this.btnPromoteFavouriteBuild, "Move the highlighted row up the list");
            this.btnPromoteFavouriteBuild.UseVisualStyleBackColor = true;
            this.btnPromoteFavouriteBuild.Click += new System.EventHandler(this.btnPromoteFavouriteBuild_Click);
            //
            // btnEditDbBuildConfig
            //
            this.btnEditDbBuildConfig.Location = new System.Drawing.Point(87, 249);
            this.btnEditDbBuildConfig.Name = "btnEditDbBuildConfig";
            this.btnEditDbBuildConfig.Size = new System.Drawing.Size(75, 23);
            this.btnEditDbBuildConfig.TabIndex = 5;
            this.btnEditDbBuildConfig.Text = "Edit";
            this.toolTip.SetToolTip(this.btnEditDbBuildConfig, "Edit the highlighted database build configuration.");
            this.btnEditDbBuildConfig.UseVisualStyleBackColor = true;
            this.btnEditDbBuildConfig.Click += new System.EventHandler(this.btnEditDbBuildConfig_Click);
            //
            // label12
            //
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 83);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(159, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "My favourite build configurations";
            //
            // btnRemoveDbBuildConfig
            //
            this.btnRemoveDbBuildConfig.Location = new System.Drawing.Point(168, 249);
            this.btnRemoveDbBuildConfig.Name = "btnRemoveDbBuildConfig";
            this.btnRemoveDbBuildConfig.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveDbBuildConfig.TabIndex = 6;
            this.btnRemoveDbBuildConfig.Text = "Remove";
            this.toolTip.SetToolTip(this.btnRemoveDbBuildConfig, "Remove the highlighted database build configuration.");
            this.btnRemoveDbBuildConfig.UseVisualStyleBackColor = true;
            this.btnRemoveDbBuildConfig.Click += new System.EventHandler(this.btnRemoveDbBuildConfig_Click);
            //
            // btnAddDbBuildConfig
            //
            this.btnAddDbBuildConfig.Location = new System.Drawing.Point(6, 249);
            this.btnAddDbBuildConfig.Name = "btnAddDbBuildConfig";
            this.btnAddDbBuildConfig.Size = new System.Drawing.Size(75, 23);
            this.btnAddDbBuildConfig.TabIndex = 4;
            this.btnAddDbBuildConfig.Text = "Add";
            this.toolTip.SetToolTip(this.btnAddDbBuildConfig, "Add a new database build configuration");
            this.btnAddDbBuildConfig.UseVisualStyleBackColor = true;
            this.btnAddDbBuildConfig.Click += new System.EventHandler(this.btnAddDbBuildConfig_Click);
            //
            // lblDbBuildConfig
            //
            this.lblDbBuildConfig.Location = new System.Drawing.Point(140, 27);
            this.lblDbBuildConfig.Name = "lblDbBuildConfig";
            this.lblDbBuildConfig.Size = new System.Drawing.Size(581, 36);
            this.lblDbBuildConfig.TabIndex = 1;
            this.lblDbBuildConfig.Text = "label12";
            //
            // label11
            //
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(26, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Current configuration:";
            //
            // listDbBuildConfig
            //
            this.listDbBuildConfig.FormattingEnabled = true;
            this.listDbBuildConfig.Location = new System.Drawing.Point(6, 99);
            this.listDbBuildConfig.Name = "listDbBuildConfig";
            this.listDbBuildConfig.Size = new System.Drawing.Size(715, 147);
            this.listDbBuildConfig.TabIndex = 3;
            this.listDbBuildConfig.SelectedIndexChanged += new System.EventHandler(this.listDbBuildConfig_SelectedIndexChanged);
            this.listDbBuildConfig.DoubleClick += new System.EventHandler(this.listDbBuildConfig_DoubleClick);
            //
            // btnSaveDbBuildConfig
            //
            this.btnSaveDbBuildConfig.Location = new System.Drawing.Point(506, 249);
            this.btnSaveDbBuildConfig.Name = "btnSaveDbBuildConfig";
            this.btnSaveDbBuildConfig.Size = new System.Drawing.Size(215, 23);
            this.btnSaveDbBuildConfig.TabIndex = 9;
            this.btnSaveDbBuildConfig.Text = "Save As Current Build Configuration";
            this.toolTip.SetToolTip(this.btnSaveDbBuildConfig, "Save the highlighted database build configuration as the default for this compute" +
                "r.");
            this.btnSaveDbBuildConfig.UseVisualStyleBackColor = true;
            this.btnSaveDbBuildConfig.Click += new System.EventHandler(this.btnSaveDbBuildConfig_Click);
            //
            // btnDatabase
            //
            this.btnDatabase.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDatabase.Location = new System.Drawing.Point(523, 364);
            this.btnDatabase.Name = "btnDatabase";
            this.btnDatabase.Size = new System.Drawing.Size(32, 23);
            this.btnDatabase.TabIndex = 5;
            this.btnDatabase.Text = "Go";
            this.toolTip.SetToolTip(this.btnDatabase, "Run the database task specified in the adjacent list.");
            this.btnDatabase.UseVisualStyleBackColor = true;
            this.btnDatabase.Click += new System.EventHandler(this.btnDatabase_Click);
            //
            // cboDatabase
            //
            this.cboDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDatabase.FormattingEnabled = true;
            this.cboDatabase.Location = new System.Drawing.Point(258, 365);
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.Size = new System.Drawing.Size(259, 21);
            this.cboDatabase.TabIndex = 4;
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(168, 369);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Database Tasks";
            //
            // OutputPage
            //
            this.OutputPage.Controls.Add(this.btnNextWarning);
            this.OutputPage.Controls.Add(this.btnPrevWarning);
            this.OutputPage.Controls.Add(this.lblWarnings);
            this.OutputPage.Controls.Add(this.chkVerbose);
            this.OutputPage.Controls.Add(this.txtOutput);
            this.OutputPage.Location = new System.Drawing.Point(4, 22);
            this.OutputPage.Name = "OutputPage";
            this.OutputPage.Padding = new System.Windows.Forms.Padding(3);
            this.OutputPage.Size = new System.Drawing.Size(733, 456);
            this.OutputPage.TabIndex = 1;
            this.OutputPage.Text = "Output";
            this.OutputPage.UseVisualStyleBackColor = true;
            //
            // btnNextWarning
            //
            this.btnNextWarning.Enabled = false;
            this.btnNextWarning.Location = new System.Drawing.Point(677, 9);
            this.btnNextWarning.Name = "btnNextWarning";
            this.btnNextWarning.Size = new System.Drawing.Size(50, 23);
            this.btnNextWarning.TabIndex = 3;
            this.btnNextWarning.Text = "Next";
            this.btnNextWarning.UseVisualStyleBackColor = true;
            this.btnNextWarning.Click += new System.EventHandler(this.btnNextWarning_Click);
            //
            // btnPrevWarning
            //
            this.btnPrevWarning.Enabled = false;
            this.btnPrevWarning.Location = new System.Drawing.Point(621, 9);
            this.btnPrevWarning.Name = "btnPrevWarning";
            this.btnPrevWarning.Size = new System.Drawing.Size(50, 23);
            this.btnPrevWarning.TabIndex = 2;
            this.btnPrevWarning.Text = "Prev";
            this.btnPrevWarning.UseVisualStyleBackColor = true;
            this.btnPrevWarning.Click += new System.EventHandler(this.btnPrevWarning_Click);
            //
            // lblWarnings
            //
            this.lblWarnings.Location = new System.Drawing.Point(344, 14);
            this.lblWarnings.Name = "lblWarnings";
            this.lblWarnings.Size = new System.Drawing.Size(271, 16);
            this.lblWarnings.TabIndex = 1;
            this.lblWarnings.Text = "Warnings/Errors";
            this.lblWarnings.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkVerbose
            //
            this.chkVerbose.AutoSize = true;
            this.chkVerbose.Location = new System.Drawing.Point(3, 13);
            this.chkVerbose.Name = "chkVerbose";
            this.chkVerbose.Size = new System.Drawing.Size(127, 17);
            this.chkVerbose.TabIndex = 0;
            this.chkVerbose.Text = "Show verbose output";
            this.chkVerbose.UseVisualStyleBackColor = true;
            this.chkVerbose.CheckedChanged += new System.EventHandler(this.chkVerbose_CheckedChanged);
            //
            // txtOutput
            //
            this.txtOutput.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(6, 36);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(721, 414);
            this.txtOutput.TabIndex = 4;
            //
            // ExternalPage
            //
            this.ExternalPage.Controls.Add(this.groupBox5);
            this.ExternalPage.Location = new System.Drawing.Point(4, 22);
            this.ExternalPage.Name = "ExternalPage";
            this.ExternalPage.Padding = new System.Windows.Forms.Padding(3);
            this.ExternalPage.Size = new System.Drawing.Size(733, 456);
            this.ExternalPage.TabIndex = 4;
            this.ExternalPage.Text = "External";
            this.ExternalPage.UseVisualStyleBackColor = true;
            //
            // groupBox5
            //
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.linkSuggestedLinksUpdates);
            this.groupBox5.Controls.Add(this.linkRefreshLinks);
            this.groupBox5.Controls.Add(this.linkEditLinks);
            this.groupBox5.Controls.Add(this.lblExternalWebLink);
            this.groupBox5.Controls.Add(this.btnBrowseWeb);
            this.groupBox5.Controls.Add(this.lblWebLinkInfo);
            this.groupBox5.Controls.Add(this.lstExternalWebLinks);
            this.groupBox5.Location = new System.Drawing.Point(27, 21);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(670, 318);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Useful Web Links";
            //
            // label21
            //
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(251, 209);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(253, 13);
            this.label21.TabIndex = 4;
            this.label21.Text = "Click this link to check that your links are up to date.";
            //
            // linkSuggestedLinksUpdates
            //
            this.linkSuggestedLinksUpdates.AutoSize = true;
            this.linkSuggestedLinksUpdates.Location = new System.Drawing.Point(254, 225);
            this.linkSuggestedLinksUpdates.Name = "linkSuggestedLinksUpdates";
            this.linkSuggestedLinksUpdates.Size = new System.Drawing.Size(127, 13);
            this.linkSuggestedLinksUpdates.TabIndex = 5;
            this.linkSuggestedLinksUpdates.TabStop = true;
            this.linkSuggestedLinksUpdates.Text = "View Suggested Updates";
            this.linkSuggestedLinksUpdates.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(
                this.linkSuggestedLinksUpdates_LinkClicked);
            //
            // linkRefreshLinks
            //
            this.linkRefreshLinks.AutoSize = true;
            this.linkRefreshLinks.Location = new System.Drawing.Point(140, 286);
            this.linkRefreshLinks.Name = "linkRefreshLinks";
            this.linkRefreshLinks.Size = new System.Drawing.Size(63, 13);
            this.linkRefreshLinks.TabIndex = 7;
            this.linkRefreshLinks.TabStop = true;
            this.linkRefreshLinks.Text = "Refresh List";
            this.linkRefreshLinks.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRefreshLinks_LinkClicked);
            //
            // linkEditLinks
            //
            this.linkEditLinks.AutoSize = true;
            this.linkEditLinks.Location = new System.Drawing.Point(7, 286);
            this.linkEditLinks.Name = "linkEditLinks";
            this.linkEditLinks.Size = new System.Drawing.Size(44, 13);
            this.linkEditLinks.TabIndex = 6;
            this.linkEditLinks.TabStop = true;
            this.linkEditLinks.Text = "Edit List";
            this.linkEditLinks.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkEditLinks_LinkClicked);
            //
            // lblExternalWebLink
            //
            this.lblExternalWebLink.AutoSize = true;
            this.lblExternalWebLink.Location = new System.Drawing.Point(251, 138);
            this.lblExternalWebLink.Name = "lblExternalWebLink";
            this.lblExternalWebLink.Size = new System.Drawing.Size(38, 13);
            this.lblExternalWebLink.TabIndex = 2;
            this.lblExternalWebLink.Text = "http://";
            //
            // btnBrowseWeb
            //
            this.btnBrowseWeb.Location = new System.Drawing.Point(589, 165);
            this.btnBrowseWeb.Name = "btnBrowseWeb";
            this.btnBrowseWeb.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseWeb.TabIndex = 3;
            this.btnBrowseWeb.Text = "Browse";
            this.btnBrowseWeb.UseVisualStyleBackColor = true;
            this.btnBrowseWeb.Click += new System.EventHandler(this.btnBrowseWeb_Click);
            //
            // lblWebLinkInfo
            //
            this.lblWebLinkInfo.Location = new System.Drawing.Point(251, 38);
            this.lblWebLinkInfo.Name = "lblWebLinkInfo";
            this.lblWebLinkInfo.Size = new System.Drawing.Size(413, 87);
            this.lblWebLinkInfo.TabIndex = 1;
            this.lblWebLinkInfo.Text = "More Info";
            //
            // lstExternalWebLinks
            //
            this.lstExternalWebLinks.FormattingEnabled = true;
            this.lstExternalWebLinks.Location = new System.Drawing.Point(6, 28);
            this.lstExternalWebLinks.Name = "lstExternalWebLinks";
            this.lstExternalWebLinks.Size = new System.Drawing.Size(239, 251);
            this.lstExternalWebLinks.TabIndex = 0;
            this.lstExternalWebLinks.SelectedIndexChanged += new System.EventHandler(this.lstExternalWebLinks_SelectedIndexChanged);
            this.lstExternalWebLinks.DoubleClick += new System.EventHandler(this.lstExternalWebLinks_DoubleClick);
            //
            // OptionsPage
            //
            this.OptionsPage.Controls.Add(this.chkCheckForUpdatesAtStartup);
            this.OptionsPage.Controls.Add(this.btnCheckForUpdates);
            this.OptionsPage.Controls.Add(this.groupBox4);
            this.OptionsPage.Controls.Add(this.groupBox3);
            this.OptionsPage.Controls.Add(this.lblVersion);
            this.OptionsPage.Controls.Add(this.groupBox1);
            this.OptionsPage.Location = new System.Drawing.Point(4, 22);
            this.OptionsPage.Name = "OptionsPage";
            this.OptionsPage.Size = new System.Drawing.Size(733, 456);
            this.OptionsPage.TabIndex = 2;
            this.OptionsPage.Text = "Options";
            this.OptionsPage.UseVisualStyleBackColor = true;
            //
            // chkCheckForUpdatesAtStartup
            //
            this.chkCheckForUpdatesAtStartup.AutoSize = true;
            this.chkCheckForUpdatesAtStartup.Checked = true;
            this.chkCheckForUpdatesAtStartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCheckForUpdatesAtStartup.Location = new System.Drawing.Point(344, 431);
            this.chkCheckForUpdatesAtStartup.Name = "chkCheckForUpdatesAtStartup";
            this.chkCheckForUpdatesAtStartup.Size = new System.Drawing.Size(283, 17);
            this.chkCheckForUpdatesAtStartup.TabIndex = 5;
            this.chkCheckForUpdatesAtStartup.Text = "Always check for updates when the Assistant starts up";
            this.chkCheckForUpdatesAtStartup.UseVisualStyleBackColor = true;
            //
            // btnCheckForUpdates
            //
            this.btnCheckForUpdates.Location = new System.Drawing.Point(185, 427);
            this.btnCheckForUpdates.Name = "btnCheckForUpdates";
            this.btnCheckForUpdates.Size = new System.Drawing.Size(118, 23);
            this.btnCheckForUpdates.TabIndex = 4;
            this.btnCheckForUpdates.Text = "Check for Updates";
            this.btnCheckForUpdates.UseVisualStyleBackColor = true;
            this.btnCheckForUpdates.Click += new System.EventHandler(this.btnCheckForUpdates_Click);
            //
            // groupBox4
            //
            this.groupBox4.Controls.Add(this.linkLabel_LaunchpadUrl);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.txtLaunchpadUserName);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.btnAdvancedOptions);
            this.groupBox4.Controls.Add(this.btnBrowseBazaar);
            this.groupBox4.Controls.Add(this.txtBazaarPath);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.txtFlashAfterSeconds);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Location = new System.Drawing.Point(21, 296);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(691, 124);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Miscellaneous Options";
            //
            // linkLabel_LaunchpadUrl
            //
            this.linkLabel_LaunchpadUrl.AutoSize = true;
            this.linkLabel_LaunchpadUrl.Location = new System.Drawing.Point(172, 96);
            this.linkLabel_LaunchpadUrl.Name = "linkLabel_LaunchpadUrl";
            this.linkLabel_LaunchpadUrl.Size = new System.Drawing.Size(55, 13);
            this.linkLabel_LaunchpadUrl.TabIndex = 9;
            this.linkLabel_LaunchpadUrl.TabStop = true;
            this.linkLabel_LaunchpadUrl.Text = "linkLabel1";
            this.linkLabel_LaunchpadUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(
                this.linkLabel_LaunchpadUrl_LinkClicked);
            //
            // label20
            //
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(85, 96);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(81, 13);
            this.label20.TabIndex = 8;
            this.label20.Text = "which points to:";
            //
            // txtLaunchpadUserName
            //
            this.txtLaunchpadUserName.Location = new System.Drawing.Point(172, 73);
            this.txtLaunchpadUserName.Name = "txtLaunchpadUserName";
            this.txtLaunchpadUserName.Size = new System.Drawing.Size(123, 20);
            this.txtLaunchpadUserName.TabIndex = 7;
            this.txtLaunchpadUserName.Text = "Unknown";
            this.txtLaunchpadUserName.TextChanged += new System.EventHandler(this.txtLaunchpadUserName_TextChanged);
            //
            // label19
            //
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(26, 76);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(140, 13);
            this.label19.TabIndex = 6;
            this.label19.Text = "My Launchpad user name is";
            //
            // btnAdvancedOptions
            //
            this.btnAdvancedOptions.Location = new System.Drawing.Point(610, 95);
            this.btnAdvancedOptions.Name = "btnAdvancedOptions";
            this.btnAdvancedOptions.Size = new System.Drawing.Size(75, 23);
            this.btnAdvancedOptions.TabIndex = 10;
            this.btnAdvancedOptions.Text = "Advanced";
            this.toolTip.SetToolTip(this.btnAdvancedOptions, "Click to set more advanced options.");
            this.btnAdvancedOptions.UseVisualStyleBackColor = true;
            this.btnAdvancedOptions.Click += new System.EventHandler(this.btnAdvancedOptions_Click);
            //
            // btnBrowseBazaar
            //
            this.btnBrowseBazaar.Location = new System.Drawing.Point(659, 42);
            this.btnBrowseBazaar.Name = "btnBrowseBazaar";
            this.btnBrowseBazaar.Size = new System.Drawing.Size(26, 23);
            this.btnBrowseBazaar.TabIndex = 5;
            this.btnBrowseBazaar.Text = "...";
            this.btnBrowseBazaar.UseVisualStyleBackColor = true;
            this.btnBrowseBazaar.Click += new System.EventHandler(this.btnBrowseBazaar_Click);
            //
            // txtBazaarPath
            //
            this.txtBazaarPath.Enabled = false;
            this.txtBazaarPath.Location = new System.Drawing.Point(135, 44);
            this.txtBazaarPath.Name = "txtBazaarPath";
            this.txtBazaarPath.Size = new System.Drawing.Size(518, 20);
            this.txtBazaarPath.TabIndex = 4;
            //
            // label17
            //
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(26, 47);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(103, 13);
            this.label17.TabIndex = 3;
            this.label17.Text = "Bazaar is installed at";
            //
            // label14
            //
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(299, 21);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(297, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "seconds and I am working on a different Windows application";
            //
            // txtFlashAfterSeconds
            //
            this.txtFlashAfterSeconds.Location = new System.Drawing.Point(264, 18);
            this.txtFlashAfterSeconds.Name = "txtFlashAfterSeconds";
            this.txtFlashAfterSeconds.Size = new System.Drawing.Size(29, 20);
            this.txtFlashAfterSeconds.TabIndex = 1;
            this.txtFlashAfterSeconds.Text = "15";
            this.txtFlashAfterSeconds.Validating += new System.ComponentModel.CancelEventHandler(this.txtFlashAfterSeconds_Validating);
            //
            // label13
            //
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(26, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(232, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Alert me if a task or sequence takes longer than";
            //
            // groupBox3
            //
            this.groupBox3.Controls.Add(this.btnResetClientConfig);
            this.groupBox3.Controls.Add(this.chkUseAutoLogon);
            this.groupBox3.Controls.Add(this.btnUpdateMyClientConfig);
            this.groupBox3.Controls.Add(this.txtAutoLogonAction);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtAutoLogonPW);
            this.groupBox3.Controls.Add(this.lblAutoLogonPW);
            this.groupBox3.Controls.Add(this.txtAutoLogonUser);
            this.groupBox3.Controls.Add(this.lblAutoLogonUser);
            this.groupBox3.Location = new System.Drawing.Point(21, 119);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(691, 171);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options that apply to the startup of the client";
            //
            // btnResetClientConfig
            //
            this.btnResetClientConfig.Location = new System.Drawing.Point(610, 17);
            this.btnResetClientConfig.Name = "btnResetClientConfig";
            this.btnResetClientConfig.Size = new System.Drawing.Size(75, 23);
            this.btnResetClientConfig.TabIndex = 7;
            this.btnResetClientConfig.Text = "Reset";
            this.toolTip.SetToolTip(this.btnResetClientConfig, "Click to reset you client configuration to the latest Open Petra default settings" +
                " .");
            this.btnResetClientConfig.UseVisualStyleBackColor = true;
            this.btnResetClientConfig.Click += new System.EventHandler(this.btnResetClientConfig_Click);
            //
            // chkUseAutoLogon
            //
            this.chkUseAutoLogon.AutoSize = true;
            this.chkUseAutoLogon.Location = new System.Drawing.Point(41, 20);
            this.chkUseAutoLogon.Name = "chkUseAutoLogon";
            this.chkUseAutoLogon.Size = new System.Drawing.Size(315, 17);
            this.chkUseAutoLogon.TabIndex = 0;
            this.chkUseAutoLogon.Text = "Use the auto-logon capability  (Over-ride at run-time with ALT)";
            this.chkUseAutoLogon.UseVisualStyleBackColor = true;
            this.chkUseAutoLogon.CheckedChanged += new System.EventHandler(this.chkUseAutoLogon_CheckedChanged);
            //
            // btnUpdateMyClientConfig
            //
            this.btnUpdateMyClientConfig.Location = new System.Drawing.Point(610, 136);
            this.btnUpdateMyClientConfig.Name = "btnUpdateMyClientConfig";
            this.btnUpdateMyClientConfig.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateMyClientConfig.TabIndex = 8;
            this.btnUpdateMyClientConfig.Text = "Update";
            this.toolTip.SetToolTip(this.btnUpdateMyClientConfig,
                "Click to update you client configuration to contain the specified auto-logon and " +
                "developer test actions.");
            this.btnUpdateMyClientConfig.UseVisualStyleBackColor = true;
            this.btnUpdateMyClientConfig.Click += new System.EventHandler(this.btnUpdateMyClientConfig_Click);
            //
            // txtAutoLogonAction
            //
            this.txtAutoLogonAction.Location = new System.Drawing.Point(9, 99);
            this.txtAutoLogonAction.Multiline = true;
            this.txtAutoLogonAction.Name = "txtAutoLogonAction";
            this.txtAutoLogonAction.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAutoLogonAction.Size = new System.Drawing.Size(583, 60);
            this.txtAutoLogonAction.TabIndex = 6;
            //
            // label15
            //
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 83);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(530, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "Test action (Put one property=value on each line and do not include commas.  Over" +
                                "-ride at run-time with CTRL)";
            //
            // txtAutoLogonPW
            //
            this.txtAutoLogonPW.Enabled = false;
            this.txtAutoLogonPW.Location = new System.Drawing.Point(475, 43);
            this.txtAutoLogonPW.Name = "txtAutoLogonPW";
            this.txtAutoLogonPW.Size = new System.Drawing.Size(117, 20);
            this.txtAutoLogonPW.TabIndex = 4;
            //
            // lblAutoLogonPW
            //
            this.lblAutoLogonPW.AutoSize = true;
            this.lblAutoLogonPW.Enabled = false;
            this.lblAutoLogonPW.Location = new System.Drawing.Point(363, 46);
            this.lblAutoLogonPW.Name = "lblAutoLogonPW";
            this.lblAutoLogonPW.Size = new System.Drawing.Size(106, 13);
            this.lblAutoLogonPW.TabIndex = 3;
            this.lblAutoLogonPW.Text = "Auto-logon password";
            //
            // txtAutoLogonUser
            //
            this.txtAutoLogonUser.Enabled = false;
            this.txtAutoLogonUser.Location = new System.Drawing.Point(164, 43);
            this.txtAutoLogonUser.Name = "txtAutoLogonUser";
            this.txtAutoLogonUser.Size = new System.Drawing.Size(149, 20);
            this.txtAutoLogonUser.TabIndex = 2;
            //
            // lblAutoLogonUser
            //
            this.lblAutoLogonUser.AutoSize = true;
            this.lblAutoLogonUser.Enabled = false;
            this.lblAutoLogonUser.Location = new System.Drawing.Point(77, 46);
            this.lblAutoLogonUser.Name = "lblAutoLogonUser";
            this.lblAutoLogonUser.Size = new System.Drawing.Size(81, 13);
            this.lblAutoLogonUser.TabIndex = 1;
            this.lblAutoLogonUser.Text = "Auto-logon user";
            //
            // lblVersion
            //
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(18, 432);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "Version";
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.chkMinimizeServer);
            this.groupBox1.Controls.Add(this.chkAutoStopServer);
            this.groupBox1.Controls.Add(this.chkAutoStartServer);
            this.groupBox1.Location = new System.Drawing.Point(21, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(691, 97);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options that apply to individual tasks";
            //
            // chkMinimizeServer
            //
            this.chkMinimizeServer.AutoSize = true;
            this.chkMinimizeServer.Location = new System.Drawing.Point(41, 70);
            this.chkMinimizeServer.Name = "chkMinimizeServer";
            this.chkMinimizeServer.Size = new System.Drawing.Size(302, 17);
            this.chkMinimizeServer.TabIndex = 2;
            this.chkMinimizeServer.Text = "Minimize the server window as soon as the server starts up";
            this.chkMinimizeServer.UseVisualStyleBackColor = true;
            //
            // chkAutoStopServer
            //
            this.chkAutoStopServer.AutoSize = true;
            this.chkAutoStopServer.Location = new System.Drawing.Point(41, 47);
            this.chkAutoStopServer.Name = "chkAutoStopServer";
            this.chkAutoStopServer.Size = new System.Drawing.Size(419, 17);
            this.chkAutoStopServer.TabIndex = 1;
            this.chkAutoStopServer.Text = "Automatically stop the server before any task that compiles it, if the server is " +
                                          "running";
            this.chkAutoStopServer.UseVisualStyleBackColor = true;
            //
            // chkAutoStartServer
            //
            this.chkAutoStartServer.AutoSize = true;
            this.chkAutoStartServer.Location = new System.Drawing.Point(41, 24);
            this.chkAutoStartServer.Name = "chkAutoStartServer";
            this.chkAutoStartServer.Size = new System.Drawing.Size(428, 17);
            this.chkAutoStartServer.TabIndex = 0;
            this.chkAutoStartServer.Text = "Automatically start the server when the client starts, if the server is not alrea" +
                                           "dy running";
            this.chkAutoStartServer.UseVisualStyleBackColor = true;
            //
            // ShutdownTimer
            //
            this.ShutdownTimer.Interval = 200;
            this.ShutdownTimer.Tick += new System.EventHandler(this.ShutdownTimer_Tick);
            //
            // TickTimer
            //
            this.TickTimer.Enabled = true;
            this.TickTimer.Interval = 10000;
            this.TickTimer.Tick += new System.EventHandler(this.TickTimer_Tick);
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.toolStripProgressBar,
                    this.toolStripStatusLabel
                });
            this.statusStrip.Location = new System.Drawing.Point(0, 530);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(765, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            //
            // toolStripProgressBar
            //
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar.Visible = false;
            //
            // toolStripStatusLabel
            //
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";
            //
            // mainToolStrip
            //
            this.mainToolStrip.AutoSize = false;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tbbGenerateSolutionFullCompile,
                    this.tbbGenerateSolutionMinCompile,
                    this.tbbGenerateWinForms,
                    this.tbbGenerateGlue,
                    this.toolStripSeparator1,
                    this.tbbUncrustify,
                    this.tbbRunAllTests,
                    this.tbbRunMainNavigationScreensTests,
                    this.toolStripSeparator2,
                    this.tbbSourceHistoryLog,
                    this.tbbShowSourceDifferences,
                    this.tbbCommitSourceChanges,
                    this.tbbShelveSourceChanges,
                    this.tbbUnshelveSourceChanges,
                    this.toolStripSeparator4,
                    this.tbbMergeSourceFromTrunk,
                    this.tbbCreateNewSourceBranch,
                    this.toolStripSeparator3,
                    this.tbbCreateDatabase,
                    this.tbbDatabaseContent
                });
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(765, 41);
            this.mainToolStrip.TabIndex = 2;
            this.mainToolStrip.Text = "toolStrip1";
            //
            // tbbGenerateSolutionFullCompile
            //
            this.tbbGenerateSolutionFullCompile.AutoSize = false;
            this.tbbGenerateSolutionFullCompile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbGenerateSolutionFullCompile.Image = ((System.Drawing.Image)(resources.GetObject("tbbGenerateSolutionFullCompile.Image")));
            this.tbbGenerateSolutionFullCompile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbGenerateSolutionFullCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbGenerateSolutionFullCompile.Name = "tbbGenerateSolutionFullCompile";
            this.tbbGenerateSolutionFullCompile.Size = new System.Drawing.Size(39, 38);
            this.tbbGenerateSolutionFullCompile.Text = "Generate Solution with Full Compile";
            this.tbbGenerateSolutionFullCompile.Click += new System.EventHandler(this.tbbGenerateSolutionFullCompile_Click);
            //
            // tbbGenerateSolutionMinCompile
            //
            this.tbbGenerateSolutionMinCompile.AutoSize = false;
            this.tbbGenerateSolutionMinCompile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbGenerateSolutionMinCompile.Image = ((System.Drawing.Image)(resources.GetObject("tbbGenerateSolutionMinCompile.Image")));
            this.tbbGenerateSolutionMinCompile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbGenerateSolutionMinCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbGenerateSolutionMinCompile.Name = "tbbGenerateSolutionMinCompile";
            this.tbbGenerateSolutionMinCompile.Size = new System.Drawing.Size(39, 38);
            this.tbbGenerateSolutionMinCompile.Text = "Generate Solution with Minimal Compile";
            this.tbbGenerateSolutionMinCompile.Click += new System.EventHandler(this.tbbGenerateSolutionMinCompile_Click);
            //
            // tbbGenerateWinForms
            //
            this.tbbGenerateWinForms.AutoSize = false;
            this.tbbGenerateWinForms.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbGenerateWinForms.Image = ((System.Drawing.Image)(resources.GetObject("tbbGenerateWinForms.Image")));
            this.tbbGenerateWinForms.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbGenerateWinForms.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbGenerateWinForms.Name = "tbbGenerateWinForms";
            this.tbbGenerateWinForms.Size = new System.Drawing.Size(39, 38);
            this.tbbGenerateWinForms.Text = "Generate All the Windows Forms";
            this.tbbGenerateWinForms.Click += new System.EventHandler(this.tbbGenerateWinForms_Click);
            //
            // tbbGenerateGlue
            //
            this.tbbGenerateGlue.AutoSize = false;
            this.tbbGenerateGlue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbGenerateGlue.Image = ((System.Drawing.Image)(resources.GetObject("tbbGenerateGlue.Image")));
            this.tbbGenerateGlue.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbGenerateGlue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbGenerateGlue.Name = "tbbGenerateGlue";
            this.tbbGenerateGlue.Size = new System.Drawing.Size(39, 38);
            this.tbbGenerateGlue.Text = "Generate the Glue";
            this.tbbGenerateGlue.Click += new System.EventHandler(this.tbbGenerateGlue_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 41);
            //
            // tbbUncrustify
            //
            this.tbbUncrustify.AutoSize = false;
            this.tbbUncrustify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbUncrustify.Image = ((System.Drawing.Image)(resources.GetObject("tbbUncrustify.Image")));
            this.tbbUncrustify.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbUncrustify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbUncrustify.Name = "tbbUncrustify";
            this.tbbUncrustify.Size = new System.Drawing.Size(39, 38);
            this.tbbUncrustify.Text = "Uncrustify the Source Code";
            this.tbbUncrustify.Click += new System.EventHandler(this.tbbUncrustify_Click);
            //
            // tbbRunAllTests
            //
            this.tbbRunAllTests.AutoSize = false;
            this.tbbRunAllTests.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbRunAllTests.Image = ((System.Drawing.Image)(resources.GetObject("tbbRunAllTests.Image")));
            this.tbbRunAllTests.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbRunAllTests.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbRunAllTests.Name = "tbbRunAllTests";
            this.tbbRunAllTests.Size = new System.Drawing.Size(39, 38);
            this.tbbRunAllTests.Text = "Run All Tests";
            this.tbbRunAllTests.Click += new System.EventHandler(this.tbbRunAllTests_Click);
            //
            // tbbRunMainNavigationScreensTests
            //
            this.tbbRunMainNavigationScreensTests.AutoSize = false;
            this.tbbRunMainNavigationScreensTests.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbRunMainNavigationScreensTests.Image = ((System.Drawing.Image)(resources.GetObject("tbbRunMainNavigationScreensTests.Image")));
            this.tbbRunMainNavigationScreensTests.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbRunMainNavigationScreensTests.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbRunMainNavigationScreensTests.Name = "tbbRunMainNavigationScreensTests";
            this.tbbRunMainNavigationScreensTests.Size = new System.Drawing.Size(39, 38);
            this.tbbRunMainNavigationScreensTests.Text = "Run All Main Menu Tests";
            this.tbbRunMainNavigationScreensTests.Click += new System.EventHandler(this.tbbRunMainNavigationScreensTests_Click);
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 41);
            //
            // tbbSourceHistoryLog
            //
            this.tbbSourceHistoryLog.AutoSize = false;
            this.tbbSourceHistoryLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbSourceHistoryLog.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tbbSourceHistoryAllMenuItem,
                    this.tbbSourceHistoryFileMenuItem
                });
            this.tbbSourceHistoryLog.Image = ((System.Drawing.Image)(resources.GetObject("tbbSourceHistoryLog.Image")));
            this.tbbSourceHistoryLog.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbSourceHistoryLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbSourceHistoryLog.Name = "tbbSourceHistoryLog";
            this.tbbSourceHistoryLog.Size = new System.Drawing.Size(51, 38);
            this.tbbSourceHistoryLog.Text = "View History Log";
            this.tbbSourceHistoryLog.ButtonClick += new System.EventHandler(this.tbbSourceHistoryLog_ButtonClick);
            //
            // tbbSourceHistoryAllMenuItem
            //
            this.tbbSourceHistoryAllMenuItem.Name = "tbbSourceHistoryAllMenuItem";
            this.tbbSourceHistoryAllMenuItem.Size = new System.Drawing.Size(207, 22);
            this.tbbSourceHistoryAllMenuItem.Text = "History for &All Files";
            this.tbbSourceHistoryAllMenuItem.Click += new System.EventHandler(this.tbbSourceHistoryAllMenuItem_Click);
            //
            // tbbSourceHistoryFileMenuItem
            //
            this.tbbSourceHistoryFileMenuItem.Name = "tbbSourceHistoryFileMenuItem";
            this.tbbSourceHistoryFileMenuItem.Size = new System.Drawing.Size(207, 22);
            this.tbbSourceHistoryFileMenuItem.Text = "History for a Single &File ...";
            this.tbbSourceHistoryFileMenuItem.Click += new System.EventHandler(this.tbbSourceHistoryFileMenuItem_Click);
            //
            // tbbShowSourceDifferences
            //
            this.tbbShowSourceDifferences.AutoSize = false;
            this.tbbShowSourceDifferences.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbShowSourceDifferences.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tbbShowSourceDifferencesAllMenuItem,
                    this.tbbShowSourceDifferencesFileMenuItem
                });
            this.tbbShowSourceDifferences.Image = ((System.Drawing.Image)(resources.GetObject("tbbShowSourceDifferences.Image")));
            this.tbbShowSourceDifferences.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbShowSourceDifferences.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbShowSourceDifferences.Name = "tbbShowSourceDifferences";
            this.tbbShowSourceDifferences.Size = new System.Drawing.Size(51, 38);
            this.tbbShowSourceDifferences.Text = "Show Source Differences";
            this.tbbShowSourceDifferences.ButtonClick += new System.EventHandler(this.tbbShowSourceDifferences_ButtonClick);
            //
            // tbbShowSourceDifferencesAllMenuItem
            //
            this.tbbShowSourceDifferencesAllMenuItem.Name = "tbbShowSourceDifferencesAllMenuItem";
            this.tbbShowSourceDifferencesAllMenuItem.Size = new System.Drawing.Size(216, 22);
            this.tbbShowSourceDifferencesAllMenuItem.Text = "Show &All Differences";
            this.tbbShowSourceDifferencesAllMenuItem.Click += new System.EventHandler(this.tbbShowSourceDifferencesAllMenuItem_Click);
            //
            // tbbShowSourceDifferencesFileMenuItem
            //
            this.tbbShowSourceDifferencesFileMenuItem.Name = "tbbShowSourceDifferencesFileMenuItem";
            this.tbbShowSourceDifferencesFileMenuItem.Size = new System.Drawing.Size(216, 22);
            this.tbbShowSourceDifferencesFileMenuItem.Text = "Differences for a Single &File";
            this.tbbShowSourceDifferencesFileMenuItem.Click += new System.EventHandler(this.tbbShowSourceDifferencesFileMenuItem_Click);
            //
            // tbbCommitSourceChanges
            //
            this.tbbCommitSourceChanges.AutoSize = false;
            this.tbbCommitSourceChanges.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbCommitSourceChanges.Image = ((System.Drawing.Image)(resources.GetObject("tbbCommitSourceChanges.Image")));
            this.tbbCommitSourceChanges.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbCommitSourceChanges.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbCommitSourceChanges.Name = "tbbCommitSourceChanges";
            this.tbbCommitSourceChanges.Size = new System.Drawing.Size(39, 38);
            this.tbbCommitSourceChanges.Text = "Commit Source Changes";
            this.tbbCommitSourceChanges.Click += new System.EventHandler(this.tbbCommitSourceChanges_Click);
            //
            // tbbShelveSourceChanges
            //
            this.tbbShelveSourceChanges.AutoSize = false;
            this.tbbShelveSourceChanges.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbShelveSourceChanges.Image = ((System.Drawing.Image)(resources.GetObject("tbbShelveSourceChanges.Image")));
            this.tbbShelveSourceChanges.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbShelveSourceChanges.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbShelveSourceChanges.Name = "tbbShelveSourceChanges";
            this.tbbShelveSourceChanges.Size = new System.Drawing.Size(39, 38);
            this.tbbShelveSourceChanges.Text = "Shelve Source Changes";
            this.tbbShelveSourceChanges.Click += new System.EventHandler(this.tbbShelveSourceChanges_Click);
            //
            // tbbUnshelveSourceChanges
            //
            this.tbbUnshelveSourceChanges.AutoSize = false;
            this.tbbUnshelveSourceChanges.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbUnshelveSourceChanges.Image = ((System.Drawing.Image)(resources.GetObject("tbbUnshelveSourceChanges.Image")));
            this.tbbUnshelveSourceChanges.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbUnshelveSourceChanges.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbUnshelveSourceChanges.Name = "tbbUnshelveSourceChanges";
            this.tbbUnshelveSourceChanges.Size = new System.Drawing.Size(39, 38);
            this.tbbUnshelveSourceChanges.Text = "Unshelve Source Changes";
            this.tbbUnshelveSourceChanges.Click += new System.EventHandler(this.tbbUnshelveSourceChanges_Click);
            //
            // toolStripSeparator4
            //
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 41);
            //
            // tbbMergeSourceFromTrunk
            //
            this.tbbMergeSourceFromTrunk.AutoSize = false;
            this.tbbMergeSourceFromTrunk.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbMergeSourceFromTrunk.Image = ((System.Drawing.Image)(resources.GetObject("tbbMergeSourceFromTrunk.Image")));
            this.tbbMergeSourceFromTrunk.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbMergeSourceFromTrunk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbMergeSourceFromTrunk.Name = "tbbMergeSourceFromTrunk";
            this.tbbMergeSourceFromTrunk.Size = new System.Drawing.Size(39, 38);
            this.tbbMergeSourceFromTrunk.Text = "Merge Source From Trunk";
            this.tbbMergeSourceFromTrunk.Click += new System.EventHandler(this.tbbMergeSourceFromTrunk_Click);
            //
            // tbbCreateNewSourceBranch
            //
            this.tbbCreateNewSourceBranch.AutoSize = false;
            this.tbbCreateNewSourceBranch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbCreateNewSourceBranch.Image = ((System.Drawing.Image)(resources.GetObject("tbbCreateNewSourceBranch.Image")));
            this.tbbCreateNewSourceBranch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbCreateNewSourceBranch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbCreateNewSourceBranch.Name = "tbbCreateNewSourceBranch";
            this.tbbCreateNewSourceBranch.Size = new System.Drawing.Size(39, 38);
            this.tbbCreateNewSourceBranch.Text = "Create New Source Branch";
            this.tbbCreateNewSourceBranch.Click += new System.EventHandler(this.tbbCreateNewSourceBranch_Click);
            //
            // toolStripSeparator3
            //
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 41);
            //
            // tbbCreateDatabase
            //
            this.tbbCreateDatabase.AutoSize = false;
            this.tbbCreateDatabase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbCreateDatabase.Image = ((System.Drawing.Image)(resources.GetObject("tbbCreateDatabase.Image")));
            this.tbbCreateDatabase.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbCreateDatabase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbCreateDatabase.Name = "tbbCreateDatabase";
            this.tbbCreateDatabase.Size = new System.Drawing.Size(39, 38);
            this.tbbCreateDatabase.Text = "Create a New Database";
            this.tbbCreateDatabase.Click += new System.EventHandler(this.tbbCreateDatabase_Click);
            //
            // tbbDatabaseContent
            //
            this.tbbDatabaseContent.AutoSize = false;
            this.tbbDatabaseContent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbDatabaseContent.Image = ((System.Drawing.Image)(resources.GetObject("tbbDatabaseContent.Image")));
            this.tbbDatabaseContent.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbbDatabaseContent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbDatabaseContent.Name = "tbbDatabaseContent";
            this.tbbDatabaseContent.Size = new System.Drawing.Size(39, 38);
            this.tbbDatabaseContent.Text = "Reset the Database Content";
            this.tbbDatabaseContent.Click += new System.EventHandler(this.tbbDatabaseContent_Click);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 552);
            this.Controls.Add(this.mainToolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Open Petra Developer\'s Assistant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tabControl.ResumeLayout(false);
            this.TaskPage.ResumeLayout(false);
            this.TaskPage.PerformLayout();
            this.grpMultiple.ResumeLayout(false);
            this.grpMultiple.PerformLayout();
            this.grpSingle.ResumeLayout(false);
            this.grpSingle.PerformLayout();
            this.DatabasePage.ResumeLayout(false);
            this.DatabasePage.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.OutputPage.ResumeLayout(false);
            this.OutputPage.PerformLayout();
            this.ExternalPage.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.OptionsPage.ResumeLayout(false);
            this.OptionsPage.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage TaskPage;
        private System.Windows.Forms.TabPage OutputPage;
        private System.Windows.Forms.GroupBox grpMultiple;
        private System.Windows.Forms.GroupBox grpSingle;
        private System.Windows.Forms.CheckBox chkVerbose;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.LinkLabel linkLabelBranchLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboMiscellaneous;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnStartClient;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGenerateWinform;
        private System.Windows.Forms.ComboBox cboYAMLHistory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMiscellaneous;
        private System.Windows.Forms.LinkLabel linkLabelRestartServer;
        private System.Windows.Forms.LinkLabel linkLabelStopServer;
        private System.Windows.Forms.LinkLabel linkLabelStartServer;
        private System.Windows.Forms.LinkLabel linkLabelYamlFile;
        private System.Windows.Forms.Button btnCompilation;
        private System.Windows.Forms.Button btnCodeGeneration;
        private System.Windows.Forms.ComboBox cboCompilation;
        private System.Windows.Forms.ComboBox cboCodeGeneration;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRunAltSequence;
        private System.Windows.Forms.LinkLabel linkModifyAltSequence;
        private System.Windows.Forms.TextBox txtAltSequence;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnRunSequence;
        private System.Windows.Forms.LinkLabel linkModifySequence;
        private System.Windows.Forms.TextBox txtSequence;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnNextWarning;
        private System.Windows.Forms.Button btnPrevWarning;
        private System.Windows.Forms.Label lblWarnings;
        private System.Windows.Forms.TabPage OptionsPage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAutoStopServer;
        private System.Windows.Forms.CheckBox chkAutoStartServer;
        private System.Windows.Forms.CheckBox chkMinimizeServer;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TabPage DatabasePage;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSaveDbBuildConfig;
        private System.Windows.Forms.Button btnDatabase;
        private System.Windows.Forms.ComboBox cboDatabase;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblBranchLocation;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnRemoveDbBuildConfig;
        private System.Windows.Forms.Button btnAddDbBuildConfig;
        private System.Windows.Forms.Label lblDbBuildConfig;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListBox listDbBuildConfig;
        private System.Windows.Forms.Button btnEditDbBuildConfig;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnUpdateMyClientConfig;
        private System.Windows.Forms.TextBox txtAutoLogonAction;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtAutoLogonPW;
        private System.Windows.Forms.Label lblAutoLogonPW;
        private System.Windows.Forms.TextBox txtAutoLogonUser;
        private System.Windows.Forms.Label lblAutoLogonUser;
        private System.Windows.Forms.CheckBox chkUseAutoLogon;
        private System.Windows.Forms.Button btnResetClientConfig;
        private System.Windows.Forms.CheckBox chkStartClientAfterGenerateWinform;
        private System.Windows.Forms.CheckBox chkCompileWinform;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtFlashAfterSeconds;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.LinkLabel linkLabelBazaar;
        private System.Windows.Forms.Button btnBrowseBazaar;
        private System.Windows.Forms.TextBox txtBazaarPath;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chkTreatWarningsAsErrors;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Timer ShutdownTimer;
        private System.Windows.Forms.Button btnPreviewWinform;
        private System.Windows.Forms.Button btnAdvancedOptions;
        private System.Windows.Forms.TabPage ExternalPage;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblExternalWebLink;
        private System.Windows.Forms.Button btnBrowseWeb;
        private System.Windows.Forms.Label lblWebLinkInfo;
        private System.Windows.Forms.ListBox lstExternalWebLinks;
        private System.Windows.Forms.LinkLabel linkRefreshLinks;
        private System.Windows.Forms.LinkLabel linkEditLinks;
        private System.Windows.Forms.Timer TickTimer;
        private System.Windows.Forms.Button btnCheckForUpdates;
        private System.Windows.Forms.CheckBox chkCheckForUpdatesAtStartup;
        private System.Windows.Forms.ComboBox cboBranchLocation;
        private System.Windows.Forms.Button btnSourceCode;
        private System.Windows.Forms.ComboBox cboSourceCode;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtLaunchpadUserName;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblDatabaseName;
        private System.Windows.Forms.LinkLabel linkLabel_LaunchpadUrl;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Button btnDemoteFavouriteBuild;
        private System.Windows.Forms.Button btnPromoteFavouriteBuild;
        private System.Windows.Forms.LinkLabel linkSuggestedLinksUpdates;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripButton tbbGenerateSolutionFullCompile;
        private System.Windows.Forms.ToolStripButton tbbGenerateSolutionMinCompile;
        private System.Windows.Forms.ToolStripButton tbbGenerateWinForms;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tbbUncrustify;
        private System.Windows.Forms.ToolStripButton tbbRunAllTests;
        private System.Windows.Forms.ToolStripButton tbbRunMainNavigationScreensTests;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tbbCommitSourceChanges;
        private System.Windows.Forms.ToolStripButton tbbShelveSourceChanges;
        private System.Windows.Forms.ToolStripButton tbbUnshelveSourceChanges;
        private System.Windows.Forms.ToolStripButton tbbMergeSourceFromTrunk;
        private System.Windows.Forms.ToolStripButton tbbCreateNewSourceBranch;
        private System.Windows.Forms.ToolStripButton tbbGenerateGlue;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tbbCreateDatabase;
        private System.Windows.Forms.ToolStripButton tbbDatabaseContent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSplitButton tbbSourceHistoryLog;
        private System.Windows.Forms.ToolStripMenuItem tbbSourceHistoryAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tbbSourceHistoryFileMenuItem;
        private System.Windows.Forms.ToolStripSplitButton tbbShowSourceDifferences;
        private System.Windows.Forms.ToolStripMenuItem tbbShowSourceDifferencesAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tbbShowSourceDifferencesFileMenuItem;
    }
}