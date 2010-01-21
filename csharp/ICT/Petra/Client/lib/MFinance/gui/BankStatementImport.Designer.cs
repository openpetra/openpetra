/* auto generated with nant generateWinforms from BankStatementImport.yaml
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
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MFinance.Gui
{
    partial class TFrmBankStatementImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmBankStatementImport));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tabTransactions = new Ict.Common.Controls.TTabVersatile();
            this.tpgAll = new System.Windows.Forms.TabPage();
            this.sptTransactionDetails = new System.Windows.Forms.SplitContainer();
            this.grdAllTransactions = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.rgrTransactionCategory = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtUnmatched = new System.Windows.Forms.RadioButton();
            this.rbtGift = new System.Windows.Forms.RadioButton();
            this.rbtGL = new System.Windows.Forms.RadioButton();
            this.pnlHostCategorySpecificEdit = new System.Windows.Forms.Panel();
            this.pnlGiftEdit = new System.Windows.Forms.Panel();
            this.txtDonorKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.pnlDetailGrid = new System.Windows.Forms.Panel();
            this.grdGiftDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetailButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddGiftDetail = new System.Windows.Forms.Button();
            this.btnRemoveGiftDetail = new System.Windows.Forms.Button();
            this.pnlEditGiftDetail = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbMotivationDetail = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblMotivationDetail = new System.Windows.Forms.Label();
            this.cmbGiftAccount = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblGiftAccount = new System.Windows.Forms.Label();
            this.cmbGiftCostCentre = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblGiftCostCentre = new System.Windows.Forms.Label();
            this.tpgUnmatched = new System.Windows.Forms.TabPage();
            this.tpgGifts = new System.Windows.Forms.TabPage();
            this.tpgGL = new System.Windows.Forms.TabPage();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbImportNewStatement = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.tbcSelectStatement = new System.Windows.Forms.ToolStripComboBox();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniImportNewStatement = new System.Windows.Forms.ToolStripMenuItem();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlContent.SuspendLayout();
            this.tabTransactions.SuspendLayout();
            this.tpgAll.SuspendLayout();
            this.sptTransactionDetails.SuspendLayout();
            this.sptTransactionDetails.Panel1.SuspendLayout();
            this.sptTransactionDetails.Panel2.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.rgrTransactionCategory.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlHostCategorySpecificEdit.SuspendLayout();
            this.pnlGiftEdit.SuspendLayout();
            this.pnlDetailGrid.SuspendLayout();
            this.pnlDetailButtons.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlEditGiftDetail.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tpgUnmatched.SuspendLayout();
            this.tpgGifts.SuspendLayout();
            this.tpgGL.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.tabTransactions);
            //
            // tpgAll
            //
            this.tpgAll.Name = "tpgAll";
            this.tpgAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpgAll.Controls.Add(this.sptTransactionDetails);
            //
            // sptTransactionDetails
            //
            this.sptTransactionDetails.Name = "sptTransactionDetails";
            this.sptTransactionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sptTransactionDetails.SplitterDistance = 50;
            this.sptTransactionDetails.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sptTransactionDetails.Panel1.Controls.Add(this.grdAllTransactions);
            this.sptTransactionDetails.Panel2.Controls.Add(this.pnlDetails);
            //
            // grdAllTransactions
            //
            this.grdAllTransactions.Name = "grdAllTransactions";
            this.grdAllTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAllTransactions.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.AllTransactionsFocusedRowChanged);
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.AutoSize = true;
            this.pnlDetails.Controls.Add(this.pnlHostCategorySpecificEdit);
            this.pnlDetails.Controls.Add(this.rgrTransactionCategory);
            //
            // rgrTransactionCategory
            //
            this.rgrTransactionCategory.Name = "rgrTransactionCategory";
            this.rgrTransactionCategory.Dock = System.Windows.Forms.DockStyle.Left;
            this.rgrTransactionCategory.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.rgrTransactionCategory.Controls.Add(this.tableLayoutPanel1);
            //
            // rbtUnmatched
            //
            this.rbtUnmatched.Location = new System.Drawing.Point(2,2);
            this.rbtUnmatched.Name = "rbtUnmatched";
            this.rbtUnmatched.AutoSize = true;
            this.rbtUnmatched.CheckedChanged += new System.EventHandler(this.NewTransactionCategory);
            this.rbtUnmatched.Text = "Unmatched";
            this.rbtUnmatched.Checked = true;
            //
            // rbtGift
            //
            this.rbtGift.Location = new System.Drawing.Point(2,2);
            this.rbtGift.Name = "rbtGift";
            this.rbtGift.AutoSize = true;
            this.rbtGift.CheckedChanged += new System.EventHandler(this.NewTransactionCategory);
            this.rbtGift.Text = "Gift";
            //
            // rbtGL
            //
            this.rbtGL.Location = new System.Drawing.Point(2,2);
            this.rbtGL.Name = "rbtGL";
            this.rbtGL.AutoSize = true;
            this.rbtGL.CheckedChanged += new System.EventHandler(this.NewTransactionCategory);
            this.rbtGL.Text = "GL";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.rbtUnmatched, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rbtGift, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rbtGL, 0, 2);
            this.rgrTransactionCategory.Text = "Transaction Category";
            //
            // pnlHostCategorySpecificEdit
            //
            this.pnlHostCategorySpecificEdit.Name = "pnlHostCategorySpecificEdit";
            this.pnlHostCategorySpecificEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHostCategorySpecificEdit.AutoSize = true;
            this.pnlHostCategorySpecificEdit.Controls.Add(this.pnlGiftEdit);
            //
            // pnlGiftEdit
            //
            this.pnlGiftEdit.Name = "pnlGiftEdit";
            this.pnlGiftEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGiftEdit.AutoSize = true;
            this.pnlGiftEdit.Controls.Add(this.pnlDetailGrid);
            this.pnlGiftEdit.Controls.Add(this.pnlEditGiftDetail);
            this.pnlGiftEdit.Controls.Add(this.txtDonorKey);
            //
            // txtDonorKey
            //
            this.txtDonorKey.Name = "txtDonorKey";
            this.txtDonorKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtDonorKey.AutoSize = true;
            this.txtDonorKey.ASpecialSetting = true;
            this.txtDonorKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtDonorKey.ButtonWidth = 40;
            this.txtDonorKey.MaxLength = 32767;
            this.txtDonorKey.ReadOnly = false;
            this.txtDonorKey.TextBoxWidth = 80;
            this.txtDonorKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtDonorKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtDonorKey.PartnerClass = "";
            this.txtDonorKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtDonorKey.ButtonText = "Find";
            //
            // pnlDetailGrid
            //
            this.pnlDetailGrid.Name = "pnlDetailGrid";
            this.pnlDetailGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailGrid.AutoSize = true;
            this.pnlDetailGrid.Controls.Add(this.grdGiftDetails);
            this.pnlDetailGrid.Controls.Add(this.pnlDetailButtons);
            //
            // grdGiftDetails
            //
            this.grdGiftDetails.Name = "grdGiftDetails";
            this.grdGiftDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdGiftDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.GiftDetailsFocusedRowChanged);
            //
            // pnlDetailButtons
            //
            this.pnlDetailButtons.Name = "pnlDetailButtons";
            this.pnlDetailButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetailButtons.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlDetailButtons.Controls.Add(this.tableLayoutPanel2);
            //
            // btnAddGiftDetail
            //
            this.btnAddGiftDetail.Location = new System.Drawing.Point(2,2);
            this.btnAddGiftDetail.Name = "btnAddGiftDetail";
            this.btnAddGiftDetail.AutoSize = true;
            this.btnAddGiftDetail.Click += new System.EventHandler(this.AddGiftDetail);
            this.btnAddGiftDetail.Text = "&Add";
            //
            // btnRemoveGiftDetail
            //
            this.btnRemoveGiftDetail.Location = new System.Drawing.Point(2,2);
            this.btnRemoveGiftDetail.Name = "btnRemoveGiftDetail";
            this.btnRemoveGiftDetail.AutoSize = true;
            this.btnRemoveGiftDetail.Text = "Remove Gift Detail";
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnAddGiftDetail, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnRemoveGiftDetail, 0, 1);
            //
            // pnlEditGiftDetail
            //
            this.pnlEditGiftDetail.Name = "pnlEditGiftDetail";
            this.pnlEditGiftDetail.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlEditGiftDetail.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlEditGiftDetail.Controls.Add(this.tableLayoutPanel3);
            //
            // cmbMotivationDetail
            //
            this.cmbMotivationDetail.Location = new System.Drawing.Point(2,2);
            this.cmbMotivationDetail.Name = "cmbMotivationDetail";
            this.cmbMotivationDetail.Size = new System.Drawing.Size(300, 28);
            this.cmbMotivationDetail.SelectedValueChanged += new System.EventHandler(this.MotivationDetailChanged);
            this.cmbMotivationDetail.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblMotivationDetail
            //
            this.lblMotivationDetail.Location = new System.Drawing.Point(2,2);
            this.lblMotivationDetail.Name = "lblMotivationDetail";
            this.lblMotivationDetail.AutoSize = true;
            this.lblMotivationDetail.Text = "Motivation Detail:";
            this.lblMotivationDetail.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // cmbGiftAccount
            //
            this.cmbGiftAccount.Location = new System.Drawing.Point(2,2);
            this.cmbGiftAccount.Name = "cmbGiftAccount";
            this.cmbGiftAccount.Size = new System.Drawing.Size(300, 28);
            this.cmbGiftAccount.Enabled = false;
            this.cmbGiftAccount.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblGiftAccount
            //
            this.lblGiftAccount.Location = new System.Drawing.Point(2,2);
            this.lblGiftAccount.Name = "lblGiftAccount";
            this.lblGiftAccount.AutoSize = true;
            this.lblGiftAccount.Text = "Account:";
            this.lblGiftAccount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // cmbGiftCostCentre
            //
            this.cmbGiftCostCentre.Location = new System.Drawing.Point(2,2);
            this.cmbGiftCostCentre.Name = "cmbGiftCostCentre";
            this.cmbGiftCostCentre.Size = new System.Drawing.Size(300, 28);
            this.cmbGiftCostCentre.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblGiftCostCentre
            //
            this.lblGiftCostCentre.Location = new System.Drawing.Point(2,2);
            this.lblGiftCostCentre.Name = "lblGiftCostCentre";
            this.lblGiftCostCentre.AutoSize = true;
            this.lblGiftCostCentre.Text = "Cost Centre:";
            this.lblGiftCostCentre.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblMotivationDetail, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblGiftAccount, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblGiftCostCentre, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.cmbMotivationDetail, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbGiftAccount, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbGiftCostCentre, 1, 2);
            this.tpgAll.Text = "All";
            this.tpgAll.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgUnmatched
            //
            this.tpgUnmatched.Location = new System.Drawing.Point(2,2);
            this.tpgUnmatched.Name = "tpgUnmatched";
            this.tpgUnmatched.AutoSize = true;
            this.tpgUnmatched.Text = "Unmatched";
            this.tpgUnmatched.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgGifts
            //
            this.tpgGifts.Location = new System.Drawing.Point(2,2);
            this.tpgGifts.Name = "tpgGifts";
            this.tpgGifts.AutoSize = true;
            this.tpgGifts.Text = "Gifts";
            this.tpgGifts.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgGL
            //
            this.tpgGL.Location = new System.Drawing.Point(2,2);
            this.tpgGL.Name = "tpgGL";
            this.tpgGL.AutoSize = true;
            this.tpgGL.Text = "GL";
            this.tpgGL.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabTransactions
            //
            this.tabTransactions.Name = "tabTransactions";
            this.tabTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTransactions.Controls.Add(this.tpgAll);
            this.tabTransactions.Controls.Add(this.tpgUnmatched);
            this.tabTransactions.Controls.Add(this.tpgGifts);
            this.tabTransactions.Controls.Add(this.tpgGL);
            //
            // tbbImportNewStatement
            //
            this.tbbImportNewStatement.Name = "tbbImportNewStatement";
            this.tbbImportNewStatement.AutoSize = true;
            this.tbbImportNewStatement.Click += new System.EventHandler(this.ImportNewStatement);
            this.tbbImportNewStatement.Text = "&Import new statement";
            //
            // tbbSeparator0
            //
            this.tbbSeparator0.Name = "tbbSeparator0";
            this.tbbSeparator0.AutoSize = true;
            this.tbbSeparator0.Text = "Separator";
            //
            // tbcSelectStatement
            //
            this.tbcSelectStatement.Name = "tbcSelectStatement";
            this.tbcSelectStatement.AutoSize = true;
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbImportNewStatement,
                        tbbSeparator0,
                        tbcSelectStatement});
            //
            // mniImportNewStatement
            //
            this.mniImportNewStatement.Name = "mniImportNewStatement";
            this.mniImportNewStatement.AutoSize = true;
            this.mniImportNewStatement.Click += new System.EventHandler(this.ImportNewStatement);
            this.mniImportNewStatement.Text = "&Import new statement";
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
                           mniImportNewStatement,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
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
                        mniSeparator0,
                        mniHelpBugReport,
                        mniSeparator1,
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
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmBankStatementImport
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 623);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmBankStatementImport";
            this.Text = "Import Bank Statements";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tpgGL.ResumeLayout(false);
            this.tpgGifts.ResumeLayout(false);
            this.tpgUnmatched.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlEditGiftDetail.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlDetailButtons.ResumeLayout(false);
            this.pnlDetailGrid.ResumeLayout(false);
            this.pnlGiftEdit.ResumeLayout(false);
            this.pnlHostCategorySpecificEdit.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.rgrTransactionCategory.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.sptTransactionDetails.Panel2.ResumeLayout(false);
            this.sptTransactionDetails.Panel1.ResumeLayout(false);
            this.sptTransactionDetails.ResumeLayout(false);
            this.tpgAll.ResumeLayout(false);
            this.tabTransactions.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private Ict.Common.Controls.TTabVersatile tabTransactions;
        private System.Windows.Forms.TabPage tpgAll;
        private System.Windows.Forms.SplitContainer sptTransactionDetails;
        private Ict.Common.Controls.TSgrdDataGridPaged grdAllTransactions;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.GroupBox rgrTransactionCategory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rbtUnmatched;
        private System.Windows.Forms.RadioButton rbtGift;
        private System.Windows.Forms.RadioButton rbtGL;
        private System.Windows.Forms.Panel pnlHostCategorySpecificEdit;
        private System.Windows.Forms.Panel pnlGiftEdit;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtDonorKey;
        private System.Windows.Forms.Panel pnlDetailGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdGiftDetails;
        private System.Windows.Forms.Panel pnlDetailButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnAddGiftDetail;
        private System.Windows.Forms.Button btnRemoveGiftDetail;
        private System.Windows.Forms.Panel pnlEditGiftDetail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbMotivationDetail;
        private System.Windows.Forms.Label lblMotivationDetail;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbGiftAccount;
        private System.Windows.Forms.Label lblGiftAccount;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbGiftCostCentre;
        private System.Windows.Forms.Label lblGiftCostCentre;
        private System.Windows.Forms.TabPage tpgUnmatched;
        private System.Windows.Forms.TabPage tpgGifts;
        private System.Windows.Forms.TabPage tpgGL;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbImportNewStatement;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator0;
        private System.Windows.Forms.ToolStripComboBox tbcSelectStatement;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniImportNewStatement;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
