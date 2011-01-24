// auto generated with nant generateWinforms from PersonnelMain.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPersonnel.Gui
{
    partial class TFrmPersonnelMain
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmPersonnelMain));

            this.pnlTODO = new System.Windows.Forms.Panel();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniExtracts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportPartnerByCity = new System.Windows.Forms.ToolStripMenuItem();
            this.mniShortTermerReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGeneralShortTermerReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAbilitiesReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPersonnelEmergencyContactReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLanguagesReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniIndividualDataReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEmergencyDataReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLocalPersonnelDataReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniBirthdayList = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEmergencyContactReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPassportExpiryReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPersonalDocumentExpiryReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniStartOfCommitmentReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEndOfCommitmentReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPreviousExperienceReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniProgressReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniCampaignOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mniUnitHierarchy = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniMaintainTables = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTodo = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPetraModules = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPetraMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPetraPartnerModule = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPetraFinanceModule = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPetraPersonnelModule = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPetraConferenceModule = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPetraFinDevModule = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPetraSysManModule = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlTODO.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlTODO
            //
            this.pnlTODO.Name = "pnlTODO";
            this.pnlTODO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTODO.AutoSize = true;
            //
            // mniImport
            //
            this.mniImport.Name = "mniImport";
            this.mniImport.AutoSize = true;
            this.mniImport.Text = "&Import";
            //
            // mniExport
            //
            this.mniExport.Name = "mniExport";
            this.mniExport.AutoSize = true;
            this.mniExport.Text = "&Export";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "-";
            //
            // mniClose
            //
            this.mniClose.Name = "mniClose";
            this.mniClose.AutoSize = true;
            this.mniClose.Click += new System.EventHandler(this.actClose);
            this.mniClose.Image = ((System.Drawing.Bitmap)resources.GetObject("mniClose.Glyph"));
            this.mniClose.ToolTipText = "Closes this window";
            this.mniClose.Text = "&Close";
            //
            // mniFile
            //
            this.mniFile.Name = "mniFile";
            this.mniFile.AutoSize = true;
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniImport,
                        mniExport,
                        mniSeparator0,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniExtracts
            //
            this.mniExtracts.Name = "mniExtracts";
            this.mniExtracts.AutoSize = true;
            this.mniExtracts.Text = "&Extracts...";
            //
            // mniReportPartnerByCity
            //
            this.mniReportPartnerByCity.Name = "mniReportPartnerByCity";
            this.mniReportPartnerByCity.AutoSize = true;
            this.mniReportPartnerByCity.Click += new System.EventHandler(this.OpenScreenReportPartnerByCity);
            this.mniReportPartnerByCity.Text = "Progress Report";
            //
            // mniGeneralShortTermerReport
            //
            this.mniGeneralShortTermerReport.Name = "mniGeneralShortTermerReport";
            this.mniGeneralShortTermerReport.AutoSize = true;
            this.mniGeneralShortTermerReport.Click += new System.EventHandler(this.OpenScreenGeneralShortTermerReport);
            this.mniGeneralShortTermerReport.Text = "General Short Termer Report";
            //
            // mniAbilitiesReport
            //
            this.mniAbilitiesReport.Name = "mniAbilitiesReport";
            this.mniAbilitiesReport.AutoSize = true;
            this.mniAbilitiesReport.Click += new System.EventHandler(this.OpenScreenAbilitiesReport);
            this.mniAbilitiesReport.Text = "Abilities Report";
            //
            // mniPersonnelEmergencyContactReport
            //
            this.mniPersonnelEmergencyContactReport.Name = "mniPersonnelEmergencyContactReport";
            this.mniPersonnelEmergencyContactReport.AutoSize = true;
            this.mniPersonnelEmergencyContactReport.Click += new System.EventHandler(this.OpenScreenPersonnelEmergencyContactReport);
            this.mniPersonnelEmergencyContactReport.Text = "Emergency Contact Report";
            //
            // mniLanguagesReport
            //
            this.mniLanguagesReport.Name = "mniLanguagesReport";
            this.mniLanguagesReport.AutoSize = true;
            this.mniLanguagesReport.Click += new System.EventHandler(this.OpenScreenLanguagesReport);
            this.mniLanguagesReport.Text = "Languages Report";
            //
            // mniShortTermerReports
            //
            this.mniShortTermerReports.Name = "mniShortTermerReports";
            this.mniShortTermerReports.AutoSize = true;
            this.mniShortTermerReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniGeneralShortTermerReport,
                        mniAbilitiesReport,
                        mniPersonnelEmergencyContactReport,
                        mniLanguagesReport});
            this.mniShortTermerReports.Text = "Short Termer Reports";
            //
            // mniIndividualDataReport
            //
            this.mniIndividualDataReport.Name = "mniIndividualDataReport";
            this.mniIndividualDataReport.AutoSize = true;
            this.mniIndividualDataReport.Click += new System.EventHandler(this.OpenScreenIndividualDataReport);
            this.mniIndividualDataReport.Text = "Individual Data Report";
            //
            // mniEmergencyDataReport
            //
            this.mniEmergencyDataReport.Name = "mniEmergencyDataReport";
            this.mniEmergencyDataReport.AutoSize = true;
            this.mniEmergencyDataReport.Click += new System.EventHandler(this.OpenScreenEmergencyDataReport);
            this.mniEmergencyDataReport.Text = "Emergency Data Report";
            //
            // mniLocalPersonnelDataReport
            //
            this.mniLocalPersonnelDataReport.Name = "mniLocalPersonnelDataReport";
            this.mniLocalPersonnelDataReport.AutoSize = true;
            this.mniLocalPersonnelDataReport.Click += new System.EventHandler(this.OpenScreenLocalPersonnelDataReport);
            this.mniLocalPersonnelDataReport.Text = "Local Personnel Data Report";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
            //
            // mniBirthdayList
            //
            this.mniBirthdayList.Name = "mniBirthdayList";
            this.mniBirthdayList.AutoSize = true;
            this.mniBirthdayList.Click += new System.EventHandler(this.OpenScreenBirthdayList);
            this.mniBirthdayList.Text = "Birthday List";
            //
            // mniEmergencyContactReport
            //
            this.mniEmergencyContactReport.Name = "mniEmergencyContactReport";
            this.mniEmergencyContactReport.AutoSize = true;
            this.mniEmergencyContactReport.Click += new System.EventHandler(this.OpenScreenEmergencyContactReport);
            this.mniEmergencyContactReport.Text = "Emergency Contact Report";
            //
            // mniPassportExpiryReport
            //
            this.mniPassportExpiryReport.Name = "mniPassportExpiryReport";
            this.mniPassportExpiryReport.AutoSize = true;
            this.mniPassportExpiryReport.Click += new System.EventHandler(this.OpenScreenPassportExpiryReport);
            this.mniPassportExpiryReport.Text = "Passport Expiry Report";
            //
            // mniPersonalDocumentExpiryReport
            //
            this.mniPersonalDocumentExpiryReport.Name = "mniPersonalDocumentExpiryReport";
            this.mniPersonalDocumentExpiryReport.AutoSize = true;
            this.mniPersonalDocumentExpiryReport.Click += new System.EventHandler(this.OpenScreenPersonalDocumentExpiryReport);
            this.mniPersonalDocumentExpiryReport.Text = "Personal Document Expiry Report";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
            //
            // mniStartOfCommitmentReport
            //
            this.mniStartOfCommitmentReport.Name = "mniStartOfCommitmentReport";
            this.mniStartOfCommitmentReport.AutoSize = true;
            this.mniStartOfCommitmentReport.Click += new System.EventHandler(this.OpenScreenStartOfCommitmentReport);
            this.mniStartOfCommitmentReport.Text = "Start Of Commitment Report";
            //
            // mniEndOfCommitmentReport
            //
            this.mniEndOfCommitmentReport.Name = "mniEndOfCommitmentReport";
            this.mniEndOfCommitmentReport.AutoSize = true;
            this.mniEndOfCommitmentReport.Click += new System.EventHandler(this.OpenScreenEndOfCommitmentReport);
            this.mniEndOfCommitmentReport.Text = "End Of Commitment Report";
            //
            // mniPreviousExperienceReport
            //
            this.mniPreviousExperienceReport.Name = "mniPreviousExperienceReport";
            this.mniPreviousExperienceReport.AutoSize = true;
            this.mniPreviousExperienceReport.Click += new System.EventHandler(this.OpenScreenPreviousExperienceReport);
            this.mniPreviousExperienceReport.Text = "Previous Experience Report";
            //
            // mniProgressReport
            //
            this.mniProgressReport.Name = "mniProgressReport";
            this.mniProgressReport.AutoSize = true;
            this.mniProgressReport.Click += new System.EventHandler(this.OpenScreenProgressReport);
            this.mniProgressReport.Text = "Progress Report";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "-";
            //
            // mniCampaignOptions
            //
            this.mniCampaignOptions.Name = "mniCampaignOptions";
            this.mniCampaignOptions.AutoSize = true;
            this.mniCampaignOptions.Click += new System.EventHandler(this.OpenScreenCampaignOptions);
            this.mniCampaignOptions.Text = "Campaign Options";
            //
            // mniUnitHierarchy
            //
            this.mniUnitHierarchy.Name = "mniUnitHierarchy";
            this.mniUnitHierarchy.AutoSize = true;
            this.mniUnitHierarchy.Click += new System.EventHandler(this.OpenScreenUnitHierarchy);
            this.mniUnitHierarchy.Text = "Unit Hierarchy";
            //
            // mniReports
            //
            this.mniReports.Name = "mniReports";
            this.mniReports.AutoSize = true;
            this.mniReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniReportPartnerByCity,
                        mniShortTermerReports,
                        mniIndividualDataReport,
                        mniEmergencyDataReport,
                        mniLocalPersonnelDataReport,
                        mniSeparator1,
                        mniBirthdayList,
                        mniEmergencyContactReport,
                        mniPassportExpiryReport,
                        mniPersonalDocumentExpiryReport,
                        mniSeparator2,
                        mniStartOfCommitmentReport,
                        mniEndOfCommitmentReport,
                        mniPreviousExperienceReport,
                        mniProgressReport,
                        mniSeparator3,
                        mniCampaignOptions,
                        mniUnitHierarchy});
            this.mniReports.Text = "&Reports...";
            //
            // mniSeparator4
            //
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.AutoSize = true;
            this.mniSeparator4.Text = "-";
            //
            // mniPartner
            //
            this.mniPartner.Name = "mniPartner";
            this.mniPartner.AutoSize = true;
            this.mniPartner.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniExtracts,
                        mniReports,
                        mniSeparator4});
            this.mniPartner.Text = "Partner";
            //
            // mniTodo
            //
            this.mniTodo.Name = "mniTodo";
            this.mniTodo.AutoSize = true;
            this.mniTodo.Text = "Todo";
            //
            // mniMaintainTables
            //
            this.mniMaintainTables.Name = "mniMaintainTables";
            this.mniMaintainTables.AutoSize = true;
            this.mniMaintainTables.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniTodo});
            this.mniMaintainTables.Text = "Maintain &Tables";
            //
            // mniPetraMainMenu
            //
            this.mniPetraMainMenu.Name = "mniPetraMainMenu";
            this.mniPetraMainMenu.AutoSize = true;
            this.mniPetraMainMenu.Click += new System.EventHandler(this.actMainMenu);
            this.mniPetraMainMenu.Text = "Petra &Main Menu";
            //
            // mniPetraPartnerModule
            //
            this.mniPetraPartnerModule.Name = "mniPetraPartnerModule";
            this.mniPetraPartnerModule.AutoSize = true;
            this.mniPetraPartnerModule.Click += new System.EventHandler(this.actPartnerModule);
            this.mniPetraPartnerModule.Text = "Pa&rtner";
            //
            // mniPetraFinanceModule
            //
            this.mniPetraFinanceModule.Name = "mniPetraFinanceModule";
            this.mniPetraFinanceModule.AutoSize = true;
            this.mniPetraFinanceModule.Click += new System.EventHandler(this.actFinanceModule);
            this.mniPetraFinanceModule.Text = "&Finance";
            //
            // mniPetraPersonnelModule
            //
            this.mniPetraPersonnelModule.Name = "mniPetraPersonnelModule";
            this.mniPetraPersonnelModule.AutoSize = true;
            this.mniPetraPersonnelModule.Click += new System.EventHandler(this.actPersonnelModule);
            this.mniPetraPersonnelModule.Text = "P&ersonnel";
            //
            // mniPetraConferenceModule
            //
            this.mniPetraConferenceModule.Name = "mniPetraConferenceModule";
            this.mniPetraConferenceModule.AutoSize = true;
            this.mniPetraConferenceModule.Click += new System.EventHandler(this.actConferenceModule);
            this.mniPetraConferenceModule.Text = "C&onference";
            //
            // mniPetraFinDevModule
            //
            this.mniPetraFinDevModule.Name = "mniPetraFinDevModule";
            this.mniPetraFinDevModule.AutoSize = true;
            this.mniPetraFinDevModule.Click += new System.EventHandler(this.actFinDevModule);
            this.mniPetraFinDevModule.Text = "Financial &Development";
            //
            // mniPetraSysManModule
            //
            this.mniPetraSysManModule.Name = "mniPetraSysManModule";
            this.mniPetraSysManModule.AutoSize = true;
            this.mniPetraSysManModule.Click += new System.EventHandler(this.actSysManModule);
            this.mniPetraSysManModule.Text = "&System Manager";
            //
            // mniPetraModules
            //
            this.mniPetraModules.Name = "mniPetraModules";
            this.mniPetraModules.AutoSize = true;
            this.mniPetraModules.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniPetraMainMenu,
                        mniPetraPartnerModule,
                        mniPetraFinanceModule,
                        mniPetraPersonnelModule,
                        mniPetraConferenceModule,
                        mniPetraFinDevModule,
                        mniPetraSysManModule});
            this.mniPetraModules.Text = "&OpenPetra";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator5
            //
            this.mniSeparator5.Name = "mniSeparator5";
            this.mniSeparator5.AutoSize = true;
            this.mniSeparator5.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator6
            //
            this.mniSeparator6.Name = "mniSeparator6";
            this.mniSeparator6.AutoSize = true;
            this.mniSeparator6.Text = "-";
            //
            // mniHelpAboutPetra
            //
            this.mniHelpAboutPetra.Name = "mniHelpAboutPetra";
            this.mniHelpAboutPetra.AutoSize = true;
            this.mniHelpAboutPetra.Text = "&About Petra";
            //
            // mniHelpDevelopmentTeam
            //
            this.mniHelpDevelopmentTeam.Name = "mniHelpDevelopmentTeam";
            this.mniHelpDevelopmentTeam.AutoSize = true;
            this.mniHelpDevelopmentTeam.Text = "&The Development Team...";
            //
            // mniHelp
            //
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.AutoSize = true;
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniHelpPetraHelp,
                        mniSeparator5,
                        mniHelpBugReport,
                        mniSeparator6,
                        mniHelpAboutPetra,
                        mniHelpDevelopmentTeam});
            this.mniHelp.Text = "&Help";
            //
            // mnuMain
            //
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.mnuMain.AutoSize = true;
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniFile,
                        mniPartner,
                        mniMaintainTables,
                        mniPetraModules,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmPersonnelMain
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(510, 476);

            this.Controls.Add(this.pnlTODO);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmPersonnelMain";
            this.Text = "Personnel Module OpenPetra.org";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.pnlTODO.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlTODO;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniImport;
        private System.Windows.Forms.ToolStripMenuItem mniExport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniPartner;
        private System.Windows.Forms.ToolStripMenuItem mniExtracts;
        private System.Windows.Forms.ToolStripMenuItem mniReports;
        private System.Windows.Forms.ToolStripMenuItem mniReportPartnerByCity;
        private System.Windows.Forms.ToolStripMenuItem mniShortTermerReports;
        private System.Windows.Forms.ToolStripMenuItem mniGeneralShortTermerReport;
        private System.Windows.Forms.ToolStripMenuItem mniAbilitiesReport;
        private System.Windows.Forms.ToolStripMenuItem mniPersonnelEmergencyContactReport;
        private System.Windows.Forms.ToolStripMenuItem mniLanguagesReport;
        private System.Windows.Forms.ToolStripMenuItem mniIndividualDataReport;
        private System.Windows.Forms.ToolStripMenuItem mniEmergencyDataReport;
        private System.Windows.Forms.ToolStripMenuItem mniLocalPersonnelDataReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniBirthdayList;
        private System.Windows.Forms.ToolStripMenuItem mniEmergencyContactReport;
        private System.Windows.Forms.ToolStripMenuItem mniPassportExpiryReport;
        private System.Windows.Forms.ToolStripMenuItem mniPersonalDocumentExpiryReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniStartOfCommitmentReport;
        private System.Windows.Forms.ToolStripMenuItem mniEndOfCommitmentReport;
        private System.Windows.Forms.ToolStripMenuItem mniPreviousExperienceReport;
        private System.Windows.Forms.ToolStripMenuItem mniProgressReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniCampaignOptions;
        private System.Windows.Forms.ToolStripMenuItem mniUnitHierarchy;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainTables;
        private System.Windows.Forms.ToolStripMenuItem mniTodo;
        private System.Windows.Forms.ToolStripMenuItem mniPetraModules;
        private System.Windows.Forms.ToolStripMenuItem mniPetraMainMenu;
        private System.Windows.Forms.ToolStripMenuItem mniPetraPartnerModule;
        private System.Windows.Forms.ToolStripMenuItem mniPetraFinanceModule;
        private System.Windows.Forms.ToolStripMenuItem mniPetraPersonnelModule;
        private System.Windows.Forms.ToolStripMenuItem mniPetraConferenceModule;
        private System.Windows.Forms.ToolStripMenuItem mniPetraFinDevModule;
        private System.Windows.Forms.ToolStripMenuItem mniPetraSysManModule;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator6;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
