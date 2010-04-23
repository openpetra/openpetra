/* auto generated with nant generateWinforms from APPayment.yaml
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

namespace Ict.Petra.Client.MFinance.Gui.AccountsPayable
{
    partial class TFrmAPPayment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmAPPayment));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.grpPaymentList = new System.Windows.Forms.GroupBox();
            this.grdPayments = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlSupplierDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtCurrency = new System.Windows.Forms.TextBox();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.cmbPaymentType = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblPaymentType = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.txtExchangeRate = new System.Windows.Forms.TextBox();
            this.lblExchangeRate = new System.Windows.Forms.Label();
            this.cmbBankAccount = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblBankAccount = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.lblReference = new System.Windows.Forms.Label();
            this.chkPrintRemittance = new System.Windows.Forms.CheckBox();
            this.chkPrintLabel = new System.Windows.Forms.CheckBox();
            this.chkPrintCheque = new System.Windows.Forms.CheckBox();
            this.txtChequeNumber = new System.Windows.Forms.TextBox();
            this.lblChequeNumber = new System.Windows.Forms.Label();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrAmountToPay = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtPayFullOutstandingAmount = new System.Windows.Forms.RadioButton();
            this.rbtPayAPartialAmount = new System.Windows.Forms.RadioButton();
            this.chkClaimDiscount = new System.Windows.Forms.CheckBox();
            this.txtAmountToPay = new System.Windows.Forms.TextBox();
            this.lblAmountToPay = new System.Windows.Forms.Label();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbMakePayment = new System.Windows.Forms.ToolStripButton();
            this.tbbSplitByInvoice = new System.Windows.Forms.ToolStripButton();
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
            this.grpPaymentList.SuspendLayout();
            this.pnlSupplierDetails.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpDetails.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.rgrAmountToPay.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.grpDetails);
            this.pnlContent.Controls.Add(this.pnlSupplierDetails);
            this.pnlContent.Controls.Add(this.grpPaymentList);
            //
            // grpPaymentList
            //
            this.grpPaymentList.Name = "grpPaymentList";
            this.grpPaymentList.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpPaymentList.Width = 200;
            this.grpPaymentList.Controls.Add(this.grdPayments);
            //
            // grdPayments
            //
            this.grdPayments.Name = "grdPayments";
            this.grdPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPayments.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChanged);
            this.grpPaymentList.Text = "Suppliers to Pay";
            //
            // pnlSupplierDetails
            //
            this.pnlSupplierDetails.Name = "pnlSupplierDetails";
            this.pnlSupplierDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSupplierDetails.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlSupplierDetails.Controls.Add(this.tableLayoutPanel1);
            //
            // txtCurrency
            //
            this.txtCurrency.Location = new System.Drawing.Point(2,2);
            this.txtCurrency.Name = "txtCurrency";
            this.txtCurrency.Size = new System.Drawing.Size(150, 28);
            this.txtCurrency.ReadOnly = true;
            //
            // lblCurrency
            //
            this.lblCurrency.Location = new System.Drawing.Point(2,2);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Text = "Currency:";
            this.lblCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCurrency.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbPaymentType
            //
            this.cmbPaymentType.Location = new System.Drawing.Point(2,2);
            this.cmbPaymentType.Name = "cmbPaymentType";
            this.cmbPaymentType.Size = new System.Drawing.Size(150, 28);
            this.cmbPaymentType.Items.AddRange(new object[] {"Cash","Cheque"});
            this.cmbPaymentType.Text = "Cash";
            //
            // lblPaymentType
            //
            this.lblPaymentType.Location = new System.Drawing.Point(2,2);
            this.lblPaymentType.Name = "lblPaymentType";
            this.lblPaymentType.AutoSize = true;
            this.lblPaymentType.Text = "Payment T&ype:";
            this.lblPaymentType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPaymentType.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtTotalAmount
            //
            this.txtTotalAmount.Location = new System.Drawing.Point(2,2);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(150, 28);
            this.txtTotalAmount.ReadOnly = true;
            //
            // lblTotalAmount
            //
            this.lblTotalAmount.Location = new System.Drawing.Point(2,2);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Text = "&Amount:";
            this.lblTotalAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblTotalAmount.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtExchangeRate
            //
            this.txtExchangeRate.Location = new System.Drawing.Point(2,2);
            this.txtExchangeRate.Name = "txtExchangeRate";
            this.txtExchangeRate.Size = new System.Drawing.Size(150, 28);
            //
            // lblExchangeRate
            //
            this.lblExchangeRate.Location = new System.Drawing.Point(2,2);
            this.lblExchangeRate.Name = "lblExchangeRate";
            this.lblExchangeRate.AutoSize = true;
            this.lblExchangeRate.Text = "Exchange Rate:";
            this.lblExchangeRate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblExchangeRate.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbBankAccount
            //
            this.cmbBankAccount.Location = new System.Drawing.Point(2,2);
            this.cmbBankAccount.Name = "cmbBankAccount";
            this.cmbBankAccount.Size = new System.Drawing.Size(300, 28);
            this.cmbBankAccount.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblBankAccount
            //
            this.lblBankAccount.Location = new System.Drawing.Point(2,2);
            this.lblBankAccount.Name = "lblBankAccount";
            this.lblBankAccount.AutoSize = true;
            this.lblBankAccount.Text = "Bank Account:";
            this.lblBankAccount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblBankAccount.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtReference
            //
            this.txtReference.Location = new System.Drawing.Point(2,2);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(150, 28);
            //
            // lblReference
            //
            this.lblReference.Location = new System.Drawing.Point(2,2);
            this.lblReference.Name = "lblReference";
            this.lblReference.AutoSize = true;
            this.lblReference.Text = "&Reference:";
            this.lblReference.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblReference.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // chkPrintRemittance
            //
            this.chkPrintRemittance.Location = new System.Drawing.Point(2,2);
            this.chkPrintRemittance.Name = "chkPrintRemittance";
            this.chkPrintRemittance.AutoSize = true;
            this.chkPrintRemittance.Text = "Print Remittance";
            this.chkPrintRemittance.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // chkPrintLabel
            //
            this.chkPrintLabel.Location = new System.Drawing.Point(2,2);
            this.chkPrintLabel.Name = "chkPrintLabel";
            this.chkPrintLabel.AutoSize = true;
            this.chkPrintLabel.Text = "Print Label";
            this.chkPrintLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // chkPrintCheque
            //
            this.chkPrintCheque.Location = new System.Drawing.Point(2,2);
            this.chkPrintCheque.Name = "chkPrintCheque";
            this.chkPrintCheque.AutoSize = true;
            this.chkPrintCheque.Text = "Print Cheque";
            this.chkPrintCheque.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // txtChequeNumber
            //
            this.txtChequeNumber.Location = new System.Drawing.Point(2,2);
            this.txtChequeNumber.Name = "txtChequeNumber";
            this.txtChequeNumber.Size = new System.Drawing.Size(150, 28);
            //
            // lblChequeNumber
            //
            this.lblChequeNumber.Location = new System.Drawing.Point(2,2);
            this.lblChequeNumber.Name = "lblChequeNumber";
            this.lblChequeNumber.AutoSize = true;
            this.lblChequeNumber.Text = "Cheque Number:";
            this.lblChequeNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblChequeNumber.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblCurrency, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalAmount, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblBankAccount, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblReference, 0, 3);
            this.tableLayoutPanel1.SetColumnSpan(this.chkPrintRemittance, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkPrintRemittance, 0, 4);
            this.tableLayoutPanel1.SetColumnSpan(this.chkPrintCheque, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkPrintCheque, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtCurrency, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtTotalAmount, 1, 1);
            this.tableLayoutPanel1.SetColumnSpan(this.cmbBankAccount, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbBankAccount, 1, 2);
            this.tableLayoutPanel1.SetColumnSpan(this.txtReference, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtReference, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblPaymentType, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblExchangeRate, 2, 1);
            this.tableLayoutPanel1.SetColumnSpan(this.chkPrintLabel, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkPrintLabel, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblChequeNumber, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.cmbPaymentType, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtExchangeRate, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtChequeNumber, 3, 5);
            //
            // grpDetails
            //
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetails.AutoSize = true;
            this.grpDetails.Controls.Add(this.grdDetails);
            this.grpDetails.Controls.Add(this.pnlDetails);
            //
            // grdDetails
            //
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChangedDetails);
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
            // rgrAmountToPay
            //
            this.rgrAmountToPay.Location = new System.Drawing.Point(2,2);
            this.rgrAmountToPay.Name = "rgrAmountToPay";
            this.rgrAmountToPay.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.rgrAmountToPay.Controls.Add(this.tableLayoutPanel3);
            //
            // rbtPayFullOutstandingAmount
            //
            this.rbtPayFullOutstandingAmount.Location = new System.Drawing.Point(2,2);
            this.rbtPayFullOutstandingAmount.Name = "rbtPayFullOutstandingAmount";
            this.rbtPayFullOutstandingAmount.AutoSize = true;
            this.rbtPayFullOutstandingAmount.Text = "PayFullOutstandingAmount";
            this.rbtPayFullOutstandingAmount.Checked = true;
            //
            // rbtPayAPartialAmount
            //
            this.rbtPayAPartialAmount.Location = new System.Drawing.Point(2,2);
            this.rbtPayAPartialAmount.Name = "rbtPayAPartialAmount";
            this.rbtPayAPartialAmount.AutoSize = true;
            this.rbtPayAPartialAmount.Text = "PayAPartialAmount";
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.rbtPayFullOutstandingAmount, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtPayAPartialAmount, 0, 1);
            this.rgrAmountToPay.Text = "Amount To Pay";
            //
            // chkClaimDiscount
            //
            this.chkClaimDiscount.Location = new System.Drawing.Point(2,2);
            this.chkClaimDiscount.Name = "chkClaimDiscount";
            this.chkClaimDiscount.AutoSize = true;
            this.chkClaimDiscount.Enabled = false;
            this.chkClaimDiscount.Text = "Claim Discount";
            this.chkClaimDiscount.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // txtAmountToPay
            //
            this.txtAmountToPay.Location = new System.Drawing.Point(2,2);
            this.txtAmountToPay.Name = "txtAmountToPay";
            this.txtAmountToPay.Size = new System.Drawing.Size(150, 28);
            this.txtAmountToPay.ReadOnly = true;
            //
            // lblAmountToPay
            //
            this.lblAmountToPay.Location = new System.Drawing.Point(2,2);
            this.lblAmountToPay.Name = "lblAmountToPay";
            this.lblAmountToPay.AutoSize = true;
            this.lblAmountToPay.Text = "Amount To Pay:";
            this.lblAmountToPay.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAmountToPay.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.SetColumnSpan(this.rgrAmountToPay, 2);
            this.tableLayoutPanel2.Controls.Add(this.rgrAmountToPay, 0, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.chkClaimDiscount, 2);
            this.tableLayoutPanel2.Controls.Add(this.chkClaimDiscount, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblAmountToPay, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtAmountToPay, 1, 2);
            this.grpDetails.Text = "Invoices in this payment";
            //
            // tbbMakePayment
            //
            this.tbbMakePayment.Name = "tbbMakePayment";
            this.tbbMakePayment.AutoSize = true;
            this.tbbMakePayment.Click += new System.EventHandler(this.MakePayment);
            this.tbbMakePayment.Text = "&Make Payment";
            //
            // tbbSplitByInvoice
            //
            this.tbbSplitByInvoice.Name = "tbbSplitByInvoice";
            this.tbbSplitByInvoice.AutoSize = true;
            this.tbbSplitByInvoice.Text = "Split By Invoice";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbMakePayment,
                        tbbSplitByInvoice});
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
            // TFrmAPPayment
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 600);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmAPPayment";
            this.Text = "AP Payment";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.rgrAmountToPay.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.grpDetails.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlSupplierDetails.ResumeLayout(false);
            this.grpPaymentList.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.GroupBox grpPaymentList;
        private Ict.Common.Controls.TSgrdDataGridPaged grdPayments;
        private System.Windows.Forms.Panel pnlSupplierDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtCurrency;
        private System.Windows.Forms.Label lblCurrency;
        private Ict.Common.Controls.TCmbAutoComplete cmbPaymentType;
        private System.Windows.Forms.Label lblPaymentType;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.TextBox txtExchangeRate;
        private System.Windows.Forms.Label lblExchangeRate;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbBankAccount;
        private System.Windows.Forms.Label lblBankAccount;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Label lblReference;
        private System.Windows.Forms.CheckBox chkPrintRemittance;
        private System.Windows.Forms.CheckBox chkPrintLabel;
        private System.Windows.Forms.CheckBox chkPrintCheque;
        private System.Windows.Forms.TextBox txtChequeNumber;
        private System.Windows.Forms.Label lblChequeNumber;
        private System.Windows.Forms.GroupBox grpDetails;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox rgrAmountToPay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbtPayFullOutstandingAmount;
        private System.Windows.Forms.RadioButton rbtPayAPartialAmount;
        private System.Windows.Forms.CheckBox chkClaimDiscount;
        private System.Windows.Forms.TextBox txtAmountToPay;
        private System.Windows.Forms.Label lblAmountToPay;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbMakePayment;
        private System.Windows.Forms.ToolStripButton tbbSplitByInvoice;
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
