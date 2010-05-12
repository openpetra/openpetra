// auto generated with nant generateWinforms from BankStatementImport.yaml
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbSelectBankAccount = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblSelectBankAccount = new System.Windows.Forms.Label();
            this.rgrSelectTransaction = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtListAll = new System.Windows.Forms.RadioButton();
            this.rbtListUnmatched = new System.Windows.Forms.RadioButton();
            this.rbtListGift = new System.Windows.Forms.RadioButton();
            this.rbtListGL = new System.Windows.Forms.RadioButton();
            this.sptTransactionDetails = new System.Windows.Forms.SplitContainer();
            this.grdAllTransactions = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.rgrTransactionCategory = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtUnmatched = new System.Windows.Forms.RadioButton();
            this.rbtGift = new System.Windows.Forms.RadioButton();
            this.rbtGL = new System.Windows.Forms.RadioButton();
            this.pnlHostCategorySpecificEdit = new System.Windows.Forms.Panel();
            this.pnlGiftEdit = new System.Windows.Forms.Panel();
            this.txtDonorKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.pnlDetailGrid = new System.Windows.Forms.Panel();
            this.grdGiftDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetailButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddGiftDetail = new System.Windows.Forms.Button();
            this.btnRemoveGiftDetail = new System.Windows.Forms.Button();
            this.pnlEditGiftDetail = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.cmbMotivationDetail = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblMotivationDetail = new System.Windows.Forms.Label();
            this.cmbGiftAccount = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblGiftAccount = new System.Windows.Forms.Label();
            this.cmbGiftCostCentre = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblGiftCostCentre = new System.Windows.Forms.Label();
            this.pnlGLEdit = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.txtGLNarrative = new System.Windows.Forms.TextBox();
            this.lblGLNarrative = new System.Windows.Forms.Label();
            this.txtGLReference = new System.Windows.Forms.TextBox();
            this.lblGLReference = new System.Windows.Forms.Label();
            this.cmbGLAccount = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblGLAccount = new System.Windows.Forms.Label();
            this.cmbGLCostCentre = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblGLCostCentre = new System.Windows.Forms.Label();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbImportNewStatement = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.cmbSelectStatement = new Ict.Common.Controls.TCmbAutoComplete();
            this.tchSelectStatement = new System.Windows.Forms.ToolStripControlHost(cmbSelectStatement);
            this.tbbSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbSaveMatches = new System.Windows.Forms.ToolStripButton();
            this.tbbCreateGiftBatch = new System.Windows.Forms.ToolStripButton();
            this.tbbCreateGLBatch = new System.Windows.Forms.ToolStripButton();
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
            this.tableLayoutPanel1.SuspendLayout();
            this.rgrSelectTransaction.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.sptTransactionDetails.SuspendLayout();
            this.sptTransactionDetails.Panel1.SuspendLayout();
            this.sptTransactionDetails.Panel2.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.rgrTransactionCategory.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlHostCategorySpecificEdit.SuspendLayout();
            this.pnlGiftEdit.SuspendLayout();
            this.pnlDetailGrid.SuspendLayout();
            this.pnlDetailButtons.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnlEditGiftDetail.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.pnlGLEdit.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlContent.Controls.Add(this.tableLayoutPanel1);
            //
            // cmbSelectBankAccount
            //
            this.cmbSelectBankAccount.Location = new System.Drawing.Point(2,2);
            this.cmbSelectBankAccount.Name = "cmbSelectBankAccount";
            this.cmbSelectBankAccount.Size = new System.Drawing.Size(300, 28);
            this.cmbSelectBankAccount.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblSelectBankAccount
            //
            this.lblSelectBankAccount.Location = new System.Drawing.Point(2,2);
            this.lblSelectBankAccount.Name = "lblSelectBankAccount";
            this.lblSelectBankAccount.AutoSize = true;
            this.lblSelectBankAccount.Text = "Select Bank Account:";
            this.lblSelectBankAccount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblSelectBankAccount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSelectBankAccount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // rgrSelectTransaction
            //
            this.rgrSelectTransaction.Name = "rgrSelectTransaction";
            this.rgrSelectTransaction.Dock = System.Windows.Forms.DockStyle.Top;
            this.rgrSelectTransaction.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.rgrSelectTransaction.Controls.Add(this.tableLayoutPanel2);
            //
            // rbtListAll
            //
            this.rbtListAll.Location = new System.Drawing.Point(2,2);
            this.rbtListAll.Name = "rbtListAll";
            this.rbtListAll.AutoSize = true;
            this.rbtListAll.CheckedChanged += new System.EventHandler(this.TransactionFilterChanged);
            this.rbtListAll.Text = "ListAll";
            this.rbtListAll.Checked = true;
            //
            // rbtListUnmatched
            //
            this.rbtListUnmatched.Location = new System.Drawing.Point(2,2);
            this.rbtListUnmatched.Name = "rbtListUnmatched";
            this.rbtListUnmatched.AutoSize = true;
            this.rbtListUnmatched.CheckedChanged += new System.EventHandler(this.TransactionFilterChanged);
            this.rbtListUnmatched.Text = "ListUnmatched";
            //
            // rbtListGift
            //
            this.rbtListGift.Location = new System.Drawing.Point(2,2);
            this.rbtListGift.Name = "rbtListGift";
            this.rbtListGift.AutoSize = true;
            this.rbtListGift.CheckedChanged += new System.EventHandler(this.TransactionFilterChanged);
            this.rbtListGift.Text = "ListGift";
            //
            // rbtListGL
            //
            this.rbtListGL.Location = new System.Drawing.Point(2,2);
            this.rbtListGL.Name = "rbtListGL";
            this.rbtListGL.AutoSize = true;
            this.rbtListGL.CheckedChanged += new System.EventHandler(this.TransactionFilterChanged);
            this.rbtListGL.Text = "ListGL";
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.rbtListAll, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtListUnmatched, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtListGift, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtListGL, 3, 0);
            this.rgrSelectTransaction.Text = "Select Transaction";
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
            this.pnlDetails.Visible = false;
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
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.rgrTransactionCategory.Controls.Add(this.tableLayoutPanel3);
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
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.rbtUnmatched, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtGift, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.rbtGL, 0, 2);
            this.rgrTransactionCategory.Text = "Transaction Category";
            //
            // pnlHostCategorySpecificEdit
            //
            this.pnlHostCategorySpecificEdit.Name = "pnlHostCategorySpecificEdit";
            this.pnlHostCategorySpecificEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHostCategorySpecificEdit.AutoSize = true;
            this.pnlHostCategorySpecificEdit.Controls.Add(this.pnlGiftEdit);
            this.pnlHostCategorySpecificEdit.Controls.Add(this.pnlGLEdit);
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
            this.txtDonorKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtDonorKey.PartnerClass = "";
            this.txtDonorKey.MaxLength = 32767;
            this.txtDonorKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtDonorKey.TextBoxWidth = 80;
            this.txtDonorKey.ButtonWidth = 40;
            this.txtDonorKey.ReadOnly = false;
            this.txtDonorKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
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
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.pnlDetailButtons.Controls.Add(this.tableLayoutPanel4);
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
            this.btnRemoveGiftDetail.Click += new System.EventHandler(this.RemoveGiftDetail);
            this.btnRemoveGiftDetail.Text = "&Delete";
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnAddGiftDetail, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnRemoveGiftDetail, 0, 1);
            //
            // pnlEditGiftDetail
            //
            this.pnlEditGiftDetail.Name = "pnlEditGiftDetail";
            this.pnlEditGiftDetail.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlEditGiftDetail.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.pnlEditGiftDetail.Controls.Add(this.tableLayoutPanel5);
            //
            // txtAmount
            //
            this.txtAmount.Location = new System.Drawing.Point(2,2);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(150, 28);
            //
            // lblAmount
            //
            this.lblAmount.Location = new System.Drawing.Point(2,2);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.AutoSize = true;
            this.lblAmount.Text = "Amount:";
            this.lblAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAmount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.lblMotivationDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMotivationDetail.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.lblGiftAccount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGiftAccount.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.lblGiftCostCentre.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGiftCostCentre.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.lblAmount, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblMotivationDetail, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.lblGiftAccount, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.lblGiftCostCentre, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.txtAmount, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.cmbMotivationDetail, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.cmbGiftAccount, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.cmbGiftCostCentre, 1, 3);
            //
            // pnlGLEdit
            //
            this.pnlGLEdit.Name = "pnlGLEdit";
            this.pnlGLEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGLEdit.Visible = false;
            this.pnlGLEdit.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            this.pnlGLEdit.Controls.Add(this.tableLayoutPanel6);
            //
            // txtGLNarrative
            //
            this.txtGLNarrative.Location = new System.Drawing.Point(2,2);
            this.txtGLNarrative.Name = "txtGLNarrative";
            this.txtGLNarrative.Size = new System.Drawing.Size(150, 28);
            //
            // lblGLNarrative
            //
            this.lblGLNarrative.Location = new System.Drawing.Point(2,2);
            this.lblGLNarrative.Name = "lblGLNarrative";
            this.lblGLNarrative.AutoSize = true;
            this.lblGLNarrative.Text = "Narrative:";
            this.lblGLNarrative.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGLNarrative.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGLNarrative.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtGLReference
            //
            this.txtGLReference.Location = new System.Drawing.Point(2,2);
            this.txtGLReference.Name = "txtGLReference";
            this.txtGLReference.Size = new System.Drawing.Size(150, 28);
            //
            // lblGLReference
            //
            this.lblGLReference.Location = new System.Drawing.Point(2,2);
            this.lblGLReference.Name = "lblGLReference";
            this.lblGLReference.AutoSize = true;
            this.lblGLReference.Text = "Reference:";
            this.lblGLReference.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGLReference.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGLReference.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbGLAccount
            //
            this.cmbGLAccount.Location = new System.Drawing.Point(2,2);
            this.cmbGLAccount.Name = "cmbGLAccount";
            this.cmbGLAccount.Size = new System.Drawing.Size(300, 28);
            this.cmbGLAccount.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblGLAccount
            //
            this.lblGLAccount.Location = new System.Drawing.Point(2,2);
            this.lblGLAccount.Name = "lblGLAccount";
            this.lblGLAccount.AutoSize = true;
            this.lblGLAccount.Text = "Account:";
            this.lblGLAccount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGLAccount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGLAccount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbGLCostCentre
            //
            this.cmbGLCostCentre.Location = new System.Drawing.Point(2,2);
            this.cmbGLCostCentre.Name = "cmbGLCostCentre";
            this.cmbGLCostCentre.Size = new System.Drawing.Size(300, 28);
            this.cmbGLCostCentre.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblGLCostCentre
            //
            this.lblGLCostCentre.Location = new System.Drawing.Point(2,2);
            this.lblGLCostCentre.Name = "lblGLCostCentre";
            this.lblGLCostCentre.AutoSize = true;
            this.lblGLCostCentre.Text = "Cost Centre:";
            this.lblGLCostCentre.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGLCostCentre.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGLCostCentre.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 4;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.lblGLNarrative, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblGLReference, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.lblGLAccount, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.lblGLCostCentre, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.txtGLNarrative, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.txtGLReference, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.cmbGLAccount, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.cmbGLCostCentre, 1, 3);
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblSelectBankAccount, 0, 0);
            this.tableLayoutPanel1.SetColumnSpan(this.rgrSelectTransaction, 2);
            this.tableLayoutPanel1.Controls.Add(this.rgrSelectTransaction, 0, 1);
            this.tableLayoutPanel1.SetColumnSpan(this.sptTransactionDetails, 2);
            this.tableLayoutPanel1.Controls.Add(this.sptTransactionDetails, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbSelectBankAccount, 1, 0);
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
            // cmbSelectStatement
            //
            this.cmbSelectStatement.Location = new System.Drawing.Point(2,2);
            this.cmbSelectStatement.Name = "cmbSelectStatement";
            this.cmbSelectStatement.Size = new System.Drawing.Size(250, 28);
            //
            // tchSelectStatement
            //
            this.tchSelectStatement.Name = "tchSelectStatement";
            this.tchSelectStatement.AutoSize = true;
            //
            // tbbSeparator1
            //
            this.tbbSeparator1.Name = "tbbSeparator1";
            this.tbbSeparator1.AutoSize = true;
            this.tbbSeparator1.Text = "Separator";
            //
            // tbbSaveMatches
            //
            this.tbbSaveMatches.Name = "tbbSaveMatches";
            this.tbbSaveMatches.AutoSize = true;
            this.tbbSaveMatches.Click += new System.EventHandler(this.SaveMatches);
            this.tbbSaveMatches.Text = "Save Matches";
            //
            // tbbCreateGiftBatch
            //
            this.tbbCreateGiftBatch.Name = "tbbCreateGiftBatch";
            this.tbbCreateGiftBatch.AutoSize = true;
            this.tbbCreateGiftBatch.Click += new System.EventHandler(this.CreateGiftBatch);
            this.tbbCreateGiftBatch.Text = "Create Gift Batch";
            //
            // tbbCreateGLBatch
            //
            this.tbbCreateGLBatch.Name = "tbbCreateGLBatch";
            this.tbbCreateGLBatch.AutoSize = true;
            this.tbbCreateGLBatch.Click += new System.EventHandler(this.CreateGLBatch);
            this.tbbCreateGLBatch.Text = "CreateGL Batch";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbImportNewStatement,
                        tbbSeparator0,
                        tchSelectStatement,
                        tbbSeparator1,
                        tbbSaveMatches,
                        tbbCreateGiftBatch,
                        tbbCreateGLBatch});
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
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(754, 623);

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
            this.tableLayoutPanel6.ResumeLayout(false);
            this.pnlGLEdit.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.pnlEditGiftDetail.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlDetailButtons.ResumeLayout(false);
            this.pnlDetailGrid.ResumeLayout(false);
            this.pnlGiftEdit.ResumeLayout(false);
            this.pnlHostCategorySpecificEdit.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.rgrTransactionCategory.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.sptTransactionDetails.Panel2.ResumeLayout(false);
            this.sptTransactionDetails.Panel1.ResumeLayout(false);
            this.sptTransactionDetails.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.rgrSelectTransaction.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbSelectBankAccount;
        private System.Windows.Forms.Label lblSelectBankAccount;
        private System.Windows.Forms.GroupBox rgrSelectTransaction;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtListAll;
        private System.Windows.Forms.RadioButton rbtListUnmatched;
        private System.Windows.Forms.RadioButton rbtListGift;
        private System.Windows.Forms.RadioButton rbtListGL;
        private System.Windows.Forms.SplitContainer sptTransactionDetails;
        private Ict.Common.Controls.TSgrdDataGridPaged grdAllTransactions;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.GroupBox rgrTransactionCategory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbtUnmatched;
        private System.Windows.Forms.RadioButton rbtGift;
        private System.Windows.Forms.RadioButton rbtGL;
        private System.Windows.Forms.Panel pnlHostCategorySpecificEdit;
        private System.Windows.Forms.Panel pnlGiftEdit;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtDonorKey;
        private System.Windows.Forms.Panel pnlDetailGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdGiftDetails;
        private System.Windows.Forms.Panel pnlDetailButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnAddGiftDetail;
        private System.Windows.Forms.Button btnRemoveGiftDetail;
        private System.Windows.Forms.Panel pnlEditGiftDetail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label lblAmount;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbMotivationDetail;
        private System.Windows.Forms.Label lblMotivationDetail;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbGiftAccount;
        private System.Windows.Forms.Label lblGiftAccount;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbGiftCostCentre;
        private System.Windows.Forms.Label lblGiftCostCentre;
        private System.Windows.Forms.Panel pnlGLEdit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TextBox txtGLNarrative;
        private System.Windows.Forms.Label lblGLNarrative;
        private System.Windows.Forms.TextBox txtGLReference;
        private System.Windows.Forms.Label lblGLReference;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbGLAccount;
        private System.Windows.Forms.Label lblGLAccount;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbGLCostCentre;
        private System.Windows.Forms.Label lblGLCostCentre;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbImportNewStatement;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator0;
        private Ict.Common.Controls.TCmbAutoComplete cmbSelectStatement;
        private System.Windows.Forms.ToolStripControlHost tchSelectStatement;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator1;
        private System.Windows.Forms.ToolStripButton tbbSaveMatches;
        private System.Windows.Forms.ToolStripButton tbbCreateGiftBatch;
        private System.Windows.Forms.ToolStripButton tbbCreateGLBatch;
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
