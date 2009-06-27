/* auto generated with nant generateWinforms from APSupplierTransactions.yaml
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
    partial class TFrmAccountsPayableSupplierTransactions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmAccountsPayableSupplierTransactions));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlCurrentSupplierInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtCurrentSupplierName = new System.Windows.Forms.TextBox();
            this.lblCurrentSupplierName = new System.Windows.Forms.Label();
            this.txtCurrentSupplierCurrency = new System.Windows.Forms.TextBox();
            this.lblCurrentSupplierCurrency = new System.Windows.Forms.Label();
            this.pnlFilter1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbDate = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.cmbDateField = new System.Windows.Forms.ComboBox();
            this.lblDateField = new System.Windows.Forms.Label();
            this.pnlFilter2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkHideAgedTransactions = new System.Windows.Forms.CheckBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTagApprovable = new System.Windows.Forms.Button();
            this.btnTagPostable = new System.Windows.Forms.Button();
            this.btnTagPayable = new System.Windows.Forms.Button();
            this.btnUntagAll = new System.Windows.Forms.Button();
            this.txtSumOfTagged = new System.Windows.Forms.TextBox();
            this.lblSumOfTagged = new System.Windows.Forms.Label();
            this.grdResult = new System.Windows.Forms.DataGridView();
            this.pnlDisplayedBalance = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDisplayedBalance = new System.Windows.Forms.TextBox();
            this.lblDisplayedBalance = new System.Windows.Forms.Label();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbNewInvoice = new System.Windows.Forms.ToolStripButton();
            this.tbbNewCreditNote = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbOpenSelected = new System.Windows.Forms.ToolStripButton();
            this.tbbReverseSelected = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbApproveTagged = new System.Windows.Forms.ToolStripButton();
            this.tbbPostTagged = new System.Windows.Forms.ToolStripButton();
            this.tbbAddTaggedToPayment = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReprintRemittanceAdvice = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReprintCheque = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReprintPaymentReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAction = new System.Windows.Forms.ToolStripMenuItem();
            this.mniActionNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewInvoice = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewCreditNote = new System.Windows.Forms.ToolStripMenuItem();
            this.mniOpenSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniReverseTransaction = new System.Windows.Forms.ToolStripMenuItem();
            this.mniApproveTagged = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPostTagged = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAddTaggedToPayment = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlContent.SuspendLayout();
            this.pnlCurrentSupplierInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlFilter1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlFilter2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnlDisplayedBalance.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Controls.Add(this.grdResult);
            this.pnlContent.Controls.Add(this.pnlDisplayedBalance);
            this.pnlContent.Controls.Add(this.pnlButtons);
            this.pnlContent.Controls.Add(this.pnlFilter2);
            this.pnlContent.Controls.Add(this.pnlFilter1);
            this.pnlContent.Controls.Add(this.pnlCurrentSupplierInfo);
            //
            // pnlCurrentSupplierInfo
            //
            this.pnlCurrentSupplierInfo.Name = "pnlCurrentSupplierInfo";
            this.pnlCurrentSupplierInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCurrentSupplierInfo.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlCurrentSupplierInfo.Controls.Add(this.tableLayoutPanel1);
            //
            // txtCurrentSupplierName
            //
            this.txtCurrentSupplierName.Location = new System.Drawing.Point(2,2);
            this.txtCurrentSupplierName.Name = "txtCurrentSupplierName";
            this.txtCurrentSupplierName.Size = new System.Drawing.Size(150, 28);
            this.txtCurrentSupplierName.ReadOnly = true;
            //
            // lblCurrentSupplierName
            //
            this.lblCurrentSupplierName.Location = new System.Drawing.Point(2,2);
            this.lblCurrentSupplierName.Name = "lblCurrentSupplierName";
            this.lblCurrentSupplierName.AutoSize = true;
            this.lblCurrentSupplierName.Text = "Current Supplier:";
            this.lblCurrentSupplierName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrentSupplierName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtCurrentSupplierName, 1, 0);
            //
            // txtCurrentSupplierCurrency
            //
            this.txtCurrentSupplierCurrency.Location = new System.Drawing.Point(2,2);
            this.txtCurrentSupplierCurrency.Name = "txtCurrentSupplierCurrency";
            this.txtCurrentSupplierCurrency.Size = new System.Drawing.Size(150, 28);
            this.txtCurrentSupplierCurrency.ReadOnly = true;
            //
            // lblCurrentSupplierCurrency
            //
            this.lblCurrentSupplierCurrency.Location = new System.Drawing.Point(2,2);
            this.lblCurrentSupplierCurrency.Name = "lblCurrentSupplierCurrency";
            this.lblCurrentSupplierCurrency.AutoSize = true;
            this.lblCurrentSupplierCurrency.Text = "Currency:";
            this.lblCurrentSupplierCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrentSupplierCurrency, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtCurrentSupplierCurrency, 3, 0);
            //
            // pnlFilter1
            //
            this.pnlFilter1.Name = "pnlFilter1";
            this.pnlFilter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter1.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlFilter1.Controls.Add(this.tableLayoutPanel2);
            //
            // cmbType
            //
            this.cmbType.Location = new System.Drawing.Point(2,2);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(150, 28);
            //
            // lblType
            //
            this.lblType.Location = new System.Drawing.Point(2,2);
            this.lblType.Name = "lblType";
            this.lblType.AutoSize = true;
            this.lblType.Text = "&Type:";
            this.lblType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblType, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbType, 1, 0);
            //
            // cmbDate
            //
            this.cmbDate.Location = new System.Drawing.Point(2,2);
            this.cmbDate.Name = "cmbDate";
            this.cmbDate.Size = new System.Drawing.Size(150, 28);
            //
            // lblDate
            //
            this.lblDate.Location = new System.Drawing.Point(2,2);
            this.lblDate.Name = "lblDate";
            this.lblDate.AutoSize = true;
            this.lblDate.Text = "&Date:";
            this.lblDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDate, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbDate, 3, 0);
            //
            // cmbDateField
            //
            this.cmbDateField.Location = new System.Drawing.Point(2,2);
            this.cmbDateField.Name = "cmbDateField";
            this.cmbDateField.Size = new System.Drawing.Size(150, 28);
            //
            // lblDateField
            //
            this.lblDateField.Location = new System.Drawing.Point(2,2);
            this.lblDateField.Name = "lblDateField";
            this.lblDateField.AutoSize = true;
            this.lblDateField.Text = "Date &Field:";
            this.lblDateField.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDateField, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbDateField, 5, 0);
            //
            // pnlFilter2
            //
            this.pnlFilter2.Name = "pnlFilter2";
            this.pnlFilter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter2.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlFilter2.Controls.Add(this.tableLayoutPanel3);
            //
            // cmbStatus
            //
            this.cmbStatus.Location = new System.Drawing.Point(2,2);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(150, 28);
            //
            // lblStatus
            //
            this.lblStatus.Location = new System.Drawing.Point(2,2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.AutoSize = true;
            this.lblStatus.Text = "&Status:";
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblStatus, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbStatus, 1, 0);
            //
            // chkHideAgedTransactions
            //
            this.chkHideAgedTransactions.Location = new System.Drawing.Point(2,2);
            this.chkHideAgedTransactions.Name = "chkHideAgedTransactions";
            this.chkHideAgedTransactions.AutoSize = true;
            this.chkHideAgedTransactions.Text = "Hide &Aged Transactions";
            this.chkHideAgedTransactions.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkHideAgedTransactions, 2, 0);
            this.tableLayoutPanel3.SetColumnSpan(this.chkHideAgedTransactions, 2);
            //
            // pnlButtons
            //
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtons.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.ColumnCount = 10;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlButtons.Controls.Add(this.tableLayoutPanel4);
            //
            // btnTagApprovable
            //
            this.btnTagApprovable.Location = new System.Drawing.Point(2,2);
            this.btnTagApprovable.Name = "btnTagApprovable";
            this.btnTagApprovable.AutoSize = true;
            this.btnTagApprovable.Text = "Tag all Appro&vable";
            this.tableLayoutPanel4.Controls.Add(this.btnTagApprovable, 0, 0);
            this.tableLayoutPanel4.SetColumnSpan(this.btnTagApprovable, 2);
            //
            // btnTagPostable
            //
            this.btnTagPostable.Location = new System.Drawing.Point(2,2);
            this.btnTagPostable.Name = "btnTagPostable";
            this.btnTagPostable.AutoSize = true;
            this.btnTagPostable.Text = "Tag all P&ostable";
            this.tableLayoutPanel4.Controls.Add(this.btnTagPostable, 2, 0);
            this.tableLayoutPanel4.SetColumnSpan(this.btnTagPostable, 2);
            //
            // btnTagPayable
            //
            this.btnTagPayable.Location = new System.Drawing.Point(2,2);
            this.btnTagPayable.Name = "btnTagPayable";
            this.btnTagPayable.AutoSize = true;
            this.btnTagPayable.Text = "Tag all Paya&ble";
            this.tableLayoutPanel4.Controls.Add(this.btnTagPayable, 4, 0);
            this.tableLayoutPanel4.SetColumnSpan(this.btnTagPayable, 2);
            //
            // btnUntagAll
            //
            this.btnUntagAll.Location = new System.Drawing.Point(2,2);
            this.btnUntagAll.Name = "btnUntagAll";
            this.btnUntagAll.AutoSize = true;
            this.btnUntagAll.Text = "&Untag All";
            this.tableLayoutPanel4.Controls.Add(this.btnUntagAll, 6, 0);
            this.tableLayoutPanel4.SetColumnSpan(this.btnUntagAll, 2);
            //
            // txtSumOfTagged
            //
            this.txtSumOfTagged.Location = new System.Drawing.Point(2,2);
            this.txtSumOfTagged.Name = "txtSumOfTagged";
            this.txtSumOfTagged.Size = new System.Drawing.Size(150, 28);
            this.txtSumOfTagged.ReadOnly = true;
            //
            // lblSumOfTagged
            //
            this.lblSumOfTagged.Location = new System.Drawing.Point(2,2);
            this.lblSumOfTagged.Name = "lblSumOfTagged";
            this.lblSumOfTagged.AutoSize = true;
            this.lblSumOfTagged.Text = "Sum of Tagged:";
            this.lblSumOfTagged.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblSumOfTagged, 8, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtSumOfTagged, 9, 0);
            //
            // grdResult
            //
            this.grdResult.Name = "grdResult";
            this.grdResult.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // pnlDisplayedBalance
            //
            this.pnlDisplayedBalance.Name = "pnlDisplayedBalance";
            this.pnlDisplayedBalance.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDisplayedBalance.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlDisplayedBalance.Controls.Add(this.tableLayoutPanel5);
            //
            // txtDisplayedBalance
            //
            this.txtDisplayedBalance.Location = new System.Drawing.Point(2,2);
            this.txtDisplayedBalance.Name = "txtDisplayedBalance";
            this.txtDisplayedBalance.Size = new System.Drawing.Size(150, 28);
            this.txtDisplayedBalance.ReadOnly = true;
            //
            // lblDisplayedBalance
            //
            this.lblDisplayedBalance.Location = new System.Drawing.Point(2,2);
            this.lblDisplayedBalance.Name = "lblDisplayedBalance";
            this.lblDisplayedBalance.AutoSize = true;
            this.lblDisplayedBalance.Text = "Displayed Balance:";
            this.lblDisplayedBalance.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblDisplayedBalance, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtDisplayedBalance, 1, 0);
            //
            // tbbNewInvoice
            //
            this.tbbNewInvoice.Name = "tbbNewInvoice";
            this.tbbNewInvoice.AutoSize = true;
            this.tbbNewInvoice.Click += new System.EventHandler(this.tbbNewInvoiceClick);
            this.tbbNewInvoice.Text = "&Invoice";
            //
            // tbbNewCreditNote
            //
            this.tbbNewCreditNote.Name = "tbbNewCreditNote";
            this.tbbNewCreditNote.AutoSize = true;
            this.tbbNewCreditNote.Click += new System.EventHandler(this.tbbNewCreditNoteClick);
            this.tbbNewCreditNote.Text = "&Credit Note";
            //
            // tbbSeparator0
            //
            this.tbbSeparator0.Name = "tbbSeparator0";
            this.tbbSeparator0.AutoSize = true;
            this.tbbSeparator0.Text = "-";
            //
            // tbbOpenSelected
            //
            this.tbbOpenSelected.Name = "tbbOpenSelected";
            this.tbbOpenSelected.AutoSize = true;
            this.tbbOpenSelected.Click += new System.EventHandler(this.tbbOpenSelectedClick);
            this.tbbOpenSelected.Text = "&Open Selected";
            //
            // tbbReverseSelected
            //
            this.tbbReverseSelected.Name = "tbbReverseSelected";
            this.tbbReverseSelected.AutoSize = true;
            this.tbbReverseSelected.Click += new System.EventHandler(this.tbbReverseSelectedClick);
            this.tbbReverseSelected.Text = "Re&verse Selected";
            //
            // tbbSeparator1
            //
            this.tbbSeparator1.Name = "tbbSeparator1";
            this.tbbSeparator1.AutoSize = true;
            this.tbbSeparator1.Text = "-";
            //
            // tbbApproveTagged
            //
            this.tbbApproveTagged.Name = "tbbApproveTagged";
            this.tbbApproveTagged.AutoSize = true;
            this.tbbApproveTagged.Click += new System.EventHandler(this.tbbApproveTaggedClick);
            this.tbbApproveTagged.Text = "&Approve Tagged";
            //
            // tbbPostTagged
            //
            this.tbbPostTagged.Name = "tbbPostTagged";
            this.tbbPostTagged.AutoSize = true;
            this.tbbPostTagged.Click += new System.EventHandler(this.tbbPostTaggedClick);
            this.tbbPostTagged.Text = "&Post Tagged";
            //
            // tbbAddTaggedToPayment
            //
            this.tbbAddTaggedToPayment.Name = "tbbAddTaggedToPayment";
            this.tbbAddTaggedToPayment.AutoSize = true;
            this.tbbAddTaggedToPayment.Click += new System.EventHandler(this.tbbAddTaggedToPaymentClick);
            this.tbbAddTaggedToPayment.Text = "Add Tagged to Pa&yment";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbNewInvoice,
                        tbbNewCreditNote,
                        tbbSeparator0,
                        tbbOpenSelected,
                        tbbReverseSelected,
                        tbbSeparator1,
                        tbbApproveTagged,
                        tbbPostTagged,
                        tbbAddTaggedToPayment});
            //
            // mniReprintRemittanceAdvice
            //
            this.mniReprintRemittanceAdvice.Name = "mniReprintRemittanceAdvice";
            this.mniReprintRemittanceAdvice.AutoSize = true;
            this.mniReprintRemittanceAdvice.Text = "Reprint Re&mittance Advice";
            //
            // mniReprintCheque
            //
            this.mniReprintCheque.Name = "mniReprintCheque";
            this.mniReprintCheque.AutoSize = true;
            this.mniReprintCheque.Text = "Reprint &Cheque";
            //
            // mniReprintPaymentReport
            //
            this.mniReprintPaymentReport.Name = "mniReprintPaymentReport";
            this.mniReprintPaymentReport.AutoSize = true;
            this.mniReprintPaymentReport.Text = "Reprint Pa&yment Report";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "-";
            //
            // mniClose
            //
            this.mniClose.Name = "mniClose";
            this.mniClose.AutoSize = true;
            this.mniClose.Click += new System.EventHandler(this.mniCloseClick);
            this.mniClose.Image = ((System.Drawing.Bitmap)resources.GetObject("mniClose.Glyph"));
            this.mniClose.ToolTipText = "Closes this window";
            this.mniClose.Text = "&Close";
            //
            // mniFile
            //
            this.mniFile.Name = "mniFile";
            this.mniFile.AutoSize = true;
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniReprintRemittanceAdvice,
                        mniReprintCheque,
                        mniReprintPaymentReport,
                        mniSeparator0,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniNewInvoice
            //
            this.mniNewInvoice.Name = "mniNewInvoice";
            this.mniNewInvoice.AutoSize = true;
            this.mniNewInvoice.Click += new System.EventHandler(this.mniNewInvoiceClick);
            this.mniNewInvoice.Text = "&Invoice";
            //
            // mniNewCreditNote
            //
            this.mniNewCreditNote.Name = "mniNewCreditNote";
            this.mniNewCreditNote.AutoSize = true;
            this.mniNewCreditNote.Click += new System.EventHandler(this.mniNewCreditNoteClick);
            this.mniNewCreditNote.Text = "&Credit Note";
            //
            // mniActionNew
            //
            this.mniActionNew.Name = "mniActionNew";
            this.mniActionNew.AutoSize = true;
            this.mniActionNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniNewInvoice,
                        mniNewCreditNote});
            this.mniActionNew.Text = "&New...";
            //
            // mniOpenSelected
            //
            this.mniOpenSelected.Name = "mniOpenSelected";
            this.mniOpenSelected.AutoSize = true;
            this.mniOpenSelected.Click += new System.EventHandler(this.mniOpenSelectedClick);
            this.mniOpenSelected.Text = "&Open Selected";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
            //
            // mniReverseTransaction
            //
            this.mniReverseTransaction.Name = "mniReverseTransaction";
            this.mniReverseTransaction.AutoSize = true;
            this.mniReverseTransaction.Click += new System.EventHandler(this.mniReverseTransactionClick);
            this.mniReverseTransaction.Text = "Re&verse Selected";
            //
            // mniApproveTagged
            //
            this.mniApproveTagged.Name = "mniApproveTagged";
            this.mniApproveTagged.AutoSize = true;
            this.mniApproveTagged.Click += new System.EventHandler(this.mniApproveTaggedClick);
            this.mniApproveTagged.Text = "&Approve Tagged";
            //
            // mniPostTagged
            //
            this.mniPostTagged.Name = "mniPostTagged";
            this.mniPostTagged.AutoSize = true;
            this.mniPostTagged.Click += new System.EventHandler(this.mniPostTaggedClick);
            this.mniPostTagged.Text = "&Post Tagged";
            //
            // mniAddTaggedToPayment
            //
            this.mniAddTaggedToPayment.Name = "mniAddTaggedToPayment";
            this.mniAddTaggedToPayment.AutoSize = true;
            this.mniAddTaggedToPayment.Click += new System.EventHandler(this.mniAddTaggedToPaymentClick);
            this.mniAddTaggedToPayment.Text = "Add Tagged to Pa&yment";
            //
            // mniAction
            //
            this.mniAction.Name = "mniAction";
            this.mniAction.AutoSize = true;
            this.mniAction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniActionNew,
                        mniOpenSelected,
                        mniSeparator1,
                        mniReverseTransaction,
                        mniApproveTagged,
                        mniPostTagged,
                        mniAddTaggedToPayment});
            this.mniAction.Text = "&Action";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "-";
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
                        mniSeparator2,
                        mniHelpBugReport,
                        mniSeparator3,
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
                        mniAction,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmAccountsPayableSupplierTransactions
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
            this.Name = "TFrmAccountsPayableSupplierTransactions";
            this.Text = "Supplier Transactions";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.pnlDisplayedBalance.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlFilter2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlFilter1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlCurrentSupplierInfo.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlCurrentSupplierInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtCurrentSupplierName;
        private System.Windows.Forms.Label lblCurrentSupplierName;
        private System.Windows.Forms.TextBox txtCurrentSupplierCurrency;
        private System.Windows.Forms.Label lblCurrentSupplierCurrency;
        private System.Windows.Forms.Panel pnlFilter1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbDate;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.ComboBox cmbDateField;
        private System.Windows.Forms.Label lblDateField;
        private System.Windows.Forms.Panel pnlFilter2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkHideAgedTransactions;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnTagApprovable;
        private System.Windows.Forms.Button btnTagPostable;
        private System.Windows.Forms.Button btnTagPayable;
        private System.Windows.Forms.Button btnUntagAll;
        private System.Windows.Forms.TextBox txtSumOfTagged;
        private System.Windows.Forms.Label lblSumOfTagged;
        private System.Windows.Forms.DataGridView grdResult;
        private System.Windows.Forms.Panel pnlDisplayedBalance;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TextBox txtDisplayedBalance;
        private System.Windows.Forms.Label lblDisplayedBalance;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbNewInvoice;
        private System.Windows.Forms.ToolStripButton tbbNewCreditNote;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator0;
        private System.Windows.Forms.ToolStripButton tbbOpenSelected;
        private System.Windows.Forms.ToolStripButton tbbReverseSelected;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator1;
        private System.Windows.Forms.ToolStripButton tbbApproveTagged;
        private System.Windows.Forms.ToolStripButton tbbPostTagged;
        private System.Windows.Forms.ToolStripButton tbbAddTaggedToPayment;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniReprintRemittanceAdvice;
        private System.Windows.Forms.ToolStripMenuItem mniReprintCheque;
        private System.Windows.Forms.ToolStripMenuItem mniReprintPaymentReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniAction;
        private System.Windows.Forms.ToolStripMenuItem mniActionNew;
        private System.Windows.Forms.ToolStripMenuItem mniNewInvoice;
        private System.Windows.Forms.ToolStripMenuItem mniNewCreditNote;
        private System.Windows.Forms.ToolStripMenuItem mniOpenSelected;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniReverseTransaction;
        private System.Windows.Forms.ToolStripMenuItem mniApproveTagged;
        private System.Windows.Forms.ToolStripMenuItem mniPostTagged;
        private System.Windows.Forms.ToolStripMenuItem mniAddTaggedToPayment;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
