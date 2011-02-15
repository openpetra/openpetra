// auto generated with nant generateWinforms from UC_GiftTransactions.yaml
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
    partial class TUC_GiftTransactions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_GiftTransactions));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLedgerNumber = new System.Windows.Forms.TextBox();
            this.lblLedgerNumber = new System.Windows.Forms.Label();
            this.txtBatchNumber = new System.Windows.Forms.TextBox();
            this.lblBatchNumber = new System.Windows.Forms.Label();
            this.pnlDetailGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetailButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNewGift = new System.Windows.Forms.Button();
            this.btnNewDetail = new System.Windows.Forms.Button();
            this.btnDeleteDetail = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailDonorKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblDetailDonorKey = new System.Windows.Forms.Label();
            this.cmbDetailMethodOfGivingCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailMethodOfGivingCode = new System.Windows.Forms.Label();
            this.cmbDetailMethodOfPaymentCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailMethodOfPaymentCode = new System.Windows.Forms.Label();
            this.txtDetailReference = new System.Windows.Forms.TextBox();
            this.lblDetailReference = new System.Windows.Forms.Label();
            this.cmbDetailReceiptLetterCode = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDetailReceiptLetterCode = new System.Windows.Forms.Label();
            this.dtpDateEntered = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDateEntered = new System.Windows.Forms.Label();
            this.txtGiftTotal = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblGiftTotal = new System.Windows.Forms.Label();
            this.txtDetailGiftTransactionAmount = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailGiftTransactionAmount = new System.Windows.Forms.Label();
            this.chkDetailConfidentialGiftFlag = new System.Windows.Forms.CheckBox();
            this.lblDetailConfidentialGiftFlag = new System.Windows.Forms.Label();
            this.txtDetailRecipientKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblDetailRecipientKey = new System.Windows.Forms.Label();
            this.txtField = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblField = new System.Windows.Forms.Label();
            this.chkDetailChargeFlag = new System.Windows.Forms.CheckBox();
            this.lblDetailChargeFlag = new System.Windows.Forms.Label();
            this.cmbMinistry = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblMinistry = new System.Windows.Forms.Label();
            this.cmbDetailMailingCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailMailingCode = new System.Windows.Forms.Label();
            this.chkDetailTaxDeductable = new System.Windows.Forms.CheckBox();
            this.lblDetailTaxDeductable = new System.Windows.Forms.Label();
            this.cmbDetailMotivationGroupCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailMotivationGroupCode = new System.Windows.Forms.Label();
            this.cmbDetailMotivationDetailCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailMotivationDetailCode = new System.Windows.Forms.Label();
            this.txtDetailCostCentreCode = new System.Windows.Forms.TextBox();
            this.lblDetailCostCentreCode = new System.Windows.Forms.Label();
            this.txtDetailAccountCode = new System.Windows.Forms.TextBox();
            this.lblDetailAccountCode = new System.Windows.Forms.Label();
            this.txtDetailGiftCommentOne = new System.Windows.Forms.TextBox();
            this.lblDetailGiftCommentOne = new System.Windows.Forms.Label();
            this.cmbDetailCommentOneType = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDetailCommentOneType = new System.Windows.Forms.Label();
            this.txtDetailGiftCommentTwo = new System.Windows.Forms.TextBox();
            this.lblDetailGiftCommentTwo = new System.Windows.Forms.Label();
            this.cmbDetailCommentTwoType = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDetailCommentTwoType = new System.Windows.Forms.Label();
            this.txtDetailGiftCommentThree = new System.Windows.Forms.TextBox();
            this.lblDetailGiftCommentThree = new System.Windows.Forms.Label();
            this.cmbDetailCommentThreeType = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDetailCommentThreeType = new System.Windows.Forms.Label();

            this.pnlContent.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlDetailGrid.SuspendLayout();
            this.pnlDetailButtons.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Size = new System.Drawing.Size(1000, 700);
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
            this.lblBatchNumber.Text = "Gift Batch:";
            this.lblBatchNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblBatchNumber.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblBatchNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel1.ColumnCount = 4;
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
            // btnNewGift
            //
            this.btnNewGift.Location = new System.Drawing.Point(2,2);
            this.btnNewGift.Name = "btnNewGift";
            this.btnNewGift.AutoSize = true;
            this.btnNewGift.Click += new System.EventHandler(this.NewGift);
            this.btnNewGift.Text = "&Add Gift";
            //
            // btnNewDetail
            //
            this.btnNewDetail.Location = new System.Drawing.Point(2,2);
            this.btnNewDetail.Name = "btnNewDetail";
            this.btnNewDetail.AutoSize = true;
            this.btnNewDetail.Click += new System.EventHandler(this.NewGiftDetail);
            this.btnNewDetail.Text = "Add Detai&l";
            //
            // btnDeleteDetail
            //
            this.btnDeleteDetail.Location = new System.Drawing.Point(2,2);
            this.btnDeleteDetail.Name = "btnDeleteDetail";
            this.btnDeleteDetail.AutoSize = true;
            this.btnDeleteDetail.Click += new System.EventHandler(this.DeleteDetail);
            this.btnDeleteDetail.Text = "&Delete Detail";
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnNewGift, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnNewDetail, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnDeleteDetail, 0, 2);
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
            // txtDetailDonorKey
            //
            this.txtDetailDonorKey.Location = new System.Drawing.Point(2,2);
            this.txtDetailDonorKey.Name = "txtDetailDonorKey";
            this.txtDetailDonorKey.Size = new System.Drawing.Size(370, 28);
            this.txtDetailDonorKey.ValueChanged += new TDelegatePartnerChanged(this.DonorKeyChanged);
            this.txtDetailDonorKey.ASpecialSetting = true;
            this.txtDetailDonorKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtDetailDonorKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtDetailDonorKey.PartnerClass = "";
            this.txtDetailDonorKey.MaxLength = 32767;
            this.txtDetailDonorKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtDetailDonorKey.TextBoxWidth = 80;
            this.txtDetailDonorKey.ButtonWidth = 40;
            this.txtDetailDonorKey.ReadOnly = false;
            this.txtDetailDonorKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtDetailDonorKey.ButtonText = "Find";
            //
            // lblDetailDonorKey
            //
            this.lblDetailDonorKey.Location = new System.Drawing.Point(2,2);
            this.lblDetailDonorKey.Name = "lblDetailDonorKey";
            this.lblDetailDonorKey.AutoSize = true;
            this.lblDetailDonorKey.Text = "Donor:";
            this.lblDetailDonorKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailDonorKey.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailDonorKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailMethodOfGivingCode
            //
            this.cmbDetailMethodOfGivingCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailMethodOfGivingCode.Name = "cmbDetailMethodOfGivingCode";
            this.cmbDetailMethodOfGivingCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailMethodOfGivingCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailMethodOfGivingCode
            //
            this.lblDetailMethodOfGivingCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailMethodOfGivingCode.Name = "lblDetailMethodOfGivingCode";
            this.lblDetailMethodOfGivingCode.AutoSize = true;
            this.lblDetailMethodOfGivingCode.Text = "Method of Giving:";
            this.lblDetailMethodOfGivingCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailMethodOfGivingCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailMethodOfGivingCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailMethodOfPaymentCode
            //
            this.cmbDetailMethodOfPaymentCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailMethodOfPaymentCode.Name = "cmbDetailMethodOfPaymentCode";
            this.cmbDetailMethodOfPaymentCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailMethodOfPaymentCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailMethodOfPaymentCode
            //
            this.lblDetailMethodOfPaymentCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailMethodOfPaymentCode.Name = "lblDetailMethodOfPaymentCode";
            this.lblDetailMethodOfPaymentCode.AutoSize = true;
            this.lblDetailMethodOfPaymentCode.Text = "Method of payment:";
            this.lblDetailMethodOfPaymentCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailMethodOfPaymentCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailMethodOfPaymentCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            // cmbDetailReceiptLetterCode
            //
            this.cmbDetailReceiptLetterCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailReceiptLetterCode.Name = "cmbDetailReceiptLetterCode";
            this.cmbDetailReceiptLetterCode.Size = new System.Drawing.Size(150, 28);
            this.cmbDetailReceiptLetterCode.Items.AddRange(new object[] {"NO*RECET"});
            //
            // lblDetailReceiptLetterCode
            //
            this.lblDetailReceiptLetterCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailReceiptLetterCode.Name = "lblDetailReceiptLetterCode";
            this.lblDetailReceiptLetterCode.AutoSize = true;
            this.lblDetailReceiptLetterCode.Text = "Letter Code:";
            this.lblDetailReceiptLetterCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailReceiptLetterCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailReceiptLetterCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // dtpDateEntered
            //
            this.dtpDateEntered.Location = new System.Drawing.Point(2,2);
            this.dtpDateEntered.Name = "dtpDateEntered";
            this.dtpDateEntered.Size = new System.Drawing.Size(94, 28);
            //
            // lblDateEntered
            //
            this.lblDateEntered.Location = new System.Drawing.Point(2,2);
            this.lblDateEntered.Name = "lblDateEntered";
            this.lblDateEntered.AutoSize = true;
            this.lblDateEntered.Text = "Gift Date:";
            this.lblDateEntered.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateEntered.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDateEntered.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtGiftTotal
            //
            this.txtGiftTotal.Location = new System.Drawing.Point(2,2);
            this.txtGiftTotal.Name = "txtGiftTotal";
            this.txtGiftTotal.Size = new System.Drawing.Size(150, 28);
            this.txtGiftTotal.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtGiftTotal.DecimalPlaces = 2;
            this.txtGiftTotal.NullValueAllowed = true;
            //
            // lblGiftTotal
            //
            this.lblGiftTotal.Location = new System.Drawing.Point(2,2);
            this.lblGiftTotal.Name = "lblGiftTotal";
            this.lblGiftTotal.AutoSize = true;
            this.lblGiftTotal.Text = "Total:";
            this.lblGiftTotal.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGiftTotal.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGiftTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailGiftTransactionAmount
            //
            this.txtDetailGiftTransactionAmount.Location = new System.Drawing.Point(2,2);
            this.txtDetailGiftTransactionAmount.Name = "txtDetailGiftTransactionAmount";
            this.txtDetailGiftTransactionAmount.Size = new System.Drawing.Size(150, 28);
            this.txtDetailGiftTransactionAmount.TextChanged += new System.EventHandler(this.GiftDetailAmountChanged);
            this.txtDetailGiftTransactionAmount.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            this.txtDetailGiftTransactionAmount.DecimalPlaces = 2;
            this.txtDetailGiftTransactionAmount.NullValueAllowed = true;
            //
            // lblDetailGiftTransactionAmount
            //
            this.lblDetailGiftTransactionAmount.Location = new System.Drawing.Point(2,2);
            this.lblDetailGiftTransactionAmount.Name = "lblDetailGiftTransactionAmount";
            this.lblDetailGiftTransactionAmount.AutoSize = true;
            this.lblDetailGiftTransactionAmount.Text = "Amount:";
            this.lblDetailGiftTransactionAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailGiftTransactionAmount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailGiftTransactionAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkDetailConfidentialGiftFlag
            //
            this.chkDetailConfidentialGiftFlag.Location = new System.Drawing.Point(2,2);
            this.chkDetailConfidentialGiftFlag.Name = "chkDetailConfidentialGiftFlag";
            this.chkDetailConfidentialGiftFlag.Size = new System.Drawing.Size(30, 28);
            this.chkDetailConfidentialGiftFlag.Text = "";
            this.chkDetailConfidentialGiftFlag.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblDetailConfidentialGiftFlag
            //
            this.lblDetailConfidentialGiftFlag.Location = new System.Drawing.Point(2,2);
            this.lblDetailConfidentialGiftFlag.Name = "lblDetailConfidentialGiftFlag";
            this.lblDetailConfidentialGiftFlag.AutoSize = true;
            this.lblDetailConfidentialGiftFlag.Text = "Confidential?:";
            this.lblDetailConfidentialGiftFlag.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailConfidentialGiftFlag.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailConfidentialGiftFlag.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailRecipientKey
            //
            this.txtDetailRecipientKey.Location = new System.Drawing.Point(2,2);
            this.txtDetailRecipientKey.Name = "txtDetailRecipientKey";
            this.txtDetailRecipientKey.Size = new System.Drawing.Size(370, 28);
            this.txtDetailRecipientKey.ValueChanged += new TDelegatePartnerChanged(this.RecipientKeyChanged);
            this.txtDetailRecipientKey.ASpecialSetting = true;
            this.txtDetailRecipientKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtDetailRecipientKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtDetailRecipientKey.PartnerClass = "";
            this.txtDetailRecipientKey.MaxLength = 32767;
            this.txtDetailRecipientKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtDetailRecipientKey.TextBoxWidth = 80;
            this.txtDetailRecipientKey.ButtonWidth = 40;
            this.txtDetailRecipientKey.ReadOnly = false;
            this.txtDetailRecipientKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtDetailRecipientKey.ButtonText = "Find";
            //
            // lblDetailRecipientKey
            //
            this.lblDetailRecipientKey.Location = new System.Drawing.Point(2,2);
            this.lblDetailRecipientKey.Name = "lblDetailRecipientKey";
            this.lblDetailRecipientKey.AutoSize = true;
            this.lblDetailRecipientKey.Text = "Recipient:";
            this.lblDetailRecipientKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailRecipientKey.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailRecipientKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtField
            //
            this.txtField.Location = new System.Drawing.Point(2,2);
            this.txtField.Name = "txtField";
            this.txtField.Size = new System.Drawing.Size(370, 28);
            this.txtField.ReadOnly = true;
            this.txtField.TabStop = false;
            this.txtField.ASpecialSetting = true;
            this.txtField.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtField.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtField.PartnerClass = "";
            this.txtField.MaxLength = 32767;
            this.txtField.Tag = "CustomDisableAlthoughInvisible";
            this.txtField.TextBoxWidth = 80;
            this.txtField.ButtonWidth = 0;
            this.txtField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtField.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            //
            // lblField
            //
            this.lblField.Location = new System.Drawing.Point(2,2);
            this.lblField.Name = "lblField";
            this.lblField.AutoSize = true;
            this.lblField.Text = "Field:";
            this.lblField.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblField.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblField.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkDetailChargeFlag
            //
            this.chkDetailChargeFlag.Location = new System.Drawing.Point(2,2);
            this.chkDetailChargeFlag.Name = "chkDetailChargeFlag";
            this.chkDetailChargeFlag.Size = new System.Drawing.Size(30, 28);
            this.chkDetailChargeFlag.Text = "";
            this.chkDetailChargeFlag.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblDetailChargeFlag
            //
            this.lblDetailChargeFlag.Location = new System.Drawing.Point(2,2);
            this.lblDetailChargeFlag.Name = "lblDetailChargeFlag";
            this.lblDetailChargeFlag.AutoSize = true;
            this.lblDetailChargeFlag.Text = "Admin Grants?:";
            this.lblDetailChargeFlag.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailChargeFlag.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailChargeFlag.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbMinistry
            //
            this.cmbMinistry.Location = new System.Drawing.Point(2,2);
            this.cmbMinistry.Name = "cmbMinistry";
            this.cmbMinistry.Size = new System.Drawing.Size(250, 28);
            this.cmbMinistry.SelectedValueChanged += new System.EventHandler(this.KeyMinistryChanged);
            this.cmbMinistry.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblMinistry
            //
            this.lblMinistry.Location = new System.Drawing.Point(2,2);
            this.lblMinistry.Name = "lblMinistry";
            this.lblMinistry.AutoSize = true;
            this.lblMinistry.Text = "Key Ministry:";
            this.lblMinistry.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblMinistry.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMinistry.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailMailingCode
            //
            this.cmbDetailMailingCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailMailingCode.Name = "cmbDetailMailingCode";
            this.cmbDetailMailingCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailMailingCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailMailingCode
            //
            this.lblDetailMailingCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailMailingCode.Name = "lblDetailMailingCode";
            this.lblDetailMailingCode.AutoSize = true;
            this.lblDetailMailingCode.Text = "Mailing:";
            this.lblDetailMailingCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailMailingCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailMailingCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkDetailTaxDeductable
            //
            this.chkDetailTaxDeductable.Location = new System.Drawing.Point(2,2);
            this.chkDetailTaxDeductable.Name = "chkDetailTaxDeductable";
            this.chkDetailTaxDeductable.Size = new System.Drawing.Size(30, 28);
            this.chkDetailTaxDeductable.Text = "";
            this.chkDetailTaxDeductable.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblDetailTaxDeductable
            //
            this.lblDetailTaxDeductable.Location = new System.Drawing.Point(2,2);
            this.lblDetailTaxDeductable.Name = "lblDetailTaxDeductable";
            this.lblDetailTaxDeductable.AutoSize = true;
            this.lblDetailTaxDeductable.Text = "Tax deductable?:";
            this.lblDetailTaxDeductable.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailTaxDeductable.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailTaxDeductable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailMotivationGroupCode
            //
            this.cmbDetailMotivationGroupCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailMotivationGroupCode.Name = "cmbDetailMotivationGroupCode";
            this.cmbDetailMotivationGroupCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailMotivationGroupCode.SelectedValueChanged += new System.EventHandler(this.FilterMotivationDetail);
            this.cmbDetailMotivationGroupCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailMotivationGroupCode
            //
            this.lblDetailMotivationGroupCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailMotivationGroupCode.Name = "lblDetailMotivationGroupCode";
            this.lblDetailMotivationGroupCode.AutoSize = true;
            this.lblDetailMotivationGroupCode.Text = "Motivation Group:";
            this.lblDetailMotivationGroupCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailMotivationGroupCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailMotivationGroupCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailMotivationDetailCode
            //
            this.cmbDetailMotivationDetailCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailMotivationDetailCode.Name = "cmbDetailMotivationDetailCode";
            this.cmbDetailMotivationDetailCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailMotivationDetailCode.SelectedValueChanged += new System.EventHandler(this.MotivationDetailChanged);
            this.cmbDetailMotivationDetailCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailMotivationDetailCode
            //
            this.lblDetailMotivationDetailCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailMotivationDetailCode.Name = "lblDetailMotivationDetailCode";
            this.lblDetailMotivationDetailCode.AutoSize = true;
            this.lblDetailMotivationDetailCode.Text = "Motivation Detail:";
            this.lblDetailMotivationDetailCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailMotivationDetailCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailMotivationDetailCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailCostCentreCode
            //
            this.txtDetailCostCentreCode.Location = new System.Drawing.Point(2,2);
            this.txtDetailCostCentreCode.Name = "txtDetailCostCentreCode";
            this.txtDetailCostCentreCode.Size = new System.Drawing.Size(150, 28);
            this.txtDetailCostCentreCode.ReadOnly = true;
            this.txtDetailCostCentreCode.TabStop = false;
            //
            // lblDetailCostCentreCode
            //
            this.lblDetailCostCentreCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailCostCentreCode.Name = "lblDetailCostCentreCode";
            this.lblDetailCostCentreCode.AutoSize = true;
            this.lblDetailCostCentreCode.Text = "Cost Centre:";
            this.lblDetailCostCentreCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCostCentreCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCostCentreCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailAccountCode
            //
            this.txtDetailAccountCode.Location = new System.Drawing.Point(2,2);
            this.txtDetailAccountCode.Name = "txtDetailAccountCode";
            this.txtDetailAccountCode.Size = new System.Drawing.Size(150, 28);
            this.txtDetailAccountCode.ReadOnly = true;
            this.txtDetailAccountCode.TabStop = false;
            //
            // lblDetailAccountCode
            //
            this.lblDetailAccountCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailAccountCode.Name = "lblDetailAccountCode";
            this.lblDetailAccountCode.AutoSize = true;
            this.lblDetailAccountCode.Text = "Account:";
            this.lblDetailAccountCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAccountCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailAccountCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailGiftCommentOne
            //
            this.txtDetailGiftCommentOne.Location = new System.Drawing.Point(2,2);
            this.txtDetailGiftCommentOne.Name = "txtDetailGiftCommentOne";
            this.txtDetailGiftCommentOne.Size = new System.Drawing.Size(250, 28);
            //
            // lblDetailGiftCommentOne
            //
            this.lblDetailGiftCommentOne.Location = new System.Drawing.Point(2,2);
            this.lblDetailGiftCommentOne.Name = "lblDetailGiftCommentOne";
            this.lblDetailGiftCommentOne.AutoSize = true;
            this.lblDetailGiftCommentOne.Text = "Comment 1:";
            this.lblDetailGiftCommentOne.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailGiftCommentOne.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailGiftCommentOne.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailCommentOneType
            //
            this.cmbDetailCommentOneType.Location = new System.Drawing.Point(2,2);
            this.cmbDetailCommentOneType.Name = "cmbDetailCommentOneType";
            this.cmbDetailCommentOneType.Size = new System.Drawing.Size(150, 28);
            this.cmbDetailCommentOneType.Items.AddRange(new object[] {"Donor","Recipient","Both","Office"});
            //
            // lblDetailCommentOneType
            //
            this.lblDetailCommentOneType.Location = new System.Drawing.Point(2,2);
            this.lblDetailCommentOneType.Name = "lblDetailCommentOneType";
            this.lblDetailCommentOneType.AutoSize = true;
            this.lblDetailCommentOneType.Text = "for:";
            this.lblDetailCommentOneType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCommentOneType.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCommentOneType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailGiftCommentTwo
            //
            this.txtDetailGiftCommentTwo.Location = new System.Drawing.Point(2,2);
            this.txtDetailGiftCommentTwo.Name = "txtDetailGiftCommentTwo";
            this.txtDetailGiftCommentTwo.Size = new System.Drawing.Size(250, 28);
            //
            // lblDetailGiftCommentTwo
            //
            this.lblDetailGiftCommentTwo.Location = new System.Drawing.Point(2,2);
            this.lblDetailGiftCommentTwo.Name = "lblDetailGiftCommentTwo";
            this.lblDetailGiftCommentTwo.AutoSize = true;
            this.lblDetailGiftCommentTwo.Text = "Comment 2:";
            this.lblDetailGiftCommentTwo.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailGiftCommentTwo.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailGiftCommentTwo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailCommentTwoType
            //
            this.cmbDetailCommentTwoType.Location = new System.Drawing.Point(2,2);
            this.cmbDetailCommentTwoType.Name = "cmbDetailCommentTwoType";
            this.cmbDetailCommentTwoType.Size = new System.Drawing.Size(150, 28);
            this.cmbDetailCommentTwoType.Items.AddRange(new object[] {"Donor","Recipient","Both","Office"});
            //
            // lblDetailCommentTwoType
            //
            this.lblDetailCommentTwoType.Location = new System.Drawing.Point(2,2);
            this.lblDetailCommentTwoType.Name = "lblDetailCommentTwoType";
            this.lblDetailCommentTwoType.AutoSize = true;
            this.lblDetailCommentTwoType.Text = "for:";
            this.lblDetailCommentTwoType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCommentTwoType.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCommentTwoType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailGiftCommentThree
            //
            this.txtDetailGiftCommentThree.Location = new System.Drawing.Point(2,2);
            this.txtDetailGiftCommentThree.Name = "txtDetailGiftCommentThree";
            this.txtDetailGiftCommentThree.Size = new System.Drawing.Size(250, 28);
            //
            // lblDetailGiftCommentThree
            //
            this.lblDetailGiftCommentThree.Location = new System.Drawing.Point(2,2);
            this.lblDetailGiftCommentThree.Name = "lblDetailGiftCommentThree";
            this.lblDetailGiftCommentThree.AutoSize = true;
            this.lblDetailGiftCommentThree.Text = "Comment 3:";
            this.lblDetailGiftCommentThree.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailGiftCommentThree.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailGiftCommentThree.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailCommentThreeType
            //
            this.cmbDetailCommentThreeType.Location = new System.Drawing.Point(2,2);
            this.cmbDetailCommentThreeType.Name = "cmbDetailCommentThreeType";
            this.cmbDetailCommentThreeType.Size = new System.Drawing.Size(150, 28);
            this.cmbDetailCommentThreeType.Items.AddRange(new object[] {"Donor","Recipient","Both","Office"});
            //
            // lblDetailCommentThreeType
            //
            this.lblDetailCommentThreeType.Location = new System.Drawing.Point(2,2);
            this.lblDetailCommentThreeType.Name = "lblDetailCommentThreeType";
            this.lblDetailCommentThreeType.AutoSize = true;
            this.lblDetailCommentThreeType.Text = "for:";
            this.lblDetailCommentThreeType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCommentThreeType.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCommentThreeType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 14;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblDetailDonorKey, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailMethodOfGivingCode, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailReference, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblDateEntered, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailGiftTransactionAmount, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailRecipientKey, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.lblField, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.lblMinistry, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailMailingCode, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailMotivationGroupCode, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailCostCentreCode, 0, 10);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailGiftCommentOne, 0, 11);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailGiftCommentTwo, 0, 12);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailGiftCommentThree, 0, 13);
            this.tableLayoutPanel3.SetColumnSpan(this.txtDetailDonorKey, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailDonorKey, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailMethodOfGivingCode, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailReference, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.dtpDateEntered, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailGiftTransactionAmount, 1, 4);
            this.tableLayoutPanel3.SetColumnSpan(this.txtDetailRecipientKey, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailRecipientKey, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.txtField, 1, 6);
            this.tableLayoutPanel3.SetColumnSpan(this.cmbMinistry, 2);
            this.tableLayoutPanel3.Controls.Add(this.cmbMinistry, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailMailingCode, 1, 8);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailMotivationGroupCode, 1, 9);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailCostCentreCode, 1, 10);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailGiftCommentOne, 1, 11);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailGiftCommentTwo, 1, 12);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailGiftCommentThree, 1, 13);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailMethodOfPaymentCode, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailReceiptLetterCode, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblGiftTotal, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailConfidentialGiftFlag, 2, 4);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailChargeFlag, 2, 6);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailTaxDeductable, 2, 8);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailMotivationDetailCode, 2, 9);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailAccountCode, 2, 10);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailCommentOneType, 2, 11);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailCommentTwoType, 2, 12);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailCommentThreeType, 2, 13);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailMethodOfPaymentCode, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailReceiptLetterCode, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtGiftTotal, 3, 3);
            this.tableLayoutPanel3.Controls.Add(this.chkDetailConfidentialGiftFlag, 3, 4);
            this.tableLayoutPanel3.Controls.Add(this.chkDetailChargeFlag, 3, 6);
            this.tableLayoutPanel3.Controls.Add(this.chkDetailTaxDeductable, 3, 8);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailMotivationDetailCode, 3, 9);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailAccountCode, 3, 10);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailCommentOneType, 3, 11);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailCommentTwoType, 3, 12);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailCommentThreeType, 3, 13);

            //
            // TUC_GiftTransactions
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TUC_GiftTransactions";
            this.Text = "";

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
        private System.Windows.Forms.Panel pnlDetailGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlDetailButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnNewGift;
        private System.Windows.Forms.Button btnNewDetail;
        private System.Windows.Forms.Button btnDeleteDetail;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtDetailDonorKey;
        private System.Windows.Forms.Label lblDetailDonorKey;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailMethodOfGivingCode;
        private System.Windows.Forms.Label lblDetailMethodOfGivingCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailMethodOfPaymentCode;
        private System.Windows.Forms.Label lblDetailMethodOfPaymentCode;
        private System.Windows.Forms.TextBox txtDetailReference;
        private System.Windows.Forms.Label lblDetailReference;
        private Ict.Common.Controls.TCmbAutoComplete cmbDetailReceiptLetterCode;
        private System.Windows.Forms.Label lblDetailReceiptLetterCode;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateEntered;
        private System.Windows.Forms.Label lblDateEntered;
        private Ict.Common.Controls.TTxtNumericTextBox txtGiftTotal;
        private System.Windows.Forms.Label lblGiftTotal;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailGiftTransactionAmount;
        private System.Windows.Forms.Label lblDetailGiftTransactionAmount;
        private System.Windows.Forms.CheckBox chkDetailConfidentialGiftFlag;
        private System.Windows.Forms.Label lblDetailConfidentialGiftFlag;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtDetailRecipientKey;
        private System.Windows.Forms.Label lblDetailRecipientKey;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtField;
        private System.Windows.Forms.Label lblField;
        private System.Windows.Forms.CheckBox chkDetailChargeFlag;
        private System.Windows.Forms.Label lblDetailChargeFlag;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbMinistry;
        private System.Windows.Forms.Label lblMinistry;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailMailingCode;
        private System.Windows.Forms.Label lblDetailMailingCode;
        private System.Windows.Forms.CheckBox chkDetailTaxDeductable;
        private System.Windows.Forms.Label lblDetailTaxDeductable;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailMotivationGroupCode;
        private System.Windows.Forms.Label lblDetailMotivationGroupCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailMotivationDetailCode;
        private System.Windows.Forms.Label lblDetailMotivationDetailCode;
        private System.Windows.Forms.TextBox txtDetailCostCentreCode;
        private System.Windows.Forms.Label lblDetailCostCentreCode;
        private System.Windows.Forms.TextBox txtDetailAccountCode;
        private System.Windows.Forms.Label lblDetailAccountCode;
        private System.Windows.Forms.TextBox txtDetailGiftCommentOne;
        private System.Windows.Forms.Label lblDetailGiftCommentOne;
        private Ict.Common.Controls.TCmbAutoComplete cmbDetailCommentOneType;
        private System.Windows.Forms.Label lblDetailCommentOneType;
        private System.Windows.Forms.TextBox txtDetailGiftCommentTwo;
        private System.Windows.Forms.Label lblDetailGiftCommentTwo;
        private Ict.Common.Controls.TCmbAutoComplete cmbDetailCommentTwoType;
        private System.Windows.Forms.Label lblDetailCommentTwoType;
        private System.Windows.Forms.TextBox txtDetailGiftCommentThree;
        private System.Windows.Forms.Label lblDetailGiftCommentThree;
        private Ict.Common.Controls.TCmbAutoComplete cmbDetailCommentThreeType;
        private System.Windows.Forms.Label lblDetailCommentThreeType;
    }
}
