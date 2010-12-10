// auto generated with nant generateWinforms from PartnerFind.yaml
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

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TPartnerFindScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPartnerFindScreen));

            this.pnlMain = new System.Windows.Forms.Panel();
            this.tabPartnerFindMethods = new Ict.Common.Controls.TTabVersatile();
            this.tpgFindPartner = new System.Windows.Forms.TabPage();
            this.ucoFindByPartnerDetails = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerFind_ByPartnerDetails();
            this.tpgFindBankDetails = new System.Windows.Forms.TabPage();
            this.pnlModalButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFullyLoadData = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbEditPartner = new System.Windows.Forms.ToolStripButton();
            this.tbbNewPartner = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbPartnerInfo = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileWorkWithLastPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartners = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartner1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartner2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartner3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartner4 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartner5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartner6 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartner7 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartner8 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartner9 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRecentPartner10 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileNewPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherd = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherdFamily = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherdChurch = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherdOrganisation = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewPartnerWithShepherdUnit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileEditPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileMergePartners = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileDeletePartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileSendEmail = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFilePrintPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileExportPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileImportPartner = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mniEditCopyPartnerKey = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditCopyAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditCopyEmailAddress = new System.Windows.Forms.ToolStripMenuItem();
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
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniMaintainPersonnelData = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniMaintainDonorHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainRecipientHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainFinanceReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainBankAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainGiftReceipting = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainFinanceDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMailing = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMailingGenerateExtract = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMailingExtracts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMailingSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniMailingDuplicateAddressCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMailingMergeAddresses = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMailingPartnersAtLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMailingSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniMailingSubscriptionExpNotice = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMailingSubscriptionCancellation = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mniToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mniView = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewPartnerInfo = new System.Windows.Forms.ToolStripMenuItem();
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
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlMain.SuspendLayout();
            this.tabPartnerFindMethods.SuspendLayout();
            this.tpgFindPartner.SuspendLayout();
            this.tpgFindBankDetails.SuspendLayout();
            this.pnlModalButtons.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlMain
            //
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.AutoSize = true;
            this.pnlMain.Controls.Add(this.tabPartnerFindMethods);
            this.pnlMain.Controls.Add(this.pnlModalButtons);
            //
            // tpgFindPartner
            //
            this.tpgFindPartner.Location = new System.Drawing.Point(2,2);
            this.tpgFindPartner.Name = "tpgFindPartner";
            this.tpgFindPartner.AutoSize = true;
            this.tpgFindPartner.Controls.Add(this.ucoFindByPartnerDetails);
            //
            // ucoFindByPartnerDetails
            //
            this.ucoFindByPartnerDetails.Name = "ucoFindByPartnerDetails";
            this.ucoFindByPartnerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpgFindPartner.Text = "Find by Partner Details";
            this.tpgFindPartner.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgFindBankDetails
            //
            this.tpgFindBankDetails.Location = new System.Drawing.Point(2,2);
            this.tpgFindBankDetails.Name = "tpgFindBankDetails";
            this.tpgFindBankDetails.AutoSize = true;
            this.tpgFindBankDetails.Text = "Find by Bank Details";
            this.tpgFindBankDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabPartnerFindMethods
            //
            this.tabPartnerFindMethods.Name = "tabPartnerFindMethods";
            this.tabPartnerFindMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPartnerFindMethods.Controls.Add(this.tpgFindPartner);
            this.tabPartnerFindMethods.Controls.Add(this.tpgFindBankDetails);
            this.tabPartnerFindMethods.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            //
            // pnlModalButtons
            //
            this.pnlModalButtons.Name = "pnlModalButtons";
            this.pnlModalButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlModalButtons.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlModalButtons.Controls.Add(this.tableLayoutPanel1);
            //
            // btnHelp
            //
            this.btnHelp.Location = new System.Drawing.Point(2,2);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.AutoSize = true;
            this.btnHelp.Text = "&Help";
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(2,2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.AutoSize = true;
            this.btnCancel.Text = "&Cancel";
            //
            // btnFullyLoadData
            //
            this.btnFullyLoadData.Location = new System.Drawing.Point(2,2);
            this.btnFullyLoadData.Name = "btnFullyLoadData";
            this.btnFullyLoadData.AutoSize = true;
            this.btnFullyLoadData.Text = "Fully load Data";
            //
            // btnAccept
            //
            this.btnAccept.Location = new System.Drawing.Point(2,2);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.AutoSize = true;
            this.btnAccept.Text = "&Accept";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnHelp, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFullyLoadData, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnAccept, 0, 3);
            //
            // tbbEditPartner
            //
            this.tbbEditPartner.Name = "tbbEditPartner";
            this.tbbEditPartner.AutoSize = true;
            this.tbbEditPartner.Click += new System.EventHandler(this.MniFile_Click);
            this.tbbEditPartner.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbEditPartner.Glyph"));
            this.tbbEditPartner.Text = "Edit Partner";
            //
            // tbbNewPartner
            //
            this.tbbNewPartner.Name = "tbbNewPartner";
            this.tbbNewPartner.AutoSize = true;
            this.tbbNewPartner.Click += new System.EventHandler(this.MniFile_Click);
            this.tbbNewPartner.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbNewPartner.Glyph"));
            this.tbbNewPartner.Text = "New Partner";
            //
            // tbbSeparator0
            //
            this.tbbSeparator0.Name = "tbbSeparator0";
            this.tbbSeparator0.AutoSize = true;
            this.tbbSeparator0.Text = "Separator";
            //
            // tbbPartnerInfo
            //
            this.tbbPartnerInfo.Name = "tbbPartnerInfo";
            this.tbbPartnerInfo.AutoSize = true;
            this.tbbPartnerInfo.Click += new System.EventHandler(this.MniView_Click);
            this.tbbPartnerInfo.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbPartnerInfo.Glyph"));
            this.tbbPartnerInfo.Text = "Partner Info";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbEditPartner,
                        tbbNewPartner,
                        tbbSeparator0,
                        tbbPartnerInfo});
            //
            // mniFileWorkWithLastPartner
            //
            this.mniFileWorkWithLastPartner.Name = "mniFileWorkWithLastPartner";
            this.mniFileWorkWithLastPartner.AutoSize = true;
            this.mniFileWorkWithLastPartner.Click += new System.EventHandler(this.MniFile_Click);
            this.mniFileWorkWithLastPartner.Text = "&Work with Last Partner...";
            //
            // mniFileRecentPartner1
            //
            this.mniFileRecentPartner1.Name = "mniFileRecentPartner1";
            this.mniFileRecentPartner1.AutoSize = true;
            this.mniFileRecentPartner1.Text = "0";
            //
            // mniFileRecentPartner2
            //
            this.mniFileRecentPartner2.Name = "mniFileRecentPartner2";
            this.mniFileRecentPartner2.AutoSize = true;
            this.mniFileRecentPartner2.Text = "1";
            //
            // mniFileRecentPartner3
            //
            this.mniFileRecentPartner3.Name = "mniFileRecentPartner3";
            this.mniFileRecentPartner3.AutoSize = true;
            this.mniFileRecentPartner3.Text = "2";
            //
            // mniFileRecentPartner4
            //
            this.mniFileRecentPartner4.Name = "mniFileRecentPartner4";
            this.mniFileRecentPartner4.AutoSize = true;
            this.mniFileRecentPartner4.Text = "3";
            //
            // mniFileRecentPartner5
            //
            this.mniFileRecentPartner5.Name = "mniFileRecentPartner5";
            this.mniFileRecentPartner5.AutoSize = true;
            this.mniFileRecentPartner5.Text = "4";
            //
            // mniFileRecentPartner6
            //
            this.mniFileRecentPartner6.Name = "mniFileRecentPartner6";
            this.mniFileRecentPartner6.AutoSize = true;
            this.mniFileRecentPartner6.Text = "5";
            //
            // mniFileRecentPartner7
            //
            this.mniFileRecentPartner7.Name = "mniFileRecentPartner7";
            this.mniFileRecentPartner7.AutoSize = true;
            this.mniFileRecentPartner7.Text = "6";
            //
            // mniFileRecentPartner8
            //
            this.mniFileRecentPartner8.Name = "mniFileRecentPartner8";
            this.mniFileRecentPartner8.AutoSize = true;
            this.mniFileRecentPartner8.Text = "7";
            //
            // mniFileRecentPartner9
            //
            this.mniFileRecentPartner9.Name = "mniFileRecentPartner9";
            this.mniFileRecentPartner9.AutoSize = true;
            this.mniFileRecentPartner9.Text = "8";
            //
            // mniFileRecentPartner10
            //
            this.mniFileRecentPartner10.Name = "mniFileRecentPartner10";
            this.mniFileRecentPartner10.AutoSize = true;
            this.mniFileRecentPartner10.Text = "9";
            //
            // mniFileRecentPartners
            //
            this.mniFileRecentPartners.Name = "mniFileRecentPartners";
            this.mniFileRecentPartners.AutoSize = true;
            this.mniFileRecentPartners.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniFileRecentPartner1,
                        mniFileRecentPartner2,
                        mniFileRecentPartner3,
                        mniFileRecentPartner4,
                        mniFileRecentPartner5,
                        mniFileRecentPartner6,
                        mniFileRecentPartner7,
                        mniFileRecentPartner8,
                        mniFileRecentPartner9,
                        mniFileRecentPartner10});
            this.mniFileRecentPartners.Text = "&Recent Partners";
            //
            // mniFileSeparator1
            //
            this.mniFileSeparator1.Name = "mniFileSeparator1";
            this.mniFileSeparator1.AutoSize = true;
            this.mniFileSeparator1.Text = "-";
            //
            // mniFileNewPartner
            //
            this.mniFileNewPartner.Name = "mniFileNewPartner";
            this.mniFileNewPartner.AutoSize = true;
            this.mniFileNewPartner.Click += new System.EventHandler(this.MniFile_Click);
            this.mniFileNewPartner.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileNewPartner.Glyph"));
            this.mniFileNewPartner.Text = "&New Partner...";
            //
            // mniFileNewPartnerWithShepherdFamily
            //
            this.mniFileNewPartnerWithShepherdFamily.Name = "mniFileNewPartnerWithShepherdFamily";
            this.mniFileNewPartnerWithShepherdFamily.AutoSize = true;
            this.mniFileNewPartnerWithShepherdFamily.Text = "Add &Family with Shepherd...";
            //
            // mniFileNewPartnerWithShepherdChurch
            //
            this.mniFileNewPartnerWithShepherdChurch.Name = "mniFileNewPartnerWithShepherdChurch";
            this.mniFileNewPartnerWithShepherdChurch.AutoSize = true;
            this.mniFileNewPartnerWithShepherdChurch.Text = "Add &Church with Shepherd...";
            //
            // mniFileNewPartnerWithShepherdOrganisation
            //
            this.mniFileNewPartnerWithShepherdOrganisation.Name = "mniFileNewPartnerWithShepherdOrganisation";
            this.mniFileNewPartnerWithShepherdOrganisation.AutoSize = true;
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
                           mniFileNewPartnerWithShepherdFamily,
                        mniFileNewPartnerWithShepherdChurch,
                        mniFileNewPartnerWithShepherdOrganisation,
                        mniFileNewPartnerWithShepherdUnit});
            this.mniFileNewPartnerWithShepherd.Text = "N&ew Partner (Shepherd)";
            //
            // mniFileEditPartner
            //
            this.mniFileEditPartner.Name = "mniFileEditPartner";
            this.mniFileEditPartner.AutoSize = true;
            this.mniFileEditPartner.Click += new System.EventHandler(this.MniFile_Click);
            this.mniFileEditPartner.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileEditPartner.Glyph"));
            this.mniFileEditPartner.Text = "&Edit Partner";
            //
            // mniFileMergePartners
            //
            this.mniFileMergePartners.Name = "mniFileMergePartners";
            this.mniFileMergePartners.AutoSize = true;
            this.mniFileMergePartners.Click += new System.EventHandler(this.MniFile_Click);
            this.mniFileMergePartners.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileMergePartners.Glyph"));
            this.mniFileMergePartners.Text = "Mer&ge Partners...";
            //
            // mniFileDeletePartner
            //
            this.mniFileDeletePartner.Name = "mniFileDeletePartner";
            this.mniFileDeletePartner.AutoSize = true;
            this.mniFileDeletePartner.Click += new System.EventHandler(this.MniFile_Click);
            this.mniFileDeletePartner.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileDeletePartner.Glyph"));
            this.mniFileDeletePartner.Text = "&Delete Partner";
            //
            // mniFileSeparator2
            //
            this.mniFileSeparator2.Name = "mniFileSeparator2";
            this.mniFileSeparator2.AutoSize = true;
            this.mniFileSeparator2.Text = "-";
            //
            // mniFileSendEmail
            //
            this.mniFileSendEmail.Name = "mniFileSendEmail";
            this.mniFileSendEmail.AutoSize = true;
            this.mniFileSendEmail.Click += new System.EventHandler(this.MniFile_Click);
            this.mniFileSendEmail.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileSendEmail.Glyph"));
            this.mniFileSendEmail.Text = "Send E&mail to Partner";
            //
            // mniFileSeparator3
            //
            this.mniFileSeparator3.Name = "mniFileSeparator3";
            this.mniFileSeparator3.AutoSize = true;
            this.mniFileSeparator3.Text = "-";
            //
            // mniFilePrintPartner
            //
            this.mniFilePrintPartner.Name = "mniFilePrintPartner";
            this.mniFilePrintPartner.AutoSize = true;
            this.mniFilePrintPartner.Click += new System.EventHandler(this.MniFile_Click);
            this.mniFilePrintPartner.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFilePrintPartner.Glyph"));
            this.mniFilePrintPartner.Text = "&Print Partner";
            //
            // mniFileSeparator4
            //
            this.mniFileSeparator4.Name = "mniFileSeparator4";
            this.mniFileSeparator4.AutoSize = true;
            this.mniFileSeparator4.Text = "-";
            //
            // mniFileExportPartner
            //
            this.mniFileExportPartner.Name = "mniFileExportPartner";
            this.mniFileExportPartner.AutoSize = true;
            this.mniFileExportPartner.Click += new System.EventHandler(this.MniFile_Click);
            this.mniFileExportPartner.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileExportPartner.Glyph"));
            this.mniFileExportPartner.Text = "E&xport Partner";
            //
            // mniFileImportPartner
            //
            this.mniFileImportPartner.Name = "mniFileImportPartner";
            this.mniFileImportPartner.AutoSize = true;
            this.mniFileImportPartner.Click += new System.EventHandler(this.MniFile_Click);
            this.mniFileImportPartner.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileImportPartner.Glyph"));
            this.mniFileImportPartner.Text = "&Import Partner";
            //
            // mniFileSeparator5
            //
            this.mniFileSeparator5.Name = "mniFileSeparator5";
            this.mniFileSeparator5.AutoSize = true;
            this.mniFileSeparator5.Text = "-";
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
                           mniFileWorkWithLastPartner,
                        mniFileRecentPartners,
                        mniFileSeparator1,
                        mniFileNewPartner,
                        mniFileNewPartnerWithShepherd,
                        mniFileEditPartner,
                        mniFileMergePartners,
                        mniFileDeletePartner,
                        mniFileSeparator2,
                        mniFileSendEmail,
                        mniFileSeparator3,
                        mniFilePrintPartner,
                        mniFileSeparator4,
                        mniFileExportPartner,
                        mniFileImportPartner,
                        mniFileSeparator5,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniEditSearch
            //
            this.mniEditSearch.Name = "mniEditSearch";
            this.mniEditSearch.AutoSize = true;
            this.mniEditSearch.Click += new System.EventHandler(this.MniEdit_Click);
            this.mniEditSearch.Image = ((System.Drawing.Bitmap)resources.GetObject("mniEditSearch.Glyph"));
            this.mniEditSearch.Text = "&Search";
            //
            // mniEditSeparator
            //
            this.mniEditSeparator.Name = "mniEditSeparator";
            this.mniEditSeparator.AutoSize = true;
            this.mniEditSeparator.Text = "-";
            //
            // mniEditCopyPartnerKey
            //
            this.mniEditCopyPartnerKey.Name = "mniEditCopyPartnerKey";
            this.mniEditCopyPartnerKey.AutoSize = true;
            this.mniEditCopyPartnerKey.Click += new System.EventHandler(this.MniEdit_Click);
            this.mniEditCopyPartnerKey.Image = ((System.Drawing.Bitmap)resources.GetObject("mniEditCopyPartnerKey.Glyph"));
            this.mniEditCopyPartnerKey.Text = "Copy Partner's Partner &Key";
            //
            // mniEditCopyAddress
            //
            this.mniEditCopyAddress.Name = "mniEditCopyAddress";
            this.mniEditCopyAddress.AutoSize = true;
            this.mniEditCopyAddress.Click += new System.EventHandler(this.MniEdit_Click);
            this.mniEditCopyAddress.Image = ((System.Drawing.Bitmap)resources.GetObject("mniEditCopyAddress.Glyph"));
            this.mniEditCopyAddress.Text = "Copy Partner's &Address...";
            //
            // mniEditCopyEmailAddress
            //
            this.mniEditCopyEmailAddress.Name = "mniEditCopyEmailAddress";
            this.mniEditCopyEmailAddress.AutoSize = true;
            this.mniEditCopyEmailAddress.Click += new System.EventHandler(this.MniEdit_Click);
            this.mniEditCopyEmailAddress.Image = ((System.Drawing.Bitmap)resources.GetObject("mniEditCopyEmailAddress.Glyph"));
            this.mniEditCopyEmailAddress.Text = "Copy Partner's E&mail Address";
            //
            // mniEdit
            //
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.AutoSize = true;
            this.mniEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniEditSearch,
                        mniEditSeparator,
                        mniEditCopyPartnerKey,
                        mniEditCopyAddress,
                        mniEditCopyEmailAddress});
            this.mniEdit.Text = "&Edit";
            //
            // mniMaintainAddresses
            //
            this.mniMaintainAddresses.Name = "mniMaintainAddresses";
            this.mniMaintainAddresses.AutoSize = true;
            this.mniMaintainAddresses.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainAddresses.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainAddresses.Glyph"));
            this.mniMaintainAddresses.Text = "&Addresses";
            //
            // mniMaintainPartnerDetails
            //
            this.mniMaintainPartnerDetails.Name = "mniMaintainPartnerDetails";
            this.mniMaintainPartnerDetails.AutoSize = true;
            this.mniMaintainPartnerDetails.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainPartnerDetails.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainPartnerDetails.Glyph"));
            this.mniMaintainPartnerDetails.Text = "Partner &Details";
            //
            // mniMaintainFoundationDetails
            //
            this.mniMaintainFoundationDetails.Name = "mniMaintainFoundationDetails";
            this.mniMaintainFoundationDetails.AutoSize = true;
            this.mniMaintainFoundationDetails.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainFoundationDetails.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainFoundationDetails.Glyph"));
            this.mniMaintainFoundationDetails.Text = "Foundation Details";
            //
            // mniMaintainSubscriptions
            //
            this.mniMaintainSubscriptions.Name = "mniMaintainSubscriptions";
            this.mniMaintainSubscriptions.AutoSize = true;
            this.mniMaintainSubscriptions.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainSubscriptions.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainSubscriptions.Glyph"));
            this.mniMaintainSubscriptions.Text = "&Subscriptions";
            //
            // mniMaintainSpecialTypes
            //
            this.mniMaintainSpecialTypes.Name = "mniMaintainSpecialTypes";
            this.mniMaintainSpecialTypes.AutoSize = true;
            this.mniMaintainSpecialTypes.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainSpecialTypes.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainSpecialTypes.Glyph"));
            this.mniMaintainSpecialTypes.Text = "Special &Types";
            //
            // mniMaintainContacts
            //
            this.mniMaintainContacts.Name = "mniMaintainContacts";
            this.mniMaintainContacts.AutoSize = true;
            this.mniMaintainContacts.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainContacts.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainContacts.Glyph"));
            this.mniMaintainContacts.Text = "&Contacts";
            //
            // mniMaintainFamilyMembers
            //
            this.mniMaintainFamilyMembers.Name = "mniMaintainFamilyMembers";
            this.mniMaintainFamilyMembers.AutoSize = true;
            this.mniMaintainFamilyMembers.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainFamilyMembers.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainFamilyMembers.Glyph"));
            this.mniMaintainFamilyMembers.Text = "Fa&mily Members";
            //
            // mniMaintainRelationships
            //
            this.mniMaintainRelationships.Name = "mniMaintainRelationships";
            this.mniMaintainRelationships.AutoSize = true;
            this.mniMaintainRelationships.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainRelationships.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainRelationships.Glyph"));
            this.mniMaintainRelationships.Text = "&Relationships";
            //
            // mniMaintainInterests
            //
            this.mniMaintainInterests.Name = "mniMaintainInterests";
            this.mniMaintainInterests.AutoSize = true;
            this.mniMaintainInterests.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainInterests.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainInterests.Glyph"));
            this.mniMaintainInterests.Text = "&Interests";
            //
            // mniMaintainReminders
            //
            this.mniMaintainReminders.Name = "mniMaintainReminders";
            this.mniMaintainReminders.AutoSize = true;
            this.mniMaintainReminders.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainReminders.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainReminders.Glyph"));
            this.mniMaintainReminders.Text = "&Reminders";
            //
            // mniMaintainNotes
            //
            this.mniMaintainNotes.Name = "mniMaintainNotes";
            this.mniMaintainNotes.AutoSize = true;
            this.mniMaintainNotes.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainNotes.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainNotes.Glyph"));
            this.mniMaintainNotes.Text = "&Notes";
            //
            // mniMaintainLocalPartnerData
            //
            this.mniMaintainLocalPartnerData.Name = "mniMaintainLocalPartnerData";
            this.mniMaintainLocalPartnerData.AutoSize = true;
            this.mniMaintainLocalPartnerData.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainLocalPartnerData.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainLocalPartnerData.Glyph"));
            this.mniMaintainLocalPartnerData.Text = "&Local Partner Data";
            //
            // mniMaintainWorkerField
            //
            this.mniMaintainWorkerField.Name = "mniMaintainWorkerField";
            this.mniMaintainWorkerField.AutoSize = true;
            this.mniMaintainWorkerField.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainWorkerField.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainWorkerField.Glyph"));
            this.mniMaintainWorkerField.Text = "&Worker Field";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "Separator";
            //
            // mniMaintainPersonnelData
            //
            this.mniMaintainPersonnelData.Name = "mniMaintainPersonnelData";
            this.mniMaintainPersonnelData.AutoSize = true;
            this.mniMaintainPersonnelData.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainPersonnelData.Image = ((System.Drawing.Bitmap)resources.GetObject("mniMaintainPersonnelData.Glyph"));
            this.mniMaintainPersonnelData.Text = "&Personnel/Individual Data";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "Separator";
            //
            // mniMaintainDonorHistory
            //
            this.mniMaintainDonorHistory.Name = "mniMaintainDonorHistory";
            this.mniMaintainDonorHistory.AutoSize = true;
            this.mniMaintainDonorHistory.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainDonorHistory.Text = "Donor &History";
            //
            // mniMaintainRecipientHistory
            //
            this.mniMaintainRecipientHistory.Name = "mniMaintainRecipientHistory";
            this.mniMaintainRecipientHistory.AutoSize = true;
            this.mniMaintainRecipientHistory.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainRecipientHistory.Text = "Recipient Histor&y";
            //
            // mniMaintainFinanceReports
            //
            this.mniMaintainFinanceReports.Name = "mniMaintainFinanceReports";
            this.mniMaintainFinanceReports.AutoSize = true;
            this.mniMaintainFinanceReports.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainFinanceReports.Text = "Finance Report&s";
            //
            // mniMaintainBankAccounts
            //
            this.mniMaintainBankAccounts.Name = "mniMaintainBankAccounts";
            this.mniMaintainBankAccounts.AutoSize = true;
            this.mniMaintainBankAccounts.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainBankAccounts.Text = "Ban&k Accounts";
            //
            // mniMaintainGiftReceipting
            //
            this.mniMaintainGiftReceipting.Name = "mniMaintainGiftReceipting";
            this.mniMaintainGiftReceipting.AutoSize = true;
            this.mniMaintainGiftReceipting.Click += new System.EventHandler(this.MniMaintain_Click);
            this.mniMaintainGiftReceipting.Text = "&Gift Receipting";
            //
            // mniMaintainFinanceDetails
            //
            this.mniMaintainFinanceDetails.Name = "mniMaintainFinanceDetails";
            this.mniMaintainFinanceDetails.AutoSize = true;
            this.mniMaintainFinanceDetails.Click += new System.EventHandler(this.MniMaintain_Click);
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
                        mniSeparator0,
                        mniMaintainPersonnelData,
                        mniSeparator1,
                        mniMaintainDonorHistory,
                        mniMaintainRecipientHistory,
                        mniMaintainFinanceReports,
                        mniMaintainBankAccounts,
                        mniMaintainGiftReceipting,
                        mniMaintainFinanceDetails});
            this.mniMaintain.Text = "Maintain";
            //
            // mniMailingGenerateExtract
            //
            this.mniMailingGenerateExtract.Name = "mniMailingGenerateExtract";
            this.mniMailingGenerateExtract.AutoSize = true;
            this.mniMailingGenerateExtract.Click += new System.EventHandler(this.MniMailing_Click);
            this.mniMailingGenerateExtract.Text = "&Generate Extract From Found Partners...";
            //
            // mniMailingExtracts
            //
            this.mniMailingExtracts.Name = "mniMailingExtracts";
            this.mniMailingExtracts.AutoSize = true;
            this.mniMailingExtracts.Click += new System.EventHandler(this.MniMailing_Click);
            this.mniMailingExtracts.Text = "&Extracts...";
            //
            // mniMailingSeparator1
            //
            this.mniMailingSeparator1.Name = "mniMailingSeparator1";
            this.mniMailingSeparator1.AutoSize = true;
            this.mniMailingSeparator1.Text = "-";
            //
            // mniMailingDuplicateAddressCheck
            //
            this.mniMailingDuplicateAddressCheck.Name = "mniMailingDuplicateAddressCheck";
            this.mniMailingDuplicateAddressCheck.AutoSize = true;
            this.mniMailingDuplicateAddressCheck.Click += new System.EventHandler(this.MniMailing_Click);
            this.mniMailingDuplicateAddressCheck.Text = "&Duplicate  Address Check...";
            //
            // mniMailingMergeAddresses
            //
            this.mniMailingMergeAddresses.Name = "mniMailingMergeAddresses";
            this.mniMailingMergeAddresses.AutoSize = true;
            this.mniMailingMergeAddresses.Click += new System.EventHandler(this.MniMailing_Click);
            this.mniMailingMergeAddresses.Text = "&Merge  Addresses...";
            //
            // mniMailingPartnersAtLocation
            //
            this.mniMailingPartnersAtLocation.Name = "mniMailingPartnersAtLocation";
            this.mniMailingPartnersAtLocation.AutoSize = true;
            this.mniMailingPartnersAtLocation.Click += new System.EventHandler(this.MniMailing_Click);
            this.mniMailingPartnersAtLocation.Text = "Find Partners at &Location...";
            //
            // mniMailingSeparator2
            //
            this.mniMailingSeparator2.Name = "mniMailingSeparator2";
            this.mniMailingSeparator2.AutoSize = true;
            this.mniMailingSeparator2.Text = "-";
            //
            // mniMailingSubscriptionExpNotice
            //
            this.mniMailingSubscriptionExpNotice.Name = "mniMailingSubscriptionExpNotice";
            this.mniMailingSubscriptionExpNotice.AutoSize = true;
            this.mniMailingSubscriptionExpNotice.Click += new System.EventHandler(this.MniMailing_Click);
            this.mniMailingSubscriptionExpNotice.Text = "Subscription Expiry &Notices...";
            //
            // mniMailingSubscriptionCancellation
            //
            this.mniMailingSubscriptionCancellation.Name = "mniMailingSubscriptionCancellation";
            this.mniMailingSubscriptionCancellation.AutoSize = true;
            this.mniMailingSubscriptionCancellation.Click += new System.EventHandler(this.MniMailing_Click);
            this.mniMailingSubscriptionCancellation.Text = "Subscription  &Cancellation...";
            //
            // mniMailing
            //
            this.mniMailing.Name = "mniMailing";
            this.mniMailing.AutoSize = true;
            this.mniMailing.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniMailingGenerateExtract,
                        mniMailingExtracts,
                        mniMailingSeparator1,
                        mniMailingDuplicateAddressCheck,
                        mniMailingMergeAddresses,
                        mniMailingPartnersAtLocation,
                        mniMailingSeparator2,
                        mniMailingSubscriptionExpNotice,
                        mniMailingSubscriptionCancellation});
            this.mniMailing.Text = "&Mailing";
            //
            // mniToolsOptions
            //
            this.mniToolsOptions.Name = "mniToolsOptions";
            this.mniToolsOptions.AutoSize = true;
            this.mniToolsOptions.Click += new System.EventHandler(this.MniTools_Click);
            this.mniToolsOptions.Image = ((System.Drawing.Bitmap)resources.GetObject("mniToolsOptions.Glyph"));
            this.mniToolsOptions.Text = "&Options...";
            //
            // mniTools
            //
            this.mniTools.Name = "mniTools";
            this.mniTools.AutoSize = true;
            this.mniTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniToolsOptions});
            this.mniTools.Text = "&Tools";
            //
            // mniViewPartnerInfo
            //
            this.mniViewPartnerInfo.Name = "mniViewPartnerInfo";
            this.mniViewPartnerInfo.AutoSize = true;
            this.mniViewPartnerInfo.Click += new System.EventHandler(this.MniView_Click);
            this.mniViewPartnerInfo.Image = ((System.Drawing.Bitmap)resources.GetObject("mniViewPartnerInfo.Glyph"));
            this.mniViewPartnerInfo.Text = "Partner &Info Panel";
            //
            // mniView
            //
            this.mniView.Name = "mniView";
            this.mniView.AutoSize = true;
            this.mniView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniViewPartnerInfo});
            this.mniView.Text = "Vie&w";
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
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "-";
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
                        mniSeparator2,
                        mniHelpBugReport,
                        mniSeparator3,
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
                        mniMaintain,
                        mniMailing,
                        mniTools,
                        mniView,
                        mniPetraModules,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TPartnerFindScreen
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(740, 570);

            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TPartnerFindScreen";
            this.Text = "Partner Find OpenPetra.org";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Load += new System.EventHandler(this.TPartnerFindScreen_Load);
            this.Closed += new System.EventHandler(this.TPartnerFindScreen_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlModalButtons.ResumeLayout(false);
            this.tpgFindBankDetails.ResumeLayout(false);
            this.tpgFindPartner.ResumeLayout(false);
            this.tabPartnerFindMethods.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlMain;
        private Ict.Common.Controls.TTabVersatile tabPartnerFindMethods;
        private System.Windows.Forms.TabPage tpgFindPartner;
        private Ict.Petra.Client.MPartner.Gui.TUC_PartnerFind_ByPartnerDetails ucoFindByPartnerDetails;
        private System.Windows.Forms.TabPage tpgFindBankDetails;
        private System.Windows.Forms.Panel pnlModalButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFullyLoadData;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbEditPartner;
        private System.Windows.Forms.ToolStripButton tbbNewPartner;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator0;
        private System.Windows.Forms.ToolStripButton tbbPartnerInfo;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileWorkWithLastPartner;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartners;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartner1;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartner2;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartner3;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartner4;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartner5;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartner6;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartner7;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartner8;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartner9;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentPartner10;
        private System.Windows.Forms.ToolStripSeparator mniFileSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartner;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherd;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherdFamily;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherdChurch;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherdOrganisation;
        private System.Windows.Forms.ToolStripMenuItem mniFileNewPartnerWithShepherdUnit;
        private System.Windows.Forms.ToolStripMenuItem mniFileEditPartner;
        private System.Windows.Forms.ToolStripMenuItem mniFileMergePartners;
        private System.Windows.Forms.ToolStripMenuItem mniFileDeletePartner;
        private System.Windows.Forms.ToolStripSeparator mniFileSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniFileSendEmail;
        private System.Windows.Forms.ToolStripSeparator mniFileSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniFilePrintPartner;
        private System.Windows.Forms.ToolStripSeparator mniFileSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniFileExportPartner;
        private System.Windows.Forms.ToolStripMenuItem mniFileImportPartner;
        private System.Windows.Forms.ToolStripSeparator mniFileSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniEdit;
        private System.Windows.Forms.ToolStripMenuItem mniEditSearch;
        private System.Windows.Forms.ToolStripSeparator mniEditSeparator;
        private System.Windows.Forms.ToolStripMenuItem mniEditCopyPartnerKey;
        private System.Windows.Forms.ToolStripMenuItem mniEditCopyAddress;
        private System.Windows.Forms.ToolStripMenuItem mniEditCopyEmailAddress;
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
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainPersonnelData;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainDonorHistory;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainRecipientHistory;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainFinanceReports;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainBankAccounts;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainGiftReceipting;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainFinanceDetails;
        private System.Windows.Forms.ToolStripMenuItem mniMailing;
        private System.Windows.Forms.ToolStripMenuItem mniMailingGenerateExtract;
        private System.Windows.Forms.ToolStripMenuItem mniMailingExtracts;
        private System.Windows.Forms.ToolStripSeparator mniMailingSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniMailingDuplicateAddressCheck;
        private System.Windows.Forms.ToolStripMenuItem mniMailingMergeAddresses;
        private System.Windows.Forms.ToolStripMenuItem mniMailingPartnersAtLocation;
        private System.Windows.Forms.ToolStripSeparator mniMailingSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniMailingSubscriptionExpNotice;
        private System.Windows.Forms.ToolStripMenuItem mniMailingSubscriptionCancellation;
        private System.Windows.Forms.ToolStripMenuItem mniTools;
        private System.Windows.Forms.ToolStripMenuItem mniToolsOptions;
        private System.Windows.Forms.ToolStripMenuItem mniView;
        private System.Windows.Forms.ToolStripMenuItem mniViewPartnerInfo;
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
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
