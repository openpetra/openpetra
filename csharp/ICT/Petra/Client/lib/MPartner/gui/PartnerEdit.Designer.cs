// auto generated with nant generateWinforms from PartnerEdit.yaml
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
    partial class TPartnerEditDSWinForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPartnerEditDSWinForm));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.ucoUpperPart = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerEdit_CollapsiblePart();
            this.pnlLowerPart = new System.Windows.Forms.Panel();
            this.ucoPartnerTabSet = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerEdit_PartnerTabSet();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbSave = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbNew = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbTogglePartner = new System.Windows.Forms.ToolStripButton();
            this.tbbTogglePersonnel = new System.Windows.Forms.ToolStripButton();
            this.tbbToggleFinance = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileDeactivatePartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileDeletePartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileCopyPartnerKey = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileCopyAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSendEmail = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFilePrintSection = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileExportPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditUndoCurrentField = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditUndoScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniEditFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditFindNewAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.mniView = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewPartnerData = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewPersonnelData = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewFinanceData = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniViewUpperScreenPart = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewUpperPartExpanded = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewUpperPartCollapsed = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintain = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainAddresses = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainPartnerDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainFoundationDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainSubscriptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainSpecialTypes = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainContacts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainFamilyMembers = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainRelationships = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainInterests = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainReminders = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainNotes = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainOfficeSpecific = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainWorkerField = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mniMaintainPersonnelIndividualData = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mniMaintainDonorHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainRecipientHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainFinanceReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainBankAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainGiftReceipting = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainFinanceDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlContent.SuspendLayout();
            this.pnlLowerPart.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.pnlLowerPart);
            this.pnlContent.Controls.Add(this.ucoUpperPart);
            //
            // ucoUpperPart
            //
            this.ucoUpperPart.Name = "ucoUpperPart";
            this.ucoUpperPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucoUpperPart.AutoSize = true;
            //
            // pnlLowerPart
            //
            this.pnlLowerPart.Name = "pnlLowerPart";
            this.pnlLowerPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLowerPart.AutoSize = true;
            this.pnlLowerPart.Controls.Add(this.ucoPartnerTabSet);
            //
            // ucoPartnerTabSet
            //
            this.ucoPartnerTabSet.Name = "ucoPartnerTabSet";
            this.ucoPartnerTabSet.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tbbSave
            //
            this.tbbSave.Name = "tbbSave";
            this.tbbSave.AutoSize = true;
            this.tbbSave.Click += new System.EventHandler(this.FileSave);
            this.tbbSave.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbSave.Glyph"));
            this.tbbSave.ToolTipText = "Saves changed data";
            this.tbbSave.Text = "&Save";
            //
            // tbbSeparator0
            //
            this.tbbSeparator0.Name = "tbbSeparator0";
            this.tbbSeparator0.AutoSize = true;
            this.tbbSeparator0.Text = "-";
            //
            // tbbNew
            //
            this.tbbNew.Name = "tbbNew";
            this.tbbNew.AutoSize = true;
            this.tbbNew.Text = "New Partner";
            //
            // tbbSeparator1
            //
            this.tbbSeparator1.Name = "tbbSeparator1";
            this.tbbSeparator1.AutoSize = true;
            this.tbbSeparator1.Text = "-";
            //
            // tbbTogglePartner
            //
            this.tbbTogglePartner.Name = "tbbTogglePartner";
            this.tbbTogglePartner.AutoSize = true;
            this.tbbTogglePartner.Text = "Partner Data";
            //
            // tbbTogglePersonnel
            //
            this.tbbTogglePersonnel.Name = "tbbTogglePersonnel";
            this.tbbTogglePersonnel.AutoSize = true;
            this.tbbTogglePersonnel.Text = "Personnel Data";
            //
            // tbbToggleFinance
            //
            this.tbbToggleFinance.Name = "tbbToggleFinance";
            this.tbbToggleFinance.AutoSize = true;
            this.tbbToggleFinance.Text = "Finance Data";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbSave,
                        tbbSeparator0,
                        tbbNew,
                        tbbSeparator1,
                        tbbTogglePartner,
                        tbbTogglePersonnel,
                        tbbToggleFinance});
            //
            // mniFileSave
            //
            this.mniFileSave.Name = "mniFileSave";
            this.mniFileSave.AutoSize = true;
            this.mniFileSave.Click += new System.EventHandler(this.FileSave);
            this.mniFileSave.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileSave.Glyph"));
            this.mniFileSave.ToolTipText = "Saves changed data";
            this.mniFileSave.Text = "&Save";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "-";
            //
            // mniFilePrint
            //
            this.mniFilePrint.Name = "mniFilePrint";
            this.mniFilePrint.AutoSize = true;
            this.mniFilePrint.Text = "&Print...";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
            //
            // mniFileNew
            //
            this.mniFileNew.Name = "mniFileNew";
            this.mniFileNew.AutoSize = true;
            this.mniFileNew.Text = "&New Partner...";
            //
            // mniFileDeactivatePartner
            //
            this.mniFileDeactivatePartner.Name = "mniFileDeactivatePartner";
            this.mniFileDeactivatePartner.AutoSize = true;
            this.mniFileDeactivatePartner.Text = "Deacti&vate Partner...";
            //
            // mniFileDeletePartner
            //
            this.mniFileDeletePartner.Name = "mniFileDeletePartner";
            this.mniFileDeletePartner.AutoSize = true;
            this.mniFileDeletePartner.Text = "&Delete THIS Partner...";
            //
            // mniFileCopyPartnerKey
            //
            this.mniFileCopyPartnerKey.Name = "mniFileCopyPartnerKey";
            this.mniFileCopyPartnerKey.AutoSize = true;
            this.mniFileCopyPartnerKey.Text = "Copy Partner's Partner &Key";
            //
            // mniFileCopyAddress
            //
            this.mniFileCopyAddress.Name = "mniFileCopyAddress";
            this.mniFileCopyAddress.AutoSize = true;
            this.mniFileCopyAddress.Text = "Copy Partner's &Address...";
            //
            // mniFileSendEmail
            //
            this.mniFileSendEmail.Name = "mniFileSendEmail";
            this.mniFileSendEmail.AutoSize = true;
            this.mniFileSendEmail.Text = "&Send Email";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
            //
            // mniFilePrintSection
            //
            this.mniFilePrintSection.Name = "mniFilePrintSection";
            this.mniFilePrintSection.AutoSize = true;
            this.mniFilePrintSection.Text = "P&rint Section...";
            //
            // mniFileExportPartner
            //
            this.mniFileExportPartner.Name = "mniFileExportPartner";
            this.mniFileExportPartner.AutoSize = true;
            this.mniFileExportPartner.Text = "&Export Partner";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "-";
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
                           mniFileSave,
                        mniSeparator0,
                        mniFilePrint,
                        mniSeparator1,
                        mniFileNew,
                        mniFileDeactivatePartner,
                        mniFileDeletePartner,
                        mniFileCopyPartnerKey,
                        mniFileCopyAddress,
                        mniFileSendEmail,
                        mniSeparator2,
                        mniFilePrintSection,
                        mniFileExportPartner,
                        mniSeparator3,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniEditUndoCurrentField
            //
            this.mniEditUndoCurrentField.Name = "mniEditUndoCurrentField";
            this.mniEditUndoCurrentField.AutoSize = true;
            this.mniEditUndoCurrentField.Text = "Undo &Current Field";
            //
            // mniEditUndoScreen
            //
            this.mniEditUndoScreen.Name = "mniEditUndoScreen";
            this.mniEditUndoScreen.AutoSize = true;
            this.mniEditUndoScreen.Text = "&Undo Screen";
            //
            // mniSeparator4
            //
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.AutoSize = true;
            this.mniSeparator4.Text = "-";
            //
            // mniEditFind
            //
            this.mniEditFind.Name = "mniEditFind";
            this.mniEditFind.AutoSize = true;
            this.mniEditFind.Text = "&Find...";
            //
            // mniEditFindNewAddress
            //
            this.mniEditFindNewAddress.Name = "mniEditFindNewAddress";
            this.mniEditFindNewAddress.AutoSize = true;
            this.mniEditFindNewAddress.Text = "Find New &Address...";
            //
            // mniEdit
            //
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.AutoSize = true;
            this.mniEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniEditUndoCurrentField,
                        mniEditUndoScreen,
                        mniSeparator4,
                        mniEditFind,
                        mniEditFindNewAddress});
            this.mniEdit.Text = "&Edit";
            //
            // mniViewPartnerData
            //
            this.mniViewPartnerData.Name = "mniViewPartnerData";
            this.mniViewPartnerData.AutoSize = true;
            this.mniViewPartnerData.Text = "&Partner Data";
            //
            // mniViewPersonnelData
            //
            this.mniViewPersonnelData.Name = "mniViewPersonnelData";
            this.mniViewPersonnelData.AutoSize = true;
            this.mniViewPersonnelData.Text = "P&ersonnel Data";
            //
            // mniViewFinanceData
            //
            this.mniViewFinanceData.Name = "mniViewFinanceData";
            this.mniViewFinanceData.AutoSize = true;
            this.mniViewFinanceData.Text = "&Finance Data";
            //
            // mniSeparator5
            //
            this.mniSeparator5.Name = "mniSeparator5";
            this.mniSeparator5.AutoSize = true;
            this.mniSeparator5.Text = "-";
            //
            // mniViewUpperPartExpanded
            //
            this.mniViewUpperPartExpanded.Name = "mniViewUpperPartExpanded";
            this.mniViewUpperPartExpanded.AutoSize = true;
            this.mniViewUpperPartExpanded.Text = "&Expanded";
            //
            // mniViewUpperPartCollapsed
            //
            this.mniViewUpperPartCollapsed.Name = "mniViewUpperPartCollapsed";
            this.mniViewUpperPartCollapsed.AutoSize = true;
            this.mniViewUpperPartCollapsed.Text = "&Collapsed";
            //
            // mniViewUpperScreenPart
            //
            this.mniViewUpperScreenPart.Name = "mniViewUpperScreenPart";
            this.mniViewUpperScreenPart.AutoSize = true;
            this.mniViewUpperScreenPart.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniViewUpperPartExpanded,
                        mniViewUpperPartCollapsed});
            this.mniViewUpperScreenPart.Text = "&Upper Screen Part";
            //
            // mniView
            //
            this.mniView.Name = "mniView";
            this.mniView.AutoSize = true;
            this.mniView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniViewPartnerData,
                        mniViewPersonnelData,
                        mniViewFinanceData,
                        mniSeparator5,
                        mniViewUpperScreenPart});
            this.mniView.Text = "View";
            //
            // mniMaintainAddresses
            //
            this.mniMaintainAddresses.Name = "mniMaintainAddresses";
            this.mniMaintainAddresses.AutoSize = true;
            this.mniMaintainAddresses.Text = "&Addresses";
            //
            // mniMaintainPartnerDetails
            //
            this.mniMaintainPartnerDetails.Name = "mniMaintainPartnerDetails";
            this.mniMaintainPartnerDetails.AutoSize = true;
            this.mniMaintainPartnerDetails.Text = "Partner &Details";
            //
            // mniMaintainFoundationDetails
            //
            this.mniMaintainFoundationDetails.Name = "mniMaintainFoundationDetails";
            this.mniMaintainFoundationDetails.AutoSize = true;
            this.mniMaintainFoundationDetails.Text = "Foundation Details";
            //
            // mniMaintainSubscriptions
            //
            this.mniMaintainSubscriptions.Name = "mniMaintainSubscriptions";
            this.mniMaintainSubscriptions.AutoSize = true;
            this.mniMaintainSubscriptions.Text = "&Subscriptions";
            //
            // mniMaintainSpecialTypes
            //
            this.mniMaintainSpecialTypes.Name = "mniMaintainSpecialTypes";
            this.mniMaintainSpecialTypes.AutoSize = true;
            this.mniMaintainSpecialTypes.Text = "Special &Types";
            //
            // mniMaintainContacts
            //
            this.mniMaintainContacts.Name = "mniMaintainContacts";
            this.mniMaintainContacts.AutoSize = true;
            this.mniMaintainContacts.Text = "&Contacts...";
            //
            // mniMaintainFamilyMembers
            //
            this.mniMaintainFamilyMembers.Name = "mniMaintainFamilyMembers";
            this.mniMaintainFamilyMembers.AutoSize = true;
            this.mniMaintainFamilyMembers.Text = "Family &Members...";
            //
            // mniMaintainRelationships
            //
            this.mniMaintainRelationships.Name = "mniMaintainRelationships";
            this.mniMaintainRelationships.AutoSize = true;
            this.mniMaintainRelationships.Text = "&Relationships...";
            //
            // mniMaintainInterests
            //
            this.mniMaintainInterests.Name = "mniMaintainInterests";
            this.mniMaintainInterests.AutoSize = true;
            this.mniMaintainInterests.Text = "&Interests...";
            //
            // mniMaintainReminders
            //
            this.mniMaintainReminders.Name = "mniMaintainReminders";
            this.mniMaintainReminders.AutoSize = true;
            this.mniMaintainReminders.Text = "R&eminders...";
            //
            // mniMaintainNotes
            //
            this.mniMaintainNotes.Name = "mniMaintainNotes";
            this.mniMaintainNotes.AutoSize = true;
            this.mniMaintainNotes.Text = "&Notes";
            //
            // mniMaintainOfficeSpecific
            //
            this.mniMaintainOfficeSpecific.Name = "mniMaintainOfficeSpecific";
            this.mniMaintainOfficeSpecific.AutoSize = true;
            this.mniMaintainOfficeSpecific.Text = "&Local Partner Data";
            //
            // mniMaintainWorkerField
            //
            this.mniMaintainWorkerField.Name = "mniMaintainWorkerField";
            this.mniMaintainWorkerField.AutoSize = true;
            this.mniMaintainWorkerField.Text = "&Worker Field...";
            //
            // mniSeparator6
            //
            this.mniSeparator6.Name = "mniSeparator6";
            this.mniSeparator6.AutoSize = true;
            this.mniSeparator6.Text = "-";
            //
            // mniMaintainPersonnelIndividualData
            //
            this.mniMaintainPersonnelIndividualData.Name = "mniMaintainPersonnelIndividualData";
            this.mniMaintainPersonnelIndividualData.AutoSize = true;
            this.mniMaintainPersonnelIndividualData.Text = "&Personnel/Individual Data";
            //
            // mniSeparator7
            //
            this.mniSeparator7.Name = "mniSeparator7";
            this.mniSeparator7.AutoSize = true;
            this.mniSeparator7.Text = "-";
            //
            // mniMaintainDonorHistory
            //
            this.mniMaintainDonorHistory.Name = "mniMaintainDonorHistory";
            this.mniMaintainDonorHistory.AutoSize = true;
            this.mniMaintainDonorHistory.Text = "Donor &History...";
            //
            // mniMaintainRecipientHistory
            //
            this.mniMaintainRecipientHistory.Name = "mniMaintainRecipientHistory";
            this.mniMaintainRecipientHistory.AutoSize = true;
            this.mniMaintainRecipientHistory.Text = "Recipient Histor&y...";
            //
            // mniMaintainFinanceReports
            //
            this.mniMaintainFinanceReports.Name = "mniMaintainFinanceReports";
            this.mniMaintainFinanceReports.AutoSize = true;
            this.mniMaintainFinanceReports.Text = "Finance Reports";
            //
            // mniMaintainBankAccounts
            //
            this.mniMaintainBankAccounts.Name = "mniMaintainBankAccounts";
            this.mniMaintainBankAccounts.AutoSize = true;
            this.mniMaintainBankAccounts.Text = "Ban&k Accounts";
            //
            // mniMaintainGiftReceipting
            //
            this.mniMaintainGiftReceipting.Name = "mniMaintainGiftReceipting";
            this.mniMaintainGiftReceipting.AutoSize = true;
            this.mniMaintainGiftReceipting.Text = "&Gift Receipting";
            //
            // mniMaintainFinanceDetails
            //
            this.mniMaintainFinanceDetails.Name = "mniMaintainFinanceDetails";
            this.mniMaintainFinanceDetails.AutoSize = true;
            this.mniMaintainFinanceDetails.Text = "&Finance Details...";
            //
            // mniMaintain
            //
            this.mniMaintain.Name = "mniMaintain";
            this.mniMaintain.AutoSize = true;
            this.mniMaintain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniMaintainAddresses,
                        mniMaintainPartnerDetails,
                        mniMaintainFoundationDetails,
                        mniMaintainSubscriptions,
                        mniMaintainSpecialTypes,
                        mniMaintainContacts,
                        mniMaintainFamilyMembers,
                        mniMaintainRelationships,
                        mniMaintainInterests,
                        mniMaintainReminders,
                        mniMaintainNotes,
                        mniMaintainOfficeSpecific,
                        mniMaintainWorkerField,
                        mniSeparator6,
                        mniMaintainPersonnelIndividualData,
                        mniSeparator7,
                        mniMaintainDonorHistory,
                        mniMaintainRecipientHistory,
                        mniMaintainFinanceReports,
                        mniMaintainBankAccounts,
                        mniMaintainGiftReceipting,
                        mniMaintainFinanceDetails});
            this.mniMaintain.Text = "Maintain";
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
                        mniEdit,
                        mniView,
                        mniMaintain,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TPartnerEditDSWinForm
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(754, 623);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TPartnerEditDSWinForm";
            this.Text = "Partner Edit";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	        this.Load += new System.EventHandler(this.TPartnerEditDSWinForm_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TPartnerEditDSWinForm_Closing);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.pnlLowerPart.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private Ict.Petra.Client.MPartner.Gui.TUC_PartnerEdit_CollapsiblePart ucoUpperPart;
        private System.Windows.Forms.Panel pnlLowerPart;
        private Ict.Petra.Client.MPartner.Gui.TUC_PartnerEdit_PartnerTabSet ucoPartnerTabSet;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbSave;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator0;
        private System.Windows.Forms.ToolStripButton tbbNew;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator1;
        private System.Windows.Forms.ToolStripButton tbbTogglePartner;
        private System.Windows.Forms.ToolStripButton tbbTogglePersonnel;
        private System.Windows.Forms.ToolStripButton tbbToggleFinance;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileSave;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniFilePrint;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniFileNew;
        private System.Windows.Forms.ToolStripMenuItem mniFileDeactivatePartner;
        private System.Windows.Forms.ToolStripMenuItem mniFileDeletePartner;
        private System.Windows.Forms.ToolStripMenuItem mniFileCopyPartnerKey;
        private System.Windows.Forms.ToolStripMenuItem mniFileCopyAddress;
        private System.Windows.Forms.ToolStripMenuItem mniFileSendEmail;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniFilePrintSection;
        private System.Windows.Forms.ToolStripMenuItem mniFileExportPartner;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniEdit;
        private System.Windows.Forms.ToolStripMenuItem mniEditUndoCurrentField;
        private System.Windows.Forms.ToolStripMenuItem mniEditUndoScreen;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniEditFind;
        private System.Windows.Forms.ToolStripMenuItem mniEditFindNewAddress;
        private System.Windows.Forms.ToolStripMenuItem mniView;
        private System.Windows.Forms.ToolStripMenuItem mniViewPartnerData;
        private System.Windows.Forms.ToolStripMenuItem mniViewPersonnelData;
        private System.Windows.Forms.ToolStripMenuItem mniViewFinanceData;
        private System.Windows.Forms.ToolStripSeparator mniSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mniViewUpperScreenPart;
        private System.Windows.Forms.ToolStripMenuItem mniViewUpperPartExpanded;
        private System.Windows.Forms.ToolStripMenuItem mniViewUpperPartCollapsed;
        private System.Windows.Forms.ToolStripMenuItem mniMaintain;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainAddresses;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainPartnerDetails;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainFoundationDetails;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainSubscriptions;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainSpecialTypes;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainContacts;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainFamilyMembers;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainRelationships;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainInterests;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainReminders;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainNotes;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainOfficeSpecific;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainWorkerField;
        private System.Windows.Forms.ToolStripSeparator mniSeparator6;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainPersonnelIndividualData;
        private System.Windows.Forms.ToolStripSeparator mniSeparator7;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainDonorHistory;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainRecipientHistory;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainFinanceReports;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainBankAccounts;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainGiftReceipting;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainFinanceDetails;
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
