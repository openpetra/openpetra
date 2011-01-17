// auto generated with nant generateWinforms from GLAccountHierarchy.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using System;
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    partial class TFrmGLAccountHierarchy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmGLAccountHierarchy));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.sptAccountSplitter = new System.Windows.Forms.SplitContainer();
            this.trvAccounts = new Ict.Common.Controls.TTrvTreeView();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailAccountCode = new System.Windows.Forms.TextBox();
            this.lblDetailAccountCode = new System.Windows.Forms.Label();
            this.cmbDetailAccountType = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDetailAccountType = new System.Windows.Forms.Label();
            this.txtDetailEngAccountCodeLongDesc = new System.Windows.Forms.TextBox();
            this.lblDetailEngAccountCodeLongDesc = new System.Windows.Forms.Label();
            this.txtDetailEngAccountCodeShortDesc = new System.Windows.Forms.TextBox();
            this.lblDetailEngAccountCodeShortDesc = new System.Windows.Forms.Label();
            this.txtDetailAccountCodeLongDesc = new System.Windows.Forms.TextBox();
            this.lblDetailAccountCodeLongDesc = new System.Windows.Forms.Label();
            this.txtDetailAccountCodeShortDesc = new System.Windows.Forms.TextBox();
            this.lblDetailAccountCodeShortDesc = new System.Windows.Forms.Label();
            this.cmbDetailValidCcCombo = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDetailValidCcCombo = new System.Windows.Forms.Label();
            this.chkDetailBankAccountFlag = new System.Windows.Forms.CheckBox();
            this.lblDetailBankAccountFlag = new System.Windows.Forms.Label();
            this.chkDetailAccountActiveFlag = new System.Windows.Forms.CheckBox();
            this.lblDetailAccountActiveFlag = new System.Windows.Forms.Label();
            this.chkDetailForeignCurrencyFlag = new System.Windows.Forms.CheckBox();
            this.cmbDetailForeignCurrencyCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.ucoAccountAnalysisAttributes = new Ict.Petra.Client.MFinance.Gui.Setup.TUC_AccountAnalysisAttributes();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbSave = new System.Windows.Forms.ToolStripButton();
            this.tbbAddNewAccount = new System.Windows.Forms.ToolStripButton();
            this.tbbDeleteUnusedAccount = new System.Windows.Forms.ToolStripButton();
            this.tbbExportHierarchy = new System.Windows.Forms.ToolStripButton();
            this.tbbImportHierarchy = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditUndoCurrentField = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditUndoScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniEditFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAddNewAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.mniDeleteUnusedAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniExportHierarchy = new System.Windows.Forms.ToolStripMenuItem();
            this.mniImportHierarchy = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlContent.SuspendLayout();
            this.sptAccountSplitter.SuspendLayout();
            this.sptAccountSplitter.Panel1.SuspendLayout();
            this.sptAccountSplitter.Panel2.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.sptAccountSplitter);
            //
            // sptAccountSplitter
            //
            this.sptAccountSplitter.Name = "sptAccountSplitter";
            this.sptAccountSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sptAccountSplitter.SplitterDistance = 50;
            this.sptAccountSplitter.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.sptAccountSplitter.Panel1.Controls.Add(this.trvAccounts);
            this.sptAccountSplitter.Panel2.Controls.Add(this.pnlDetails);
            //
            // trvAccounts
            //
            this.trvAccounts.Name = "trvAccounts";
            this.trvAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlDetails.Controls.Add(this.tableLayoutPanel1);
            //
            // txtDetailAccountCode
            //
            this.txtDetailAccountCode.Location = new System.Drawing.Point(2,2);
            this.txtDetailAccountCode.Name = "txtDetailAccountCode";
            this.txtDetailAccountCode.Size = new System.Drawing.Size(150, 28);
            this.txtDetailAccountCode.Leave += new System.EventHandler(this.ChangeAccountCodeValue);
            //
            // lblDetailAccountCode
            //
            this.lblDetailAccountCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailAccountCode.Name = "lblDetailAccountCode";
            this.lblDetailAccountCode.AutoSize = true;
            this.lblDetailAccountCode.Text = "Account Code:";
            this.lblDetailAccountCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAccountCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailAccountCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailAccountType
            //
            this.cmbDetailAccountType.Location = new System.Drawing.Point(2,2);
            this.cmbDetailAccountType.Name = "cmbDetailAccountType";
            this.cmbDetailAccountType.Size = new System.Drawing.Size(150, 28);
            this.cmbDetailAccountType.Items.AddRange(new object[] {"Income","Expense","Asset","Equity","Liability"});
            //
            // lblDetailAccountType
            //
            this.lblDetailAccountType.Location = new System.Drawing.Point(2,2);
            this.lblDetailAccountType.Name = "lblDetailAccountType";
            this.lblDetailAccountType.AutoSize = true;
            this.lblDetailAccountType.Text = "Account Type:";
            this.lblDetailAccountType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAccountType.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailAccountType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailEngAccountCodeLongDesc
            //
            this.txtDetailEngAccountCodeLongDesc.Location = new System.Drawing.Point(2,2);
            this.txtDetailEngAccountCodeLongDesc.Name = "txtDetailEngAccountCodeLongDesc";
            this.txtDetailEngAccountCodeLongDesc.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailEngAccountCodeLongDesc
            //
            this.lblDetailEngAccountCodeLongDesc.Location = new System.Drawing.Point(2,2);
            this.lblDetailEngAccountCodeLongDesc.Name = "lblDetailEngAccountCodeLongDesc";
            this.lblDetailEngAccountCodeLongDesc.AutoSize = true;
            this.lblDetailEngAccountCodeLongDesc.Text = "Description Long English:";
            this.lblDetailEngAccountCodeLongDesc.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailEngAccountCodeLongDesc.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailEngAccountCodeLongDesc.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailEngAccountCodeShortDesc
            //
            this.txtDetailEngAccountCodeShortDesc.Location = new System.Drawing.Point(2,2);
            this.txtDetailEngAccountCodeShortDesc.Name = "txtDetailEngAccountCodeShortDesc";
            this.txtDetailEngAccountCodeShortDesc.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailEngAccountCodeShortDesc
            //
            this.lblDetailEngAccountCodeShortDesc.Location = new System.Drawing.Point(2,2);
            this.lblDetailEngAccountCodeShortDesc.Name = "lblDetailEngAccountCodeShortDesc";
            this.lblDetailEngAccountCodeShortDesc.AutoSize = true;
            this.lblDetailEngAccountCodeShortDesc.Text = "Description Short English:";
            this.lblDetailEngAccountCodeShortDesc.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailEngAccountCodeShortDesc.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailEngAccountCodeShortDesc.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailAccountCodeLongDesc
            //
            this.txtDetailAccountCodeLongDesc.Location = new System.Drawing.Point(2,2);
            this.txtDetailAccountCodeLongDesc.Name = "txtDetailAccountCodeLongDesc";
            this.txtDetailAccountCodeLongDesc.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailAccountCodeLongDesc
            //
            this.lblDetailAccountCodeLongDesc.Location = new System.Drawing.Point(2,2);
            this.lblDetailAccountCodeLongDesc.Name = "lblDetailAccountCodeLongDesc";
            this.lblDetailAccountCodeLongDesc.AutoSize = true;
            this.lblDetailAccountCodeLongDesc.Text = "Description Long Local:";
            this.lblDetailAccountCodeLongDesc.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAccountCodeLongDesc.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailAccountCodeLongDesc.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailAccountCodeShortDesc
            //
            this.txtDetailAccountCodeShortDesc.Location = new System.Drawing.Point(2,2);
            this.txtDetailAccountCodeShortDesc.Name = "txtDetailAccountCodeShortDesc";
            this.txtDetailAccountCodeShortDesc.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailAccountCodeShortDesc
            //
            this.lblDetailAccountCodeShortDesc.Location = new System.Drawing.Point(2,2);
            this.lblDetailAccountCodeShortDesc.Name = "lblDetailAccountCodeShortDesc";
            this.lblDetailAccountCodeShortDesc.AutoSize = true;
            this.lblDetailAccountCodeShortDesc.Text = "Description Short Local:";
            this.lblDetailAccountCodeShortDesc.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAccountCodeShortDesc.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailAccountCodeShortDesc.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailValidCcCombo
            //
            this.cmbDetailValidCcCombo.Location = new System.Drawing.Point(2,2);
            this.cmbDetailValidCcCombo.Name = "cmbDetailValidCcCombo";
            this.cmbDetailValidCcCombo.Size = new System.Drawing.Size(150, 28);
            this.cmbDetailValidCcCombo.Items.AddRange(new object[] {"All","Foreign","Local"});
            //
            // lblDetailValidCcCombo
            //
            this.lblDetailValidCcCombo.Location = new System.Drawing.Point(2,2);
            this.lblDetailValidCcCombo.Name = "lblDetailValidCcCombo";
            this.lblDetailValidCcCombo.AutoSize = true;
            this.lblDetailValidCcCombo.Text = "Valid Cost Centres:";
            this.lblDetailValidCcCombo.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailValidCcCombo.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailValidCcCombo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkDetailBankAccountFlag
            //
            this.chkDetailBankAccountFlag.Location = new System.Drawing.Point(2,2);
            this.chkDetailBankAccountFlag.Name = "chkDetailBankAccountFlag";
            this.chkDetailBankAccountFlag.Size = new System.Drawing.Size(30, 28);
            this.chkDetailBankAccountFlag.Text = "";
            this.chkDetailBankAccountFlag.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblDetailBankAccountFlag
            //
            this.lblDetailBankAccountFlag.Location = new System.Drawing.Point(2,2);
            this.lblDetailBankAccountFlag.Name = "lblDetailBankAccountFlag";
            this.lblDetailBankAccountFlag.AutoSize = true;
            this.lblDetailBankAccountFlag.Text = "Bank Account:";
            this.lblDetailBankAccountFlag.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailBankAccountFlag.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailBankAccountFlag.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkDetailAccountActiveFlag
            //
            this.chkDetailAccountActiveFlag.Location = new System.Drawing.Point(2,2);
            this.chkDetailAccountActiveFlag.Name = "chkDetailAccountActiveFlag";
            this.chkDetailAccountActiveFlag.Size = new System.Drawing.Size(30, 28);
            this.chkDetailAccountActiveFlag.Text = "";
            this.chkDetailAccountActiveFlag.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblDetailAccountActiveFlag
            //
            this.lblDetailAccountActiveFlag.Location = new System.Drawing.Point(2,2);
            this.lblDetailAccountActiveFlag.Name = "lblDetailAccountActiveFlag";
            this.lblDetailAccountActiveFlag.AutoSize = true;
            this.lblDetailAccountActiveFlag.Text = "Active:";
            this.lblDetailAccountActiveFlag.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAccountActiveFlag.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailAccountActiveFlag.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkDetailForeignCurrencyFlag
            //
            this.chkDetailForeignCurrencyFlag.Location = new System.Drawing.Point(2,2);
            this.chkDetailForeignCurrencyFlag.Name = "chkDetailForeignCurrencyFlag";
            this.chkDetailForeignCurrencyFlag.AutoSize = true;
            this.chkDetailForeignCurrencyFlag.Text = "Foreign Currency";
            this.chkDetailForeignCurrencyFlag.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkDetailForeignCurrencyFlag.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            //
            // cmbDetailForeignCurrencyCode
            //
            this.cmbDetailForeignCurrencyCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailForeignCurrencyCode.Name = "cmbDetailForeignCurrencyCode";
            this.cmbDetailForeignCurrencyCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailForeignCurrencyCode.ListTable = TCmbAutoPopulated.TListTableEnum.CurrencyCodeList;
            this.chkDetailForeignCurrencyFlag.CheckedChanged += new System.EventHandler(this.chkDetailForeignCurrencyFlagCheckedChanged);
            //
            // ucoAccountAnalysisAttributes
            //
            this.ucoAccountAnalysisAttributes.Name = "ucoAccountAnalysisAttributes";
            this.ucoAccountAnalysisAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblDetailAccountCode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailAccountType, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailEngAccountCodeLongDesc, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailEngAccountCodeShortDesc, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailAccountCodeLongDesc, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailAccountCodeShortDesc, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailValidCcCombo, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailBankAccountFlag, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailAccountActiveFlag, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.chkDetailForeignCurrencyFlag, 0, 9);
            this.tableLayoutPanel1.SetColumnSpan(this.ucoAccountAnalysisAttributes, 2);
            this.tableLayoutPanel1.Controls.Add(this.ucoAccountAnalysisAttributes, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.txtDetailAccountCode, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbDetailAccountType, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtDetailEngAccountCodeLongDesc, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtDetailEngAccountCodeShortDesc, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtDetailAccountCodeLongDesc, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtDetailAccountCodeShortDesc, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cmbDetailValidCcCombo, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.chkDetailBankAccountFlag, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.chkDetailAccountActiveFlag, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.cmbDetailForeignCurrencyCode, 1, 9);
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
            // tbbAddNewAccount
            //
            this.tbbAddNewAccount.Name = "tbbAddNewAccount";
            this.tbbAddNewAccount.AutoSize = true;
            this.tbbAddNewAccount.Click += new System.EventHandler(this.AddNewAccount);
            this.tbbAddNewAccount.Text = "Add Account";
            //
            // tbbDeleteUnusedAccount
            //
            this.tbbDeleteUnusedAccount.Name = "tbbDeleteUnusedAccount";
            this.tbbDeleteUnusedAccount.AutoSize = true;
            this.tbbDeleteUnusedAccount.Click += new System.EventHandler(this.DeleteUnusedAccount);
            this.tbbDeleteUnusedAccount.Text = "Delete Account";
            //
            // tbbExportHierarchy
            //
            this.tbbExportHierarchy.Name = "tbbExportHierarchy";
            this.tbbExportHierarchy.AutoSize = true;
            this.tbbExportHierarchy.Click += new System.EventHandler(this.ExportHierarchy);
            this.tbbExportHierarchy.Text = "Export Hierarchy";
            //
            // tbbImportHierarchy
            //
            this.tbbImportHierarchy.Name = "tbbImportHierarchy";
            this.tbbImportHierarchy.AutoSize = true;
            this.tbbImportHierarchy.Click += new System.EventHandler(this.ImportHierarchy);
            this.tbbImportHierarchy.Text = "Import Hierarchy";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbSave,
                        tbbAddNewAccount,
                        tbbDeleteUnusedAccount,
                        tbbExportHierarchy,
                        tbbImportHierarchy});
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
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
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
                        mniSeparator2,
                        mniEditFind});
            this.mniEdit.Text = "&Edit";
            //
            // mniAddNewAccount
            //
            this.mniAddNewAccount.Name = "mniAddNewAccount";
            this.mniAddNewAccount.AutoSize = true;
            this.mniAddNewAccount.Click += new System.EventHandler(this.AddNewAccount);
            this.mniAddNewAccount.Text = "Add Account";
            //
            // mniDeleteUnusedAccount
            //
            this.mniDeleteUnusedAccount.Name = "mniDeleteUnusedAccount";
            this.mniDeleteUnusedAccount.AutoSize = true;
            this.mniDeleteUnusedAccount.Click += new System.EventHandler(this.DeleteUnusedAccount);
            this.mniDeleteUnusedAccount.Text = "Delete Account";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "Separator";
            //
            // mniExportHierarchy
            //
            this.mniExportHierarchy.Name = "mniExportHierarchy";
            this.mniExportHierarchy.AutoSize = true;
            this.mniExportHierarchy.Click += new System.EventHandler(this.ExportHierarchy);
            this.mniExportHierarchy.Text = "Export Hierarchy";
            //
            // mniImportHierarchy
            //
            this.mniImportHierarchy.Name = "mniImportHierarchy";
            this.mniImportHierarchy.AutoSize = true;
            this.mniImportHierarchy.Click += new System.EventHandler(this.ImportHierarchy);
            this.mniImportHierarchy.Text = "Import Hierarchy";
            //
            // mniAccounts
            //
            this.mniAccounts.Name = "mniAccounts";
            this.mniAccounts.AutoSize = true;
            this.mniAccounts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniAddNewAccount,
                        mniDeleteUnusedAccount,
                        mniSeparator3,
                        mniExportHierarchy,
                        mniImportHierarchy});
            this.mniAccounts.Text = "Accounts";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator4
            //
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.AutoSize = true;
            this.mniSeparator4.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator5
            //
            this.mniSeparator5.Name = "mniSeparator5";
            this.mniSeparator5.AutoSize = true;
            this.mniSeparator5.Text = "-";
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
                        mniSeparator4,
                        mniHelpBugReport,
                        mniSeparator5,
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
                        mniAccounts,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmGLAccountHierarchy
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(660, 700);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmGLAccountHierarchy";
            this.Text = "GL Account Hierarchy";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.sptAccountSplitter.Panel2.ResumeLayout(false);
            this.sptAccountSplitter.Panel1.ResumeLayout(false);
            this.sptAccountSplitter.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.SplitContainer sptAccountSplitter;
        private Ict.Common.Controls.TTrvTreeView trvAccounts;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtDetailAccountCode;
        private System.Windows.Forms.Label lblDetailAccountCode;
        private Ict.Common.Controls.TCmbAutoComplete cmbDetailAccountType;
        private System.Windows.Forms.Label lblDetailAccountType;
        private System.Windows.Forms.TextBox txtDetailEngAccountCodeLongDesc;
        private System.Windows.Forms.Label lblDetailEngAccountCodeLongDesc;
        private System.Windows.Forms.TextBox txtDetailEngAccountCodeShortDesc;
        private System.Windows.Forms.Label lblDetailEngAccountCodeShortDesc;
        private System.Windows.Forms.TextBox txtDetailAccountCodeLongDesc;
        private System.Windows.Forms.Label lblDetailAccountCodeLongDesc;
        private System.Windows.Forms.TextBox txtDetailAccountCodeShortDesc;
        private System.Windows.Forms.Label lblDetailAccountCodeShortDesc;
        private Ict.Common.Controls.TCmbAutoComplete cmbDetailValidCcCombo;
        private System.Windows.Forms.Label lblDetailValidCcCombo;
        private System.Windows.Forms.CheckBox chkDetailBankAccountFlag;
        private System.Windows.Forms.Label lblDetailBankAccountFlag;
        private System.Windows.Forms.CheckBox chkDetailAccountActiveFlag;
        private System.Windows.Forms.Label lblDetailAccountActiveFlag;
        private System.Windows.Forms.CheckBox chkDetailForeignCurrencyFlag;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailForeignCurrencyCode;
        private Ict.Petra.Client.MFinance.Gui.Setup.TUC_AccountAnalysisAttributes ucoAccountAnalysisAttributes;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbSave;
        private System.Windows.Forms.ToolStripButton tbbAddNewAccount;
        private System.Windows.Forms.ToolStripButton tbbDeleteUnusedAccount;
        private System.Windows.Forms.ToolStripButton tbbExportHierarchy;
        private System.Windows.Forms.ToolStripButton tbbImportHierarchy;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileSave;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniFilePrint;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniEdit;
        private System.Windows.Forms.ToolStripMenuItem mniEditUndoCurrentField;
        private System.Windows.Forms.ToolStripMenuItem mniEditUndoScreen;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniEditFind;
        private System.Windows.Forms.ToolStripMenuItem mniAccounts;
        private System.Windows.Forms.ToolStripMenuItem mniAddNewAccount;
        private System.Windows.Forms.ToolStripMenuItem mniDeleteUnusedAccount;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniExportHierarchy;
        private System.Windows.Forms.ToolStripMenuItem mniImportHierarchy;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
