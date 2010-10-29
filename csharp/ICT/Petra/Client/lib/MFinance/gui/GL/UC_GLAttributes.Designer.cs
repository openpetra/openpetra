// auto generated with nant generateWinforms from UC_GLAttributes.yaml
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

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    partial class TUC_GLAttributes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_GLAttributes));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLedgerNumber = new System.Windows.Forms.TextBox();
            this.lblLedgerNumber = new System.Windows.Forms.Label();
            this.txtBatchNumber = new System.Windows.Forms.TextBox();
            this.lblBatchNumber = new System.Windows.Forms.Label();
            this.txtJournalNumber = new System.Windows.Forms.TextBox();
            this.lblJournalNumber = new System.Windows.Forms.Label();
            this.txtTransactionNumber = new System.Windows.Forms.TextBox();
            this.lblTransactionNumber = new System.Windows.Forms.Label();
            this.pnlDetailGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtReadonlyAnalysisTypeCode = new System.Windows.Forms.TextBox();
            this.lblReadonlyAnalysisTypeCode = new System.Windows.Forms.Label();
            this.txtReadonlyDescription = new System.Windows.Forms.TextBox();
            this.lblReadonlyDescription = new System.Windows.Forms.Label();
            this.cmbDetailAnalysisAttributeValue = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDetailAnalysisAttributeValue = new System.Windows.Forms.Label();

            this.pnlContent.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlDetailGrid.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();

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
            //
            // txtTransactionNumber
            //
            this.txtTransactionNumber.Location = new System.Drawing.Point(2,2);
            this.txtTransactionNumber.Name = "txtTransactionNumber";
            this.txtTransactionNumber.Size = new System.Drawing.Size(150, 28);
            this.txtTransactionNumber.ReadOnly = true;
            this.txtTransactionNumber.TabStop = false;
            //
            // lblTransactionNumber
            //
            this.lblTransactionNumber.Location = new System.Drawing.Point(2,2);
            this.lblTransactionNumber.Name = "lblTransactionNumber";
            this.lblTransactionNumber.AutoSize = true;
            this.lblTransactionNumber.Text = "Transaction:";
            this.lblTransactionNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblTransactionNumber.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTransactionNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblLedgerNumber, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblJournalNumber, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtLedgerNumber, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtJournalNumber, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblBatchNumber, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTransactionNumber, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtBatchNumber, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtTransactionNumber, 3, 1);
            //
            // pnlDetailGrid
            //
            this.pnlDetailGrid.Name = "pnlDetailGrid";
            this.pnlDetailGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailGrid.AutoSize = true;
            this.pnlDetailGrid.Controls.Add(this.grdDetails);
            //
            // grdDetails
            //
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChanged);
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlDetails.Controls.Add(this.tableLayoutPanel2);
            //
            // txtReadonlyAnalysisTypeCode
            //
            this.txtReadonlyAnalysisTypeCode.Location = new System.Drawing.Point(2,2);
            this.txtReadonlyAnalysisTypeCode.Name = "txtReadonlyAnalysisTypeCode";
            this.txtReadonlyAnalysisTypeCode.Size = new System.Drawing.Size(150, 28);
            this.txtReadonlyAnalysisTypeCode.ReadOnly = true;
            this.txtReadonlyAnalysisTypeCode.TabStop = false;
            //
            // lblReadonlyAnalysisTypeCode
            //
            this.lblReadonlyAnalysisTypeCode.Location = new System.Drawing.Point(2,2);
            this.lblReadonlyAnalysisTypeCode.Name = "lblReadonlyAnalysisTypeCode";
            this.lblReadonlyAnalysisTypeCode.AutoSize = true;
            this.lblReadonlyAnalysisTypeCode.Text = "Type Code:";
            this.lblReadonlyAnalysisTypeCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblReadonlyAnalysisTypeCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblReadonlyAnalysisTypeCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtReadonlyDescription
            //
            this.txtReadonlyDescription.Location = new System.Drawing.Point(2,2);
            this.txtReadonlyDescription.Name = "txtReadonlyDescription";
            this.txtReadonlyDescription.Size = new System.Drawing.Size(400, 28);
            this.txtReadonlyDescription.ReadOnly = true;
            this.txtReadonlyDescription.TabStop = false;
            //
            // lblReadonlyDescription
            //
            this.lblReadonlyDescription.Location = new System.Drawing.Point(2,2);
            this.lblReadonlyDescription.Name = "lblReadonlyDescription";
            this.lblReadonlyDescription.AutoSize = true;
            this.lblReadonlyDescription.Text = "Description:";
            this.lblReadonlyDescription.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblReadonlyDescription.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblReadonlyDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailAnalysisAttributeValue
            //
            this.cmbDetailAnalysisAttributeValue.Location = new System.Drawing.Point(2,2);
            this.cmbDetailAnalysisAttributeValue.Name = "cmbDetailAnalysisAttributeValue";
            this.cmbDetailAnalysisAttributeValue.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailAnalysisAttributeValue
            //
            this.lblDetailAnalysisAttributeValue.Location = new System.Drawing.Point(2,2);
            this.lblDetailAnalysisAttributeValue.Name = "lblDetailAnalysisAttributeValue";
            this.lblDetailAnalysisAttributeValue.AutoSize = true;
            this.lblDetailAnalysisAttributeValue.Text = "Value:";
            this.lblDetailAnalysisAttributeValue.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAnalysisAttributeValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailAnalysisAttributeValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblReadonlyAnalysisTypeCode, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblReadonlyDescription, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailAnalysisAttributeValue, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtReadonlyAnalysisTypeCode, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtReadonlyDescription, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbDetailAnalysisAttributeValue, 1, 2);

            //
            // TUC_GLAttributes
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TUC_GLAttributes";
            this.Text = "";

            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox txtTransactionNumber;
        private System.Windows.Forms.Label lblTransactionNumber;
        private System.Windows.Forms.Panel pnlDetailGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtReadonlyAnalysisTypeCode;
        private System.Windows.Forms.Label lblReadonlyAnalysisTypeCode;
        private System.Windows.Forms.TextBox txtReadonlyDescription;
        private System.Windows.Forms.Label lblReadonlyDescription;
        private Ict.Common.Controls.TCmbAutoComplete cmbDetailAnalysisAttributeValue;
        private System.Windows.Forms.Label lblDetailAnalysisAttributeValue;
    }
}
