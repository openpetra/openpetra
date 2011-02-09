// auto generated with nant generateWinforms from GiftBatchExport.yaml
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
    partial class TFrmGiftBatchExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmGiftBatchExport));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrDetailSummary = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtDetail = new System.Windows.Forms.RadioButton();
            this.rbtSummary = new System.Windows.Forms.RadioButton();
            this.dtpDateSummary = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.rgrCurrency = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtBaseCurrency = new System.Windows.Forms.RadioButton();
            this.rbtOriginalTransactionCurrency = new System.Windows.Forms.RadioButton();
            this.pnlRecipient = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailRecipientKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblDetailRecipientKey = new System.Windows.Forms.Label();
            this.txtDetailFieldKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblDetailFieldKey = new System.Windows.Forms.Label();
            this.rgrDateOrBatchRange = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtDateRange = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpDateFrom = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.dtpDateTo = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.rbtBatchNumberSelection = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.txtBatchNumberStart = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblBatchNumberStart = new System.Windows.Forms.Label();
            this.txtBatchNumberEnd = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblBatchNumberEnd = new System.Windows.Forms.Label();
            this.chkIncludeUnposted = new System.Windows.Forms.CheckBox();
            this.lblIncludeUnposted = new System.Windows.Forms.Label();
            this.chkTransactionsOnly = new System.Windows.Forms.CheckBox();
            this.lblTransactionsOnly = new System.Windows.Forms.Label();
            this.chkExtraColumns = new System.Windows.Forms.CheckBox();
            this.lblExtraColumns = new System.Windows.Forms.Label();
            this.pnlFilename = new System.Windows.Forms.Panel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.lblFilename = new System.Windows.Forms.Label();
            this.btnBrowseFilename = new System.Windows.Forms.Button();
            this.cmbDelimiter = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDelimiter = new System.Windows.Forms.Label();
            this.cmbDateFormat = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDateFormat = new System.Windows.Forms.Label();
            this.cmbNumberFormat = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblNumberFormat = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbExportBatches = new System.Windows.Forms.ToolStripButton();
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
            this.rgrDetailSummary.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.rgrCurrency.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlRecipient.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.rgrDateOrBatchRange.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.pnlFilename.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
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
            // rgrDetailSummary
            //
            this.rgrDetailSummary.Location = new System.Drawing.Point(2,2);
            this.rgrDetailSummary.Name = "rgrDetailSummary";
            this.rgrDetailSummary.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.rgrDetailSummary.Controls.Add(this.tableLayoutPanel2);
            //
            // rbtDetail
            //
            this.rbtDetail.Location = new System.Drawing.Point(2,2);
            this.rbtDetail.Name = "rbtDetail";
            this.rbtDetail.AutoSize = true;
            this.rbtDetail.Text = "Detail";
            this.rbtDetail.Checked = true;
            //
            // rbtSummary
            //
            this.rbtSummary.Location = new System.Drawing.Point(2,2);
            this.rbtSummary.Name = "rbtSummary";
            this.rbtSummary.AutoSize = true;
            this.rbtSummary.Text = "Date for summary";
            //
            // dtpDateSummary
            //
            this.dtpDateSummary.Location = new System.Drawing.Point(2,2);
            this.dtpDateSummary.Name = "dtpDateSummary";
            this.dtpDateSummary.Size = new System.Drawing.Size(94, 28);
            this.rbtSummary.CheckedChanged += new System.EventHandler(this.rbtSummaryCheckedChanged);
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.SetColumnSpan(this.rbtDetail, 2);
            this.tableLayoutPanel2.Controls.Add(this.rbtDetail, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtSummary, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.dtpDateSummary, 1, 1);
            this.rgrDetailSummary.Text = "Detail or Summary";
            //
            // rgrCurrency
            //
            this.rgrCurrency.Location = new System.Drawing.Point(2,2);
            this.rgrCurrency.Name = "rgrCurrency";
            this.rgrCurrency.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.rgrCurrency.Controls.Add(this.tableLayoutPanel3);
            //
            // rbtBaseCurrency
            //
            this.rbtBaseCurrency.Location = new System.Drawing.Point(2,2);
            this.rbtBaseCurrency.Name = "rbtBaseCurrency";
            this.rbtBaseCurrency.AutoSize = true;
            this.rbtBaseCurrency.Text = "Base Currency";
            this.rbtBaseCurrency.Checked = true;
            //
            // rbtOriginalTransactionCurrency
            //
            this.rbtOriginalTransactionCurrency.Location = new System.Drawing.Point(2,2);
            this.rbtOriginalTransactionCurrency.Name = "rbtOriginalTransactionCurrency";
            this.rbtOriginalTransactionCurrency.AutoSize = true;
            this.rbtOriginalTransactionCurrency.Text = "Original Transaction Currency";
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.rbtBaseCurrency, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtOriginalTransactionCurrency, 0, 1);
            this.rgrCurrency.Text = "Currency";
            //
            // pnlRecipient
            //
            this.pnlRecipient.Location = new System.Drawing.Point(2,2);
            this.pnlRecipient.Name = "pnlRecipient";
            this.pnlRecipient.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.pnlRecipient.Controls.Add(this.tableLayoutPanel4);
            //
            // txtDetailRecipientKey
            //
            this.txtDetailRecipientKey.Location = new System.Drawing.Point(2,2);
            this.txtDetailRecipientKey.Name = "txtDetailRecipientKey";
            this.txtDetailRecipientKey.Size = new System.Drawing.Size(370, 28);
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
            // txtDetailFieldKey
            //
            this.txtDetailFieldKey.Location = new System.Drawing.Point(2,2);
            this.txtDetailFieldKey.Name = "txtDetailFieldKey";
            this.txtDetailFieldKey.Size = new System.Drawing.Size(370, 28);
            this.txtDetailFieldKey.ASpecialSetting = true;
            this.txtDetailFieldKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtDetailFieldKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtDetailFieldKey.PartnerClass = "";
            this.txtDetailFieldKey.MaxLength = 32767;
            this.txtDetailFieldKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtDetailFieldKey.TextBoxWidth = 80;
            this.txtDetailFieldKey.ButtonWidth = 40;
            this.txtDetailFieldKey.ReadOnly = false;
            this.txtDetailFieldKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtDetailFieldKey.ButtonText = "Find";
            //
            // lblDetailFieldKey
            //
            this.lblDetailFieldKey.Location = new System.Drawing.Point(2,2);
            this.lblDetailFieldKey.Name = "lblDetailFieldKey";
            this.lblDetailFieldKey.AutoSize = true;
            this.lblDetailFieldKey.Text = "Field:";
            this.lblDetailFieldKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailFieldKey.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailFieldKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.lblDetailRecipientKey, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblDetailFieldKey, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtDetailRecipientKey, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtDetailFieldKey, 1, 1);
            //
            // rgrDateOrBatchRange
            //
            this.rgrDateOrBatchRange.Location = new System.Drawing.Point(2,2);
            this.rgrDateOrBatchRange.Name = "rgrDateOrBatchRange";
            this.rgrDateOrBatchRange.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.rgrDateOrBatchRange.Controls.Add(this.tableLayoutPanel5);
            //
            // rbtDateRange
            //
            this.rbtDateRange.Location = new System.Drawing.Point(2,2);
            this.rbtDateRange.Name = "rbtDateRange";
            this.rbtDateRange.AutoSize = true;
            this.rbtDateRange.Checked = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            //
            // dtpDateFrom
            //
            this.dtpDateFrom.Location = new System.Drawing.Point(2,2);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(94, 28);
            //
            // lblDateFrom
            //
            this.lblDateFrom.Location = new System.Drawing.Point(2,2);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Text = "Date from:";
            this.lblDateFrom.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateFrom.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDateFrom.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // dtpDateTo
            //
            this.dtpDateTo.Location = new System.Drawing.Point(2,2);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(94, 28);
            //
            // lblDateTo
            //
            this.lblDateTo.Location = new System.Drawing.Point(2,2);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.AutoSize = true;
            this.lblDateTo.Text = "To:";
            this.lblDateTo.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateTo.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDateTo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.lblDateFrom, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.dtpDateFrom, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblDateTo, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.dtpDateTo, 3, 0);
            this.rbtDateRange.CheckedChanged += new System.EventHandler(this.rbtDateRangeCheckedChanged);
            //
            // rbtBatchNumberSelection
            //
            this.rbtBatchNumberSelection.Location = new System.Drawing.Point(2,2);
            this.rbtBatchNumberSelection.Name = "rbtBatchNumberSelection";
            this.rbtBatchNumberSelection.AutoSize = true;
            //
            // tableLayoutPanel7
            //
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.AutoSize = true;
            //
            // txtBatchNumberStart
            //
            this.txtBatchNumberStart.Location = new System.Drawing.Point(2,2);
            this.txtBatchNumberStart.Name = "txtBatchNumberStart";
            this.txtBatchNumberStart.Size = new System.Drawing.Size(80, 28);
            this.txtBatchNumberStart.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
            this.txtBatchNumberStart.DecimalPlaces = 2;
            this.txtBatchNumberStart.NullValueAllowed = true;
            //
            // lblBatchNumberStart
            //
            this.lblBatchNumberStart.Location = new System.Drawing.Point(2,2);
            this.lblBatchNumberStart.Name = "lblBatchNumberStart";
            this.lblBatchNumberStart.AutoSize = true;
            this.lblBatchNumberStart.Text = "Batch Number from:";
            this.lblBatchNumberStart.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblBatchNumberStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblBatchNumberStart.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtBatchNumberEnd
            //
            this.txtBatchNumberEnd.Location = new System.Drawing.Point(2,2);
            this.txtBatchNumberEnd.Name = "txtBatchNumberEnd";
            this.txtBatchNumberEnd.Size = new System.Drawing.Size(80, 28);
            this.txtBatchNumberEnd.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
            this.txtBatchNumberEnd.DecimalPlaces = 2;
            this.txtBatchNumberEnd.NullValueAllowed = true;
            //
            // lblBatchNumberEnd
            //
            this.lblBatchNumberEnd.Location = new System.Drawing.Point(2,2);
            this.lblBatchNumberEnd.Name = "lblBatchNumberEnd";
            this.lblBatchNumberEnd.AutoSize = true;
            this.lblBatchNumberEnd.Text = "To:";
            this.lblBatchNumberEnd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblBatchNumberEnd.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblBatchNumberEnd.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.Controls.Add(this.lblBatchNumberStart, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.txtBatchNumberStart, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.lblBatchNumberEnd, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.txtBatchNumberEnd, 3, 0);
            this.rbtBatchNumberSelection.CheckedChanged += new System.EventHandler(this.rbtBatchNumberSelectionCheckedChanged);
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.rbtDateRange, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.rbtBatchNumberSelection, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 1, 1);
            this.rgrDateOrBatchRange.Text = "Date Or Batch Range";
            //
            // chkIncludeUnposted
            //
            this.chkIncludeUnposted.Location = new System.Drawing.Point(2,2);
            this.chkIncludeUnposted.Name = "chkIncludeUnposted";
            this.chkIncludeUnposted.Size = new System.Drawing.Size(30, 28);
            this.chkIncludeUnposted.Text = "";
            this.chkIncludeUnposted.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblIncludeUnposted
            //
            this.lblIncludeUnposted.Location = new System.Drawing.Point(2,2);
            this.lblIncludeUnposted.Name = "lblIncludeUnposted";
            this.lblIncludeUnposted.AutoSize = true;
            this.lblIncludeUnposted.Text = "Include Unposted Batches:";
            this.lblIncludeUnposted.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblIncludeUnposted.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblIncludeUnposted.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkTransactionsOnly
            //
            this.chkTransactionsOnly.Location = new System.Drawing.Point(2,2);
            this.chkTransactionsOnly.Name = "chkTransactionsOnly";
            this.chkTransactionsOnly.Size = new System.Drawing.Size(30, 28);
            this.chkTransactionsOnly.Text = "";
            this.chkTransactionsOnly.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblTransactionsOnly
            //
            this.lblTransactionsOnly.Location = new System.Drawing.Point(2,2);
            this.lblTransactionsOnly.Name = "lblTransactionsOnly";
            this.lblTransactionsOnly.AutoSize = true;
            this.lblTransactionsOnly.Text = "Transactions Only:";
            this.lblTransactionsOnly.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblTransactionsOnly.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTransactionsOnly.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkExtraColumns
            //
            this.chkExtraColumns.Location = new System.Drawing.Point(2,2);
            this.chkExtraColumns.Name = "chkExtraColumns";
            this.chkExtraColumns.Size = new System.Drawing.Size(30, 28);
            this.chkExtraColumns.Text = "";
            this.chkExtraColumns.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblExtraColumns
            //
            this.lblExtraColumns.Location = new System.Drawing.Point(2,2);
            this.lblExtraColumns.Name = "lblExtraColumns";
            this.lblExtraColumns.AutoSize = true;
            this.lblExtraColumns.Text = "With extra columns:";
            this.lblExtraColumns.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblExtraColumns.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblExtraColumns.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // pnlFilename
            //
            this.pnlFilename.Location = new System.Drawing.Point(2,2);
            this.pnlFilename.Name = "pnlFilename";
            this.pnlFilename.AutoSize = true;
            //
            // tableLayoutPanel8
            //
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.AutoSize = true;
            this.pnlFilename.Controls.Add(this.tableLayoutPanel8);
            //
            // txtFilename
            //
            this.txtFilename.Location = new System.Drawing.Point(2,2);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(300, 28);
            //
            // lblFilename
            //
            this.lblFilename.Location = new System.Drawing.Point(2,2);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.AutoSize = true;
            this.lblFilename.Text = "Filename:";
            this.lblFilename.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblFilename.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblFilename.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // btnBrowseFilename
            //
            this.btnBrowseFilename.Location = new System.Drawing.Point(2,2);
            this.btnBrowseFilename.Name = "btnBrowseFilename";
            this.btnBrowseFilename.AutoSize = true;
            this.btnBrowseFilename.Click += new System.EventHandler(this.BtnBrowseClick);
            this.btnBrowseFilename.Text = "Browse Filename";
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.Controls.Add(this.lblFilename, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.txtFilename, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnBrowseFilename, 2, 0);
            //
            // cmbDelimiter
            //
            this.cmbDelimiter.Location = new System.Drawing.Point(2,2);
            this.cmbDelimiter.Name = "cmbDelimiter";
            this.cmbDelimiter.Size = new System.Drawing.Size(80, 28);
            this.cmbDelimiter.Items.AddRange(new object[] {";",",",":","[SPACE]"});
            //
            // lblDelimiter
            //
            this.lblDelimiter.Location = new System.Drawing.Point(2,2);
            this.lblDelimiter.Name = "lblDelimiter";
            this.lblDelimiter.AutoSize = true;
            this.lblDelimiter.Text = "Delimiter:";
            this.lblDelimiter.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDelimiter.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDelimiter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDateFormat
            //
            this.cmbDateFormat.Location = new System.Drawing.Point(2,2);
            this.cmbDateFormat.Name = "cmbDateFormat";
            this.cmbDateFormat.Size = new System.Drawing.Size(160, 28);
            this.cmbDateFormat.Items.AddRange(new object[] {"MM/dd/yyyy","dd/MM/yyyy","yyyy-MM-dd"});
            //
            // lblDateFormat
            //
            this.lblDateFormat.Location = new System.Drawing.Point(2,2);
            this.lblDateFormat.Name = "lblDateFormat";
            this.lblDateFormat.AutoSize = true;
            this.lblDateFormat.Text = "Date Format:";
            this.lblDateFormat.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateFormat.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDateFormat.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbNumberFormat
            //
            this.cmbNumberFormat.Location = new System.Drawing.Point(2,2);
            this.cmbNumberFormat.Name = "cmbNumberFormat";
            this.cmbNumberFormat.Size = new System.Drawing.Size(160, 28);
            this.cmbNumberFormat.Items.AddRange(new object[] {"Decimal Point (12.34)","Decimal Comma (12,34)"});
            //
            // lblNumberFormat
            //
            this.lblNumberFormat.Location = new System.Drawing.Point(2,2);
            this.lblNumberFormat.Name = "lblNumberFormat";
            this.lblNumberFormat.AutoSize = true;
            this.lblNumberFormat.Text = "Number Format:";
            this.lblNumberFormat.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblNumberFormat.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblNumberFormat.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // pnlButtons
            //
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.AutoSize = true;
            //
            // tableLayoutPanel9
            //
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.AutoSize = true;
            this.pnlButtons.Controls.Add(this.tableLayoutPanel9);
            //
            // btnHelp
            //
            this.btnHelp.Location = new System.Drawing.Point(2,2);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.AutoSize = true;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelpClick);
            this.btnHelp.Text = "&Help";
            //
            // btnOK
            //
            this.btnOK.Location = new System.Drawing.Point(2,2);
            this.btnOK.Name = "btnOK";
            this.btnOK.AutoSize = true;
            this.btnOK.Click += new System.EventHandler(this.ExportBatches);
            this.btnOK.Text = "&Start";
            //
            // btnClose
            //
            this.btnClose.Location = new System.Drawing.Point(2,2);
            this.btnClose.Name = "btnClose";
            this.btnClose.AutoSize = true;
            this.btnClose.Click += new System.EventHandler(this.BtnCloseClick);
            this.btnClose.Text = "&Close";
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 400));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 100));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 100));
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.Controls.Add(this.btnHelp, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.btnOK, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.btnClose, 2, 0);
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 12;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.SetColumnSpan(this.rgrDetailSummary, 2);
            this.tableLayoutPanel1.Controls.Add(this.rgrDetailSummary, 0, 0);
            this.tableLayoutPanel1.SetColumnSpan(this.rgrCurrency, 2);
            this.tableLayoutPanel1.Controls.Add(this.rgrCurrency, 0, 1);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlRecipient, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlRecipient, 0, 2);
            this.tableLayoutPanel1.SetColumnSpan(this.rgrDateOrBatchRange, 2);
            this.tableLayoutPanel1.Controls.Add(this.rgrDateOrBatchRange, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblIncludeUnposted, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblTransactionsOnly, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblExtraColumns, 0, 6);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlFilename, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlFilename, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblDelimiter, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblDateFormat, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblNumberFormat, 0, 10);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlButtons, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlButtons, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.chkIncludeUnposted, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.chkTransactionsOnly, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.chkExtraColumns, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.cmbDelimiter, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.cmbDateFormat, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.cmbNumberFormat, 1, 10);
            //
            // tbbExportBatches
            //
            this.tbbExportBatches.Name = "tbbExportBatches";
            this.tbbExportBatches.AutoSize = true;
            this.tbbExportBatches.Click += new System.EventHandler(this.ExportBatches);
            this.tbbExportBatches.Text = "&Start";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbExportBatches});
            //
            // mniClose
            //
            this.mniClose.Name = "mniClose";
            this.mniClose.AutoSize = true;
            this.mniClose.Click += new System.EventHandler(this.BtnCloseClick);
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
            this.mniHelp.Click += new System.EventHandler(this.BtnHelpClick);
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
            // TFrmGiftBatchExport
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

            this.Name = "TFrmGiftBatchExport";
            this.Text = "Export Gift Batches";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.pnlFilename.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.rgrDateOrBatchRange.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlRecipient.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.rgrCurrency.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.rgrDetailSummary.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox rgrDetailSummary;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtDetail;
        private System.Windows.Forms.RadioButton rbtSummary;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateSummary;
        private System.Windows.Forms.GroupBox rgrCurrency;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbtBaseCurrency;
        private System.Windows.Forms.RadioButton rbtOriginalTransactionCurrency;
        private System.Windows.Forms.Panel pnlRecipient;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtDetailRecipientKey;
        private System.Windows.Forms.Label lblDetailRecipientKey;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtDetailFieldKey;
        private System.Windows.Forms.Label lblDetailFieldKey;
        private System.Windows.Forms.GroupBox rgrDateOrBatchRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.RadioButton rbtDateRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateFrom;
        private System.Windows.Forms.Label lblDateFrom;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateTo;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.RadioButton rbtBatchNumberSelection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private Ict.Common.Controls.TTxtNumericTextBox txtBatchNumberStart;
        private System.Windows.Forms.Label lblBatchNumberStart;
        private Ict.Common.Controls.TTxtNumericTextBox txtBatchNumberEnd;
        private System.Windows.Forms.Label lblBatchNumberEnd;
        private System.Windows.Forms.CheckBox chkIncludeUnposted;
        private System.Windows.Forms.Label lblIncludeUnposted;
        private System.Windows.Forms.CheckBox chkTransactionsOnly;
        private System.Windows.Forms.Label lblTransactionsOnly;
        private System.Windows.Forms.CheckBox chkExtraColumns;
        private System.Windows.Forms.Label lblExtraColumns;
        private System.Windows.Forms.Panel pnlFilename;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.Button btnBrowseFilename;
        private Ict.Common.Controls.TCmbAutoComplete cmbDelimiter;
        private System.Windows.Forms.Label lblDelimiter;
        private Ict.Common.Controls.TCmbAutoComplete cmbDateFormat;
        private System.Windows.Forms.Label lblDateFormat;
        private Ict.Common.Controls.TCmbAutoComplete cmbNumberFormat;
        private System.Windows.Forms.Label lblNumberFormat;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbExportBatches;
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
