// auto generated with nant generateWinforms from UC_GiftBatches.yaml
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

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    partial class TUC_GiftBatches
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_GiftBatches));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLedgerInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLedgerNumber = new System.Windows.Forms.TextBox();
            this.lblLedgerNumber = new System.Windows.Forms.Label();
            this.rgrShowBatches = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtPosted = new System.Windows.Forms.RadioButton();
            this.rbtEditing = new System.Windows.Forms.RadioButton();
            this.rbtAll = new System.Windows.Forms.RadioButton();
            this.pnlDetailGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetailButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPostBatch = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailBatchDescription = new System.Windows.Forms.TextBox();
            this.lblDetailBatchDescription = new System.Windows.Forms.Label();
            this.cmbDetailBankCostCentre = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailBankCostCentre = new System.Windows.Forms.Label();
            this.cmbDetailBankAccountCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailBankAccountCode = new System.Windows.Forms.Label();
            this.dtpDetailGlEffectiveDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDetailGlEffectiveDate = new System.Windows.Forms.Label();
            this.lblValidDateRange = new System.Windows.Forms.Label();
            this.txtDetailHashTotal = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailHashTotal = new System.Windows.Forms.Label();
            this.cmbDetailCurrencyCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailCurrencyCode = new System.Windows.Forms.Label();
            this.txtDetailExchangeRateToBase = new System.Windows.Forms.TextBox();
            this.lblDetailExchangeRateToBase = new System.Windows.Forms.Label();
            this.cmbDetailMethodOfPaymentCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailMethodOfPaymentCode = new System.Windows.Forms.Label();
            this.rgrDetailGiftType = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtGift = new System.Windows.Forms.RadioButton();
            this.rbtGiftInKind = new System.Windows.Forms.RadioButton();
            this.rbtOther = new System.Windows.Forms.RadioButton();
            this.tbrTabPage = new System.Windows.Forms.ToolStrip();
            this.tbbPostBatch = new System.Windows.Forms.ToolStripButton();
            this.tbbExportBatches = new System.Windows.Forms.ToolStripButton();
            this.tbbImportBatches = new System.Windows.Forms.ToolStripButton();
            this.mnuTabPage = new System.Windows.Forms.MenuStrip();
            this.mniBatch = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPost = new System.Windows.Forms.ToolStripMenuItem();

            this.pnlContent.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlLedgerInfo.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.rgrShowBatches.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlDetailGrid.SuspendLayout();
            this.pnlDetailButtons.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.rgrDetailGiftType.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tbrTabPage.SuspendLayout();
            this.mnuTabPage.SuspendLayout();

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
            // pnlLedgerInfo
            //
            this.pnlLedgerInfo.Location = new System.Drawing.Point(2,2);
            this.pnlLedgerInfo.Name = "pnlLedgerInfo";
            this.pnlLedgerInfo.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlLedgerInfo.Controls.Add(this.tableLayoutPanel2);
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
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblLedgerNumber, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLedgerNumber, 1, 0);
            //
            // rgrShowBatches
            //
            this.rgrShowBatches.Location = new System.Drawing.Point(2,2);
            this.rgrShowBatches.Name = "rgrShowBatches";
            this.rgrShowBatches.AutoSize = true;
            this.rgrShowBatches.Tag = "SuppressChangeDetection";
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.rgrShowBatches.Controls.Add(this.tableLayoutPanel3);
            //
            // rbtPosted
            //
            this.rbtPosted.Location = new System.Drawing.Point(2,2);
            this.rbtPosted.Name = "rbtPosted";
            this.rbtPosted.AutoSize = true;
            this.rbtPosted.Tag = "SuppressChangeDetection";
            this.rbtPosted.CheckedChanged += new System.EventHandler(this.ChangeBatchFilter);
            this.rbtPosted.Text = "Posted";
            //
            // rbtEditing
            //
            this.rbtEditing.Location = new System.Drawing.Point(2,2);
            this.rbtEditing.Name = "rbtEditing";
            this.rbtEditing.AutoSize = true;
            this.rbtEditing.Tag = "SuppressChangeDetection";
            this.rbtEditing.CheckedChanged += new System.EventHandler(this.ChangeBatchFilter);
            this.rbtEditing.Text = "Editing";
            this.rbtEditing.Checked = true;
            //
            // rbtAll
            //
            this.rbtAll.Location = new System.Drawing.Point(2,2);
            this.rbtAll.Name = "rbtAll";
            this.rbtAll.AutoSize = true;
            this.rbtAll.Tag = "SuppressChangeDetection";
            this.rbtAll.CheckedChanged += new System.EventHandler(this.ChangeBatchFilter);
            this.rbtAll.Text = "All";
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.rbtPosted, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtEditing, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtAll, 2, 0);
            this.rgrShowBatches.Text = "Show batches";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlLedgerInfo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rgrShowBatches, 0, 1);
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
            this.grdDetails.DoubleClick += new System.EventHandler(this.ShowTransactionTab);
            this.grdDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChanged);
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
            // btnNew
            //
            this.btnNew.Location = new System.Drawing.Point(2,2);
            this.btnNew.Name = "btnNew";
            this.btnNew.AutoSize = true;
            this.btnNew.Click += new System.EventHandler(this.NewRow);
            this.btnNew.Text = "&Add";
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(2,2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.AutoSize = true;
            this.btnDelete.Click += new System.EventHandler(this.CancelRow);
            this.btnDelete.Text = "&Cancel";
            //
            // btnPostBatch
            //
            this.btnPostBatch.Location = new System.Drawing.Point(2,2);
            this.btnPostBatch.Name = "btnPostBatch";
            this.btnPostBatch.AutoSize = true;
            this.btnPostBatch.Click += new System.EventHandler(this.PostBatch);
            this.btnPostBatch.Text = "&Post Batch";
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnNew, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnDelete, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnPostBatch, 0, 2);
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.pnlDetails.Controls.Add(this.tableLayoutPanel5);
            //
            // txtDetailBatchDescription
            //
            this.txtDetailBatchDescription.Location = new System.Drawing.Point(2,2);
            this.txtDetailBatchDescription.Name = "txtDetailBatchDescription";
            this.txtDetailBatchDescription.Size = new System.Drawing.Size(350, 28);
            //
            // lblDetailBatchDescription
            //
            this.lblDetailBatchDescription.Location = new System.Drawing.Point(2,2);
            this.lblDetailBatchDescription.Name = "lblDetailBatchDescription";
            this.lblDetailBatchDescription.AutoSize = true;
            this.lblDetailBatchDescription.Text = "Batch Description:";
            this.lblDetailBatchDescription.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailBatchDescription.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailBatchDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailBankCostCentre
            //
            this.cmbDetailBankCostCentre.Location = new System.Drawing.Point(2,2);
            this.cmbDetailBankCostCentre.Name = "cmbDetailBankCostCentre";
            this.cmbDetailBankCostCentre.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailBankCostCentre.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailBankCostCentre
            //
            this.lblDetailBankCostCentre.Location = new System.Drawing.Point(2,2);
            this.lblDetailBankCostCentre.Name = "lblDetailBankCostCentre";
            this.lblDetailBankCostCentre.AutoSize = true;
            this.lblDetailBankCostCentre.Text = "Cost Centre:";
            this.lblDetailBankCostCentre.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailBankCostCentre.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailBankCostCentre.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailBankAccountCode
            //
            this.cmbDetailBankAccountCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailBankAccountCode.Name = "cmbDetailBankAccountCode";
            this.cmbDetailBankAccountCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailBankAccountCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailBankAccountCode
            //
            this.lblDetailBankAccountCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailBankAccountCode.Name = "lblDetailBankAccountCode";
            this.lblDetailBankAccountCode.AutoSize = true;
            this.lblDetailBankAccountCode.Text = "Bank Account:";
            this.lblDetailBankAccountCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailBankAccountCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailBankAccountCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // dtpDetailGlEffectiveDate
            //
            this.dtpDetailGlEffectiveDate.Location = new System.Drawing.Point(2,2);
            this.dtpDetailGlEffectiveDate.Name = "dtpDetailGlEffectiveDate";
            this.dtpDetailGlEffectiveDate.Size = new System.Drawing.Size(94, 28);
            //
            // lblDetailGlEffectiveDate
            //
            this.lblDetailGlEffectiveDate.Location = new System.Drawing.Point(2,2);
            this.lblDetailGlEffectiveDate.Name = "lblDetailGlEffectiveDate";
            this.lblDetailGlEffectiveDate.AutoSize = true;
            this.lblDetailGlEffectiveDate.Text = "GL Effective Date:";
            this.lblDetailGlEffectiveDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailGlEffectiveDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailGlEffectiveDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // lblValidDateRange
            //
            this.lblValidDateRange.Location = new System.Drawing.Point(2,2);
            this.lblValidDateRange.Name = "lblValidDateRange";
            this.lblValidDateRange.AutoSize = true;
            this.lblValidDateRange.Text = "Valid Date Range:";
            this.lblValidDateRange.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtDetailHashTotal
            //
            this.txtDetailHashTotal.Location = new System.Drawing.Point(2,2);
            this.txtDetailHashTotal.Name = "txtDetailHashTotal";
            this.txtDetailHashTotal.Size = new System.Drawing.Size(150, 28);
            this.txtDetailHashTotal.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtDetailHashTotal.DecimalPlaces = 2;
            this.txtDetailHashTotal.NullValueAllowed = true;
            //
            // lblDetailHashTotal
            //
            this.lblDetailHashTotal.Location = new System.Drawing.Point(2,2);
            this.lblDetailHashTotal.Name = "lblDetailHashTotal";
            this.lblDetailHashTotal.AutoSize = true;
            this.lblDetailHashTotal.Text = "Hash Total:";
            this.lblDetailHashTotal.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailHashTotal.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailHashTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailCurrencyCode
            //
            this.cmbDetailCurrencyCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailCurrencyCode.Name = "cmbDetailCurrencyCode";
            this.cmbDetailCurrencyCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailCurrencyCode.SelectedValueChanged += new System.EventHandler(this.CurrencyChanged);
            this.cmbDetailCurrencyCode.ListTable = TCmbAutoPopulated.TListTableEnum.CurrencyCodeList;
            //
            // lblDetailCurrencyCode
            //
            this.lblDetailCurrencyCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailCurrencyCode.Name = "lblDetailCurrencyCode";
            this.lblDetailCurrencyCode.AutoSize = true;
            this.lblDetailCurrencyCode.Text = "Currency Code:";
            this.lblDetailCurrencyCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCurrencyCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCurrencyCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailExchangeRateToBase
            //
            this.txtDetailExchangeRateToBase.Location = new System.Drawing.Point(2,2);
            this.txtDetailExchangeRateToBase.Name = "txtDetailExchangeRateToBase";
            this.txtDetailExchangeRateToBase.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailExchangeRateToBase
            //
            this.lblDetailExchangeRateToBase.Location = new System.Drawing.Point(2,2);
            this.lblDetailExchangeRateToBase.Name = "lblDetailExchangeRateToBase";
            this.lblDetailExchangeRateToBase.AutoSize = true;
            this.lblDetailExchangeRateToBase.Text = "Exchange Rate To Base:";
            this.lblDetailExchangeRateToBase.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailExchangeRateToBase.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailExchangeRateToBase.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailMethodOfPaymentCode
            //
            this.cmbDetailMethodOfPaymentCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailMethodOfPaymentCode.Name = "cmbDetailMethodOfPaymentCode";
            this.cmbDetailMethodOfPaymentCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailMethodOfPaymentCode.SelectedValueChanged += new System.EventHandler(this.MethodOfPaymentChanged);
            this.cmbDetailMethodOfPaymentCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailMethodOfPaymentCode
            //
            this.lblDetailMethodOfPaymentCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailMethodOfPaymentCode.Name = "lblDetailMethodOfPaymentCode";
            this.lblDetailMethodOfPaymentCode.AutoSize = true;
            this.lblDetailMethodOfPaymentCode.Text = "Method of Payment:";
            this.lblDetailMethodOfPaymentCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailMethodOfPaymentCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailMethodOfPaymentCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // rgrDetailGiftType
            //
            this.rgrDetailGiftType.Location = new System.Drawing.Point(2,2);
            this.rgrDetailGiftType.Name = "rgrDetailGiftType";
            this.rgrDetailGiftType.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            this.rgrDetailGiftType.Controls.Add(this.tableLayoutPanel6);
            //
            // rbtGift
            //
            this.rbtGift.Location = new System.Drawing.Point(2,2);
            this.rbtGift.Name = "rbtGift";
            this.rbtGift.AutoSize = true;
            this.rbtGift.Text = "Gift";
            this.rbtGift.Checked = true;
            //
            // rbtGiftInKind
            //
            this.rbtGiftInKind.Location = new System.Drawing.Point(2,2);
            this.rbtGiftInKind.Name = "rbtGiftInKind";
            this.rbtGiftInKind.AutoSize = true;
            this.rbtGiftInKind.Text = "Gift In Kind";
            //
            // rbtOther
            //
            this.rbtOther.Location = new System.Drawing.Point(2,2);
            this.rbtOther.Name = "rbtOther";
            this.rbtOther.AutoSize = true;
            this.rbtOther.Text = "Other";
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.rbtGift, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.rbtGiftInKind, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.rbtOther, 2, 0);
            this.rgrDetailGiftType.Text = "Gift Type";
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 8;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.lblDetailBatchDescription, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblDetailBankCostCentre, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.lblDetailBankAccountCode, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.lblDetailGlEffectiveDate, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.lblDetailHashTotal, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.lblDetailExchangeRateToBase, 0, 5);
            this.tableLayoutPanel5.Controls.Add(this.lblDetailMethodOfPaymentCode, 0, 6);
            this.tableLayoutPanel5.SetColumnSpan(this.rgrDetailGiftType, 2);
            this.tableLayoutPanel5.Controls.Add(this.rgrDetailGiftType, 0, 7);
            this.tableLayoutPanel5.SetColumnSpan(this.txtDetailBatchDescription, 3);
            this.tableLayoutPanel5.Controls.Add(this.txtDetailBatchDescription, 1, 0);
            this.tableLayoutPanel5.SetColumnSpan(this.cmbDetailBankCostCentre, 2);
            this.tableLayoutPanel5.Controls.Add(this.cmbDetailBankCostCentre, 1, 1);
            this.tableLayoutPanel5.SetColumnSpan(this.cmbDetailBankAccountCode, 2);
            this.tableLayoutPanel5.Controls.Add(this.cmbDetailBankAccountCode, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.dtpDetailGlEffectiveDate, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.txtDetailHashTotal, 1, 4);
            this.tableLayoutPanel5.Controls.Add(this.txtDetailExchangeRateToBase, 1, 5);
            this.tableLayoutPanel5.Controls.Add(this.cmbDetailMethodOfPaymentCode, 1, 6);
            this.tableLayoutPanel5.SetColumnSpan(this.lblValidDateRange, 2);
            this.tableLayoutPanel5.Controls.Add(this.lblValidDateRange, 2, 3);
            this.tableLayoutPanel5.Controls.Add(this.lblDetailCurrencyCode, 2, 4);
            this.tableLayoutPanel5.Controls.Add(this.cmbDetailCurrencyCode, 3, 4);
            //
            // tbbPostBatch
            //
            this.tbbPostBatch.Name = "tbbPostBatch";
            this.tbbPostBatch.AutoSize = true;
            this.tbbPostBatch.Click += new System.EventHandler(this.PostBatch);
            this.tbbPostBatch.Text = "&Post Batch";
            //
            // tbbExportBatches
            //
            this.tbbExportBatches.Name = "tbbExportBatches";
            this.tbbExportBatches.AutoSize = true;
            this.tbbExportBatches.Click += new System.EventHandler(this.ExportBatches);
            this.tbbExportBatches.Text = "&Export Batches";
            //
            // tbbImportBatches
            //
            this.tbbImportBatches.Name = "tbbImportBatches";
            this.tbbImportBatches.AutoSize = true;
            this.tbbImportBatches.Click += new System.EventHandler(this.ImportBatches);
            this.tbbImportBatches.Text = "&Import Batches";
            //
            // tbrTabPage
            //
            this.tbrTabPage.Name = "tbrTabPage";
            this.tbrTabPage.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrTabPage.AutoSize = true;
            this.tbrTabPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbPostBatch,
                        tbbExportBatches,
                        tbbImportBatches});
            //
            // mniPost
            //
            this.mniPost.Name = "mniPost";
            this.mniPost.AutoSize = true;
            this.mniPost.Click += new System.EventHandler(this.PostBatch);
            this.mniPost.Text = "&Post Batch";
            //
            // mniBatch
            //
            this.mniBatch.Name = "mniBatch";
            this.mniBatch.AutoSize = true;
            this.mniBatch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniPost});
            this.mniBatch.Text = "&Batch";
            //
            // mnuTabPage
            //
            this.mnuTabPage.Name = "mnuTabPage";
            this.mnuTabPage.Dock = System.Windows.Forms.DockStyle.Top;
            this.mnuTabPage.AutoSize = true;
            this.mnuTabPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniBatch});

            //
            // TUC_GiftBatches
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrTabPage);
            this.Controls.Add(this.mnuTabPage);

            this.Name = "TUC_GiftBatches";
            this.Text = "";

            this.mnuTabPage.ResumeLayout(false);
            this.tbrTabPage.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.rgrDetailGiftType.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlDetailButtons.ResumeLayout(false);
            this.pnlDetailGrid.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.rgrShowBatches.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlLedgerInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlLedgerInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtLedgerNumber;
        private System.Windows.Forms.Label lblLedgerNumber;
        private System.Windows.Forms.GroupBox rgrShowBatches;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbtPosted;
        private System.Windows.Forms.RadioButton rbtEditing;
        private System.Windows.Forms.RadioButton rbtAll;
        private System.Windows.Forms.Panel pnlDetailGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlDetailButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPostBatch;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TextBox txtDetailBatchDescription;
        private System.Windows.Forms.Label lblDetailBatchDescription;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailBankCostCentre;
        private System.Windows.Forms.Label lblDetailBankCostCentre;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailBankAccountCode;
        private System.Windows.Forms.Label lblDetailBankAccountCode;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDetailGlEffectiveDate;
        private System.Windows.Forms.Label lblDetailGlEffectiveDate;
        private System.Windows.Forms.Label lblValidDateRange;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailHashTotal;
        private System.Windows.Forms.Label lblDetailHashTotal;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailCurrencyCode;
        private System.Windows.Forms.Label lblDetailCurrencyCode;
        private System.Windows.Forms.TextBox txtDetailExchangeRateToBase;
        private System.Windows.Forms.Label lblDetailExchangeRateToBase;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailMethodOfPaymentCode;
        private System.Windows.Forms.Label lblDetailMethodOfPaymentCode;
        private System.Windows.Forms.GroupBox rgrDetailGiftType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton rbtGift;
        private System.Windows.Forms.RadioButton rbtGiftInKind;
        private System.Windows.Forms.RadioButton rbtOther;
        private System.Windows.Forms.ToolStrip tbrTabPage;
        private System.Windows.Forms.ToolStripButton tbbPostBatch;
        private System.Windows.Forms.ToolStripButton tbbExportBatches;
        private System.Windows.Forms.ToolStripButton tbbImportBatches;
        private System.Windows.Forms.MenuStrip mnuTabPage;
        private System.Windows.Forms.ToolStripMenuItem mniBatch;
        private System.Windows.Forms.ToolStripMenuItem mniPost;
    }
}
