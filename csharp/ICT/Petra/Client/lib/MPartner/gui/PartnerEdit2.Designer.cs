// auto generated with nant generateWinforms from PartnerEdit2.yaml
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
    partial class TFrmPartnerEdit2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmPartnerEdit2));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.ucoUpperPart = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerEdit_TopPart();
            this.pnlLowerPart = new System.Windows.Forms.Panel();
            this.ucoLowerPart = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerEdit_LowerPart();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbSave = new System.Windows.Forms.ToolStripButton();
            this.tbbNewPartner = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbViewPartnerData = new System.Windows.Forms.ToolStripButton();
            this.tbbViewPersonnelData = new System.Windows.Forms.ToolStripButton();
            this.tbbViewFinanceData = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherd = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherdPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherdFamily = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherdChurch = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherdOrganisation = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherdUnit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mniDeactivatePartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileDeletePartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileSendEmail = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFilePrintSection = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileExportPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditUndoCurrentField = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditUndoScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mniEditFind = new System.Windows.Forms.ToolStripMenuItem();
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
            this.mniMaintainLocalPartnerData = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainWorkerField = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mniMaintainPersonnelData = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mniMaintainDonorHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainRecipientHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainFinanceReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainBankAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainGiftReceipting = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainFinanceDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mniView = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewUpperScreenPart = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewUpperScreenPartExpanded = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewUpperScreenPartCollapsed = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewPartnerData = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewPersonnelData = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewFinanceData = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpVideoTutorial = new System.Windows.Forms.ToolStripMenuItem();
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
            this.pnlLowerPart.Controls.Add(this.ucoLowerPart);
            //
            // ucoLowerPart
            //
            this.ucoLowerPart.Name = "ucoLowerPart";
            this.ucoLowerPart.Dock = System.Windows.Forms.DockStyle.Fill;
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
            // tbbNewPartner
            //
            this.tbbNewPartner.Name = "tbbNewPartner";
            this.tbbNewPartner.AutoSize = true;
            this.tbbNewPartner.Text = "&New Partner";
            //
            // tbbSeparator0
            //
            this.tbbSeparator0.Name = "tbbSeparator0";
            this.tbbSeparator0.AutoSize = true;
            this.tbbSeparator0.Text = "Separator";
            //
            // tbbViewPartnerData
            //
            this.tbbViewPartnerData.Name = "tbbViewPartnerData";
            this.tbbViewPartnerData.AutoSize = true;
            this.tbbViewPartnerData.Click += new System.EventHandler(this.ViewPartnerData);
            this.tbbViewPartnerData.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbViewPartnerData.Glyph"));
            this.tbbViewPartnerData.Text = "&Partner Data";
            //
            // tbbViewPersonnelData
            //
            this.tbbViewPersonnelData.Name = "tbbViewPersonnelData";
            this.tbbViewPersonnelData.AutoSize = true;
            this.tbbViewPersonnelData.Click += new System.EventHandler(this.ViewPersonnelData);
            this.tbbViewPersonnelData.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbViewPersonnelData.Glyph"));
            this.tbbViewPersonnelData.Text = "P&ersonnel Data";
            //
            // tbbViewFinanceData
            //
            this.tbbViewFinanceData.Name = "tbbViewFinanceData";
            this.tbbViewFinanceData.AutoSize = true;
            this.tbbViewFinanceData.Click += new System.EventHandler(this.ViewFinanceData);
            this.tbbViewFinanceData.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbViewFinanceData.Glyph"));
            this.tbbViewFinanceData.Text = "&Finance Data";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbSave,
                        tbbNewPartner,
                        tbbSeparator0,
                        tbbViewPartnerData,
                        tbbViewPersonnelData,
                        tbbViewFinanceData});
            //
            // mniFileNewPartner
            //
            this.mniFileNewPartner.Name = "mniFileNewPartner";
            this.mniFileNewPartner.AutoSize = true;
            this.mniFileNewPartner.Click += new System.EventHandler(this.FileNewPartner);
            this.mniFileNewPartner.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileNewPartner.Glyph"));
            this.mniFileNewPartner.Text = "&New Partner...";
            //
            // mniFileNewPartnerWithShepherdPerson
            //
            this.mniFileNewPartnerWithShepherdPerson.Name = "mniFileNewPartnerWithShepherdPerson";
            this.mniFileNewPartnerWithShepherdPerson.AutoSize = true;
            this.mniFileNewPartnerWithShepherdPerson.Click += new System.EventHandler(this.FileNewPartnerWithShepherdPerson);
            this.mniFileNewPartnerWithShepherdPerson.Text = "Add &Person with Shepherd...";
            //
            // mniFileNewPartnerWithShepherdFamily
            //
            this.mniFileNewPartnerWithShepherdFamily.Name = "mniFileNewPartnerWithShepherdFamily";
            this.mniFileNewPartnerWithShepherdFamily.AutoSize = true;
            this.mniFileNewPartnerWithShepherdFamily.Click += new System.EventHandler(this.FileNewPartnerWithShepherdFamily);
            this.mniFileNewPartnerWithShepherdFamily.Text = "Add &Family with Shepherd...";
            //
            // mniFileNewPartnerWithShepherdChurch
            //
            this.mniFileNewPartnerWithShepherdChurch.Name = "mniFileNewPartnerWithShepherdChurch";
            this.mniFileNewPartnerWithShepherdChurch.AutoSize = true;
            this.mniFileNewPartnerWithShepherdChurch.Click += new System.EventHandler(this.FileNewPartnerWithShepherdChurch);
            this.mniFileNewPartnerWithShepherdChurch.Text = "Add &Church with Shepherd...";
            //
            // mniFileNewPartnerWithShepherdOrganisation
            //
            this.mniFileNewPartnerWithShepherdOrganisation.Name = "mniFileNewPartnerWithShepherdOrganisation";
            this.mniFileNewPartnerWithShepherdOrganisation.AutoSize = true;
            this.mniFileNewPartnerWithShepherdOrganisation.Click += new System.EventHandler(this.FileNewPartnerWithShepherdOrganisation);
            this.mniFileNewPartnerWithShepherdOrganisation.Text = "Add &Organisation with Shepherd...";
            //
            // mniFileNewPartnerWithShepherdUnit
            //
            this.mniFileNewPartnerWithShepherdUnit.Name = "mniFileNewPartnerWithShepherdUnit";
            this.mniFileNewPartnerWithShepherdUnit.AutoSize = true;
            this.mniFileNewPartnerWithShepherdUnit.Text = "Add &Unit with Shepherd...";
            //
            // mniFileNewPartnerWithShepherd
            //
            this.mniFileNewPartnerWithShepherd.Name = "mniFileNewPartnerWithShepherd";
            this.mniFileNewPartnerWithShepherd.AutoSize = true;
            this.mniFileNewPartnerWithShepherd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniFileNewPartnerWithShepherdPerson,
                        mniFileNewPartnerWithShepherdFamily,
                        mniFileNewPartnerWithShepherdChurch,
                        mniFileNewPartnerWithShepherdOrganisation,
                        mniFileNewPartnerWithShepherdUnit});
            this.mniFileNewPartnerWithShepherd.Text = "N&ew Partner (Shepherd)";
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
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
            //
            // mniFilePrint
            //
            this.mniFilePrint.Name = "mniFilePrint";
            this.mniFilePrint.AutoSize = true;
            this.mniFilePrint.Click += new System.EventHandler(this.FilePrintPartner);
            this.mniFilePrint.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFilePrint.Glyph"));
            this.mniFilePrint.Text = "&Print...";
            //
            // mniDeactivatePartner
            //
            this.mniDeactivatePartner.Name = "mniDeactivatePartner";
            this.mniDeactivatePartner.AutoSize = true;
            this.mniDeactivatePartner.Text = "Deactivate Partner";
            //
            // mniFileDeletePartner
            //
            this.mniFileDeletePartner.Name = "mniFileDeletePartner";
            this.mniFileDeletePartner.AutoSize = true;
            this.mniFileDeletePartner.Click += new System.EventHandler(this.FileDeletePartner);
            this.mniFileDeletePartner.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileDeletePartner.Glyph"));
            this.mniFileDeletePartner.Text = "&Delete THIS Partner...";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "Separator";
            //
            // mniFileSendEmail
            //
            this.mniFileSendEmail.Name = "mniFileSendEmail";
            this.mniFileSendEmail.AutoSize = true;
            this.mniFileSendEmail.Click += new System.EventHandler(this.FileSendEmail);
            this.mniFileSendEmail.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileSendEmail.Glyph"));
            this.mniFileSendEmail.Text = "Send E&mail to Partner";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "Separator";
            //
            // mniFilePrintSection
            //
            this.mniFilePrintSection.Name = "mniFilePrintSection";
            this.mniFilePrintSection.AutoSize = true;
            this.mniFilePrintSection.Click += new System.EventHandler(this.FileDeletePartner);
            this.mniFilePrintSection.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFilePrintSection.Glyph"));
            this.mniFilePrintSection.Text = "P&rint Section...";
            //
            // mniSeparator4
            //
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.AutoSize = true;
            this.mniSeparator4.Text = "Separator";
            //
            // mniFileExportPartner
            //
            this.mniFileExportPartner.Name = "mniFileExportPartner";
            this.mniFileExportPartner.AutoSize = true;
            this.mniFileExportPartner.Click += new System.EventHandler(this.FileExportPartner);
            this.mniFileExportPartner.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileExportPartner.Glyph"));
            this.mniFileExportPartner.Text = "E&xport Partner";
            //
            // mniSeparator5
            //
            this.mniSeparator5.Name = "mniSeparator5";
            this.mniSeparator5.AutoSize = true;
            this.mniSeparator5.Text = "Separator";
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
                           mniFileNewPartner,
                        mniFileNewPartnerWithShepherd,
                        mniFileSave,
                        mniSeparator0,
                        mniSeparator1,
                        mniFilePrint,
                        mniDeactivatePartner,
                        mniFileDeletePartner,
                        mniSeparator2,
                        mniFileSendEmail,
                        mniSeparator3,
                        mniFilePrintSection,
                        mniSeparator4,
                        mniFileExportPartner,
                        mniSeparator5,
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
            // mniSeparator6
            //
            this.mniSeparator6.Name = "mniSeparator6";
            this.mniSeparator6.AutoSize = true;
            this.mniSeparator6.Text = "-";
            //
            // mniEditFind
            //
            this.mniEditFind.Name = "mniEditFind";
            this.mniEditFind.AutoSize = true;
            this.mniEditFind.Text = "&Find...";
            //
            // mniEdit
            //
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.AutoSize = true;
            this.mniEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniEditUndoCurrentField,
                        mniEditUndoScreen,
                        mniSeparator6,
                        mniEditFind});
            this.mniEdit.Text = "&Edit";
            //
            // mniMaintainAddresses
            //
            this.mniMaintainAddresses.Name = "mniMaintainAddresses";
            this.mniMaintainAddresses.AutoSize = true;
            this.mniMaintainAddresses.Click += new System.EventHandler(this.MaintainAddresses);
            this.mniMaintainAddresses.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainAddresses.Glyph"));
            this.mniMaintainAddresses.Text = "&Addresses";
            //
            // mniMaintainPartnerDetails
            //
            this.mniMaintainPartnerDetails.Name = "mniMaintainPartnerDetails";
            this.mniMaintainPartnerDetails.AutoSize = true;
            this.mniMaintainPartnerDetails.Click += new System.EventHandler(this.MaintainPartnerDetails);
            this.mniMaintainPartnerDetails.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainPartnerDetails.Glyph"));
            this.mniMaintainPartnerDetails.Text = "Partner &Details";
            //
            // mniMaintainFoundationDetails
            //
            this.mniMaintainFoundationDetails.Name = "mniMaintainFoundationDetails";
            this.mniMaintainFoundationDetails.AutoSize = true;
            this.mniMaintainFoundationDetails.Click += new System.EventHandler(this.MaintainFoundationDetails);
            this.mniMaintainFoundationDetails.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainFoundationDetails.Glyph"));
            this.mniMaintainFoundationDetails.Text = "Foundation Details";
            //
            // mniMaintainSubscriptions
            //
            this.mniMaintainSubscriptions.Name = "mniMaintainSubscriptions";
            this.mniMaintainSubscriptions.AutoSize = true;
            this.mniMaintainSubscriptions.Click += new System.EventHandler(this.MaintainSubscriptions);
            this.mniMaintainSubscriptions.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainSubscriptions.Glyph"));
            this.mniMaintainSubscriptions.Text = "&Subscriptions";
            //
            // mniMaintainSpecialTypes
            //
            this.mniMaintainSpecialTypes.Name = "mniMaintainSpecialTypes";
            this.mniMaintainSpecialTypes.AutoSize = true;
            this.mniMaintainSpecialTypes.Click += new System.EventHandler(this.MaintainSpecialTypes);
            this.mniMaintainSpecialTypes.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainSpecialTypes.Glyph"));
            this.mniMaintainSpecialTypes.Text = "Special &Types";
            //
            // mniMaintainContacts
            //
            this.mniMaintainContacts.Name = "mniMaintainContacts";
            this.mniMaintainContacts.AutoSize = true;
            this.mniMaintainContacts.Click += new System.EventHandler(this.MaintainContacts);
            this.mniMaintainContacts.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainContacts.Glyph"));
            this.mniMaintainContacts.Text = "&Contacts";
            //
            // mniMaintainFamilyMembers
            //
            this.mniMaintainFamilyMembers.Name = "mniMaintainFamilyMembers";
            this.mniMaintainFamilyMembers.AutoSize = true;
            this.mniMaintainFamilyMembers.Click += new System.EventHandler(this.MaintainFamilyMembers);
            this.mniMaintainFamilyMembers.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainFamilyMembers.Glyph"));
            this.mniMaintainFamilyMembers.Text = "Fa&mily Members";
            //
            // mniMaintainRelationships
            //
            this.mniMaintainRelationships.Name = "mniMaintainRelationships";
            this.mniMaintainRelationships.AutoSize = true;
            this.mniMaintainRelationships.Click += new System.EventHandler(this.MaintainRelationships);
            this.mniMaintainRelationships.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainRelationships.Glyph"));
            this.mniMaintainRelationships.Text = "&Relationships";
            //
            // mniMaintainInterests
            //
            this.mniMaintainInterests.Name = "mniMaintainInterests";
            this.mniMaintainInterests.AutoSize = true;
            this.mniMaintainInterests.Click += new System.EventHandler(this.MaintainInterests);
            this.mniMaintainInterests.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainInterests.Glyph"));
            this.mniMaintainInterests.Text = "&Interests";
            //
            // mniMaintainReminders
            //
            this.mniMaintainReminders.Name = "mniMaintainReminders";
            this.mniMaintainReminders.AutoSize = true;
            this.mniMaintainReminders.Click += new System.EventHandler(this.MaintainReminders);
            this.mniMaintainReminders.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainReminders.Glyph"));
            this.mniMaintainReminders.Text = "&Reminders";
            //
            // mniMaintainNotes
            //
            this.mniMaintainNotes.Name = "mniMaintainNotes";
            this.mniMaintainNotes.AutoSize = true;
            this.mniMaintainNotes.Click += new System.EventHandler(this.MaintainNotes);
            this.mniMaintainNotes.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainNotes.Glyph"));
            this.mniMaintainNotes.Text = "&Notes";
            //
            // mniMaintainLocalPartnerData
            //
            this.mniMaintainLocalPartnerData.Name = "mniMaintainLocalPartnerData";
            this.mniMaintainLocalPartnerData.AutoSize = true;
            this.mniMaintainLocalPartnerData.Click += new System.EventHandler(this.MaintainLocalPartnerData);
            this.mniMaintainLocalPartnerData.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainLocalPartnerData.Glyph"));
            this.mniMaintainLocalPartnerData.Text = "&Local Partner Data";
            //
            // mniMaintainWorkerField
            //
            this.mniMaintainWorkerField.Name = "mniMaintainWorkerField";
            this.mniMaintainWorkerField.AutoSize = true;
            this.mniMaintainWorkerField.Click += new System.EventHandler(this.MaintainWorkerField);
            this.mniMaintainWorkerField.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainWorkerField.Glyph"));
            this.mniMaintainWorkerField.Text = "&Worker Field";
            //
            // mniSeparator7
            //
            this.mniSeparator7.Name = "mniSeparator7";
            this.mniSeparator7.AutoSize = true;
            this.mniSeparator7.Text = "Separator";
            //
            // mniMaintainPersonnelData
            //
            this.mniMaintainPersonnelData.Name = "mniMaintainPersonnelData";
            this.mniMaintainPersonnelData.AutoSize = true;
            this.mniMaintainPersonnelData.Click += new System.EventHandler(this.MaintainIndividualData);
            this.mniMaintainPersonnelData.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainPersonnelData.Glyph"));
            this.mniMaintainPersonnelData.Text = "&Personnel/Individual Data";
            //
            // mniSeparator8
            //
            this.mniSeparator8.Name = "mniSeparator8";
            this.mniSeparator8.AutoSize = true;
            this.mniSeparator8.Text = "Separator";
            //
            // mniMaintainDonorHistory
            //
            this.mniMaintainDonorHistory.Name = "mniMaintainDonorHistory";
            this.mniMaintainDonorHistory.AutoSize = true;
            this.mniMaintainDonorHistory.Click += new System.EventHandler(this.MaintainDonorHistory);
            this.mniMaintainDonorHistory.Text = "Donor &History";
            //
            // mniMaintainRecipientHistory
            //
            this.mniMaintainRecipientHistory.Name = "mniMaintainRecipientHistory";
            this.mniMaintainRecipientHistory.AutoSize = true;
            this.mniMaintainRecipientHistory.Click += new System.EventHandler(this.MaintainRecipientHistory);
            this.mniMaintainRecipientHistory.Text = "Recipient Histor&y";
            //
            // mniMaintainFinanceReports
            //
            this.mniMaintainFinanceReports.Name = "mniMaintainFinanceReports";
            this.mniMaintainFinanceReports.AutoSize = true;
            this.mniMaintainFinanceReports.Click += new System.EventHandler(this.MaintainFinanceReports);
            this.mniMaintainFinanceReports.Text = "Finance Report&s";
            //
            // mniMaintainBankAccounts
            //
            this.mniMaintainBankAccounts.Name = "mniMaintainBankAccounts";
            this.mniMaintainBankAccounts.AutoSize = true;
            this.mniMaintainBankAccounts.Click += new System.EventHandler(this.MaintainBankAccounts);
            this.mniMaintainBankAccounts.Text = "Ban&k Accounts";
            //
            // mniMaintainGiftReceipting
            //
            this.mniMaintainGiftReceipting.Name = "mniMaintainGiftReceipting";
            this.mniMaintainGiftReceipting.AutoSize = true;
            this.mniMaintainGiftReceipting.Click += new System.EventHandler(this.MaintainGiftReceipting);
            this.mniMaintainGiftReceipting.Text = "&Gift Receipting";
            //
            // mniMaintainFinanceDetails
            //
            this.mniMaintainFinanceDetails.Name = "mniMaintainFinanceDetails";
            this.mniMaintainFinanceDetails.AutoSize = true;
            this.mniMaintainFinanceDetails.Click += new System.EventHandler(this.MaintainFinanceDetails);
            this.mniMaintainFinanceDetails.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainFinanceDetails.Glyph"));
            this.mniMaintainFinanceDetails.Text = "&Finance Details";
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
                        mniMaintainLocalPartnerData,
                        mniMaintainWorkerField,
                        mniSeparator7,
                        mniMaintainPersonnelData,
                        mniSeparator8,
                        mniMaintainDonorHistory,
                        mniMaintainRecipientHistory,
                        mniMaintainFinanceReports,
                        mniMaintainBankAccounts,
                        mniMaintainGiftReceipting,
                        mniMaintainFinanceDetails});
            this.mniMaintain.Text = "Ma&intain";
            //
            // mniViewUpperScreenPartExpanded
            //
            this.mniViewUpperScreenPartExpanded.Name = "mniViewUpperScreenPartExpanded";
            this.mniViewUpperScreenPartExpanded.AutoSize = true;
            this.mniViewUpperScreenPartExpanded.Click += new System.EventHandler(this.ViewUpperScreenPartExpanded);
            this.mniViewUpperScreenPartExpanded.Text = "&Expanded";
            //
            // mniViewUpperScreenPartCollapsed
            //
            this.mniViewUpperScreenPartCollapsed.Name = "mniViewUpperScreenPartCollapsed";
            this.mniViewUpperScreenPartCollapsed.AutoSize = true;
            this.mniViewUpperScreenPartCollapsed.Click += new System.EventHandler(this.ViewUpperScreenPartExpanded);
            this.mniViewUpperScreenPartCollapsed.Text = "&Collapsed";
            //
            // mniViewUpperScreenPart
            //
            this.mniViewUpperScreenPart.Name = "mniViewUpperScreenPart";
            this.mniViewUpperScreenPart.AutoSize = true;
            this.mniViewUpperScreenPart.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniViewUpperScreenPartExpanded,
                        mniViewUpperScreenPartCollapsed});
            this.mniViewUpperScreenPart.Text = "&Upper Screen Part";
            //
            // mniViewPartnerData
            //
            this.mniViewPartnerData.Name = "mniViewPartnerData";
            this.mniViewPartnerData.AutoSize = true;
            this.mniViewPartnerData.Click += new System.EventHandler(this.ViewPartnerData);
            this.mniViewPartnerData.Image = ((System.Drawing.Bitmap)resources.GetObject("mniViewPartnerData.Glyph"));
            this.mniViewPartnerData.Text = "&Partner Data";
            //
            // mniViewPersonnelData
            //
            this.mniViewPersonnelData.Name = "mniViewPersonnelData";
            this.mniViewPersonnelData.AutoSize = true;
            this.mniViewPersonnelData.Click += new System.EventHandler(this.ViewPersonnelData);
            this.mniViewPersonnelData.Image = ((System.Drawing.Bitmap)resources.GetObject("mniViewPersonnelData.Glyph"));
            this.mniViewPersonnelData.Text = "P&ersonnel Data";
            //
            // mniViewFinanceData
            //
            this.mniViewFinanceData.Name = "mniViewFinanceData";
            this.mniViewFinanceData.AutoSize = true;
            this.mniViewFinanceData.Click += new System.EventHandler(this.ViewFinanceData);
            this.mniViewFinanceData.Image = ((System.Drawing.Bitmap)resources.GetObject("mniViewFinanceData.Glyph"));
            this.mniViewFinanceData.Text = "&Finance Data";
            //
            // mniView
            //
            this.mniView.Name = "mniView";
            this.mniView.AutoSize = true;
            this.mniView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniViewUpperScreenPart,
                        mniViewPartnerData,
                        mniViewPersonnelData,
                        mniViewFinanceData});
            this.mniView.Text = "Vie&w";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator9
            //
            this.mniSeparator9.Name = "mniSeparator9";
            this.mniSeparator9.AutoSize = true;
            this.mniSeparator9.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator10
            //
            this.mniSeparator10.Name = "mniSeparator10";
            this.mniSeparator10.AutoSize = true;
            this.mniSeparator10.Text = "-";
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
            // mniSeparator11
            //
            this.mniSeparator11.Name = "mniSeparator11";
            this.mniSeparator11.AutoSize = true;
            this.mniSeparator11.Text = "Separator";
            //
            // mniHelpVideoTutorial
            //
            this.mniHelpVideoTutorial.Name = "mniHelpVideoTutorial";
            this.mniHelpVideoTutorial.AutoSize = true;
            this.mniHelpVideoTutorial.Click += new System.EventHandler(this.HelpVideoTutorial);
            this.mniHelpVideoTutorial.Image = ((System.Drawing.Bitmap)resources.GetObject("mniHelpVideoTutorial.Glyph"));
            this.mniHelpVideoTutorial.Text = "&Video Tutorial for Partner Edit Screen...";
            //
            // mniHelp
            //
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.AutoSize = true;
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniHelpPetraHelp,
                        mniSeparator9,
                        mniHelpBugReport,
                        mniSeparator10,
                        mniHelpAboutPetra,
                        mniHelpDevelopmentTeam,
                        mniSeparator11,
                        mniHelpVideoTutorial});
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
                        mniMaintain,
                        mniView,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmPartnerEdit2
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(750, 700);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmPartnerEdit2";
            this.Text = "Partner Edit";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	        this.Load += new System.EventHandler(this.TFrmPartnerEdit2_Load);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.pnlLowerPart.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private Ict.Petra.Client.MPartner.Gui.TUC_PartnerEdit_TopPart ucoUpperPart;
        private System.Windows.Forms.Panel pnlLowerPart;
        private Ict.Petra.Client.MPartner.Gui.TUC_PartnerEdit_LowerPart ucoLowerPart;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbSave;
        private System.Windows.Forms.ToolStripButton tbbNewPartner;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator0;
        private System.Windows.Forms.ToolStripButton tbbViewPartnerData;
        private System.Windows.Forms.ToolStripButton tbbViewPersonnelData;
        private System.Windows.Forms.ToolStripButton tbbViewFinanceData;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartner;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherd;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherdPerson;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherdFamily;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherdChurch;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherdOrganisation;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherdUnit;
        private System.Windows.Forms.ToolStripMenuItem mniFileSave;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniFilePrint;
        private System.Windows.Forms.ToolStripMenuItem mniDeactivatePartner;
        private System.Windows.Forms.ToolStripMenuItem mniFileDeletePartner;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniFileSendEmail;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniFilePrintSection;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniFileExportPartner;
        private System.Windows.Forms.ToolStripSeparator mniSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniEdit;
        private System.Windows.Forms.ToolStripMenuItem mniEditUndoCurrentField;
        private System.Windows.Forms.ToolStripMenuItem mniEditUndoScreen;
        private System.Windows.Forms.ToolStripSeparator mniSeparator6;
        private System.Windows.Forms.ToolStripMenuItem mniEditFind;
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
        private System.Windows.Forms.ToolStripMenuItem mniMaintainLocalPartnerData;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainWorkerField;
        private System.Windows.Forms.ToolStripSeparator mniSeparator7;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainPersonnelData;
        private System.Windows.Forms.ToolStripSeparator mniSeparator8;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainDonorHistory;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainRecipientHistory;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainFinanceReports;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainBankAccounts;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainGiftReceipting;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainFinanceDetails;
        private System.Windows.Forms.ToolStripMenuItem mniView;
        private System.Windows.Forms.ToolStripMenuItem mniViewUpperScreenPart;
        private System.Windows.Forms.ToolStripMenuItem mniViewUpperScreenPartExpanded;
        private System.Windows.Forms.ToolStripMenuItem mniViewUpperScreenPartCollapsed;
        private System.Windows.Forms.ToolStripMenuItem mniViewPartnerData;
        private System.Windows.Forms.ToolStripMenuItem mniViewPersonnelData;
        private System.Windows.Forms.ToolStripMenuItem mniViewFinanceData;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator9;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator10;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private System.Windows.Forms.ToolStripSeparator mniSeparator11;
        private System.Windows.Forms.ToolStripMenuItem mniHelpVideoTutorial;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
