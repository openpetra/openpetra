/* auto generated with nant generateWinforms from UC_GiftTransactions.yaml
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
            this.btnNew = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailDonorKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblDetailDonorKey = new System.Windows.Forms.Label();
            this.txtDetailGiftTransactionAmount = new System.Windows.Forms.TextBox();
            this.lblDetailGiftTransactionAmount = new System.Windows.Forms.Label();
            this.cmbDetailMotivationGroupCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailMotivationGroupCode = new System.Windows.Forms.Label();
            this.cmbDetailMotivationDetailCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailMotivationDetailCode = new System.Windows.Forms.Label();
            this.txtDetailRecipientKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblDetailRecipientKey = new System.Windows.Forms.Label();

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
            //
            // lblLedgerNumber
            //
            this.lblLedgerNumber.Location = new System.Drawing.Point(2,2);
            this.lblLedgerNumber.Name = "lblLedgerNumber";
            this.lblLedgerNumber.AutoSize = true;
            this.lblLedgerNumber.Text = "Ledger:";
            this.lblLedgerNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtBatchNumber
            //
            this.txtBatchNumber.Location = new System.Drawing.Point(2,2);
            this.txtBatchNumber.Name = "txtBatchNumber";
            this.txtBatchNumber.Size = new System.Drawing.Size(150, 28);
            this.txtBatchNumber.ReadOnly = true;
            //
            // lblBatchNumber
            //
            this.lblBatchNumber.Location = new System.Drawing.Point(2,2);
            this.lblBatchNumber.Name = "lblBatchNumber";
            this.lblBatchNumber.AutoSize = true;
            this.lblBatchNumber.Text = "Gift Batch:";
            this.lblBatchNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
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
            this.btnRemove.Text = "Remove";
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
            // txtDetailDonorKey
            //
            this.txtDetailDonorKey.Location = new System.Drawing.Point(2,2);
            this.txtDetailDonorKey.Name = "txtDetailDonorKey";
            this.txtDetailDonorKey.Size = new System.Drawing.Size(150, 28);
            this.txtDetailDonorKey.ASpecialSetting = true;
            this.txtDetailDonorKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtDetailDonorKey.ButtonWidth = 40;
            this.txtDetailDonorKey.MaxLength = 32767;
            this.txtDetailDonorKey.ReadOnly = false;
            this.txtDetailDonorKey.TextBoxWidth = 80;
            this.txtDetailDonorKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtDetailDonorKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtDetailDonorKey.PartnerClass = "";
            this.txtDetailDonorKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtDetailDonorKey.ButtonText = "Find";
            //
            // lblDetailDonorKey
            //
            this.lblDetailDonorKey.Location = new System.Drawing.Point(2,2);
            this.lblDetailDonorKey.Name = "lblDetailDonorKey";
            this.lblDetailDonorKey.AutoSize = true;
            this.lblDetailDonorKey.Text = "Donor:";
            this.lblDetailDonorKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtDetailGiftTransactionAmount
            //
            this.txtDetailGiftTransactionAmount.Location = new System.Drawing.Point(2,2);
            this.txtDetailGiftTransactionAmount.Name = "txtDetailGiftTransactionAmount";
            this.txtDetailGiftTransactionAmount.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailGiftTransactionAmount
            //
            this.lblDetailGiftTransactionAmount.Location = new System.Drawing.Point(2,2);
            this.lblDetailGiftTransactionAmount.Name = "lblDetailGiftTransactionAmount";
            this.lblDetailGiftTransactionAmount.AutoSize = true;
            this.lblDetailGiftTransactionAmount.Text = "Amount:";
            this.lblDetailGiftTransactionAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
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
            //
            // cmbDetailMotivationDetailCode
            //
            this.cmbDetailMotivationDetailCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailMotivationDetailCode.Name = "cmbDetailMotivationDetailCode";
            this.cmbDetailMotivationDetailCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailMotivationDetailCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailMotivationDetailCode
            //
            this.lblDetailMotivationDetailCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailMotivationDetailCode.Name = "lblDetailMotivationDetailCode";
            this.lblDetailMotivationDetailCode.AutoSize = true;
            this.lblDetailMotivationDetailCode.Text = "Motivation Detail:";
            this.lblDetailMotivationDetailCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtDetailRecipientKey
            //
            this.txtDetailRecipientKey.Location = new System.Drawing.Point(2,2);
            this.txtDetailRecipientKey.Name = "txtDetailRecipientKey";
            this.txtDetailRecipientKey.Size = new System.Drawing.Size(150, 28);
            this.txtDetailRecipientKey.ASpecialSetting = true;
            this.txtDetailRecipientKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtDetailRecipientKey.ButtonWidth = 40;
            this.txtDetailRecipientKey.MaxLength = 32767;
            this.txtDetailRecipientKey.ReadOnly = false;
            this.txtDetailRecipientKey.TextBoxWidth = 80;
            this.txtDetailRecipientKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtDetailRecipientKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtDetailRecipientKey.PartnerClass = "";
            this.txtDetailRecipientKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtDetailRecipientKey.ButtonText = "Find";
            //
            // lblDetailRecipientKey
            //
            this.lblDetailRecipientKey.Location = new System.Drawing.Point(2,2);
            this.lblDetailRecipientKey.Name = "lblDetailRecipientKey";
            this.lblDetailRecipientKey.AutoSize = true;
            this.lblDetailRecipientKey.Text = "Recipient:";
            this.lblDetailRecipientKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblDetailDonorKey, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailGiftTransactionAmount, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailMotivationGroupCode, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailMotivationDetailCode, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailRecipientKey, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailDonorKey, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailGiftTransactionAmount, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailMotivationGroupCode, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailMotivationDetailCode, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailRecipientKey, 1, 4);

            //
            // TUC_GiftTransactions
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 500);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
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
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtDetailDonorKey;
        private System.Windows.Forms.Label lblDetailDonorKey;
        private System.Windows.Forms.TextBox txtDetailGiftTransactionAmount;
        private System.Windows.Forms.Label lblDetailGiftTransactionAmount;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailMotivationGroupCode;
        private System.Windows.Forms.Label lblDetailMotivationGroupCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailMotivationDetailCode;
        private System.Windows.Forms.Label lblDetailMotivationDetailCode;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtDetailRecipientKey;
        private System.Windows.Forms.Label lblDetailRecipientKey;
    }
}
