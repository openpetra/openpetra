// auto generated with nant generateWinforms from PartnerMain.yaml
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
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TFrmPartnerMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmPartnerMain));

            this.pnlTODO = new System.Windows.Forms.Panel();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFindMaintain = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniLastPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLastPartners = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniExtracts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportPartnerByCity = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportPartnerAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportBriefFoundation = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportSupportingChurches = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportValidBankAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportPublicationStatisticalReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportBulkAddressReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportPartnerContactReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportSubscriptionReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportLocalPartnerDataReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniNewPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewPartnerAssistant = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewFamily = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewChurch = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMergePartners = new System.Windows.Forms.ToolStripMenuItem();
            this.mniDeletePartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniCheckDuplicateAddresses = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMergeAddresses = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewPartnersAtLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMailing = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLabelPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMailsortLabelPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniSubscriptionExpiryNotices = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSubscriptionCancellation = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFormLetterPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mniExtractMailMerge = new System.Windows.Forms.ToolStripMenuItem();
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
            this.mniSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator9 = new System.Windows.Forms.ToolStripSeparator();
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
            // mniFindMaintain
            //
            this.mniFindMaintain.Name = "mniFindMaintain";
            this.mniFindMaintain.AutoSize = true;
            this.mniFindMaintain.Click += new System.EventHandler(this.FindPartner);
            this.mniFindMaintain.Text = "&Find && Maintain...";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
            //
            // mniLastPartner
            //
            this.mniLastPartner.Name = "mniLastPartner";
            this.mniLastPartner.AutoSize = true;
            this.mniLastPartner.Text = "Work with &Last Partner...";
            //
            // mniLastPartners
            //
            this.mniLastPartners.Name = "mniLastPartners";
            this.mniLastPartners.AutoSize = true;
            this.mniLastPartners.Text = "&Recent Partners...";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
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
            this.mniReportPartnerByCity.Text = "&Partner By City Report (experiment)";
            //
            // mniReportPartnerAddress
            //
            this.mniReportPartnerAddress.Name = "mniReportPartnerAddress";
            this.mniReportPartnerAddress.AutoSize = true;
            this.mniReportPartnerAddress.Click += new System.EventHandler(this.OpenScreenReportPartnerAddress);
            this.mniReportPartnerAddress.Text = "Brief &Address Report";
            //
            // mniReportBriefFoundation
            //
            this.mniReportBriefFoundation.Name = "mniReportBriefFoundation";
            this.mniReportBriefFoundation.AutoSize = true;
            this.mniReportBriefFoundation.Click += new System.EventHandler(this.OpenScreenReportBriefFoundation);
            this.mniReportBriefFoundation.Text = "Brief &Foundation Report";
            //
            // mniReportSupportingChurches
            //
            this.mniReportSupportingChurches.Name = "mniReportSupportingChurches";
            this.mniReportSupportingChurches.AutoSize = true;
            this.mniReportSupportingChurches.Click += new System.EventHandler(this.OpenScreenReportSupportingChurches);
            this.mniReportSupportingChurches.Text = "Supporting &Churches Report";
            //
            // mniReportValidBankAccount
            //
            this.mniReportValidBankAccount.Name = "mniReportValidBankAccount";
            this.mniReportValidBankAccount.AutoSize = true;
            this.mniReportValidBankAccount.Click += new System.EventHandler(this.OpenScreenReportValidBankAccount);
            this.mniReportValidBankAccount.Text = "&Valid Bank Account Report";
            //
            // mniReportPublicationStatisticalReport
            //
            this.mniReportPublicationStatisticalReport.Name = "mniReportPublicationStatisticalReport";
            this.mniReportPublicationStatisticalReport.AutoSize = true;
            this.mniReportPublicationStatisticalReport.Click += new System.EventHandler(this.OpenScreenReportPublicationStatisticalReport);
            this.mniReportPublicationStatisticalReport.Text = "Publication &Statistical Report";
            //
            // mniReportBulkAddressReport
            //
            this.mniReportBulkAddressReport.Name = "mniReportBulkAddressReport";
            this.mniReportBulkAddressReport.AutoSize = true;
            this.mniReportBulkAddressReport.Click += new System.EventHandler(this.OpenScreenReportBulkAddressReport);
            this.mniReportBulkAddressReport.Text = "&Bulk Address Report";
            //
            // mniReportPartnerContactReport
            //
            this.mniReportPartnerContactReport.Name = "mniReportPartnerContactReport";
            this.mniReportPartnerContactReport.AutoSize = true;
            this.mniReportPartnerContactReport.Click += new System.EventHandler(this.OpenScreenReportPartnerContactReport);
            this.mniReportPartnerContactReport.Text = "Partner C&ontact Report";
            //
            // mniReportSubscriptionReport
            //
            this.mniReportSubscriptionReport.Name = "mniReportSubscriptionReport";
            this.mniReportSubscriptionReport.AutoSize = true;
            this.mniReportSubscriptionReport.Click += new System.EventHandler(this.OpenScreenReportSubscriptionReport);
            this.mniReportSubscriptionReport.Text = "Subscri&ption Report";
            //
            // mniReportLocalPartnerDataReport
            //
            this.mniReportLocalPartnerDataReport.Name = "mniReportLocalPartnerDataReport";
            this.mniReportLocalPartnerDataReport.AutoSize = true;
            this.mniReportLocalPartnerDataReport.Click += new System.EventHandler(this.OpenScreenReportLocalPartnerDataReport);
            this.mniReportLocalPartnerDataReport.Text = "&Local Partner Data";
            //
            // mniReports
            //
            this.mniReports.Name = "mniReports";
            this.mniReports.AutoSize = true;
            this.mniReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniReportPartnerByCity,
                        mniReportPartnerAddress,
                        mniReportBriefFoundation,
                        mniReportSupportingChurches,
                        mniReportValidBankAccount,
                        mniReportPublicationStatisticalReport,
                        mniReportBulkAddressReport,
                        mniReportPartnerContactReport,
                        mniReportSubscriptionReport,
                        mniReportLocalPartnerDataReport});
            this.mniReports.Text = "&Reports...";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "-";
            //
            // mniNewPartner
            //
            this.mniNewPartner.Name = "mniNewPartner";
            this.mniNewPartner.AutoSize = true;
            this.mniNewPartner.Click += new System.EventHandler(this.NewPartner);
            this.mniNewPartner.Text = "New &Partner...";
            //
            // mniNewPerson
            //
            this.mniNewPerson.Name = "mniNewPerson";
            this.mniNewPerson.AutoSize = true;
            this.mniNewPerson.Text = "Add &Person";
            //
            // mniNewFamily
            //
            this.mniNewFamily.Name = "mniNewFamily";
            this.mniNewFamily.AutoSize = true;
            this.mniNewFamily.Text = "Add &Family";
            //
            // mniNewChurch
            //
            this.mniNewChurch.Name = "mniNewChurch";
            this.mniNewChurch.AutoSize = true;
            this.mniNewChurch.Text = "Add &Church";
            //
            // mniNewPartnerAssistant
            //
            this.mniNewPartnerAssistant.Name = "mniNewPartnerAssistant";
            this.mniNewPartnerAssistant.AutoSize = true;
            this.mniNewPartnerAssistant.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniNewPerson,
                        mniNewFamily,
                        mniNewChurch});
            this.mniNewPartnerAssistant.Text = "&New Partner (assistant)";
            //
            // mniMergePartners
            //
            this.mniMergePartners.Name = "mniMergePartners";
            this.mniMergePartners.AutoSize = true;
            this.mniMergePartners.Text = "&Merge Partners";
            //
            // mniDeletePartner
            //
            this.mniDeletePartner.Name = "mniDeletePartner";
            this.mniDeletePartner.AutoSize = true;
            this.mniDeletePartner.Text = "D&elete Partner";
            //
            // mniSeparator4
            //
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.AutoSize = true;
            this.mniSeparator4.Text = "-";
            //
            // mniCheckDuplicateAddresses
            //
            this.mniCheckDuplicateAddresses.Name = "mniCheckDuplicateAddresses";
            this.mniCheckDuplicateAddresses.AutoSize = true;
            this.mniCheckDuplicateAddresses.Text = "&Duplicate Address Check";
            //
            // mniMergeAddresses
            //
            this.mniMergeAddresses.Name = "mniMergeAddresses";
            this.mniMergeAddresses.AutoSize = true;
            this.mniMergeAddresses.Text = "Mer&ge Addresses";
            //
            // mniViewPartnersAtLocation
            //
            this.mniViewPartnersAtLocation.Name = "mniViewPartnersAtLocation";
            this.mniViewPartnersAtLocation.AutoSize = true;
            this.mniViewPartnersAtLocation.Text = "&View Partners at Location";
            //
            // mniPartner
            //
            this.mniPartner.Name = "mniPartner";
            this.mniPartner.AutoSize = true;
            this.mniPartner.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniFindMaintain,
                        mniSeparator1,
                        mniLastPartner,
                        mniLastPartners,
                        mniSeparator2,
                        mniExtracts,
                        mniReports,
                        mniSeparator3,
                        mniNewPartner,
                        mniNewPartnerAssistant,
                        mniMergePartners,
                        mniDeletePartner,
                        mniSeparator4,
                        mniCheckDuplicateAddresses,
                        mniMergeAddresses,
                        mniViewPartnersAtLocation});
            this.mniPartner.Text = "P&artner";
            //
            // mniLabelPrint
            //
            this.mniLabelPrint.Name = "mniLabelPrint";
            this.mniLabelPrint.AutoSize = true;
            this.mniLabelPrint.Text = "&Label Print";
            //
            // mniMailsortLabelPrint
            //
            this.mniMailsortLabelPrint.Name = "mniMailsortLabelPrint";
            this.mniMailsortLabelPrint.AutoSize = true;
            this.mniMailsortLabelPrint.Text = "Mails&ort Label Print";
            //
            // mniSeparator5
            //
            this.mniSeparator5.Name = "mniSeparator5";
            this.mniSeparator5.AutoSize = true;
            this.mniSeparator5.Text = "-";
            //
            // mniSubscriptionExpiryNotices
            //
            this.mniSubscriptionExpiryNotices.Name = "mniSubscriptionExpiryNotices";
            this.mniSubscriptionExpiryNotices.AutoSize = true;
            this.mniSubscriptionExpiryNotices.Text = "Subscription Expiry &Notices";
            //
            // mniSubscriptionCancellation
            //
            this.mniSubscriptionCancellation.Name = "mniSubscriptionCancellation";
            this.mniSubscriptionCancellation.AutoSize = true;
            this.mniSubscriptionCancellation.Text = "Subscription &Cancellation";
            //
            // mniSeparator6
            //
            this.mniSeparator6.Name = "mniSeparator6";
            this.mniSeparator6.AutoSize = true;
            this.mniSeparator6.Text = "-";
            //
            // mniFormLetterPrint
            //
            this.mniFormLetterPrint.Name = "mniFormLetterPrint";
            this.mniFormLetterPrint.AutoSize = true;
            this.mniFormLetterPrint.Text = "&Form Letter Print";
            //
            // mniSeparator7
            //
            this.mniSeparator7.Name = "mniSeparator7";
            this.mniSeparator7.AutoSize = true;
            this.mniSeparator7.Text = "-";
            //
            // mniExtractMailMerge
            //
            this.mniExtractMailMerge.Name = "mniExtractMailMerge";
            this.mniExtractMailMerge.AutoSize = true;
            this.mniExtractMailMerge.Text = "Extrac&t Mail Merge";
            //
            // mniMailing
            //
            this.mniMailing.Name = "mniMailing";
            this.mniMailing.AutoSize = true;
            this.mniMailing.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniLabelPrint,
                        mniMailsortLabelPrint,
                        mniSeparator5,
                        mniSubscriptionExpiryNotices,
                        mniSubscriptionCancellation,
                        mniSeparator6,
                        mniFormLetterPrint,
                        mniSeparator7,
                        mniExtractMailMerge});
            this.mniMailing.Text = "Mailin&g";
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
            // mniSeparator8
            //
            this.mniSeparator8.Name = "mniSeparator8";
            this.mniSeparator8.AutoSize = true;
            this.mniSeparator8.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator9
            //
            this.mniSeparator9.Name = "mniSeparator9";
            this.mniSeparator9.AutoSize = true;
            this.mniSeparator9.Text = "-";
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
                        mniSeparator8,
                        mniHelpBugReport,
                        mniSeparator9,
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
                        mniMailing,
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
            // TFrmPartnerMain
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(510, 476);

            this.Controls.Add(this.pnlTODO);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmPartnerMain";
            this.Text = "Partner Module OpenPetra.org";

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
        private System.Windows.Forms.ToolStripMenuItem mniFindMaintain;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniLastPartner;
        private System.Windows.Forms.ToolStripMenuItem mniLastPartners;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniExtracts;
        private System.Windows.Forms.ToolStripMenuItem mniReports;
        private System.Windows.Forms.ToolStripMenuItem mniReportPartnerByCity;
        private System.Windows.Forms.ToolStripMenuItem mniReportPartnerAddress;
        private System.Windows.Forms.ToolStripMenuItem mniReportBriefFoundation;
        private System.Windows.Forms.ToolStripMenuItem mniReportSupportingChurches;
        private System.Windows.Forms.ToolStripMenuItem mniReportValidBankAccount;
        private System.Windows.Forms.ToolStripMenuItem mniReportPublicationStatisticalReport;
        private System.Windows.Forms.ToolStripMenuItem mniReportBulkAddressReport;
        private System.Windows.Forms.ToolStripMenuItem mniReportPartnerContactReport;
        private System.Windows.Forms.ToolStripMenuItem mniReportSubscriptionReport;
        private System.Windows.Forms.ToolStripMenuItem mniReportLocalPartnerDataReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniNewPartner;
        private System.Windows.Forms.ToolStripMenuItem mniNewPartnerAssistant;
        private System.Windows.Forms.ToolStripMenuItem mniNewPerson;
        private System.Windows.Forms.ToolStripMenuItem mniNewFamily;
        private System.Windows.Forms.ToolStripMenuItem mniNewChurch;
        private System.Windows.Forms.ToolStripMenuItem mniMergePartners;
        private System.Windows.Forms.ToolStripMenuItem mniDeletePartner;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniCheckDuplicateAddresses;
        private System.Windows.Forms.ToolStripMenuItem mniMergeAddresses;
        private System.Windows.Forms.ToolStripMenuItem mniViewPartnersAtLocation;
        private System.Windows.Forms.ToolStripMenuItem mniMailing;
        private System.Windows.Forms.ToolStripMenuItem mniLabelPrint;
        private System.Windows.Forms.ToolStripMenuItem mniMailsortLabelPrint;
        private System.Windows.Forms.ToolStripSeparator mniSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mniSubscriptionExpiryNotices;
        private System.Windows.Forms.ToolStripMenuItem mniSubscriptionCancellation;
        private System.Windows.Forms.ToolStripSeparator mniSeparator6;
        private System.Windows.Forms.ToolStripMenuItem mniFormLetterPrint;
        private System.Windows.Forms.ToolStripSeparator mniSeparator7;
        private System.Windows.Forms.ToolStripMenuItem mniExtractMailMerge;
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
        private System.Windows.Forms.ToolStripSeparator mniSeparator8;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator9;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
