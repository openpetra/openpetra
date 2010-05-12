/* auto generated with nant generateWinforms from MainForm.yaml
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 *///
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

namespace Ict.Petra.Client.MFinance.Gui.BankImport
{
    partial class TFrmMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmMainForm));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlInfoStatement = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtBankName = new System.Windows.Forms.TextBox();
            this.lblBankName = new System.Windows.Forms.Label();
            this.txtDateStatement = new System.Windows.Forms.TextBox();
            this.lblDateStatement = new System.Windows.Forms.Label();
            this.txtStartBalance = new System.Windows.Forms.TextBox();
            this.lblStartBalance = new System.Windows.Forms.Label();
            this.txtEndBalance = new System.Windows.Forms.TextBox();
            this.lblEndBalance = new System.Windows.Forms.Label();
            this.txtNumberMatched = new System.Windows.Forms.TextBox();
            this.lblNumberMatched = new System.Windows.Forms.Label();
            this.txtValueMatchedGifts = new System.Windows.Forms.TextBox();
            this.lblValueMatchedGifts = new System.Windows.Forms.Label();
            this.txtValueMatchedGiftBatch = new System.Windows.Forms.TextBox();
            this.lblValueMatchedGiftBatch = new System.Windows.Forms.Label();
            this.txtNumberUnmatched = new System.Windows.Forms.TextBox();
            this.lblNumberUnmatched = new System.Windows.Forms.Label();
            this.txtValueUnmatchedGifts = new System.Windows.Forms.TextBox();
            this.lblValueUnmatchedGifts = new System.Windows.Forms.Label();
            this.txtNumberOther = new System.Windows.Forms.TextBox();
            this.lblNumberOther = new System.Windows.Forms.Label();
            this.txtValueOtherCredit = new System.Windows.Forms.TextBox();
            this.lblValueOtherCredit = new System.Windows.Forms.Label();
            this.txtValueOtherDebit = new System.Windows.Forms.TextBox();
            this.lblValueOtherDebit = new System.Windows.Forms.Label();
            this.txtNumberAltogether = new System.Windows.Forms.TextBox();
            this.lblNumberAltogether = new System.Windows.Forms.Label();
            this.txtSumCredit = new System.Windows.Forms.TextBox();
            this.lblSumCredit = new System.Windows.Forms.Label();
            this.txtSumDebit = new System.Windows.Forms.TextBox();
            this.lblSumDebit = new System.Windows.Forms.Label();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrFilter = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtAllTransactions = new System.Windows.Forms.RadioButton();
            this.rbtMatchedGifts = new System.Windows.Forms.RadioButton();
            this.rbtUnmatchedGifts = new System.Windows.Forms.RadioButton();
            this.rbtOther = new System.Windows.Forms.RadioButton();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.grdResult = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbSplitAndTrain = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbImportStatement = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbProcessAllNewStatements = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
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
            this.pnlInfoStatement.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.rgrFilter.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
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
            // pnlInfoStatement
            //
            this.pnlInfoStatement.Location = new System.Drawing.Point(2,2);
            this.pnlInfoStatement.Name = "pnlInfoStatement";
            this.pnlInfoStatement.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlInfoStatement.Controls.Add(this.tableLayoutPanel2);
            //
            // txtBankName
            //
            this.txtBankName.Location = new System.Drawing.Point(2,2);
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Size = new System.Drawing.Size(100, 28);
            this.txtBankName.ReadOnly = true;
            //
            // lblBankName
            //
            this.lblBankName.Location = new System.Drawing.Point(2,2);
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.AutoSize = true;
            this.lblBankName.Text = "Bank Name:";
            this.lblBankName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtDateStatement
            //
            this.txtDateStatement.Location = new System.Drawing.Point(2,2);
            this.txtDateStatement.Name = "txtDateStatement";
            this.txtDateStatement.Size = new System.Drawing.Size(100, 28);
            this.txtDateStatement.ReadOnly = true;
            //
            // lblDateStatement
            //
            this.lblDateStatement.Location = new System.Drawing.Point(2,2);
            this.lblDateStatement.Name = "lblDateStatement";
            this.lblDateStatement.AutoSize = true;
            this.lblDateStatement.Text = "Date Statement:";
            this.lblDateStatement.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtStartBalance
            //
            this.txtStartBalance.Location = new System.Drawing.Point(2,2);
            this.txtStartBalance.Name = "txtStartBalance";
            this.txtStartBalance.Size = new System.Drawing.Size(100, 28);
            this.txtStartBalance.ReadOnly = true;
            //
            // lblStartBalance
            //
            this.lblStartBalance.Location = new System.Drawing.Point(2,2);
            this.lblStartBalance.Name = "lblStartBalance";
            this.lblStartBalance.AutoSize = true;
            this.lblStartBalance.Text = "Start Balance:";
            this.lblStartBalance.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtEndBalance
            //
            this.txtEndBalance.Location = new System.Drawing.Point(2,2);
            this.txtEndBalance.Name = "txtEndBalance";
            this.txtEndBalance.Size = new System.Drawing.Size(100, 28);
            this.txtEndBalance.ReadOnly = true;
            //
            // lblEndBalance
            //
            this.lblEndBalance.Location = new System.Drawing.Point(2,2);
            this.lblEndBalance.Name = "lblEndBalance";
            this.lblEndBalance.AutoSize = true;
            this.lblEndBalance.Text = "End Balance:";
            this.lblEndBalance.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtNumberMatched
            //
            this.txtNumberMatched.Location = new System.Drawing.Point(2,2);
            this.txtNumberMatched.Name = "txtNumberMatched";
            this.txtNumberMatched.Size = new System.Drawing.Size(100, 28);
            this.txtNumberMatched.ReadOnly = true;
            //
            // lblNumberMatched
            //
            this.lblNumberMatched.Location = new System.Drawing.Point(2,2);
            this.lblNumberMatched.Name = "lblNumberMatched";
            this.lblNumberMatched.AutoSize = true;
            this.lblNumberMatched.Text = "Number Matched:";
            this.lblNumberMatched.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtValueMatchedGifts
            //
            this.txtValueMatchedGifts.Location = new System.Drawing.Point(2,2);
            this.txtValueMatchedGifts.Name = "txtValueMatchedGifts";
            this.txtValueMatchedGifts.Size = new System.Drawing.Size(100, 28);
            this.txtValueMatchedGifts.ReadOnly = true;
            //
            // lblValueMatchedGifts
            //
            this.lblValueMatchedGifts.Location = new System.Drawing.Point(2,2);
            this.lblValueMatchedGifts.Name = "lblValueMatchedGifts";
            this.lblValueMatchedGifts.AutoSize = true;
            this.lblValueMatchedGifts.Text = "Value Matched Gifts:";
            this.lblValueMatchedGifts.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtValueMatchedGiftBatch
            //
            this.txtValueMatchedGiftBatch.Location = new System.Drawing.Point(2,2);
            this.txtValueMatchedGiftBatch.Name = "txtValueMatchedGiftBatch";
            this.txtValueMatchedGiftBatch.Size = new System.Drawing.Size(100, 28);
            this.txtValueMatchedGiftBatch.ReadOnly = true;
            //
            // lblValueMatchedGiftBatch
            //
            this.lblValueMatchedGiftBatch.Location = new System.Drawing.Point(2,2);
            this.lblValueMatchedGiftBatch.Name = "lblValueMatchedGiftBatch";
            this.lblValueMatchedGiftBatch.AutoSize = true;
            this.lblValueMatchedGiftBatch.Text = "Value Matched Gift Batch:";
            this.lblValueMatchedGiftBatch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtNumberUnmatched
            //
            this.txtNumberUnmatched.Location = new System.Drawing.Point(2,2);
            this.txtNumberUnmatched.Name = "txtNumberUnmatched";
            this.txtNumberUnmatched.Size = new System.Drawing.Size(100, 28);
            this.txtNumberUnmatched.ReadOnly = true;
            //
            // lblNumberUnmatched
            //
            this.lblNumberUnmatched.Location = new System.Drawing.Point(2,2);
            this.lblNumberUnmatched.Name = "lblNumberUnmatched";
            this.lblNumberUnmatched.AutoSize = true;
            this.lblNumberUnmatched.Text = "Number Unmatched:";
            this.lblNumberUnmatched.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtValueUnmatchedGifts
            //
            this.txtValueUnmatchedGifts.Location = new System.Drawing.Point(2,2);
            this.txtValueUnmatchedGifts.Name = "txtValueUnmatchedGifts";
            this.txtValueUnmatchedGifts.Size = new System.Drawing.Size(100, 28);
            this.txtValueUnmatchedGifts.ReadOnly = true;
            //
            // lblValueUnmatchedGifts
            //
            this.lblValueUnmatchedGifts.Location = new System.Drawing.Point(2,2);
            this.lblValueUnmatchedGifts.Name = "lblValueUnmatchedGifts";
            this.lblValueUnmatchedGifts.AutoSize = true;
            this.lblValueUnmatchedGifts.Text = "Value Unmatched Gifts:";
            this.lblValueUnmatchedGifts.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtNumberOther
            //
            this.txtNumberOther.Location = new System.Drawing.Point(2,2);
            this.txtNumberOther.Name = "txtNumberOther";
            this.txtNumberOther.Size = new System.Drawing.Size(100, 28);
            this.txtNumberOther.ReadOnly = true;
            //
            // lblNumberOther
            //
            this.lblNumberOther.Location = new System.Drawing.Point(2,2);
            this.lblNumberOther.Name = "lblNumberOther";
            this.lblNumberOther.AutoSize = true;
            this.lblNumberOther.Text = "Number Other:";
            this.lblNumberOther.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtValueOtherCredit
            //
            this.txtValueOtherCredit.Location = new System.Drawing.Point(2,2);
            this.txtValueOtherCredit.Name = "txtValueOtherCredit";
            this.txtValueOtherCredit.Size = new System.Drawing.Size(100, 28);
            this.txtValueOtherCredit.ReadOnly = true;
            //
            // lblValueOtherCredit
            //
            this.lblValueOtherCredit.Location = new System.Drawing.Point(2,2);
            this.lblValueOtherCredit.Name = "lblValueOtherCredit";
            this.lblValueOtherCredit.AutoSize = true;
            this.lblValueOtherCredit.Text = "Value Other Credit:";
            this.lblValueOtherCredit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtValueOtherDebit
            //
            this.txtValueOtherDebit.Location = new System.Drawing.Point(2,2);
            this.txtValueOtherDebit.Name = "txtValueOtherDebit";
            this.txtValueOtherDebit.Size = new System.Drawing.Size(100, 28);
            this.txtValueOtherDebit.ReadOnly = true;
            //
            // lblValueOtherDebit
            //
            this.lblValueOtherDebit.Location = new System.Drawing.Point(2,2);
            this.lblValueOtherDebit.Name = "lblValueOtherDebit";
            this.lblValueOtherDebit.AutoSize = true;
            this.lblValueOtherDebit.Text = "Value Other Debit:";
            this.lblValueOtherDebit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtNumberAltogether
            //
            this.txtNumberAltogether.Location = new System.Drawing.Point(2,2);
            this.txtNumberAltogether.Name = "txtNumberAltogether";
            this.txtNumberAltogether.Size = new System.Drawing.Size(100, 28);
            this.txtNumberAltogether.ReadOnly = true;
            //
            // lblNumberAltogether
            //
            this.lblNumberAltogether.Location = new System.Drawing.Point(2,2);
            this.lblNumberAltogether.Name = "lblNumberAltogether";
            this.lblNumberAltogether.AutoSize = true;
            this.lblNumberAltogether.Text = "Number Altogether:";
            this.lblNumberAltogether.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtSumCredit
            //
            this.txtSumCredit.Location = new System.Drawing.Point(2,2);
            this.txtSumCredit.Name = "txtSumCredit";
            this.txtSumCredit.Size = new System.Drawing.Size(100, 28);
            this.txtSumCredit.ReadOnly = true;
            //
            // lblSumCredit
            //
            this.lblSumCredit.Location = new System.Drawing.Point(2,2);
            this.lblSumCredit.Name = "lblSumCredit";
            this.lblSumCredit.AutoSize = true;
            this.lblSumCredit.Text = "Sum Credit:";
            this.lblSumCredit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtSumDebit
            //
            this.txtSumDebit.Location = new System.Drawing.Point(2,2);
            this.txtSumDebit.Name = "txtSumDebit";
            this.txtSumDebit.Size = new System.Drawing.Size(100, 28);
            this.txtSumDebit.ReadOnly = true;
            //
            // lblSumDebit
            //
            this.lblSumDebit.Location = new System.Drawing.Point(2,2);
            this.lblSumDebit.Name = "lblSumDebit";
            this.lblSumDebit.AutoSize = true;
            this.lblSumDebit.Text = "Sum Debit:";
            this.lblSumDebit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblBankName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblStartBalance, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblNumberMatched, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblNumberUnmatched, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblNumberOther, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblNumberAltogether, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtBankName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtStartBalance, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtNumberMatched, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtNumberUnmatched, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtNumberOther, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtNumberAltogether, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.lblDateStatement, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblEndBalance, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblValueMatchedGifts, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblValueUnmatchedGifts, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblValueOtherCredit, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblSumCredit, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtDateStatement, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtEndBalance, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtValueMatchedGifts, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtValueUnmatchedGifts, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtValueOtherCredit, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtSumCredit, 3, 5);
            this.tableLayoutPanel2.Controls.Add(this.lblValueMatchedGiftBatch, 4, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblValueOtherDebit, 4, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblSumDebit, 4, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtValueMatchedGiftBatch, 5, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtValueOtherDebit, 5, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtSumDebit, 5, 5);
            //
            // pnlFilter
            //
            this.pnlFilter.Location = new System.Drawing.Point(2,2);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlFilter.Controls.Add(this.tableLayoutPanel3);
            //
            // rgrFilter
            //
            this.rgrFilter.Location = new System.Drawing.Point(2,2);
            this.rgrFilter.Name = "rgrFilter";
            this.rgrFilter.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.rgrFilter.Controls.Add(this.tableLayoutPanel4);
            //
            // rbtAllTransactions
            //
            this.rbtAllTransactions.Location = new System.Drawing.Point(2,2);
            this.rbtAllTransactions.Name = "rbtAllTransactions";
            this.rbtAllTransactions.AutoSize = true;
            this.rbtAllTransactions.CheckedChanged += new System.EventHandler(this.FilterChanged);
            this.rbtAllTransactions.Text = "AllTransactions";
            this.rbtAllTransactions.Checked = true;
            //
            // rbtMatchedGifts
            //
            this.rbtMatchedGifts.Location = new System.Drawing.Point(2,2);
            this.rbtMatchedGifts.Name = "rbtMatchedGifts";
            this.rbtMatchedGifts.AutoSize = true;
            this.rbtMatchedGifts.CheckedChanged += new System.EventHandler(this.FilterChanged);
            this.rbtMatchedGifts.Text = "MatchedGifts";
            //
            // rbtUnmatchedGifts
            //
            this.rbtUnmatchedGifts.Location = new System.Drawing.Point(2,2);
            this.rbtUnmatchedGifts.Name = "rbtUnmatchedGifts";
            this.rbtUnmatchedGifts.AutoSize = true;
            this.rbtUnmatchedGifts.CheckedChanged += new System.EventHandler(this.FilterChanged);
            this.rbtUnmatchedGifts.Text = "UnmatchedGifts";
            //
            // rbtOther
            //
            this.rbtOther.Location = new System.Drawing.Point(2,2);
            this.rbtOther.Name = "rbtOther";
            this.rbtOther.AutoSize = true;
            this.rbtOther.CheckedChanged += new System.EventHandler(this.FilterChanged);
            this.rbtOther.Text = "Other";
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.rbtAllTransactions, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.rbtMatchedGifts, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.rbtUnmatchedGifts, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.rbtOther, 3, 0);
            this.rgrFilter.Text = "Filter";
            //
            // pnlButtons
            //
            this.pnlButtons.Location = new System.Drawing.Point(2,2);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.pnlButtons.Controls.Add(this.tableLayoutPanel5);
            //
            // btnExport
            //
            this.btnExport.Location = new System.Drawing.Point(2,2);
            this.btnExport.Name = "btnExport";
            this.btnExport.AutoSize = true;
            this.btnExport.Click += new System.EventHandler(this.Export);
            this.btnExport.Text = "Export as CSV";
            //
            // btnPrint
            //
            this.btnPrint.Location = new System.Drawing.Point(2,2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.AutoSize = true;
            this.btnPrint.Click += new System.EventHandler(this.Print);
            this.btnPrint.Text = "Print";
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.btnExport, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnPrint, 1, 0);
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.rgrFilter, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.pnlButtons, 0, 1);
            //
            // grdResult
            //
            this.grdResult.Name = "grdResult";
            this.grdResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlInfoStatement, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlFilter, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grdResult, 0, 2);
            //
            // tbbSplitAndTrain
            //
            this.tbbSplitAndTrain.Name = "tbbSplitAndTrain";
            this.tbbSplitAndTrain.AutoSize = true;
            this.tbbSplitAndTrain.Click += new System.EventHandler(this.SplitAndTrain);
            this.tbbSplitAndTrain.Text = "Split and Train";
            //
            // tbbSeparator0
            //
            this.tbbSeparator0.Name = "tbbSeparator0";
            this.tbbSeparator0.AutoSize = true;
            this.tbbSeparator0.Text = "Separator";
            //
            // tbbImportStatement
            //
            this.tbbImportStatement.Name = "tbbImportStatement";
            this.tbbImportStatement.AutoSize = true;
            this.tbbImportStatement.Click += new System.EventHandler(this.ImportStatement);
            this.tbbImportStatement.Text = "&Import Statement";
            //
            // tbbSeparator1
            //
            this.tbbSeparator1.Name = "tbbSeparator1";
            this.tbbSeparator1.AutoSize = true;
            this.tbbSeparator1.Text = "Separator";
            //
            // tbbProcessAllNewStatements
            //
            this.tbbProcessAllNewStatements.Name = "tbbProcessAllNewStatements";
            this.tbbProcessAllNewStatements.AutoSize = true;
            this.tbbProcessAllNewStatements.Click += new System.EventHandler(this.ProcessAllNewStatements);
            this.tbbProcessAllNewStatements.Text = "Create CSV files for all current bank statements";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbSplitAndTrain,
                        tbbSeparator0,
                        tbbImportStatement,
                        tbbSeparator1,
                        tbbProcessAllNewStatements});
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
            // TFrmMainForm
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
            this.Name = "TFrmMainForm";
            this.Text = "Bank import";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.rgrFilter.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlInfoStatement.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlInfoStatement;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtBankName;
        private System.Windows.Forms.Label lblBankName;
        private System.Windows.Forms.TextBox txtDateStatement;
        private System.Windows.Forms.Label lblDateStatement;
        private System.Windows.Forms.TextBox txtStartBalance;
        private System.Windows.Forms.Label lblStartBalance;
        private System.Windows.Forms.TextBox txtEndBalance;
        private System.Windows.Forms.Label lblEndBalance;
        private System.Windows.Forms.TextBox txtNumberMatched;
        private System.Windows.Forms.Label lblNumberMatched;
        private System.Windows.Forms.TextBox txtValueMatchedGifts;
        private System.Windows.Forms.Label lblValueMatchedGifts;
        private System.Windows.Forms.TextBox txtValueMatchedGiftBatch;
        private System.Windows.Forms.Label lblValueMatchedGiftBatch;
        private System.Windows.Forms.TextBox txtNumberUnmatched;
        private System.Windows.Forms.Label lblNumberUnmatched;
        private System.Windows.Forms.TextBox txtValueUnmatchedGifts;
        private System.Windows.Forms.Label lblValueUnmatchedGifts;
        private System.Windows.Forms.TextBox txtNumberOther;
        private System.Windows.Forms.Label lblNumberOther;
        private System.Windows.Forms.TextBox txtValueOtherCredit;
        private System.Windows.Forms.Label lblValueOtherCredit;
        private System.Windows.Forms.TextBox txtValueOtherDebit;
        private System.Windows.Forms.Label lblValueOtherDebit;
        private System.Windows.Forms.TextBox txtNumberAltogether;
        private System.Windows.Forms.Label lblNumberAltogether;
        private System.Windows.Forms.TextBox txtSumCredit;
        private System.Windows.Forms.Label lblSumCredit;
        private System.Windows.Forms.TextBox txtSumDebit;
        private System.Windows.Forms.Label lblSumDebit;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox rgrFilter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton rbtAllTransactions;
        private System.Windows.Forms.RadioButton rbtMatchedGifts;
        private System.Windows.Forms.RadioButton rbtUnmatchedGifts;
        private System.Windows.Forms.RadioButton rbtOther;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrint;
        private Ict.Common.Controls.TSgrdDataGridPaged grdResult;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbSplitAndTrain;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator0;
        private System.Windows.Forms.ToolStripButton tbbImportStatement;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator1;
        private System.Windows.Forms.ToolStripButton tbbProcessAllNewStatements;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
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