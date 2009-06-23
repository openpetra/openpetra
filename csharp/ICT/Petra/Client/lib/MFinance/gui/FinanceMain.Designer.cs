/* auto generated with nant generateWinforms from FinanceMain.yaml
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
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
using System.Windows.Forms;
using Mono.Unix;
using Ict.Common.Controls;
//using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MFinance.Gui
{
    partial class TFrmFinanceMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmFinanceMain));

            this.pnlTODO = new System.Windows.Forms.Panel();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSelectLedger = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewLedger = new System.Windows.Forms.ToolStripMenuItem();
            this.mniDeleteLedger = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLedger = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLedgerSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSetupParameters = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSetupTables = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSetupTablesTodo = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSetupCostCentres = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSetupAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGLBatch = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGLCurrentPeriod = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGLPreviousPeriods = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniGLRecurring = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGLImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGLExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPeriodEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMonthEndClosing = new System.Windows.Forms.ToolStripMenuItem();
            this.mniYearEndClosing = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniGiftReceipting = new System.Windows.Forms.ToolStripMenuItem();
            this.mniImportBankStatements = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGiftBatch = new System.Windows.Forms.ToolStripMenuItem();
            this.mniRecurringGiftBatch = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniGiftImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGiftExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniGiftReceipts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAccountsPayable = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBudget = new System.Windows.Forms.ToolStripMenuItem();
            this.mniClearingHouse = new System.Windows.Forms.ToolStripMenuItem();
            this.mniICHCalculation = new System.Windows.Forms.ToolStripMenuItem();
            this.mniICHImportFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniICHReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGLReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGiftReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAPReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniConsolidations = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFinanceSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSetupCurrency = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSetupCorporateExchangeRate = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSetupDailyExchangeRate = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSetupMethodOfGiving = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSetupMethodOfPayment = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniSetupAnalysisTypes = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mniSetupLetters = new System.Windows.Forms.ToolStripMenuItem();
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
            this.mniSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator8 = new System.Windows.Forms.ToolStripSeparator();
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
            //
            // mniSelectLedger
            //
            this.mniSelectLedger.Name = "mniSelectLedger";
            this.mniSelectLedger.AutoSize = true;
            this.mniSelectLedger.Text = "&Select Ledger";
            //
            // mniNewLedger
            //
            this.mniNewLedger.Name = "mniNewLedger";
            this.mniNewLedger.AutoSize = true;
            this.mniNewLedger.Text = "&New Ledger";
            //
            // mniDeleteLedger
            //
            this.mniDeleteLedger.Name = "mniDeleteLedger";
            this.mniDeleteLedger.AutoSize = true;
            this.mniDeleteLedger.Text = "&Delete Ledger";
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
            this.mniClose.Click += new System.EventHandler(this.mniCloseClick);
            this.mniClose.Image = ((System.Drawing.Bitmap)resources.GetObject("mniClose.Glyph"));
            this.mniClose.ToolTipText = "Closes this window";
            this.mniClose.Text = "&Close";
            //
            // mniFile
            //
            this.mniFile.Name = "mniFile";
            this.mniFile.AutoSize = true;
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniSelectLedger,
                        mniNewLedger,
                        mniDeleteLedger,
                        mniSeparator0,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniSetupParameters
            //
            this.mniSetupParameters.Name = "mniSetupParameters";
            this.mniSetupParameters.AutoSize = true;
            this.mniSetupParameters.Text = "&Parameters";
            //
            // mniSetupTablesTodo
            //
            this.mniSetupTablesTodo.Name = "mniSetupTablesTodo";
            this.mniSetupTablesTodo.AutoSize = true;
            this.mniSetupTablesTodo.Text = "&Todo";
            //
            // mniSetupTables
            //
            this.mniSetupTables.Name = "mniSetupTables";
            this.mniSetupTables.AutoSize = true;
            this.mniSetupTables.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniSetupTablesTodo});
            this.mniSetupTables.Text = "&Tables";
            //
            // mniSetupCostCentres
            //
            this.mniSetupCostCentres.Name = "mniSetupCostCentres";
            this.mniSetupCostCentres.AutoSize = true;
            this.mniSetupCostCentres.Text = "&Cost Centres";
            //
            // mniSetupAccounts
            //
            this.mniSetupAccounts.Name = "mniSetupAccounts";
            this.mniSetupAccounts.AutoSize = true;
            this.mniSetupAccounts.Text = "&Accounts";
            //
            // mniLedgerSetup
            //
            this.mniLedgerSetup.Name = "mniLedgerSetup";
            this.mniLedgerSetup.AutoSize = true;
            this.mniLedgerSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniSetupParameters,
                        mniSetupTables,
                        mniSetupCostCentres,
                        mniSetupAccounts});
            this.mniLedgerSetup.Text = "&Setup";
            //
            // mniGLCurrentPeriod
            //
            this.mniGLCurrentPeriod.Name = "mniGLCurrentPeriod";
            this.mniGLCurrentPeriod.AutoSize = true;
            this.mniGLCurrentPeriod.Text = "&Current Period";
            //
            // mniGLPreviousPeriods
            //
            this.mniGLPreviousPeriods.Name = "mniGLPreviousPeriods";
            this.mniGLPreviousPeriods.AutoSize = true;
            this.mniGLPreviousPeriods.Text = "&Previous Periods";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
            //
            // mniGLRecurring
            //
            this.mniGLRecurring.Name = "mniGLRecurring";
            this.mniGLRecurring.AutoSize = true;
            this.mniGLRecurring.Text = "&Recurring";
            //
            // mniGLImport
            //
            this.mniGLImport.Name = "mniGLImport";
            this.mniGLImport.AutoSize = true;
            this.mniGLImport.Text = "&Import";
            //
            // mniGLExport
            //
            this.mniGLExport.Name = "mniGLExport";
            this.mniGLExport.AutoSize = true;
            this.mniGLExport.Text = "E&xport";
            //
            // mniGLBatch
            //
            this.mniGLBatch.Name = "mniGLBatch";
            this.mniGLBatch.AutoSize = true;
            this.mniGLBatch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniGLCurrentPeriod,
                        mniGLPreviousPeriods,
                        mniSeparator1,
                        mniGLRecurring,
                        mniGLImport,
                        mniGLExport});
            this.mniGLBatch.Text = "G&L Batch";
            //
            // mniMonthEndClosing
            //
            this.mniMonthEndClosing.Name = "mniMonthEndClosing";
            this.mniMonthEndClosing.AutoSize = true;
            this.mniMonthEndClosing.Text = "&Month End Closing";
            //
            // mniYearEndClosing
            //
            this.mniYearEndClosing.Name = "mniYearEndClosing";
            this.mniYearEndClosing.AutoSize = true;
            this.mniYearEndClosing.Text = "&Year End Closing";
            //
            // mniPeriodEnd
            //
            this.mniPeriodEnd.Name = "mniPeriodEnd";
            this.mniPeriodEnd.AutoSize = true;
            this.mniPeriodEnd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniMonthEndClosing,
                        mniYearEndClosing});
            this.mniPeriodEnd.Text = "&Period End";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
            //
            // mniImportBankStatements
            //
            this.mniImportBankStatements.Name = "mniImportBankStatements";
            this.mniImportBankStatements.AutoSize = true;
            this.mniImportBankStatements.Click += new System.EventHandler(this.mniImportBankStatementsClick);
            this.mniImportBankStatements.Text = "Import &Bank statements";
            //
            // mniGiftBatch
            //
            this.mniGiftBatch.Name = "mniGiftBatch";
            this.mniGiftBatch.AutoSize = true;
            this.mniGiftBatch.Text = "&Gift Batch";
            //
            // mniRecurringGiftBatch
            //
            this.mniRecurringGiftBatch.Name = "mniRecurringGiftBatch";
            this.mniRecurringGiftBatch.AutoSize = true;
            this.mniRecurringGiftBatch.Text = "&Recurring Gift Batch";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "-";
            //
            // mniGiftImport
            //
            this.mniGiftImport.Name = "mniGiftImport";
            this.mniGiftImport.AutoSize = true;
            this.mniGiftImport.Text = "Gift &Import";
            //
            // mniGiftExport
            //
            this.mniGiftExport.Name = "mniGiftExport";
            this.mniGiftExport.AutoSize = true;
            this.mniGiftExport.Text = "Gift E&xport";
            //
            // mniSeparator4
            //
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.AutoSize = true;
            this.mniSeparator4.Text = "-";
            //
            // mniGiftReceipts
            //
            this.mniGiftReceipts.Name = "mniGiftReceipts";
            this.mniGiftReceipts.AutoSize = true;
            this.mniGiftReceipts.Text = "&Print Receipts";
            //
            // mniGiftReceipting
            //
            this.mniGiftReceipting.Name = "mniGiftReceipting";
            this.mniGiftReceipting.AutoSize = true;
            this.mniGiftReceipting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniImportBankStatements,
                        mniGiftBatch,
                        mniRecurringGiftBatch,
                        mniSeparator3,
                        mniGiftImport,
                        mniGiftExport,
                        mniSeparator4,
                        mniGiftReceipts});
            this.mniGiftReceipting.Text = "&Gift Receipting";
            //
            // mniAccountsPayable
            //
            this.mniAccountsPayable.Name = "mniAccountsPayable";
            this.mniAccountsPayable.AutoSize = true;
            this.mniAccountsPayable.Click += new System.EventHandler(this.mniAccountsPayableClick);
            this.mniAccountsPayable.Text = "&Accounts Payable";
            //
            // mniBudget
            //
            this.mniBudget.Name = "mniBudget";
            this.mniBudget.AutoSize = true;
            this.mniBudget.Text = "&Budget";
            //
            // mniICHCalculation
            //
            this.mniICHCalculation.Name = "mniICHCalculation";
            this.mniICHCalculation.AutoSize = true;
            this.mniICHCalculation.Text = "ICH &calculation";
            //
            // mniICHImportFile
            //
            this.mniICHImportFile.Name = "mniICHImportFile";
            this.mniICHImportFile.AutoSize = true;
            this.mniICHImportFile.Text = "&Import file from field";
            //
            // mniICHReports
            //
            this.mniICHReports.Name = "mniICHReports";
            this.mniICHReports.AutoSize = true;
            this.mniICHReports.Text = "ICH &reports...";
            //
            // mniClearingHouse
            //
            this.mniClearingHouse.Name = "mniClearingHouse";
            this.mniClearingHouse.AutoSize = true;
            this.mniClearingHouse.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniICHCalculation,
                        mniICHImportFile,
                        mniICHReports});
            this.mniClearingHouse.Text = "&Clearing House";
            //
            // mniGLReports
            //
            this.mniGLReports.Name = "mniGLReports";
            this.mniGLReports.AutoSize = true;
            this.mniGLReports.Text = "&GL";
            //
            // mniGiftReports
            //
            this.mniGiftReports.Name = "mniGiftReports";
            this.mniGiftReports.AutoSize = true;
            this.mniGiftReports.Text = "G&if";
            //
            // mniAPReports
            //
            this.mniAPReports.Name = "mniAPReports";
            this.mniAPReports.AutoSize = true;
            this.mniAPReports.Text = "Accounts &Payable";
            //
            // mniReports
            //
            this.mniReports.Name = "mniReports";
            this.mniReports.AutoSize = true;
            this.mniReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniGLReports,
                        mniGiftReports,
                        mniAPReports});
            this.mniReports.Text = "&Reports";
            //
            // mniConsolidations
            //
            this.mniConsolidations.Name = "mniConsolidations";
            this.mniConsolidations.AutoSize = true;
            this.mniConsolidations.Text = "&Consolidations";
            //
            // mniSetupCurrency
            //
            this.mniSetupCurrency.Name = "mniSetupCurrency";
            this.mniSetupCurrency.AutoSize = true;
            this.mniSetupCurrency.Text = "&Currency";
            //
            // mniSetupCorporateExchangeRate
            //
            this.mniSetupCorporateExchangeRate.Name = "mniSetupCorporateExchangeRate";
            this.mniSetupCorporateExchangeRate.AutoSize = true;
            this.mniSetupCorporateExchangeRate.Text = "C&orporate Exchange Rate";
            //
            // mniSetupDailyExchangeRate
            //
            this.mniSetupDailyExchangeRate.Name = "mniSetupDailyExchangeRate";
            this.mniSetupDailyExchangeRate.AutoSize = true;
            this.mniSetupDailyExchangeRate.Text = "&Daily Exchange Rate";
            //
            // mniSetupMethodOfGiving
            //
            this.mniSetupMethodOfGiving.Name = "mniSetupMethodOfGiving";
            this.mniSetupMethodOfGiving.AutoSize = true;
            this.mniSetupMethodOfGiving.Text = "Method of &Giving";
            //
            // mniSetupMethodOfPayment
            //
            this.mniSetupMethodOfPayment.Name = "mniSetupMethodOfPayment";
            this.mniSetupMethodOfPayment.AutoSize = true;
            this.mniSetupMethodOfPayment.Text = "Method of &Payment";
            //
            // mniSeparator5
            //
            this.mniSeparator5.Name = "mniSeparator5";
            this.mniSeparator5.AutoSize = true;
            this.mniSeparator5.Text = "-";
            //
            // mniSetupAnalysisTypes
            //
            this.mniSetupAnalysisTypes.Name = "mniSetupAnalysisTypes";
            this.mniSetupAnalysisTypes.AutoSize = true;
            this.mniSetupAnalysisTypes.Text = "A&nalysis Types";
            //
            // mniSeparator6
            //
            this.mniSeparator6.Name = "mniSeparator6";
            this.mniSeparator6.AutoSize = true;
            this.mniSeparator6.Text = "-";
            //
            // mniSetupLetters
            //
            this.mniSetupLetters.Name = "mniSetupLetters";
            this.mniSetupLetters.AutoSize = true;
            this.mniSetupLetters.Text = "&Letters";
            //
            // mniFinanceSetup
            //
            this.mniFinanceSetup.Name = "mniFinanceSetup";
            this.mniFinanceSetup.AutoSize = true;
            this.mniFinanceSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniSetupCurrency,
                        mniSetupCorporateExchangeRate,
                        mniSetupDailyExchangeRate,
                        mniSetupMethodOfGiving,
                        mniSetupMethodOfPayment,
                        mniSeparator5,
                        mniSetupAnalysisTypes,
                        mniSeparator6,
                        mniSetupLetters});
            this.mniFinanceSetup.Text = "&Setup";
            //
            // mniLedger
            //
            this.mniLedger.Name = "mniLedger";
            this.mniLedger.AutoSize = true;
            this.mniLedger.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniLedgerSetup,
                        mniGLBatch,
                        mniPeriodEnd,
                        mniSeparator2,
                        mniGiftReceipting,
                        mniAccountsPayable,
                        mniBudget,
                        mniClearingHouse,
                        mniReports,
                        mniConsolidations,
                        mniFinanceSetup});
            this.mniLedger.Text = "&Ledger";
            //
            // mniPetraMainMenu
            //
            this.mniPetraMainMenu.Name = "mniPetraMainMenu";
            this.mniPetraMainMenu.AutoSize = true;
            this.mniPetraMainMenu.Click += new System.EventHandler(this.mniPetraMainMenuClick);
            this.mniPetraMainMenu.Text = "Petra &Main Menu";
            //
            // mniPetraPartnerModule
            //
            this.mniPetraPartnerModule.Name = "mniPetraPartnerModule";
            this.mniPetraPartnerModule.AutoSize = true;
            this.mniPetraPartnerModule.Click += new System.EventHandler(this.mniPetraPartnerModuleClick);
            this.mniPetraPartnerModule.Text = "Pa&rtner";
            //
            // mniPetraFinanceModule
            //
            this.mniPetraFinanceModule.Name = "mniPetraFinanceModule";
            this.mniPetraFinanceModule.AutoSize = true;
            this.mniPetraFinanceModule.Click += new System.EventHandler(this.mniPetraFinanceModuleClick);
            this.mniPetraFinanceModule.Text = "&Finance";
            //
            // mniPetraPersonnelModule
            //
            this.mniPetraPersonnelModule.Name = "mniPetraPersonnelModule";
            this.mniPetraPersonnelModule.AutoSize = true;
            this.mniPetraPersonnelModule.Click += new System.EventHandler(this.mniPetraPersonnelModuleClick);
            this.mniPetraPersonnelModule.Text = "P&ersonnel";
            //
            // mniPetraConferenceModule
            //
            this.mniPetraConferenceModule.Name = "mniPetraConferenceModule";
            this.mniPetraConferenceModule.AutoSize = true;
            this.mniPetraConferenceModule.Click += new System.EventHandler(this.mniPetraConferenceModuleClick);
            this.mniPetraConferenceModule.Text = "C&onference";
            //
            // mniPetraFinDevModule
            //
            this.mniPetraFinDevModule.Name = "mniPetraFinDevModule";
            this.mniPetraFinDevModule.AutoSize = true;
            this.mniPetraFinDevModule.Click += new System.EventHandler(this.mniPetraFinDevModuleClick);
            this.mniPetraFinDevModule.Text = "Financial &Development";
            //
            // mniPetraSysManModule
            //
            this.mniPetraSysManModule.Name = "mniPetraSysManModule";
            this.mniPetraSysManModule.AutoSize = true;
            this.mniPetraSysManModule.Click += new System.EventHandler(this.mniPetraSysManModuleClick);
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
            this.mniPetraModules.Text = "&Petra";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator7
            //
            this.mniSeparator7.Name = "mniSeparator7";
            this.mniSeparator7.AutoSize = true;
            this.mniSeparator7.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator8
            //
            this.mniSeparator8.Name = "mniSeparator8";
            this.mniSeparator8.AutoSize = true;
            this.mniSeparator8.Text = "-";
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
                        mniSeparator7,
                        mniHelpBugReport,
                        mniSeparator8,
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
                        mniLedger,
                        mniPetraModules,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmFinanceMain
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 476);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlTODO);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmFinanceMain";
            this.Text = "Finance Module OpenPetra.org";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.pnlTODO.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlTODO;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniSelectLedger;
        private System.Windows.Forms.ToolStripMenuItem mniNewLedger;
        private System.Windows.Forms.ToolStripMenuItem mniDeleteLedger;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniLedger;
        private System.Windows.Forms.ToolStripMenuItem mniLedgerSetup;
        private System.Windows.Forms.ToolStripMenuItem mniSetupParameters;
        private System.Windows.Forms.ToolStripMenuItem mniSetupTables;
        private System.Windows.Forms.ToolStripMenuItem mniSetupTablesTodo;
        private System.Windows.Forms.ToolStripMenuItem mniSetupCostCentres;
        private System.Windows.Forms.ToolStripMenuItem mniSetupAccounts;
        private System.Windows.Forms.ToolStripMenuItem mniGLBatch;
        private System.Windows.Forms.ToolStripMenuItem mniGLCurrentPeriod;
        private System.Windows.Forms.ToolStripMenuItem mniGLPreviousPeriods;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniGLRecurring;
        private System.Windows.Forms.ToolStripMenuItem mniGLImport;
        private System.Windows.Forms.ToolStripMenuItem mniGLExport;
        private System.Windows.Forms.ToolStripMenuItem mniPeriodEnd;
        private System.Windows.Forms.ToolStripMenuItem mniMonthEndClosing;
        private System.Windows.Forms.ToolStripMenuItem mniYearEndClosing;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniGiftReceipting;
        private System.Windows.Forms.ToolStripMenuItem mniImportBankStatements;
        private System.Windows.Forms.ToolStripMenuItem mniGiftBatch;
        private System.Windows.Forms.ToolStripMenuItem mniRecurringGiftBatch;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniGiftImport;
        private System.Windows.Forms.ToolStripMenuItem mniGiftExport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniGiftReceipts;
        private System.Windows.Forms.ToolStripMenuItem mniAccountsPayable;
        private System.Windows.Forms.ToolStripMenuItem mniBudget;
        private System.Windows.Forms.ToolStripMenuItem mniClearingHouse;
        private System.Windows.Forms.ToolStripMenuItem mniICHCalculation;
        private System.Windows.Forms.ToolStripMenuItem mniICHImportFile;
        private System.Windows.Forms.ToolStripMenuItem mniICHReports;
        private System.Windows.Forms.ToolStripMenuItem mniReports;
        private System.Windows.Forms.ToolStripMenuItem mniGLReports;
        private System.Windows.Forms.ToolStripMenuItem mniGiftReports;
        private System.Windows.Forms.ToolStripMenuItem mniAPReports;
        private System.Windows.Forms.ToolStripMenuItem mniConsolidations;
        private System.Windows.Forms.ToolStripMenuItem mniFinanceSetup;
        private System.Windows.Forms.ToolStripMenuItem mniSetupCurrency;
        private System.Windows.Forms.ToolStripMenuItem mniSetupCorporateExchangeRate;
        private System.Windows.Forms.ToolStripMenuItem mniSetupDailyExchangeRate;
        private System.Windows.Forms.ToolStripMenuItem mniSetupMethodOfGiving;
        private System.Windows.Forms.ToolStripMenuItem mniSetupMethodOfPayment;
        private System.Windows.Forms.ToolStripSeparator mniSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mniSetupAnalysisTypes;
        private System.Windows.Forms.ToolStripSeparator mniSeparator6;
        private System.Windows.Forms.ToolStripMenuItem mniSetupLetters;
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
        private System.Windows.Forms.ToolStripSeparator mniSeparator7;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator8;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
