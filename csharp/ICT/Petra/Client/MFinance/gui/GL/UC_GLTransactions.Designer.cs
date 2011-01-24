// auto generated with nant generateWinforms from UC_GLTransactions.yaml
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

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    partial class TUC_GLTransactions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_GLTransactions));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLedgerNumber = new System.Windows.Forms.TextBox();
            this.lblLedgerNumber = new System.Windows.Forms.Label();
            this.txtBatchNumber = new System.Windows.Forms.TextBox();
            this.lblBatchNumber = new System.Windows.Forms.Label();
            this.txtJournalNumber = new System.Windows.Forms.TextBox();
            this.lblJournalNumber = new System.Windows.Forms.Label();
            this.pnlDetailGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetailButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbDetailCostCentreCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailCostCentreCode = new System.Windows.Forms.Label();
            this.cmbDetailAccountCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailAccountCode = new System.Windows.Forms.Label();
            this.txtDetailNarrative = new System.Windows.Forms.TextBox();
            this.lblDetailNarrative = new System.Windows.Forms.Label();
            this.txtDetailReference = new System.Windows.Forms.TextBox();
            this.lblDetailReference = new System.Windows.Forms.Label();
            this.dtpDetailTransactionDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDetailTransactionDate = new System.Windows.Forms.Label();
            this.cmbDetailKeyMinistryKey = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDetailKeyMinistryKey = new System.Windows.Forms.Label();
            this.pnlDetailAmounts = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTransactionCurrency = new System.Windows.Forms.Label();
            this.lblBaseCurrency = new System.Windows.Forms.Label();
            this.txtDebitAmount = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDebitAmount = new System.Windows.Forms.Label();
            this.txtDebitAmountBase = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDebitAmountBase = new System.Windows.Forms.Label();
            this.txtCreditAmount = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblCreditAmount = new System.Windows.Forms.Label();
            this.txtCreditAmountBase = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblCreditAmountBase = new System.Windows.Forms.Label();
            this.txtDebitTotalAmount = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDebitTotalAmount = new System.Windows.Forms.Label();
            this.txtDebitTotalAmountBase = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDebitTotalAmountBase = new System.Windows.Forms.Label();
            this.txtCreditTotalAmount = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblCreditTotalAmount = new System.Windows.Forms.Label();
            this.txtCreditTotalAmountBase = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblCreditTotalAmountBase = new System.Windows.Forms.Label();

            this.pnlContent.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlDetailGrid.SuspendLayout();
            this.pnlDetailButtons.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlDetailAmounts.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.pnlDetailGrid);
            this.pnlContent.Controls.Add(this.pnlDetails);
            this.pnlContent.Controls.Add(this.pnlInfo);
            //
            // pnlInfo
            //
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlInfo.Controls.Add(this.tableLayoutPanel1);
            //
            // txtLedgerNumber
            //
            this.txtLedgerNumber.Location = new System.Drawing.Point(2,2);
            this.txtLedgerNumber.Name = "txtLedgerNumber";
            this.txtLedgerNumber.Size = new System.Drawing.Size(150, 28);
            this.txtLedgerNumber.ReadOnly = true;
            this.txtLedgerNumber.TabStop = false;
            //
            // lblLedgerNumber
            //
            this.lblLedgerNumber.Location = new System.Drawing.Point(2,2);
            this.lblLedgerNumber.Name = "lblLedgerNumber";
            this.lblLedgerNumber.AutoSize = true;
            this.lblLedgerNumber.Text = "Ledger:";
            this.lblLedgerNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblLedgerNumber.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblLedgerNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtBatchNumber
            //
            this.txtBatchNumber.Location = new System.Drawing.Point(2,2);
            this.txtBatchNumber.Name = "txtBatchNumber";
            this.txtBatchNumber.Size = new System.Drawing.Size(150, 28);
            this.txtBatchNumber.ReadOnly = true;
            this.txtBatchNumber.TabStop = false;
            //
            // lblBatchNumber
            //
            this.lblBatchNumber.Location = new System.Drawing.Point(2,2);
            this.lblBatchNumber.Name = "lblBatchNumber";
            this.lblBatchNumber.AutoSize = true;
            this.lblBatchNumber.Text = "Batch:";
            this.lblBatchNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblBatchNumber.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblBatchNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtJournalNumber
            //
            this.txtJournalNumber.Location = new System.Drawing.Point(2,2);
            this.txtJournalNumber.Name = "txtJournalNumber";
            this.txtJournalNumber.Size = new System.Drawing.Size(150, 28);
            this.txtJournalNumber.ReadOnly = true;
            this.txtJournalNumber.TabStop = false;
            //
            // lblJournalNumber
            //
            this.lblJournalNumber.Location = new System.Drawing.Point(2,2);
            this.lblJournalNumber.Name = "lblJournalNumber";
            this.lblJournalNumber.AutoSize = true;
            this.lblJournalNumber.Text = "Journal:";
            this.lblJournalNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblJournalNumber.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblJournalNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblLedgerNumber, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLedgerNumber, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblBatchNumber, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtBatchNumber, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblJournalNumber, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtJournalNumber, 5, 0);
            //
            // pnlDetailGrid
            //
            this.pnlDetailGrid.Name = "pnlDetailGrid";
            this.pnlDetailGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailGrid.AutoSize = true;
            this.pnlDetailGrid.Controls.Add(this.grdDetails);
            this.pnlDetailGrid.Controls.Add(this.pnlDetailButtons);
            //
            // grdDetails
            //
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChanged);
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
            // btnNew
            //
            this.btnNew.Location = new System.Drawing.Point(2,2);
            this.btnNew.Name = "btnNew";
            this.btnNew.AutoSize = true;
            this.btnNew.Click += new System.EventHandler(this.NewRow);
            this.btnNew.Text = "&Add";
            //
            // btnRemove
            //
            this.btnRemove.Location = new System.Drawing.Point(2,2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.AutoSize = true;
            this.btnRemove.Click += new System.EventHandler(this.RemoveRow);
            this.btnRemove.Text = "&Remove";
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnNew, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnRemove, 0, 1);
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlDetails.Controls.Add(this.tableLayoutPanel3);
            //
            // cmbDetailCostCentreCode
            //
            this.cmbDetailCostCentreCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailCostCentreCode.Name = "cmbDetailCostCentreCode";
            this.cmbDetailCostCentreCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailCostCentreCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailCostCentreCode
            //
            this.lblDetailCostCentreCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailCostCentreCode.Name = "lblDetailCostCentreCode";
            this.lblDetailCostCentreCode.AutoSize = true;
            this.lblDetailCostCentreCode.Text = "Cost Centre Code:";
            this.lblDetailCostCentreCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCostCentreCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCostCentreCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailAccountCode
            //
            this.cmbDetailAccountCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailAccountCode.Name = "cmbDetailAccountCode";
            this.cmbDetailAccountCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailAccountCode.SelectedValueChanged += new System.EventHandler(this.AccountCodeDetailChanged);
            this.cmbDetailAccountCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
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
            // txtDetailNarrative
            //
            this.txtDetailNarrative.Location = new System.Drawing.Point(2,2);
            this.txtDetailNarrative.Name = "txtDetailNarrative";
            this.txtDetailNarrative.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailNarrative
            //
            this.lblDetailNarrative.Location = new System.Drawing.Point(2,2);
            this.lblDetailNarrative.Name = "lblDetailNarrative";
            this.lblDetailNarrative.AutoSize = true;
            this.lblDetailNarrative.Text = "Narrative:";
            this.lblDetailNarrative.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailNarrative.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailNarrative.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailReference
            //
            this.txtDetailReference.Location = new System.Drawing.Point(2,2);
            this.txtDetailReference.Name = "txtDetailReference";
            this.txtDetailReference.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailReference
            //
            this.lblDetailReference.Location = new System.Drawing.Point(2,2);
            this.lblDetailReference.Name = "lblDetailReference";
            this.lblDetailReference.AutoSize = true;
            this.lblDetailReference.Text = "Reference:";
            this.lblDetailReference.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailReference.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailReference.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // dtpDetailTransactionDate
            //
            this.dtpDetailTransactionDate.Location = new System.Drawing.Point(2,2);
            this.dtpDetailTransactionDate.Name = "dtpDetailTransactionDate";
            this.dtpDetailTransactionDate.Size = new System.Drawing.Size(94, 28);
            //
            // lblDetailTransactionDate
            //
            this.lblDetailTransactionDate.Location = new System.Drawing.Point(2,2);
            this.lblDetailTransactionDate.Name = "lblDetailTransactionDate";
            this.lblDetailTransactionDate.AutoSize = true;
            this.lblDetailTransactionDate.Text = "Transaction Date:";
            this.lblDetailTransactionDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailTransactionDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailTransactionDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailKeyMinistryKey
            //
            this.cmbDetailKeyMinistryKey.Location = new System.Drawing.Point(2,2);
            this.cmbDetailKeyMinistryKey.Name = "cmbDetailKeyMinistryKey";
            this.cmbDetailKeyMinistryKey.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailKeyMinistryKey
            //
            this.lblDetailKeyMinistryKey.Location = new System.Drawing.Point(2,2);
            this.lblDetailKeyMinistryKey.Name = "lblDetailKeyMinistryKey";
            this.lblDetailKeyMinistryKey.AutoSize = true;
            this.lblDetailKeyMinistryKey.Text = "Key Ministry:";
            this.lblDetailKeyMinistryKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailKeyMinistryKey.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailKeyMinistryKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // pnlDetailAmounts
            //
            this.pnlDetailAmounts.Location = new System.Drawing.Point(2,2);
            this.pnlDetailAmounts.Name = "pnlDetailAmounts";
            this.pnlDetailAmounts.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.pnlDetailAmounts.Controls.Add(this.tableLayoutPanel4);
            //
            // lblTransactionCurrency
            //
            this.lblTransactionCurrency.Location = new System.Drawing.Point(2,2);
            this.lblTransactionCurrency.Name = "lblTransactionCurrency";
            this.lblTransactionCurrency.AutoSize = true;
            this.lblTransactionCurrency.Text = "TODOTransactionCurrency:";
            this.lblTransactionCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // lblBaseCurrency
            //
            this.lblBaseCurrency.Location = new System.Drawing.Point(2,2);
            this.lblBaseCurrency.Name = "lblBaseCurrency";
            this.lblBaseCurrency.AutoSize = true;
            this.lblBaseCurrency.Text = "TODOBase Currency:";
            this.lblBaseCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtDebitAmount
            //
            this.txtDebitAmount.Location = new System.Drawing.Point(2,2);
            this.txtDebitAmount.Name = "txtDebitAmount";
            this.txtDebitAmount.Size = new System.Drawing.Size(150, 28);
            this.txtDebitAmount.TextChanged += new System.EventHandler(this.UpdateBaseAndTotals);
            this.txtDebitAmount.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtDebitAmount.DecimalPlaces = 2;
            this.txtDebitAmount.NullValueAllowed = true;
            //
            // lblDebitAmount
            //
            this.lblDebitAmount.Location = new System.Drawing.Point(2,2);
            this.lblDebitAmount.Name = "lblDebitAmount";
            this.lblDebitAmount.AutoSize = true;
            this.lblDebitAmount.Text = "Dr Amount:";
            this.lblDebitAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDebitAmount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDebitAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDebitAmountBase
            //
            this.txtDebitAmountBase.Location = new System.Drawing.Point(2,2);
            this.txtDebitAmountBase.Name = "txtDebitAmountBase";
            this.txtDebitAmountBase.Size = new System.Drawing.Size(150, 28);
            this.txtDebitAmountBase.ReadOnly = true;
            this.txtDebitAmountBase.TabStop = false;
            this.txtDebitAmountBase.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtDebitAmountBase.DecimalPlaces = 2;
            this.txtDebitAmountBase.NullValueAllowed = true;
            //
            // lblDebitAmountBase
            //
            this.lblDebitAmountBase.Location = new System.Drawing.Point(2,2);
            this.lblDebitAmountBase.Name = "lblDebitAmountBase";
            this.lblDebitAmountBase.AutoSize = true;
            this.lblDebitAmountBase.Text = "Dr Amount:";
            this.lblDebitAmountBase.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDebitAmountBase.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDebitAmountBase.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtCreditAmount
            //
            this.txtCreditAmount.Location = new System.Drawing.Point(2,2);
            this.txtCreditAmount.Name = "txtCreditAmount";
            this.txtCreditAmount.Size = new System.Drawing.Size(150, 28);
            this.txtCreditAmount.TextChanged += new System.EventHandler(this.UpdateBaseAndTotals);
            this.txtCreditAmount.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtCreditAmount.DecimalPlaces = 2;
            this.txtCreditAmount.NullValueAllowed = true;
            //
            // lblCreditAmount
            //
            this.lblCreditAmount.Location = new System.Drawing.Point(2,2);
            this.lblCreditAmount.Name = "lblCreditAmount";
            this.lblCreditAmount.AutoSize = true;
            this.lblCreditAmount.Text = "Cr Amount:";
            this.lblCreditAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCreditAmount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCreditAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtCreditAmountBase
            //
            this.txtCreditAmountBase.Location = new System.Drawing.Point(2,2);
            this.txtCreditAmountBase.Name = "txtCreditAmountBase";
            this.txtCreditAmountBase.Size = new System.Drawing.Size(150, 28);
            this.txtCreditAmountBase.ReadOnly = true;
            this.txtCreditAmountBase.TabStop = false;
            this.txtCreditAmountBase.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtCreditAmountBase.DecimalPlaces = 2;
            this.txtCreditAmountBase.NullValueAllowed = true;
            //
            // lblCreditAmountBase
            //
            this.lblCreditAmountBase.Location = new System.Drawing.Point(2,2);
            this.lblCreditAmountBase.Name = "lblCreditAmountBase";
            this.lblCreditAmountBase.AutoSize = true;
            this.lblCreditAmountBase.Text = "Cr Amount:";
            this.lblCreditAmountBase.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCreditAmountBase.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCreditAmountBase.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDebitTotalAmount
            //
            this.txtDebitTotalAmount.Location = new System.Drawing.Point(2,2);
            this.txtDebitTotalAmount.Name = "txtDebitTotalAmount";
            this.txtDebitTotalAmount.Size = new System.Drawing.Size(150, 28);
            this.txtDebitTotalAmount.ReadOnly = true;
            this.txtDebitTotalAmount.TabStop = false;
            this.txtDebitTotalAmount.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtDebitTotalAmount.DecimalPlaces = 2;
            this.txtDebitTotalAmount.NullValueAllowed = true;
            //
            // lblDebitTotalAmount
            //
            this.lblDebitTotalAmount.Location = new System.Drawing.Point(2,2);
            this.lblDebitTotalAmount.Name = "lblDebitTotalAmount";
            this.lblDebitTotalAmount.AutoSize = true;
            this.lblDebitTotalAmount.Text = "Debit Total:";
            this.lblDebitTotalAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDebitTotalAmount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDebitTotalAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDebitTotalAmountBase
            //
            this.txtDebitTotalAmountBase.Location = new System.Drawing.Point(2,2);
            this.txtDebitTotalAmountBase.Name = "txtDebitTotalAmountBase";
            this.txtDebitTotalAmountBase.Size = new System.Drawing.Size(150, 28);
            this.txtDebitTotalAmountBase.ReadOnly = true;
            this.txtDebitTotalAmountBase.TabStop = false;
            this.txtDebitTotalAmountBase.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtDebitTotalAmountBase.DecimalPlaces = 2;
            this.txtDebitTotalAmountBase.NullValueAllowed = true;
            //
            // lblDebitTotalAmountBase
            //
            this.lblDebitTotalAmountBase.Location = new System.Drawing.Point(2,2);
            this.lblDebitTotalAmountBase.Name = "lblDebitTotalAmountBase";
            this.lblDebitTotalAmountBase.AutoSize = true;
            this.lblDebitTotalAmountBase.Text = "Debit Total:";
            this.lblDebitTotalAmountBase.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDebitTotalAmountBase.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDebitTotalAmountBase.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtCreditTotalAmount
            //
            this.txtCreditTotalAmount.Location = new System.Drawing.Point(2,2);
            this.txtCreditTotalAmount.Name = "txtCreditTotalAmount";
            this.txtCreditTotalAmount.Size = new System.Drawing.Size(150, 28);
            this.txtCreditTotalAmount.ReadOnly = true;
            this.txtCreditTotalAmount.TabStop = false;
            this.txtCreditTotalAmount.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtCreditTotalAmount.DecimalPlaces = 2;
            this.txtCreditTotalAmount.NullValueAllowed = true;
            //
            // lblCreditTotalAmount
            //
            this.lblCreditTotalAmount.Location = new System.Drawing.Point(2,2);
            this.lblCreditTotalAmount.Name = "lblCreditTotalAmount";
            this.lblCreditTotalAmount.AutoSize = true;
            this.lblCreditTotalAmount.Text = "Credit Total:";
            this.lblCreditTotalAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCreditTotalAmount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCreditTotalAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtCreditTotalAmountBase
            //
            this.txtCreditTotalAmountBase.Location = new System.Drawing.Point(2,2);
            this.txtCreditTotalAmountBase.Name = "txtCreditTotalAmountBase";
            this.txtCreditTotalAmountBase.Size = new System.Drawing.Size(150, 28);
            this.txtCreditTotalAmountBase.ReadOnly = true;
            this.txtCreditTotalAmountBase.TabStop = false;
            this.txtCreditTotalAmountBase.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtCreditTotalAmountBase.DecimalPlaces = 2;
            this.txtCreditTotalAmountBase.NullValueAllowed = true;
            //
            // lblCreditTotalAmountBase
            //
            this.lblCreditTotalAmountBase.Location = new System.Drawing.Point(2,2);
            this.lblCreditTotalAmountBase.Name = "lblCreditTotalAmountBase";
            this.lblCreditTotalAmountBase.AutoSize = true;
            this.lblCreditTotalAmountBase.Text = "Credit Total:";
            this.lblCreditTotalAmountBase.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCreditTotalAmountBase.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCreditTotalAmountBase.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 5;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.SetColumnSpan(this.lblTransactionCurrency, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblTransactionCurrency, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblDebitAmount, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblCreditAmount, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblDebitTotalAmount, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblCreditTotalAmount, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.txtDebitAmount, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtCreditAmount, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtDebitTotalAmount, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.txtCreditTotalAmount, 1, 4);
            this.tableLayoutPanel4.SetColumnSpan(this.lblBaseCurrency, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblBaseCurrency, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblDebitAmountBase, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblCreditAmountBase, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblDebitTotalAmountBase, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblCreditTotalAmountBase, 2, 4);
            this.tableLayoutPanel4.Controls.Add(this.txtDebitAmountBase, 3, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtCreditAmountBase, 3, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtDebitTotalAmountBase, 3, 3);
            this.tableLayoutPanel4.Controls.Add(this.txtCreditTotalAmountBase, 3, 4);
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 7;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblDetailCostCentreCode, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailAccountCode, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailNarrative, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailReference, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailTransactionDate, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailKeyMinistryKey, 0, 5);
            this.tableLayoutPanel3.SetColumnSpan(this.pnlDetailAmounts, 2);
            this.tableLayoutPanel3.Controls.Add(this.pnlDetailAmounts, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailCostCentreCode, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailAccountCode, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailNarrative, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailReference, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.dtpDetailTransactionDate, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailKeyMinistryKey, 1, 5);

            //
            // TUC_GLTransactions
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TUC_GLTransactions";
            this.Text = "";

            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlDetailAmounts.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlDetailButtons.ResumeLayout(false);
            this.pnlDetailGrid.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtLedgerNumber;
        private System.Windows.Forms.Label lblLedgerNumber;
        private System.Windows.Forms.TextBox txtBatchNumber;
        private System.Windows.Forms.Label lblBatchNumber;
        private System.Windows.Forms.TextBox txtJournalNumber;
        private System.Windows.Forms.Label lblJournalNumber;
        private System.Windows.Forms.Panel pnlDetailGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlDetailButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailCostCentreCode;
        private System.Windows.Forms.Label lblDetailCostCentreCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailAccountCode;
        private System.Windows.Forms.Label lblDetailAccountCode;
        private System.Windows.Forms.TextBox txtDetailNarrative;
        private System.Windows.Forms.Label lblDetailNarrative;
        private System.Windows.Forms.TextBox txtDetailReference;
        private System.Windows.Forms.Label lblDetailReference;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDetailTransactionDate;
        private System.Windows.Forms.Label lblDetailTransactionDate;
        private Ict.Common.Controls.TCmbAutoComplete cmbDetailKeyMinistryKey;
        private System.Windows.Forms.Label lblDetailKeyMinistryKey;
        private System.Windows.Forms.Panel pnlDetailAmounts;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label lblTransactionCurrency;
        private System.Windows.Forms.Label lblBaseCurrency;
        private Ict.Common.Controls.TTxtNumericTextBox txtDebitAmount;
        private System.Windows.Forms.Label lblDebitAmount;
        private Ict.Common.Controls.TTxtNumericTextBox txtDebitAmountBase;
        private System.Windows.Forms.Label lblDebitAmountBase;
        private Ict.Common.Controls.TTxtNumericTextBox txtCreditAmount;
        private System.Windows.Forms.Label lblCreditAmount;
        private Ict.Common.Controls.TTxtNumericTextBox txtCreditAmountBase;
        private System.Windows.Forms.Label lblCreditAmountBase;
        private Ict.Common.Controls.TTxtNumericTextBox txtDebitTotalAmount;
        private System.Windows.Forms.Label lblDebitTotalAmount;
        private Ict.Common.Controls.TTxtNumericTextBox txtDebitTotalAmountBase;
        private System.Windows.Forms.Label lblDebitTotalAmountBase;
        private Ict.Common.Controls.TTxtNumericTextBox txtCreditTotalAmount;
        private System.Windows.Forms.Label lblCreditTotalAmount;
        private Ict.Common.Controls.TTxtNumericTextBox txtCreditTotalAmountBase;
        private System.Windows.Forms.Label lblCreditTotalAmountBase;
    }
}
